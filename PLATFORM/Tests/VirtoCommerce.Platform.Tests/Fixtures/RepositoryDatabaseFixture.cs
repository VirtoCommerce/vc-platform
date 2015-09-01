using System;
using System.Data.Entity;
using System.IO;
using VirtoCommerce.Platform.Tests.Bases;

namespace VirtoCommerce.Platform.Tests.Fixtures
{
    /*
     * http://xunit.github.io/docs/shared-context.html#class-fixture
     * */
    public class RepositoryDatabaseFixture<TRepository, TInitializer> : IDisposable
        where TRepository : DbContext, new()
        where TInitializer : IDatabaseInitializer<TRepository>, new()
    {
        public static string DatabaseConnectionString
        {
            get { return @"Data Source=(LocalDb)\v11.0;Initial Catalog=VirtoCommerceTest;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\VirtoCommerceTest.mdf"; }
        }

        public static string TempPath
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        private readonly object _previousDataDirectory;

        public RepositoryDatabaseFixture()
        {
            _previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);

            EnsureDatabaseInitialized(() => (TRepository)Activator.CreateInstance(typeof(TRepository), DatabaseConnectionString), () => Database.SetInitializer(new TInitializer()));
            this.Db = (TRepository)Activator.CreateInstance(typeof(TRepository), DatabaseConnectionString);
        }


        /// <summary>
        /// Ensures the database for the context is created and seeded.  This is typically used
        /// when a test is going to use a transaction to ensure that the DDL happens outside of
        /// the transaction.
        /// </summary>
        /// <param name="createContext">A func to create the context.</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="dbPostInit">Action that can be executed on db after creation</param>
        private static void EnsureDatabaseInitialized(Func<DbContext> createContext, Action initializer = null, Action<DbContext> dbPostInit = null)
        {
            using (var context = createContext())
            {
                if (initializer == null)
                {
                    context.Database.CreateIfNotExists();
                }
                else
                {
                    initializer();
                }

                context.Database.Initialize(force: false);

                if (dbPostInit != null)
                {
                    dbPostInit(context);
                }
            }
        }

        public void Dispose()
        {
            try
            {
                Db.Database.Delete();
            }
            finally
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
            }
        }

        public TRepository Db { get; private set; }
    }
}
