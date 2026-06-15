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
                    await _connection.GetDatabase().PingAsync();
                    await server.PingAsync();
                }
                else
                {
                    // Use the typed CLUSTER NODES API instead of a raw "CLUSTER INFO" command.
                    // Since StackExchange.Redis 3.0.0 the raw server.Execute("CLUSTER", ...) path is
                    // classified as an admin command and throws unless AllowAdmin=true is set on the
                    // connection. ClusterNodesAsync() reads the same topology without requiring admin mode.
                    var clusterConfig = await server.ClusterNodesAsync();

                    if (clusterConfig is null || clusterConfig.Nodes.Count == 0)
                    {
                        //cluster info cannot be read for this cluster node
                        return new HealthCheckResult(context.Registration.FailureStatus, description: $"CLUSTER NODES is null or can't be read for endpoint {endPoint}");
                    }

                    // Only slot-owning masters affect keyspace availability (cluster_state:ok == all slots served).
                    // A failed replica or a demoted old master owns no slots and must not flip the cluster to Unhealthy.
                    var unhealthyNode = clusterConfig.Nodes.FirstOrDefault(node =>
                        node.Slots.Count > 0 && (node.IsFail || node.IsNoAddr || !node.IsConnected));
                    if (unhealthyNode != null)
                    {
                        //a slot-owning node is unreachable, so part of the keyspace is not served
                        return new HealthCheckResult(context.Registration.FailureStatus, description: $"CLUSTER is not in an OK state for endpoint {endPoint}: slot-owning node {unhealthyNode.NodeId} ({unhealthyNode.EndPoint}) is unreachable or failed");
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
