using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ProcessSettings
{
    public class WkHtmlToPdfSettings : ProcessSettings
    {
        public override string ToolPath => "wkhtmltopdf";
        public override string WorkingDirectory { get; internal set; }
        public override IReadOnlyDictionary<string, string> EnvironmentVariables { get; internal set; }
        public override string[] Arguments { get; internal set; } = new[] { "--print-media-type ", "--page-size Letter " };
    }
}
