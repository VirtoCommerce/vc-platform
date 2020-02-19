using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ProcessSettings
{
    public abstract class ProcessSettings
    {
        public virtual string ToolName { get; internal set; }
        public virtual string ToolPath { get; internal set; }
        public virtual string WorkingDirectory { get; internal set; }
        public virtual IReadOnlyDictionary<string, string> EnvironmentVariables { get; internal set; }
        public virtual string[] Arguments { get; internal set; }
    }
}
