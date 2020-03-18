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
            processSettings.ToolPath = Path.GetFullPath(toolPath);
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

        public static string GetFullPathTool(this ProcessSettings processSettings)
        {
            var result = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                result = new[]
                {
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, $"{processSettings.ToolName}.exe") : string.Empty,
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, "bin", $"{processSettings.ToolName}.exe") : string.Empty,
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, processSettings.ToolName, $"{processSettings.ToolName}.exe") : string.Empty,
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, processSettings.ToolName, "bin", $"{processSettings.ToolName}.exe") : string.Empty,
                    Path.Combine(GetProgramFiles(), processSettings.ToolName, "bin", $"{processSettings.ToolName}.exe"),
                    Path.Combine(GetProgramFiles(), processSettings.ToolName, $"{processSettings.ToolName}.exe")
                }.FirstOrDefault(File.Exists);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                result = new[]
                {
                    $"/usr/bin/{processSettings.ToolName}",
                    $"/usr/local/bin/{processSettings.ToolName}",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bin", processSettings.ToolName),
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, processSettings.ToolName, processSettings.ToolName) : string.Empty,
                    !string.IsNullOrEmpty(processSettings.ToolPath) ? Path.Combine(processSettings.ToolPath, processSettings.ToolName) : string.Empty
                }.FirstOrDefault(File.Exists);
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
