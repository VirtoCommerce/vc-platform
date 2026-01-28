namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Configuration options for PostgreSQL connections optimized for Azure PostgreSQL
/// </summary>
public class PostgreSqlOptions
{
    /// <summary>
    /// Command timeout in seconds for regular database operations (default: 60)
    /// </summary>
    public int CommandTimeout { get; set; } = 60;

    /// <summary>
    /// Command timeout in seconds specifically for Hangfire background jobs (default: 300 = 5 minutes)
    /// </summary>
    public int HangfireCommandTimeout { get; set; } = 300;

    /// <summary>
    /// Minimum number of connections maintained in the pool (default: 10)
    /// </summary>
    public int MinPoolSize { get; set; } = 10;

    /// <summary>
    /// Maximum number of connections in the connection pool (default: 100)
    /// </summary>
    public int MaxPoolSize { get; set; } = 100;

    /// <summary>
    /// Maximum pool size for Hangfire (allows more parallel jobs, default: 200)
    /// </summary>
    public int HangfireMaxPoolSize { get; set; } = 200;

    /// <summary>
    /// Connection lifetime in seconds for recycling connections (important for Azure load balancing, default: 300 = 5 minutes)
    /// </summary>
    public int ConnectionLifetime { get; set; } = 300;

    /// <summary>
    /// Keepalive interval in seconds to detect dead connections (default: 30)
    /// </summary>
    public int KeepAlive { get; set; } = 30;

    /// <summary>
    /// TCP keepalive interval in seconds (default: 10)
    /// </summary>
    public int TcpKeepAliveInterval { get; set; } = 10;

    /// <summary>
    /// TCP keepalive time in seconds (default: 30)
    /// </summary>
    public int TcpKeepAliveTime { get; set; } = 30;
}
