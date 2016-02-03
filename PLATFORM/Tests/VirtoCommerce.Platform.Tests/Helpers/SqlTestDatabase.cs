using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Tests.Helpers
{
    public class SqlTestDatabase : TestDatabase
    {
        private readonly string _name;

        private string ConnectionStringFormat
            = "Server=(local);Database={0};Trusted_Connection=True";
        //= "Data Source=(local);Initial Catalog={0};Integrated Security=True;Pooling=false;";


        public SqlTestDatabase(string name)
        {
            var file = @";AttachDBFilename=|DataDirectory|\{0}.mdf";
            _name = name;

            var setting = ConfigurationManager.ConnectionStrings["VirtoCommerce_MigrationTestsBase"];

            if (setting != null)
            {
                ConnectionStringFormat = setting.ConnectionString;
            }

            ConnectionString = string.Format(ConnectionStringFormat, name) + String.Format(file, name);
            ProviderName = "System.Data.SqlClient";
            //Info = CreateInfoContext(new SqlConnection(ConnectionString));
        }

        #region Overrides of TestDatabase

        public override InfoContext Info
        {
            get
            {
                if (base.Info == null)
                {
                    base.Info = CreateInfoContext(new SqlConnection(ConnectionString));
                }

                return base.Info;
            }
        }

        #endregion

        /*
        public override void EnsureDatabase()
        {
            //Database.DefaultConnectionFactory.CreateConnection()
            Database.Delete(ConnectionString);

            var sql
                = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'" + _name + "') "
                  + "CREATE DATABASE [" + _name + "]";

            if (ConnectionStringFormat.Contains("(LocalDb)"))
            {
                ExecuteNonQuery(sql, string.Format(ConnectionStringFormat, _name));
            }
            else
            {
                ExecuteNonQuery(sql, string.Format(ConnectionStringFormat, "master"));
            }

            ResetDatabase();
        }
         * */

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
            return Exists(string.Format(ConnectionStringFormat, "master"), _name);
            /*
            var sql
                = "SELECT count(name) FROM sys.databases WHERE name = N'" + _name + "'";

            var count = ExecuteScalar<int>(sql, string.Format(ConnectionStringFormat, "master"));

            return count > 0;
            */
        }

        private static bool Exists(string connectionString, string databaseName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(string.Format(
                       "SELECT db_id('{0}')", databaseName), connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
