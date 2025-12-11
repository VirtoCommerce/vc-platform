using Npgsql;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Hangfire.Extensions;

/// <summary>
/// Helper class for enhancing PostgreSQL connection strings with optimal settings for Azure PostgreSQL
/// </summary>
public static class PostgreSqlOptionsExtensions
{
    /// <summary>
    /// Enhances connection string with optimal settings for Azure PostgreSQL if not already present.
    /// </summary>
    /// <param name="options">PostgreSQL options</param>
    /// <param name="connectionString">Original connection string</param>
    /// <returns>Enhanced connection string</returns>
    public static string EnhanceConnectionString(this PostgreSqlOptions options, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return connectionString ?? string.Empty;
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        // Set Command Timeout if not already set (in seconds)
        // This is different from Timeout (connection timeout)
        if (builder.CommandTimeout == 0)
        {
            builder.CommandTimeout = options.HangfireCommandTimeout;
        }

        // Set Keepalive for Azure PostgreSQL to detect dead connections
        if (builder.KeepAlive == 0)
        {
            builder.KeepAlive = options.Keepalive;
        }

        // Ensure connection pooling is enabled (default is true, but make it explicit)
        if (!builder.Pooling)
        {
            builder.Pooling = true;
        }

        // Set optimal pool size for Azure PostgreSQL
        if (builder.MaxPoolSize == 100) // Default Npgsql value
        {
            builder.MaxPoolSize = options.HangfireMaxPoolSize;
        }

        if (builder.MinPoolSize == 0) // Default Npgsql value
        {
            builder.MinPoolSize = options.MinPoolSize;
        }

        // Set connection lifetime to ensure connections are recycled
        // Important for Azure load-balanced scenarios
        if (builder.ConnectionLifetime == 0)
        {
            builder.ConnectionLifetime = options.ConnectionLifetime;
        }

        // Enable TCP Keepalive for better connection health detection
        builder.TcpKeepAlive = true;
        builder.TcpKeepAliveInterval = options.TcpKeepAliveInterval;
        builder.TcpKeepAliveTime = options.TcpKeepAliveTime;

        return builder.ConnectionString;
    }
}

