using CNG.ChromeDriver.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CNG.ChromeDriver.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChromeDriverManagerService(this IServiceCollection services)
        {
            services.AddSingleton<IChromeDriverManagerService, ChromeDriverManagerService>();
        }
    }
}
