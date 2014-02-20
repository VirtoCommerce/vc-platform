using System;
using System.Data.Entity;
using System.IO;
using PlainElastic.Net.Utils;

namespace FunctionalTests.TestHelpers
{
    public enum RepositoryProvider
    {
        DataService,
        EntityFramework
    }

    public abstract class FunctionalTestBase : TestBase
    {
        private RepositoryProvider _provider = RepositoryProvider.EntityFramework;

        public static string TempPath 
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        public RepositoryProvider RepositoryProvider
        {
            get { return _provider; }
        }

        public virtual void Init(RepositoryProvider provider)
        {
            _provider = provider;
        }

        /*
        protected IRepository GetRepository()
        {

        }
         * */

        /*
        protected static void EnsureDatabaseInitialized(
            Func<DbContext> createContext, Action<DbContext> dbPostInit = null)
        {
            EnsureDatabaseInitialized(createContext, null, dbPostInit);
        }
         * */
        /// <summary>
        /// Ensures the database for the context is created and seeded.  This is typically used
        /// when a test is going to use a transaction to ensure that the DDL happens outside of
        /// the transaction.
        /// </summary>
        /// <param name="createContext">A func to create the context.</param>
		/// <param name="initializer">The initializer.</param>
        /// <param name="dbPostInit">Action that can be executed on db after creation</param>
		protected static void EnsureDatabaseInitialized(Func<DbContext> createContext, Action initializer = null, Action<DbContext> dbPostInit = null)
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

        /*
        protected static void EnsureDatabaseInitialized(Func<DbContext> createContext, Func<SetupMigrateDatabaseToLatestVersion<DbContext, DbMigrationsConfiguration<DbContext>>> initializer, Action<DbContext> dbPostInit = null)
        {
            using (var context = createContext())
            {
                context.Database.CreateIfNotExists();
                //initializer().InitializeDatabase(context);
                context.Database.Initialize(force: false);

                if (dbPostInit != null)
                {
                    dbPostInit(context);
                }
            }
        }
         * */

        /// <summary>
        ///     Drops the database that would be used for the context. Usually used to avoid errors during initialization.
        /// </summary>
        /// <param name="createContext"> A func to create the context. </param>
        protected static void DropDatabase(Func<DbContext> createContext)
        {
            using (var context = createContext())
            {
                context.Database.Delete();
            }
        }

	    /// <summary>
	    ///     Drops and then initializes the database that will be used for the context.
	    /// </summary>
	    /// <param name="createContext"> A func to create the context. </param>
	    /// <param name="initializer"></param>
	    protected static void ResetDatabase(Func<DbContext> createContext, Action initializer = null)
        {
            DropDatabase(createContext);
			EnsureDatabaseInitialized(createContext, initializer);
        }
    }
}
