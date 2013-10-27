using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using DbMigrator = VirtoCommerce.Foundation.Data.Infrastructure.DbMigrator;

namespace VirtoCommerce.PowerShell.DatabaseSetup
{
    using System.Data.Common;
    using System.Data.EntityClient;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    /// <summary>
    ///     An implementation of <see cref="IDatabaseInitializer{TContext}" /> that will use Code First Migrations
    ///     to update the database to the latest version.
    /// </summary>
    public class SetupMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private DbMigrationsConfiguration _config;

        /// <summary>
        /// Initializes the <see cref="SetupMigrateDatabaseToLatestVersion{TContext, TMigrationsConfiguration}"/> class.
        /// </summary>
        static SetupMigrateDatabaseToLatestVersion()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MigrateDatabaseToLatestVersion class.
        /// </summary>
        public SetupMigrateDatabaseToLatestVersion()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupMigrateDatabaseToLatestVersion{TContext, TMigrationsConfiguration}"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SetupMigrateDatabaseToLatestVersion(string connectionString)
        {
            _config = new TMigrationsConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient")
            };
        }

        /// <summary>
        /// Initializes the database.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public virtual void InitializeDatabase(TContext context)
        {
            if (_config == null)
            {
                _config = new TMigrationsConfiguration
                {
                    TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "System.Data.SqlClient")
                };
            }

            var seed = !context.Database.Exists();

			var migrator = new DbMigrator(_config);
            if (!seed)
            {
                var local = migrator.GetLocalMigrations();
                var pending = migrator.GetPendingMigrations();
                if (local.Count() == pending.Count())
                {
                    seed = true;
                }
            }

            migrator.Update();

            InitializeDbSettings(context);

            if (seed)
            {
                Seed(context);
            }
        }

        protected virtual void InitializeDbSettings(TContext context)
        {
            // update general database settings here, for azure we need to connect to master db
            var originalDbName = context.Database.Connection.Database;
            var originalConnectionString = context.Database.Connection.ConnectionString;

            if (!originalConnectionString.ToLowerInvariant().Contains("(LocalDb)".ToLowerInvariant())) // do not modify connection string for localdb
            {
                context.Database.Connection.ConnectionString =
                    context.Database.Connection.ConnectionString.Replace(originalDbName, "master");
            }

            context.Database.Connection.Open();
            context.Database.ExecuteSqlCommand(
                string.Format(@"ALTER DATABASE [{0}] SET RECURSIVE_TRIGGERS ON;", originalDbName));
            context.Database.Connection.ConnectionString = originalConnectionString;
            context.Database.Connection.Close();
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void Seed(TContext context)
        {
        }
    }
}