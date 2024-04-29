using System.Text;
using Castle.DynamicProxy;
using CNG.Aspects.Interceptors;
using CNG.Cache;
using CNG.Core;
using CNG.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CNG.Aspects.CacheAspect
{
    public class CacheAspect : MethodInterception
    {
        [NonSerialized]
        private readonly ICacheService? _cacheService;
        [NonSerialized]
        private readonly object? _syncRoot;
        public readonly CacheAction Action;
        public readonly int? Duration;
        public readonly int Db;
        public readonly string? Target;
        public CacheAspect(CacheAction action,string?target=null, int duration=0,int db=0)
        {
            _cacheService ??= (ServiceTool.ServiceProvider ?? throw new AspectException("ServiceTool.ServiceProvider is null")).GetService<ICacheService>();
            _syncRoot ??= new object();
            Action = action;
            Duration=duration is 0 ? null : duration;
            Db=db;
            Target=target;
        }

        public override void Intercept(IInvocation invocation)
        {
            switch (Action)
            {
                case CacheAction.Add:
                {
                    var cacheKey = BuildCacheKey(invocation,Target);
                    if (_syncRoot == null) return;
                    lock (_syncRoot)
                    {
                        if (_cacheService != null && _cacheService.AnyAsync(cacheKey).Result)
                        {
                            invocation.ReturnValue =
                                JsonConvert.DeserializeObject(_cacheService.GetAsync(cacheKey).Result);
                  
                        }
                        else
                        {
                            if (_cacheService != null && !_cacheService.AnyAsync(cacheKey).Result)
                            {
                                invocation.Proceed();
                                var data = JsonConvert.SerializeObject(invocation.ReturnValue);
                                _cacheService.SetAsync(cacheKey, data, Duration, Db);
                            }
                            else
                            {
                                invocation.ReturnValue = _cacheService?.GetAsync(cacheKey);
                            }
                        }
                    }

                    break;
                }
                case CacheAction.Remove:
                default:
                {
                   

                    if (_syncRoot == null) return;
                    lock (_syncRoot)
                    {
                        var typeName = Target ?? GetTypeName(invocation.TargetType);
                            if (typeName != null) _cacheService?.RemoveByPatternAsync(typeName);
                    }

                    break;
                }
            }
        }
        private string BuildCacheKey(IInvocation invocation,string? customTypeName=null)
        {
            const string divider = "_";

            var typeName = customTypeName ?? GetTypeName(invocation.TargetType);

            var cacheKey = new StringBuilder();
            cacheKey.Append(typeName);
            cacheKey.Append(divider);
            cacheKey.Append(invocation.Method.Name);

            foreach (var argument in invocation.Arguments)
            {
                cacheKey.Append(argument == null ? divider : argument.ToString());
            }

            return cacheKey.ToString();
        }
        private static string? GetTypeName(Type type)
        {
            return ((type.UnderlyingSystemType).GenericTypeArguments.Any())
                ? ((type.UnderlyingSystemType).GenericTypeArguments[0]).Name
                : type.DeclaringType?.Name;
        }
        public enum CacheAction
        {
            /// <summary>
            /// Add a new item to cache.
            /// </summary>
            Add,
            /// <summary>
            /// Remove all associated items from cache for the given domain model.
            /// </summary>
            Remove
        }
    }
}
