namespace FunctionalTests.TestHelpers
{
    using System.Data.Common;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.SqlClient;
    using System.IO;
    using System;
    using System.Data.Entity;

    public abstract class TestDatabase
    {
        public string ConnectionString { get; protected set; }
        public string ProviderName { get; protected set; }

        public virtual InfoContext Info { get; protected set; }

        public abstract bool Exists();

        public abstract void EnsureDatabase();

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

    public class SqlTestDatabase : TestDatabase
    {
        private readonly string _name;

        private const string ConnectionStringFormat
            = "Server=(local);Database={0};Trusted_Connection=True";
            //= "Data Source=(local);Initial Catalog={0};Integrated Security=True;Pooling=false;";
        

        public SqlTestDatabase(string name)
        {
            _name = name;

            ConnectionString = string.Format(ConnectionStringFormat, name);
            ProviderName = "System.Data.SqlClient";
            Info = CreateInfoContext(new SqlConnection(ConnectionString));
        }

        public override void EnsureDatabase()
        {
            var sql
                = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'" + _name + "') "
                  + "CREATE DATABASE [" + _name + "]";

            ExecuteNonQuery(sql, string.Format(ConnectionStringFormat, "master"));

            ResetDatabase();
        }

        public override void ResetDatabase()
        {
            ExecuteNonQuery(
                @"DECLARE @sql NVARCHAR(1024);
                  
                  DECLARE history_cursor CURSOR FOR
                  SELECT 'DROP TABLE ' + SCHEMA_NAME(schema_id) + '.' + object_name(object_id) + ';'
                  FROM sys.objects
                  WHERE name = '__MigrationHistory'
                  
                  OPEN history_cursor;
                  FETCH NEXT FROM history_cursor INTO @sql;
                  WHILE @@FETCH_STATUS = 0
                  BEGIN
                      EXEC sp_executesql @sql;
                      FETCH NEXT FROM history_cursor INTO  @sql;
                  END
                  CLOSE history_cursor;
                  DEALLOCATE history_cursor;
 
                  DECLARE @constraint_name NVARCHAR(256),
		                  @table_schema NVARCHAR(100),
		                  @table_name NVARCHAR(100);
                 
                  DECLARE constraint_cursor CURSOR FOR
                  SELECT constraint_name, table_schema, table_name
                  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
                  WHERE constraint_catalog = 'MigrationsTest'
                  AND constraint_type = 'FOREIGN KEY'
                 
                  OPEN constraint_cursor;
                  FETCH NEXT FROM constraint_cursor INTO @constraint_name, @table_schema, @table_name;
                  WHILE @@FETCH_STATUS = 0
                  BEGIN
                      SELECT @sql = 'ALTER TABLE [' + @table_schema + '].[' + @table_name + '] DROP CONSTRAINT [' + @constraint_name + ']';
                      EXEC sp_executesql @sql; 
                      FETCH NEXT FROM constraint_cursor INTO @constraint_name, @table_schema, @table_name;
                  END
                  CLOSE constraint_cursor;
                  DEALLOCATE constraint_cursor;

                  EXEC sp_MSforeachtable 'DROP TABLE ?';"
                );
        }

        public override void DropDatabase()
        {
            ExecuteNonQuery(
                @"ALTER DATABASE [" + _name
                + "] SET OFFLINE WITH ROLLBACK IMMEDIATE;ALTER DATABASE [" + _name
                + "] SET ONLINE;DROP DATABASE [" + _name + "]");
        }

        public override bool Exists()
        {
            return Database.Exists(ConnectionString);
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
