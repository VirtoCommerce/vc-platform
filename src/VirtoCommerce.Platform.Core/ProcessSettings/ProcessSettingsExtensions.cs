using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ProcessSettings
{
    public static class ProcessSettingsExtensions
    {
        public static ProcessSettings SetToolPath(this ProcessSettings processSettings, string toolPath)
        {
            processSettings.ToolPath = toolPath;
            return processSettings;
        }

        public static ProcessSettings SetWorkingDirectory(this ProcessSettings processSettings, string workingDirectory)
        {
            processSettings.WorkingDirectory = workingDirectory;
            return processSettings;
        }

        public static ProcessSettings SetArguments(this ProcessSettings processSettings, string[] arguments)
        {
            processSettings.Arguments.AddRange(arguments);
            return processSettings;
        }

        public static ProcessSettings SetEnvironmentVariables(this ProcessSettings processSettings, IReadOnlyDictionary<string, string> environmentVariables)
        {
            processSettings.EnvironmentVariables = environmentVariables;
            return processSettings;
        }

        public static string GetToolPathViaManualInstallation(this ProcessSettings processSettings, string toolName)
        {
            var result = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string toolnameexe = $"{toolName}.exe";
                result = new[]
                {
                    Path.Combine(GetProgramFiles(), toolName, toolnameexe),
                    Path.Combine(GetProgramFiles(), toolName, "bin", toolnameexe)
                }.FirstOrDefault(File.Exists);
                result = string.IsNullOrEmpty(result) ? toolnameexe : result;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                result = new[]
                {
                    $"/usr/bin/{toolName}",
                    $"/usr/local/bin/{toolName}",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bin", toolName)
                }.FirstOrDefault(File.Exists);
                result = string.IsNullOrEmpty(result) ? toolName : result;
            }

            return result;
        }

        private static string GetProgramFiles()
        {
            return Environment.GetFolderPath(
                Environment.Is64BitOperatingSystem
                    ? Environment.SpecialFolder.ProgramFiles
                    : Environment.SpecialFolder.ProgramFilesX86);
        }
    }
}
