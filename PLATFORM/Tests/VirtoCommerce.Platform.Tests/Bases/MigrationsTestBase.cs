using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using VirtoCommerce.Platform.Tests.Helpers;

namespace VirtoCommerce.Platform.Tests.Bases
{
    public class MigrationsTestBase : TestBase
    {
        private string _DefaultDatabaseName;

        public string DefaultDatabaseName
        {
            get
            {
                if(_DefaultDatabaseName == null)
                {
                    _DefaultDatabaseName = String.Format("VCM_{0}", Guid.NewGuid().ToString("N"));
                }
                return _DefaultDatabaseName;
            }
        }

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
            //DropDatabase();
            //TestDatabase.EnsureDatabase();
            if (DatabaseExists())
            {
                TestDatabase.ResetDatabase();
            }
            else
            {
                TestDatabase.EnsureDatabase();
                TestDatabase.ResetDatabase();
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
                AppDomain.CurrentDomain.SetData("DataDirectory", FunctionalTestBase.TempPath);
                TestDatabase = new SqlTestDatabase(databaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        protected TestDbMigrator CreateMigrator<TConfiguration>() where TConfiguration : DbMigrationsConfiguration
        {
            var configuration = typeof(TConfiguration).CreateInstance<TConfiguration>();
            //var configuration = new Configuration();
            configuration.TargetDatabase = new DbConnectionInfo(TestDatabase.ConnectionString, TestDatabase.ProviderName);
            var migrator = new TestDbMigrator(configuration);
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
