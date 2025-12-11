using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Configuration options for PostgreSQL connections optimized for Azure PostgreSQL
/// </summary>
public class PostgreSqlOptions
{
    /// <summary>
    /// Command timeout in seconds for regular database operations (default: 60)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int CommandTimeout { get; set; } = 60;

    /// <summary>
    /// Command timeout in seconds specifically for Hangfire background jobs (default: 300 = 5 minutes)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int HangfireCommandTimeout { get; set; } = 300;

    /// <summary>
    /// Maximum number of connections in the connection pool (default: 100)
    /// </summary>
    [Range(1, 10000)]
    public int MaxPoolSize { get; set; } = 100;

    /// <summary>
    /// Minimum number of connections maintained in the pool (default: 10)
    /// </summary>
    [Range(0, 10000)]
    public int MinPoolSize { get; set; } = 10;

    /// <summary>
    /// Maximum pool size for Hangfire (allows more parallel jobs, default: 200)
    /// </summary>
    [Range(1, 10000)]
    public int HangfireMaxPoolSize { get; set; } = 200;

    /// <summary>
    /// Connection lifetime in seconds for recycling connections (important for Azure load balancing, default: 300 = 5 minutes)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int ConnectionLifetime { get; set; } = 300;

    /// <summary>
    /// Keepalive interval in seconds to detect dead connections (default: 30)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Keepalive { get; set; } = 30;

    /// <summary>
    /// TCP keepalive interval in seconds (default: 10)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int TcpKeepAliveInterval { get; set; } = 10;

    /// <summary>
    /// TCP keepalive time in seconds (default: 30)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int TcpKeepAliveTime { get; set; } = 30;

    /// <summary>
    /// Enable automatic retry on transient failures (Azure-specific, default: true)
    /// </summary>
    public bool EnableRetryOnFailure { get; set; } = true;

    /// <summary>
    /// Maximum number of retry attempts for transient failures (default: 3)
    /// </summary>
    [Range(0, 10)]
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Maximum delay in seconds between retry attempts (default: 5)
    /// </summary>
    [Range(0, 60)]
    public int MaxRetryDelaySeconds { get; set; } = 5;
}
