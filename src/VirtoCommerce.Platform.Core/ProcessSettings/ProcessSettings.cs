using System.IO;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ProcessSettings
{
    public abstract class ProcessSettings
    {
        private ProcessSettings()
        {

        }

        public ProcessSettings(PlatformOptions platformOptions)
        {
            if (!string.IsNullOrEmpty(platformOptions.ProcessesPath))
            {
                ToolPath = Path.GetFullPath(Path.Combine(platformOptions.ProcessesPath));
            }
        }
        /// <summary>
        /// Name of process
        /// </summary>
        public virtual string ToolName { get; internal set; }
        /// <summary>
        /// Full path of process
        /// </summary>
        public virtual string ToolPath { get; internal set; }
        /// <summary>
        /// Directory where process is worked
        /// </summary>
        public virtual string WorkingDirectory { get; internal set; }
        /// <summary>
        /// Env. variables for running process
        /// </summary>
        public virtual IReadOnlyDictionary<string, string> EnvironmentVariables { get; internal set; }
        /// <summary>
        /// Arguments for running process
        /// </summary>
        public IList<string> Arguments { get; set; } = new List<string>();
    }
}
