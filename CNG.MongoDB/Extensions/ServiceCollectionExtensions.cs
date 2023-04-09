#nullable enable
using CNG.MongoDB.Configuration;
using CNG.MongoDB.Context;
using Microsoft.Extensions.DependencyInjection;

namespace CNG.MongoDB.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static void AddMongoDb(
      this IServiceCollection services,
      MongoDbRepositoryOptions options,
      ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
      services.AddSingleton(options);
      switch (lifetime)
      {
        case ServiceLifetime.Singleton:
          services.AddSingleton<IMongoDbContext, MongoDbContext>();
          break;
        case ServiceLifetime.Scoped:
          services.AddScoped<IMongoDbContext, MongoDbContext>();
          break;
        default:
          services.AddTransient<IMongoDbContext, MongoDbContext>();
          break;
      }
    }
  }
}
