using System.Text;
using System.Text.RegularExpressions;

namespace CNG.ChromeDriver.Driver
{
    public partial class Patcher
    {
        private readonly string? _driverExecutablePath;
        public Patcher(string? driverExecutablePath = null)
        {
            _driverExecutablePath=driverExecutablePath;
        }
        public void Auto()
        {
            if (_driverExecutablePath == null)
                throw new Exception("Parameter driverExecutablePath is required.");
            if (!IsBinaryPatched())
                PatchExe();
        }

        private bool IsBinaryPatched()
        {
            if (_driverExecutablePath == null)
                throw new Exception("Parameter driverExecutablePath is required.");

            using var fs = new FileStream(_driverExecutablePath,
                FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(fs, Encoding.GetEncoding("ISO-8859-1"));
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;
                if (line.Contains("undetected chromedriver"))
                    return true;
            }
            return false;
        }

        private void PatchExe()
        {
            if (_driverExecutablePath == null)
                throw new Exception("Parameter driverExecutablePath is required.");

            using var fs = new FileStream(_driverExecutablePath,
                FileMode.Open, FileAccess.ReadWrite);
            var buffer = new byte[1024];
            var stringBuilder = new StringBuilder();

            while (true)
            {
                var read = fs.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;
                stringBuilder.Append(
                    Encoding.GetEncoding("ISO-8859-1").GetString(buffer, 0, read));
            }

            var content = stringBuilder.ToString();
            var match = PatcherRegex().Match(content);
            if (!match.Success) return;
            var target = match.Value;
            var newTarget = "{console.log(\"undetected chromedriver 1337!\")}"
                .PadRight(target.Length, ' ');
            var newContent = content.Replace(target, newTarget);

            fs.Seek(0, SeekOrigin.Begin);
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(newContent);
            fs.Write(bytes, 0, bytes.Length);
        }

        [GeneratedRegex(@"\{window\.cdc.*?;\}")]
        private static partial Regex PatcherRegex();
    }
}
