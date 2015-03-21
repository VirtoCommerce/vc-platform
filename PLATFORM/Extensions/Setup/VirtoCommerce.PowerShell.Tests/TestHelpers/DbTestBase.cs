using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using DbMigrator = VirtoCommerce.Foundation.Data.Infrastructure.DbMigrator;

namespace FunctionalTests.TestHelpers
{
    public class DbTestBase : TestBase
    {
        public const string DefaultDatabaseName = "VCFTests";

        public TestDatabase TestDatabase { get; private set; }

        public InfoContext Info
        {
            get { return TestDatabase.Info; }
        }

        public bool TableExists(string name)
        {
            return Info.TableExists(name);
        }

        public bool ColumnExists(string table, string name)
        {
            return Info.ColumnExists(table, name);
        }

        public void ResetDatabase()
        {
            if (DatabaseExists())
            {
                TestDatabase.ResetDatabase();
            }
            else
            {
                TestDatabase.EnsureDatabase();
            }
        }

        public void DropDatabase()
        {
            if (DatabaseExists())
            {
                TestDatabase.DropDatabase();
            }
        }

        public bool DatabaseExists()
        {
            if (TestDatabase == null)
                Init(DefaultDatabaseName);

            return TestDatabase.Exists();
        }

        public string ConnectionString
        {
            get { return TestDatabase.ConnectionString; }
        }

        public virtual void Init(string databaseName)
        {
            try
            {
                TestDatabase = new SqlTestDatabase(databaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        protected DbMigrator CreateMigrator<TConfiguration>() where TConfiguration : DbMigrationsConfiguration
        {
            var configuration = typeof(TConfiguration).CreateInstance<TConfiguration>();
            //var configuration = new Configuration();
            configuration.TargetDatabase = new DbConnectionInfo(TestDatabase.ConnectionString, TestDatabase.ProviderName);
			var migrator = new DbMigrator(configuration);
            return migrator;
        }

        public TContext CreateContext<TContext>()
    where TContext : DbContext
        {
            var contextInfo = new DbContextInfo(
                typeof(TContext), new DbConnectionInfo(TestDatabase.ConnectionString, TestDatabase.ProviderName));

            return (TContext)contextInfo.CreateInstance();
        }
    }
}
