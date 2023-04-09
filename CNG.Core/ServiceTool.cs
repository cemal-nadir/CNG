using Microsoft.Extensions.DependencyInjection;

namespace CNG.Core
{
  public static class ServiceTool
  {
    public static IServiceProvider? ServiceProvider { get; private set; }

    public static IServiceCollection Create(IServiceCollection services)
    {
      ServiceProvider = services.BuildServiceProvider();
      return services;
    }
  }
}
