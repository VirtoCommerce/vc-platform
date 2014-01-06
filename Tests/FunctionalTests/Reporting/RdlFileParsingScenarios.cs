using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Reporting.Helpers;
using Xunit;

namespace FunctionalTests.Reporting
{
    public class RdlFileParsingScenarios : TestBase
    {
        public const string EmbeddedReportsPath = @"FunctionalTests.Reporting.Resources.";

        private readonly IEnumerable<RdlFile.DataSource> TestReport_v2_DataSources;
        private readonly IEnumerable<RdlFile.DataSet> TestReport_v2_DataSets;

        public RdlFileParsingScenarios()
        {
            TestReport_v2_DataSources = new List<RdlFile.DataSource>
            { 
                new RdlFile.DataSource
                {
                    Name = "VirtoCommerce",
                    ConnectionString = "Data Source=localhost;Initial Catalog=VirtoCommerce;Integrated Security=True"
                },
                new RdlFile.DataSource
                {
                    Name = "ExternalSource",
                    ConnectionString = "Data Source=localhost;Initial Catalog=VirtoCommerce;Integrated Security=True"
                }
            };

            TestReport_v2_DataSets = new List<RdlFile.DataSet>
            {
                new RdlFile.DataSet
                {
                    Name = "DataSet1",
                    CommandText = "SELECT * FROM item",
                    DataSource = TestReport_v2_DataSources.First(d=>d.Name=="VirtoCommerce")
                },
                new RdlFile.DataSet
                {
                    Name = "DataSet2",
                    CommandText = "SELECT * FROM Account",
                    DataSource = TestReport_v2_DataSources.First(d=>d.Name=="ExternalSource")
                }
            };
        }

        [Fact]
        public void Can_parse_rdl_file()
        {
            using (Stream stream = GetEmbeddedRdlFile("TestReport_v2.rdlc"))
            {
                RdlFile rdl = new RdlFile(stream);

                // Can parse Datasources?
                IEnumerable<RdlFile.DataSource> dataSources = rdl.DataSources;
                Assert.NotNull(dataSources);
                Assert.Equal(TestReport_v2_DataSources.Count(), dataSources.Count());
                foreach (var dataSource in TestReport_v2_DataSources)
                {
                    Assert.Contains(dataSource, dataSources);    
                }

                // Can parse DataSets ?
                IEnumerable<RdlFile.DataSet> dataSets = rdl.DataSets;
                Assert.NotNull(dataSets);
                Assert.Equal(TestReport_v2_DataSets.Count(), dataSets.Count());
                foreach (var dataSet in TestReport_v2_DataSets)
                {
                    Assert.Contains(dataSet, dataSets);
                }
                
            }
        }

        [Fact]
        public void Can_parse_rdl_file_without_datasources()
        {
            using (Stream stream = GetEmbeddedRdlFile("ReportWithoutDataSources.rdlc"))
            {
                RdlFile rdl = new RdlFile(stream);

                // Must return empty list 
                IEnumerable<RdlFile.DataSource> dataSources = rdl.DataSources;
                Assert.NotNull(dataSources);
                Assert.Equal(0, dataSources.Count());

                IEnumerable<RdlFile.DataSet> dataSets = rdl.DataSets;
                Assert.NotNull(dataSets);
                Assert.Equal(0, dataSets.Count());
            }
        }

        private Stream GetEmbeddedRdlFile(string fileName)
        {
            return this.GetType().Assembly.GetManifestResourceStream(EmbeddedReportsPath+fileName);
        }
    }
}
