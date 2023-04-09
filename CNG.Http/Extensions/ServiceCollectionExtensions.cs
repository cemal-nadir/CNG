#nullable enable
using CNG.Http.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CNG.Http.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static void AddHttpClientService(
      this IServiceCollection services,
      ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
      switch (lifetime)
      {
        case ServiceLifetime.Singleton:
          services.AddSingleton<HttpClient>();
          services.AddSingleton<IHttpClientService, HttpClientService>();
          break;
        case ServiceLifetime.Scoped:
          services.AddScoped<HttpClient>();
          services.AddScoped<IHttpClientService, HttpClientService>();
          break;
        case ServiceLifetime.Transient:
        default:
          services.AddTransient<HttpClient>();
          services.AddTransient<IHttpClientService, HttpClientService>();
          break;
      }
      Console.WriteLine("Http Client Service is installed. LifeTime : " + lifetime);
    }
  }
}
