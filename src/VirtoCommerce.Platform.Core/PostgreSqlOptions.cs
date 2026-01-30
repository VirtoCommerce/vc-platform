using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Configuration options for PostgreSQL connections optimized for Azure PostgreSQL
/// </summary>
public class PostgreSqlOptions : IValidatableObject
{
    /// <summary>
    /// Command timeout in seconds for regular database operations (default: 60)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "CommandTimeout must be >= 0")]
    public int CommandTimeout { get; set; } = 60;

    /// <summary>
    /// Command timeout in seconds specifically for Hangfire background jobs (default: 300 = 5 minutes)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "HangfireCommandTimeout must be >= 0")]
    public int HangfireCommandTimeout { get; set; } = 300;

    /// <summary>
    /// Minimum number of connections maintained in the pool (default: 10)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "MinPoolSize must be >= 0")]
    public int MinPoolSize { get; set; } = 10;

    /// <summary>
    /// Maximum number of connections in the connection pool (default: 100)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "MaxPoolSize must be > 0")]
    public int MaxPoolSize { get; set; } = 100;

    /// <summary>
    /// Maximum pool size for Hangfire (allows more parallel jobs, default: 200)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "HangfireMaxPoolSize must be > 0")]
    public int HangfireMaxPoolSize { get; set; } = 200;

    /// <summary>
    /// Connection lifetime in seconds for recycling connections (important for Azure load balancing, default: 300 = 5 minutes)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "ConnectionLifetime must be >= 0")]
    public int ConnectionLifetime { get; set; } = 300;

    /// <summary>
    /// Keepalive interval in seconds to detect dead connections (default: 30)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "KeepAlive must be >= 0")]
    public int KeepAlive { get; set; } = 30;

    /// <summary>
    /// TCP keepalive interval in seconds (default: 10)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "TcpKeepAliveInterval must be >= 0")]
    public int TcpKeepAliveInterval { get; set; } = 10;

    /// <summary>
    /// TCP keepalive time in seconds (default: 30)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "TcpKeepAliveTime must be >= 0")]
    public int TcpKeepAliveTime { get; set; } = 30;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPoolSize >= MaxPoolSize)
        {
            yield return new ValidationResult(
                $"MinPoolSize ({MinPoolSize}) must be less than MaxPoolSize ({MaxPoolSize})",
                [nameof(MinPoolSize), nameof(MaxPoolSize)]);
        }

        if (MinPoolSize >= HangfireMaxPoolSize)
        {
            yield return new ValidationResult(
                $"MinPoolSize ({MinPoolSize}) must be less than HangfireMaxPoolSize ({HangfireMaxPoolSize})",
                [nameof(MinPoolSize), nameof(HangfireMaxPoolSize)]);
        }
    }
}
