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
            // PING through the multiplexer rather than pinging each node directly: the multiplexer routes
            // to an available node and handles failover, so a single degraded node in a cluster (where
            // replicas take over and slots stay covered) does not fail the check. PING is also not
            // admin-gated, unlike the live CLUSTER commands that require AllowAdmin since SE.Redis 3.0.0.
            await _connection.GetDatabase().PingAsync();

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }

}
