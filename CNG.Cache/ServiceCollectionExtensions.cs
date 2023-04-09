
using Microsoft.Extensions.DependencyInjection;

namespace CNG.Cache
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedisService(
          this IServiceCollection services,
          RedisOption option,
          ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.AddSingleton(option);
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<IRedisServer, RedisServer>();
                    services.AddSingleton<ICacheService, RedisCacheService>();
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<IRedisServer, RedisServer>();
                    services.AddScoped<ICacheService, RedisCacheService>();
                    break;
                default:
                    services.AddTransient<IRedisServer, RedisServer>();
                    services.AddTransient<ICacheService, RedisCacheService>();
                    break;
            }
            Console.WriteLine("Redis Service is installed");
        }
    }
}
