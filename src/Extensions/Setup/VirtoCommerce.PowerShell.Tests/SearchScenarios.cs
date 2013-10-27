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
        public void Can_index_database()
        {
            //DropDatabase();
            var publisher = new UpdateSearchIndex();
            var sqlConnnection = "Server=(local);Database=VirtoCommerce;Trusted_Connection=True;MultipleActiveResultSets=True";
            publisher.Index(new SearchConnection(dataSource: "localhost:9200", scope: "default"), sqlConnnection, null, true);
        }
    }
}
