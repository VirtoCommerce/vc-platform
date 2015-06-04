using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.PowerShell.SearchSetup.Cmdlet;
using Xunit;

namespace VirtoCommerce.PowerShell.Tests
{
    public class SearchScenarios : DbTestBase
    {
        [Fact]
        public void Can_index_database_using_elasticsearch()
        {
            //DropDatabase();
            var publisher = new UpdateSearchIndex();
            var sqlConnnection = "Server=(local);Database=VirtoCommerce;Trusted_Connection=True;MultipleActiveResultSets=True";
            publisher.Index(new SearchConnection(dataSource: "localhost:9200", scope: "default"), sqlConnnection, null, true);
            //publisher.Index(new SearchConnection(dataSource: "temp", scope: "default", provider: "lucene"), sqlConnnection, null, true);
        }

        [Fact]
        public void Can_index_database_using_azuresearch()
        {
            //DropDatabase();
            var publisher = new UpdateSearchIndex();
            const string sqlConnnection = "Server=(local);Database=VirtoCommerce;Trusted_Connection=True;MultipleActiveResultSets=True";
            publisher.Index(new SearchConnection("server=virtocommerce;scope=default;key=128EE67AC838DF328B3BEC97ADB1A1B1;provider=azuresearch"), sqlConnnection, null, true);
            //publisher.Index(new SearchConnection(dataSource: "temp", scope: "default", provider: "lucene"), sqlConnnection, null, true);
        }

    }
}
