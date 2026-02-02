using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DbContextCommandExtensions
    {
        public static Task<int> ExecuteNonQueryAsync(this DbContext context, string rawSql, params object[] parameters)
        {
            return ExecuteCommandAsync(context, rawSql, parameters, command => command.ExecuteNonQueryAsync());
        }

        public static Task<T> ExecuteScalarAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            return ExecuteCommandAsync(context, rawSql, parameters, async command => (T)await command.ExecuteScalarAsync());
        }

        public static Task<T[]> ExecuteArrayAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            return ExecuteCommandAsync(context, rawSql, parameters, async command =>
            {
                var result = new List<T>();
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(await reader.GetFieldValueAsync<T>(0));
                }

                return result.ToArray();
            });
        }

        private static async Task<T> ExecuteCommandAsync<T>(DbContext context, string rawSql, object[] parameters, Func<DbCommand, Task<T>> executeFunc)
        {
            var conn = context.Database.GetDbConnection();
            await using var command = conn.CreateCommand();

            command.CommandText = rawSql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }

            if (context.Database.CurrentTransaction != null)
            {
                command.Transaction = context.Database.CurrentTransaction.GetDbTransaction();
            }

            var wasOpen = conn.State == ConnectionState.Open;
            if (!wasOpen)
            {
                await conn.OpenAsync();
            }

            try
            {
                return await executeFunc(command);
            }
            finally
            {
                if (!wasOpen)
                {
                    await conn.CloseAsync();
                }
            }
        }
    }
}
