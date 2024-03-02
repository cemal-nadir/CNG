using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CNG.ChromeDriver.Driver
{
    public class ChromeDriverInstaller
    {
        public async Task<string> Auto(string? browserExecutablePath = null, bool force = false)
        {
            var version = await new ChromeExecutable()
                .GetVersion(browserExecutablePath);
            return await Install(version, force);
        }

        public async Task<string> Install(string version, bool force = false)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new Exception("Parameter version is required.");
            version = version[..version.LastIndexOf('.')];

            string platform;
            string zipName;
            string driverName;
            string tempPath;
            var ext = "";



            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "win32";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}.exe";
                tempPath = "AppData/Roaming/UndetectedChromeDriver";
                ext = ".exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux64";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}";
                tempPath = ".local/share/UndetectedChromeDriver";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "mac-x64";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}";
                tempPath = "Library/Application Support/UndetectedChromeDriver";
            }
            else
            {
                throw new PlatformNotSupportedException("Your operating system is not supported.");
            }


            var driverPath = Path.GetFullPath(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), tempPath, driverName));

            if (!force && File.Exists(driverPath))
                return driverPath;

            var httpClient = Http.Client;

            var versionResponse = await httpClient.GetAsync(
                "https://googlechromelabs.github.io/chrome-for-testing/latest-patch-versions-per-build.json");

            if (!versionResponse.IsSuccessStatusCode)
                throw new Exception($"ChromeDriver version request failed with status code: {versionResponse.StatusCode}, reason phrase: {versionResponse.ReasonPhrase}");

            var json = await versionResponse.Content.ReadAsStringAsync();

            var match = Regex.Match(json, $$"""
                                            "{{version}}":{"version":"(.*?)"
                                            """);

            var driverVersion = match.Groups[1].Value;
            if (driverVersion == "")
                throw new Exception($"ChromeDriver version not found for Chrome version {version}");

            var dirPath = Path.GetDirectoryName(driverPath) ?? throw new Exception("Get ChromeDriver directory failed.");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            if (force)
            {
                void deleteDriver()
                {
                    if (File.Exists(driverPath))
                        File.Delete(driverPath);
                }
                try
                {
                    deleteDriver();
                }
                catch
                {
                    try
                    {
                        await ForceKillInstances(driverPath);
                        deleteDriver();
                    }
                    catch { }
                }
            }

            const string baseUrl = "https://storage.googleapis.com/chrome-for-testing-public";

            var zipResponse = await httpClient.GetAsync($"{baseUrl}/{driverVersion}/{platform}/{zipName}");
            if (!zipResponse.IsSuccessStatusCode)
                throw new Exception($"ChromeDriver download request failed with status code: {zipResponse.StatusCode}, reason phrase: {zipResponse.ReasonPhrase}");

            await using (var zipStream = await zipResponse.Content.ReadAsStreamAsync())
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            await using (var fs = new FileStream(driverPath, FileMode.Create))
            {
                var entryName = $"chromedriver-{platform}/chromedriver{ext}";
                var entry = zipArchive.GetEntry(entryName) ?? throw new Exception($"Not found zip entry {entryName}.");
                await using var stream = entry.Open();
                await stream.CopyToAsync(fs);
            }



            // on Linux/macOS, you need to add the executable permission (+x) to allow the execution of the chromedriver
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return driverPath;
            var args = $"""
                        +x "{driverPath}"
                        """;
            var info = new ProcessStartInfo("chmod", args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info) ?? throw new Exception("Process start error.");
            try
            {
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitPatchAsync();
                process.Kill();
                process.Dispose();

                if (!string.IsNullOrEmpty(error))
                    throw new Exception("Failed to make chromedriver executable.");
            }
            catch
            {
                process.Dispose();
                throw;
            }

            return driverPath;
        }

        public async Task<string> GetDriverVersion(string driverExecutablePath)
        {
            if (driverExecutablePath == null)
                throw new Exception("Parameter driverExecutablePath is required.");

            const string args = "--version";
            var info = new ProcessStartInfo(driverExecutablePath, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info) ?? throw new Exception("Process start error.");
            try
            {
                var version = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitPatchAsync();
                process.Kill();
                process.Dispose();

                if (!string.IsNullOrEmpty(error))
                    throw new Exception("Failed to execute driver --version.");
                return version.Split(' ').Skip(1).First();
            }
            catch
            {
                process.Dispose();
                throw;
            }
        }

        private async Task ForceKillInstances(string driverExecutablePath)
        {
            var exeName = Path.GetFileName(driverExecutablePath);

            var cmd = "";
            var args = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cmd = "taskkill";
                args = $"/f /im {exeName}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                cmd = "kill";
                args = $"-f -9 $(pidof {exeName})";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd = "kill";
                args = $"-f -9 $(pidof {exeName})";
            }

            var info = new ProcessStartInfo(cmd, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = Process.Start(info) ?? throw new Exception("Process start error.");
            try
            {
                await process.WaitForExitPatchAsync();
                process.Kill();
                process.Dispose();
            }
            catch
            {
                process.Dispose();
                throw;
            }
        }
    }
}
