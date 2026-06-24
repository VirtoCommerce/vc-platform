using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace VirtoCommerce.Platform.Web.Infrastructure.HealthCheck;

/// <summary>
/// A health check for Redis services.
/// </summary>
public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _connection;

    public RedisHealthCheck(IEnumerable<IConnectionMultiplexer> connections)
    {
        if (connections == null)
        {
            throw new ArgumentNullException(nameof(connections));

        }
        _connection = connections.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (_connection == null)
        {
            return HealthCheckResult.Degraded("RedisConnectionString is not configured");
        }

        try
        {
            // PING through the multiplexer rather than pinging each node directly: the multiplexer is a
            // shared, self-healing singleton that routes to an available node and handles failover, so a
            // single degraded node in a cluster (where replicas take over and slots stay covered) does not
            // fail the check. PING is also not admin-gated, unlike the live CLUSTER commands that require
            // AllowAdmin since StackExchange.Redis 3.0.0.
            // WaitAsync honors the health-check timeout/cancellation (PingAsync itself takes no token).
            var latency = await _connection.GetDatabase().PingAsync().WaitAsync(cancellationToken);

            // IsConnected is surfaced for observability only; PING succeeding is the source of truth, since
            // IsConnected can be transiently false while the multiplexer reconnects in the background.
            var data = new Dictionary<string, object>
            {
                ["isConnected"] = _connection.IsConnected,
                ["latencyMs"] = latency.TotalMilliseconds,
            };

            return HealthCheckResult.Healthy(description: $"Redis responded to PING in {latency.TotalMilliseconds:F0} ms", data: data);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, description: "Redis did not respond to PING", exception: ex);
        }
    }

}
