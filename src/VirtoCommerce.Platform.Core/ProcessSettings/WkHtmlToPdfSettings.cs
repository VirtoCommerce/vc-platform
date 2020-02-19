using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ProcessSettings
{
    public class WkHtmlToPdfSettings : ProcessSettings
    {
        public override string ToolName => "wkhtmltopdf";
        public override string ToolPath => base.ToolPath ?? this.GetToolPathViaManualInstallation(ToolName);
        public override string WorkingDirectory { get; internal set; }
        public override IReadOnlyDictionary<string, string> EnvironmentVariables { get; internal set; }
        public override string[] Arguments { get; internal set; } = new[] { "--print-media-type ", "--page-size Letter " };
    }
}
