using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.ProcessSettings;
using VirtoCommerce.Platform.Data.Helpers;
using Xunit;

namespace VirtoCommerce.Platform.Tests.IntegrationTests
{
    [Trait("Category", "IntegrationTest")]
    public class ProcessHelperIntegrationTests
    {
        [Fact(Skip = "Test is broken")]
        public void StartProcess_ToolPathAsBaseProcessesPath()
        {
            var settings = new SomeSettings(new PlatformOptions { ProcessesPath = "c:\\wkhtmltopdf" });
            var process = ProcessHelper.StartProcess(settings);

            Assert.NotNull(process);
            process.Close();
        }

        [Fact(Skip = "Test is broken")]
        public void StartProcess_ToolPathAsGetToolPathViaManualInstallation()
        {
            var settings = new SomeSettings(new PlatformOptions());
            var process = ProcessHelper.StartProcess(settings);

            Assert.NotNull(process);
            process.Close();
        }
    }

    internal class SomeSettings : ProcessSettings
    {
        public SomeSettings(PlatformOptions platformOptions) : base(platformOptions)
        {
        }

        public override string ToolName => "wkhtmltopdf";
    }
}
