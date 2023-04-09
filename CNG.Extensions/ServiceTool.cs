#nullable enable
using Microsoft.Extensions.DependencyInjection;

namespace CNG.Extensions
{
  public static class ServiceTool
  {
    public static IServiceProvider? ServiceProvider { get; private set; }

    public static IServiceCollection Create(IServiceCollection services)
    {
      ServiceTool.ServiceProvider = services.BuildServiceProvider();
      return services;
    }
  }
}
