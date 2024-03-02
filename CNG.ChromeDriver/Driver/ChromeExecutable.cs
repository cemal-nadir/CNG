using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CNG.ChromeDriver.Driver
{
    internal class ChromeExecutable
    {
        // refer: https://swimburger.net/blog/dotnet/download-the-right-chromedriver-version-and-keep-it-up-to-date-on-windows-linux-macos-using-csharp-dotnet
        public async Task<string> GetVersion(string? browserExecutablePath = null)
        {
            browserExecutablePath ??= GetExecutablePath();
            if (browserExecutablePath == null)
                throw new Exception("Not found chrome.exe.");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return FileVersionInfo.GetVersionInfo(browserExecutablePath).FileVersion
                    ?? throw new Exception("Chrome version not found in chrome.exe.");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                const string args = "--product-version";
                var info = new ProcessStartInfo(browserExecutablePath, args)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var process = Process.Start(info) ?? throw new Exception("Process start error.");
                try
                {
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitPatchAsync();
                    process.Kill();
                    process.Dispose();

                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                    return output;
                }
                catch
                {
                    process.Dispose();
                    throw;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                const string args = "--version";
                var info = new ProcessStartInfo(browserExecutablePath, args)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var process = Process.Start(info) ?? throw new Exception("Process start error.");
                try
                {
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitPatchAsync();
                    process.Kill();
                    process.Dispose();

                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                    return output.Replace("Google Chrome ", "");
                }
                catch
                {
                    process.Dispose();
                    throw;
                }
            }

            throw new PlatformNotSupportedException("Your operating system is not supported.");

        }

        public static string? GetExecutablePath()
        {
            var result = null as string;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                result = FindChromeExecutable();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                result = FindChromeExecutableLinux();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                result = FindChromeExecutableMacos();

            return result;
        }

        private static string? FindChromeExecutable()
        {
            var candidates = new List<string>();

            foreach (var item in new[] {
                "PROGRAMFILES", "PROGRAMFILES(X86)", "LOCALAPPDATA", "PROGRAMW6432"
            })
            {
                foreach (var subItem in new[] {
                    @"Google\Chrome\Application",
                    @"Google\Chrome Beta\Application",
                    @"Google\Chrome Canary\Application"
                })
                {
                    var variable = Environment.GetEnvironmentVariable(item);
                    if (variable != null)
                        candidates.Add(Path.Combine(variable, subItem, "chrome.exe"));
                }
            }

            return candidates.FirstOrDefault(File.Exists);
        }

        private static string? FindChromeExecutableLinux()
        {
            var candidates = new List<string>();

            var environmentPATH = Environment.GetEnvironmentVariable("PATH") ??
                                  throw new Exception("Not found environment PATH.");
            var variables = environmentPATH.Split(Path.PathSeparator);
            foreach (var item in variables)
            {
                candidates.AddRange(new[] { "google-chrome", "chromium", "chromium-browser", "chrome", "google-chrome-stable", }.Select(subItem => Path.Combine(item, subItem)));
            }

            return candidates.FirstOrDefault(File.Exists);
        }

        private static string? FindChromeExecutableMacos()
        {
            var environmentPATH = Environment.GetEnvironmentVariable("PATH") ?? throw new Exception("Not found environment PATH.");
            var variables = environmentPATH.Split(Path.PathSeparator);
            var candidates = (from item in variables
                              from subItem in new[]
                                  { "google-chrome", "chromium", "chromium-browser", "chrome", "google-chrome-stable", }
                              select Path.Combine(item, subItem)).ToList();

            candidates.AddRange(new[] {
                "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
                "/Applications/Chromium.app/Contents/MacOS/Chromium"
            });

            return candidates.FirstOrDefault(File.Exists);
        }
    }
}
