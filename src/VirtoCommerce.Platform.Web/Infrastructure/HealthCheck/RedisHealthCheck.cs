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
            foreach (var endPoint in _connection.GetEndPoints(configuredOnly: true))
            {
                var server = _connection.GetServer(endPoint);

                if (server.ServerType != ServerType.Cluster)
                {
                    await _connection.GetDatabase().PingAsync().ConfigureAwait(false);
                    await server.PingAsync().ConfigureAwait(false);
                }
                else
                {
                    var clusterInfo = await server.ExecuteAsync("CLUSTER", "INFO").ConfigureAwait(false);

                    if (clusterInfo is object && !clusterInfo.IsNull)
                    {
                        if (!clusterInfo.ToString()!.Contains("cluster_state:ok"))
                        {
                            //cluster info is not ok!
                            return new HealthCheckResult(context.Registration.FailureStatus, description: $"INFO CLUSTER is not on OK state for endpoint {endPoint}");
                        }
                    }
                    else
                    {
                        //cluster info cannot be read for this cluster node
                        return new HealthCheckResult(context.Registration.FailureStatus, description: $"INFO CLUSTER is null or can't be read for endpoint {endPoint}");
                    }
                }
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }

}
