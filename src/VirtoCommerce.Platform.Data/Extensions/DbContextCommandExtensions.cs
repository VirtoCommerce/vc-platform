using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }
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
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }
                return (T)await command.ExecuteScalarAsync();
            }
        }

        public static async Task<T[]> ExecuteArrayAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }

                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                var result = new List<T>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(await reader.GetFieldValueAsync<T>(0));
                    }
                }

                return result.ToArray();
            }
        }
    }
}
