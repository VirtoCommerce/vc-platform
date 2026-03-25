using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Npgsql;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Data.PostgreSql.Extensions;

/// <summary>
/// Helper class for enhancing PostgreSQL connection strings with optimal settings for Azure PostgreSQL
/// </summary>
public static class PostgreSqlOptionsExtensions
{
    private static readonly Dictionary<string, string> _builderPropertyDisplayNames = GetBuilderPropertyDisplayNames();

    private static Dictionary<string, string> GetBuilderPropertyDisplayNames()
    {
        var properties = typeof(NpgsqlConnectionStringBuilder).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var dictionary = new Dictionary<string, string>(properties.Length, StringComparer.OrdinalIgnoreCase);
        foreach (var property in properties)
        {
            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                dictionary[property.Name] = displayNameAttribute.DisplayName;
            }
        }

        return dictionary;
    }

    private static bool ContainsKey(HashSet<string> keys, string propertyName)
    {
        // Get the display name (e.g., "Command Timeout" for "CommandTimeout" property)
        // We must check the Keys collection to see what was actually provided in the connection string
        // Note: builder.ContainsKey() returns true for ALL valid properties, not just those set in the string
        return keys.Contains(_builderPropertyDisplayNames.GetValueOrDefault(propertyName, propertyName));
    }

    /// <summary>
    /// Enhances connection string with optimal settings for Azure PostgreSQL if not already present.
    /// </summary>
    /// <param name="options">PostgreSQL options</param>
    /// <param name="connectionString">Original connection string</param>
    /// <returns>Enhanced connection string</returns>
    [SuppressMessage("SonarAnalyzer.CSharp", "S1541:Methods and properties should not be too complex",
        Justification = "Multiple independent property checks; each is trivial and the method remains readable")]
    public static string EnhanceConnectionString(this PostgreSqlOptions options, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrEmpty(connectionString))
        {
            return connectionString ?? string.Empty;
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        // CRITICAL: builder.Keys allocates a new array every time it's accessed!
        // Cache it once as a HashSet for O(1) lookups and to avoid multiple allocations
        var keys = new HashSet<string>(builder.Keys, StringComparer.OrdinalIgnoreCase);

        // Set Command Timeout if not already set (in seconds)
        // This is different from Timeout (connection timeout)
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.CommandTimeout)))
        {
            builder.CommandTimeout = options.CommandTimeout;
        }

        // Ensure connection pooling is enabled (default is true, but make it explicit)
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.Pooling)))
        {
            builder.Pooling = true;
        }

        // Set optimal pool size for Azure PostgreSQL
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.MinPoolSize)))
        {
            builder.MinPoolSize = options.MinPoolSize;
        }

        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.MaxPoolSize)))
        {
            builder.MaxPoolSize = options.MaxPoolSize;
        }

        // Set connection lifetime to ensure connections are recycled
        // Important for Azure load-balanced scenarios
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.ConnectionLifetime)))
        {
            builder.ConnectionLifetime = options.ConnectionLifetime;
        }

        // Set Keepalive for Azure PostgreSQL to detect dead connections
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.KeepAlive)))
        {
            builder.KeepAlive = options.KeepAlive;
        }

        // Enable TCP Keepalive for better connection health detection
        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.TcpKeepAlive)))
        {
            builder.TcpKeepAlive = true;
        }

        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.TcpKeepAliveInterval)))
        {
            builder.TcpKeepAliveInterval = options.TcpKeepAliveInterval;
        }

        if (!ContainsKey(keys, nameof(NpgsqlConnectionStringBuilder.TcpKeepAliveTime)))
        {
            builder.TcpKeepAliveTime = options.TcpKeepAliveTime;
        }

        return builder.ConnectionString;
    }
}
