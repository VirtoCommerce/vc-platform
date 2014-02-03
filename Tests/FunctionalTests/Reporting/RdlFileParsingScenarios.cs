using System.IO;
using System.Linq.Expressions;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Reporting.Helpers;
using Xunit;

namespace FunctionalTests.Reporting
{
    public class RdlFileParsingScenarios : TestBase
    {
        public const string EmbeddedReportsPath = @"FunctionalTests.Reporting.Resources.";

        [Fact]
        public void Can_deserealize_rdl_file()
        {
            var rdl = RdlType.Load(GetEmbeddedRdlFile("TestReport_v2.rdl"));
        }

        [Fact]
        public void Expression_parser_test()
        {
            var expr = new RdlExpression("=DateAdd(\"d\",-1, Today())")
            {
                GetParameterValue = (p) => 12
            };
            var def = expr.Compile().ToString();
            var res = expr.Evaluate();
        }

        private Stream GetEmbeddedRdlFile(string fileName)
        {
            return this.GetType().Assembly.GetManifestResourceStream(EmbeddedReportsPath+fileName);
        }
    }
}
