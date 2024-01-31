
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

            services.AddSingleton(provider =>
            {
	            var configuration = ConfigurationOptions.Parse(option.ConnectionString);
	            return ConnectionMultiplexer.Connect(configuration);
            });

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
                case ServiceLifetime.Transient:
                default:
                    services.AddTransient<IRedisServer, RedisServer>();
                    services.AddTransient<ICacheService, RedisCacheService>();
                    break;
            }
           
			Console.WriteLine("Redis Service is installed");
        }
    }
}
