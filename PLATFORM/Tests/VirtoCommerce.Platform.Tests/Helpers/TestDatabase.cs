using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Tests.Helpers
{
    public abstract class TestDatabase
    {
        public string ConnectionString { get; protected set; }
        public string ProviderName { get; protected set; }

        public virtual InfoContext Info { get; protected set; }

        public abstract bool Exists();

        public void EnsureDatabase()
        {
            using (var context = new EmptyContext(CreateConnection(ConnectionString)))
            {
                context.Database.CreateIfNotExists();
            }
        }

        //public abstract void EnsureDatabase();

        public abstract void ResetDatabase();

        public abstract void DropDatabase();

        public abstract DbConnection CreateConnection(string connectionString);

        protected static InfoContext CreateInfoContext(DbConnection connection, bool supportsSchema = true)
        {
            var info = new InfoContext(connection, supportsSchema);
            info.Database.Initialize(force: false);

            return info;
        }

        protected void ExecuteNonQuery(string commandText, string connectionString = null)
        {
            Execute(commandText, c => c.ExecuteNonQuery(), connectionString);
        }

        protected T ExecuteScalar<T>(string commandText, string connectionString = null)
        {
            return Execute(commandText, c => (T)c.ExecuteScalar(), connectionString);
        }

        private T Execute<T>(string commandText, Func<DbCommand, T> action, string connectionString = null)
        {
            connectionString = connectionString ?? ConnectionString;

            using (var connection = CreateConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = commandText;

                    return action(command);
                }
            }
        }
    }
}
