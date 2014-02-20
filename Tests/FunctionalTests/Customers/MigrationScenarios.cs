using FunctionalTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Customers;
using Xunit;
using VirtoCommerce.Foundation.Data.Customers.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;

namespace FunctionalTests.Customers
{
    public class MigrationScenarios : MigrationsTestBase, IDisposable
    {
        #region Infrastructure/setup

        private readonly object _previousDataDirectory;
        private readonly string _databaseName;

        public MigrationScenarios()
        {
            _previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", FunctionalTestBase.TempPath);
            _databaseName = "CustomersTest";
        }

        public void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new EFCustomerRepository(_databaseName))
                {
                    context.Database.Delete();
                }
            }
            finally
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
            }
        }

        #endregion

        [Fact]
        public void Can_create_customer_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Contract"));
            migrator.Update("0");
            Assert.False(TableExists("Contract"));
            bool existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }

        [Fact]
        public void Can_update_customer_empty_database()
        {
            ResetDatabase();

            var migrator = CreateMigrator<Configuration>();
            migrator.Update();

            Assert.True(TableExists("Contract"));
            migrator.Update("0");
            Assert.False(TableExists("Contract"));
            DropDatabase();
        }
    }
}
