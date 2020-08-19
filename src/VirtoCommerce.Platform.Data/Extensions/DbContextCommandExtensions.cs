using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DbContextCommandExtensions
    {
        public static async Task<int> ExecuteNonQueryAsync(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                await conn.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<T> ExecuteScalarAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                await conn.OpenAsync();
                return (T)await command.ExecuteScalarAsync();
            }
        }

        public static T ExecuteScalar<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                conn.Open();
                return (T)command.ExecuteScalar();
            }
        }

        public static async Task<T[]> ExecuteArrayAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    foreach (var p in parameters)
                        command.Parameters.Add(p);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                var result = new List<T>();
                while (await reader.ReadAsync())
                {
                    result.Add(reader.GetFieldValue<T>(0));
                }
                reader.Close();

                return result.ToArray();
            }
        }

        public static bool ExistTable(this DbContext context, string tableName)
        {
            var countTables = ExecuteScalar<int>(context, "SELECT COUNT(OBJECT_ID(@table, 'U'))", new SqlParameter($"@table", tableName));
            return countTables > 0;
        }

        public static bool ExistTableAndThrow(this DbContext context, string tableName)
        {
            if (ExistTable(context, tableName))
            {
                return true;
            }
            else
            {
                throw new PlatformException($"There is no {tableName} table");
            }
        }
    }
}
