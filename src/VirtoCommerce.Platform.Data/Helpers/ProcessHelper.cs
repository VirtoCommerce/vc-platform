using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VirtoCommerce.Platform.Core.ProcessSettings;

namespace VirtoCommerce.Platform.Data.Helpers
{
    public static class ProcessHelper
    {
        public static Process StartProcess(ProcessSettings toolSettings)
        {
            return StartProcessInternal(toolSettings.GetFullPathTool(),
                toolSettings.Arguments.ToArray(),
                toolSettings.WorkingDirectory,
                toolSettings.EnvironmentVariables);
        }

        public static StreamReader GetOutputAsStreamReader(this Process process)
        {
            return process.StandardOutput;
        }

        public static byte[] GetOutputAsByteArray(this Process process)
        {
            byte[] buffer = new byte[32768];
            byte[] file;
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                    {
                        break;
                    }
                    ms.Write(buffer, 0, read);
                }
                file = ms.ToArray();
            }
            process.WaitForExit(30000);
            process.Close();
            return file;
        }


        private static Process StartProcessInternal(
            string toolPath,
            string[] arguments = null,
            string workingDirectory = null,
            IReadOnlyDictionary<string, string> environmentVariables = null)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = toolPath,
                Arguments = string.Join(" ", arguments),
                WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };

            ApplyEnvironmentVariables(environmentVariables, startInfo);

            var process = Process.Start(startInfo);
            if (process == null)
                return null;

            return process;
        }

        private static void ApplyEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables, ProcessStartInfo startInfo)
        {
            if (environmentVariables == null)
                return;

            startInfo.Environment.Clear();

            foreach (var pair in environmentVariables)
                startInfo.Environment[pair.Key] = pair.Value;
        }
    }
}
