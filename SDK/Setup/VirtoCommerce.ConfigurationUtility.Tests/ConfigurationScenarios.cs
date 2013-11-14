using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Activities;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels;
using Xunit;

namespace VirtoCommerce.ConfigurationUtility.Tests
{
    public class ConfigurationScenarios
    {
        /*
        [Fact]
        public void ProcessProjectReferences()
        {
            var activity = new ResolveProjectReferences();

            var outputDir = Path.GetDirectoryName(typeof(ConfigurationScenarios).Assembly.Location);

            outputDir = @"e:\Projects\VCF\src\CommerceFoundation.Presentation\FrontEnd\StoreWebApp\bin";

            var input1 = new Dictionary<string, object> 
        {
            { "Project", @"e:\Projects\VCF\src\CommerceFoundation.Presentation\FrontEnd\StoreWebApp\StoreWebApp.csproj" },
            { "Configuration", "Debug" },
            { "Platform", "Any CPU" },
            { "OutputDirectory", outputDir },
            { "IsAlreadyProcessed", false }
        };

            var output1 = WorkflowInvoker.Invoke(activity, input1);
            //var ctx = new CodeActivityContext();

        }
         * */
    }
}
