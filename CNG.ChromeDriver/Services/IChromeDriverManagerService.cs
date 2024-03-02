using CNG.ChromeDriver.Models;
using OpenQA.Selenium.Chrome;

namespace CNG.ChromeDriver.Services
{
    public interface IChromeDriverManagerService:IDisposable
    {
        /// <summary>
        /// Creates a new chrome instance and returns the id of this instance.
        /// It is mandatory to access this id value in all operations to be performed with
        /// the chrome instance.
        /// </summary>
        /// <param name="isHeadless">Run in headless mode ?</param>
        /// <param name="hideCommandPromptWindow">Hide the command prompt window?</param>
        /// <param name="commandTimeOut">Maximum time the driver should wait per action</param>
        /// <param name="extensions">Paths of extensions that should be installed by default when starting instance</param>
        /// <param name="preferences">https://chromium.googlesource.com/chromium/src/+/refs/heads/main/chrome/common/pref_names.h</param>
        /// <param name="proxy">Proxy information</param>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        Task<string> StartDriver(bool isHeadless = true, bool hideCommandPromptWindow = true,
            TimeSpan? commandTimeOut = null, string[]? extensions = null,
            Dictionary<string, object>? preferences = null, ProxyModel? proxy = null,
            Action<ChromeDriverService>? configureServices = null);
        /// <summary>
        /// The method where you can access the relevant driver by giving the id value you obtained after creating the instance
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        Driver.UndetectedChromeDriver? Driver(string driverId);
        /// <summary>
        /// The method where you can close the related driver by giving the id value you obtained after creating the instance
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        void StopDriver(string driverId);
        /// <summary>
        /// The method where you can restart the related driver by giving the id value you obtained after creating the instance
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        Task RestartDriver(string driverId);
        /// <summary>
        /// Method to close all drivers
        /// </summary>
        void StopDrivers();
        /// <summary>
        /// Method to restart all drivers
        /// </summary>
        /// <returns></returns>
        Task RestartDrivers();
    }
}
