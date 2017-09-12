﻿using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Testing.Bases
{

    public abstract class FunctionalTestBase : TestBase, IDisposable
    {
        private readonly string _connectionStringFormat = "Server=(local);Database={0};Trusted_Connection=True";
        private string _defaultDatabaseName;

        public string DefaultDatabaseName => _defaultDatabaseName ?? (_defaultDatabaseName = string.Format(CultureInfo.InvariantCulture, "VCT_{0}", Guid.NewGuid().ToString("N")));

        public string ConnectionString { get; set; }

        public static string TempPath => Path.GetTempPath();

        private readonly object _previousDataDirectory;

        protected FunctionalTestBase()
        {
            _previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);

            var connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce_MigrationTestsBase");
            if (connectionString != null)
            {
                _connectionStringFormat = connectionString;
            }

            const string file = @";AttachDBFilename=|DataDirectory|\{0}.mdf";
            ConnectionString = string.Format(_connectionStringFormat, DefaultDatabaseName) + string.Format(file, DefaultDatabaseName);
        }

        /*
        protected virtual TRepository GetRepository<TRepository, TInitializer>()
            where TRepository : DbContext, new()
            where TInitializer : IDatabaseInitializer<TRepository>, new()
        {
            EnsureDatabaseInitialized(() => (TRepository)Activator.CreateInstance(typeof(TRepository), DatabaseConnectionString), () => Database.SetInitializer(new TInitializer()));
            return (TRepository)Activator.CreateInstance(typeof(TRepository), DatabaseConnectionString);
        }
        */


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

                dbPostInit?.Invoke(context);
            }
        }

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

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
        }

        #endregion
    }
}
