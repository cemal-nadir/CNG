using System.Diagnostics;

namespace CNG.ChromeDriver.Driver
{
    internal static class ProcessExtension
    {
        public static async Task WaitForExitPatchAsync(this Process process,
            CancellationToken cancellationToken = default) =>
            await process.WaitForExitAsync(cancellationToken);
    }
}
