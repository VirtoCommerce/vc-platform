using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class DbContextCommandExtensions
    {
        public static async Task<int> ExecuteNonQueryAsync(this DbContext context, string rawSql, params object[] parameters)
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
                return await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (!wasOpen)
                {
                    await conn.CloseAsync();
                }
            }
        }

        public static async Task<T> ExecuteScalarAsync<T>(this DbContext context, string rawSql, params object[] parameters)
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
                return (T)await command.ExecuteScalarAsync();
            }
            finally
            {
                if (!wasOpen)
                {
                    await conn.CloseAsync();
                }
            }
        }

        public static async Task<T[]> ExecuteArrayAsync<T>(this DbContext context, string rawSql, params object[] parameters)
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
                var result = new List<T>();
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(await reader.GetFieldValueAsync<T>(0));
                }

                return [.. result];

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
