using System.IO;
using VirtoCommerce.Platform.Core.ProcessSettings;
using VirtoCommerce.Platform.Data.Helpers;
using Xunit;

namespace VirtoCommerce.Platform.Tests.IntegrationTests
{
    [Trait("Category", "IntegrationTest")]
    public class ProcessHelperIntegrationTests
    {
        [Fact]
        public void StartProcess_ConvertHtmlToPdf()
        {
            var result = ProcessHelper.StartProcess(new WkHtmlToPdfSettings()
                    .SetWorkingDirectory(Directory.GetCurrentDirectory())
                    .SetArguments(new[] { "input.html", " ", " - " }))
                .GetOutputAsByteArray();

            File.WriteAllBytes("output.pdf", result);
        }
    }
}
