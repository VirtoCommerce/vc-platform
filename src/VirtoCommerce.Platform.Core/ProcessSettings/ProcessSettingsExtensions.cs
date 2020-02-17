using System.Collections.Generic;

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
            var target = new string[processSettings.Arguments.Length + arguments.Length];
            processSettings.Arguments.CopyTo(target, 0);
            arguments.CopyTo(target, processSettings.Arguments.Length);
            processSettings.Arguments = target;
            return processSettings;
        }

        public static ProcessSettings SetEnvironmentVariables(this ProcessSettings processSettings, IReadOnlyDictionary<string, string> environmentVariables)
        {
            processSettings.EnvironmentVariables = environmentVariables;
            return processSettings;
        }
    }
}
