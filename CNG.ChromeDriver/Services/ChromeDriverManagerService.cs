using CNG.ChromeDriver.Driver;
using CNG.ChromeDriver.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CNG.ChromeDriver.Services
{
    public class ChromeDriverManagerService:IChromeDriverManagerService
    {
        private readonly Dictionary<string, Driver.UndetectedChromeDriver> _drivers = new();

        public async Task<string> StartDriver(bool isHeadless = true, bool hideCommandPromptWindow = true,
            TimeSpan? commandTimeOut = null, string[]? extensions = null,
            Dictionary<string, object>? preferences = null, ProxyModel? proxy = null,
            Action<ChromeDriverService>? configureServices = null)
        {
            var driverPath = await new ChromeDriverInstaller().Auto();
            var options = new ChromeOptions();
            options.AddArguments("--disable-gpu",
                "--autoplay-policy=user-gesture-required",
                "--disable-background-networking",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-breakpad",
                "--disable-client-side-phishing-detection",
                "--disable-component-update",
                "--disable-dev-shm-usage",
                "--disable-domain-reliability",
                "--disable-extensions",
                "--disable-features=AudioServiceOutOfProcess",
                "--disable-hang-monitor",
                "--disable-ipc-flooding-protection",
                "--disable-notifications",
                "--disable-offer-store-unmasked-wallet-cards",
                "--disable-popup-blocking",
                "--disable-print-preview",
                "--disable-prompt-on-repost",
                "--disable-renderer-backgrounding",
                "--disable-setuid-sandbox",
                "--disable-speech-api",
                "--disk-cache-size=33554432",
                "--hide-scrollbars",
                "--ignore-gpu-blacklist",
                "--ignore-certificate-errors",
                "--metrics-recording-only",
                "--mute-audio",
                "--no-default-browser-check",
                "--no-first-run",
                "--no-pings",
                "--no-sandbox",
                "--test-type",
                "--no-zygote",
                "--password-store=basic",
                "--use-gl=swiftshader",
                "--use-mock-keychain",
                "--single-process");
            if (proxy != null)
            {
                options.Proxy = new Proxy()
                {
                    SslProxy = $"{proxy.Ip}:{proxy.Port}",
                    HttpProxy = $"{proxy.Ip}:{proxy.Port}",
                    Kind = ProxyKind.Manual
                };
            }

            if (extensions is { Length: > 0 })
            {
                options.AddArgument(extensions.Aggregate("--load-extension=",
                    (current, extension) => current + $",{extension}"));
            }

            var driverId = Guid.NewGuid().ToString("N");
            var driver = UndetectedChromeDriver.Create(options, driverExecutablePath: driverPath, headless: isHeadless,
                hideCommandPromptWindow: hideCommandPromptWindow, commandTimeout: commandTimeOut, prefs: preferences,
                configureService: configureServices);
            if (proxy != null)
            {
                var handler = new NetworkAuthenticationHandler()
                {
                    UriMatcher = _ => true,
                    Credentials = new PasswordCredentials(proxy.Username, proxy.Password)
                };
                driver.Manage().Network.AddAuthenticationHandler(handler);
            }
            _drivers.Add(driverId, driver);
            return driverId;
        }

        public UndetectedChromeDriver? Driver(string driverId) => _drivers.GetValueOrDefault(driverId);
        public void StopDrivers()
        {
            _drivers.Values.ToList().ForEach(driver =>
            {
                driver.Dispose();
            });
            _drivers.Clear();
        }
        public void StopDriver(string driverId)
        {
            if (!_drivers.TryGetValue(driverId, out var driver)) return;
            driver.Dispose();
            _drivers.Remove(driverId);
        }
        public async Task RestartDriver(string driverId)
        {
            if (!_drivers.TryGetValue(driverId, out var driver)) return;
            await driver.Reconnect();
        }
        public async Task RestartDrivers()
        {
            foreach (var driver in _drivers.Values.ToList())
            {
                await driver.Reconnect();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               StopDrivers();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
