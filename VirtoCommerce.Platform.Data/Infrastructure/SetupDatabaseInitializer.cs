using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VirtoCommerce.Platform.Data.Common;
namespace VirtoCommerce.Platform.Data.Infrastructure
{
    /// <summary>
    /// An implementation of <see cref="IDatabaseInitializer{TContext}" /> that will use Code First Migrations
    /// to update the database to the latest version.
    /// </summary>
    public class SetupDatabaseInitializer<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private DbMigrationsConfiguration _config;

        public string DataDirectoryPath { get; set; }

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

            var migrator = new System.Data.Entity.Migrations.DbMigrator(_config);
            var pending = migrator.GetPendingMigrations().Count();
            var local = migrator.GetLocalMigrations().Count();
            var exists = context.Database.Exists();

            if (!exists || pending > 0)
            {
                try
                {
                    migrator.Update();
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException($"Migrations failed with error \"{ex.ExpandExceptionMessage()}\"", ex);
                }

                InitializeDbSettings(context);

                if (pending == local)
                {
                    Seed(context);
                }
            }
        }

        protected virtual void InitializeDbSettings(TContext context)
        {
            // Enable recursive triggers
            using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "ALTER DATABASE CURRENT SET RECURSIVE_TRIGGERS ON";
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void Seed(TContext context)
        {
        }

        protected virtual void ExecuteSqlScriptFile(DbContext context, string fileName, string modelName)
        {
            var text = ReadSqlScriptFile(fileName, modelName);

            if (!string.IsNullOrEmpty(text))
            {
                // this allows to have GO in the script
                foreach (var cmd in SplitScriptByGo(text).Where(cmd => !string.IsNullOrEmpty(cmd)))
                {
                    try
                    {
                        context.Database.ExecuteSqlCommand(cmd);
                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException($"Failed to process \"{fileName}\" in \"{modelName}\" model. Message: {ex.Message}", ex);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"File '{fileName}' is missing.");
            }
        }

        protected virtual string ReadSqlScriptFile(string fileName, string modelName)
        {
            string result = null;

            var paths = new List<string>
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, modelName, "Data", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", modelName, "Data", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", modelName, "Data", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
            };

            if (!string.IsNullOrEmpty(DataDirectoryPath))
                paths.Insert(0, Path.Combine(DataDirectoryPath, modelName, "Data", fileName));

            var path = paths.FirstOrDefault(File.Exists);

            if (path != null)
            {
                result = File.ReadAllText(path);
            }
            else
            {
                var assembly = GetType().Assembly;
                var assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));
                var name = string.Join(".", assemblyName, "Repositories.Sql", fileName);
                var stream = assembly.GetManifestResourceStream(name);

                if (stream != null)
                    result = new StreamReader(stream).ReadToEnd();
            }

            return result;
        }

        protected virtual string[] SplitScriptByGo(string script)
        {
            const string splitByGoRegex = @"(?m)^\s*GO\s*\d*\s*$";
            return Regex.Split(script + Environment.NewLine, splitByGoRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
    }
}
