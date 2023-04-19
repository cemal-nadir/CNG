#nullable enable
using System.Reflection;
using Castle.DynamicProxy;

namespace CNG.Aspects.Interceptors
{
  public class AspectInterceptorSelector : IInterceptorSelector
  {
    public IInterceptor[] SelectInterceptors(
      Type type,
      MethodInfo methodInfo,
      IInterceptor[] interceptors)
    {
      List<MethodInterceptionBase> list1 = type.GetCustomAttributes<MethodInterceptionBase>(true).ToList();
      MethodInfo? method = type.GetMethod(methodInfo.Name);
      List<MethodInterceptionBase>? list2 = method?.GetCustomAttributes<MethodInterceptionBase>(true).ToList();
      if (list2 != null && list2.Any())
        list1.AddRange(list2);
      return list1.OrderBy((Func<MethodInterceptionBase, int>) (x => x.Priority)).ToArray();
    }
  }
}
