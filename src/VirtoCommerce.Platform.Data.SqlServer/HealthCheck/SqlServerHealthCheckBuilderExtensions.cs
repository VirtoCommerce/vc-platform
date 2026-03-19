using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VirtoCommerce.Platform.Data.SqlServer.HealthCheck;

// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.SqlServer/DependencyInjection/SqlServerHealthCheckBuilderExtensions.cs
public static class SqlServerHealthCheckBuilderExtensions
{
    private const string NAME = "sqlserver";
    internal const string HEALTH_QUERY = "SELECT 1;";

    public static IHealthChecksBuilder AddSqlServer(
        this IHealthChecksBuilder builder,
        string connectionString,
        string healthQuery = HEALTH_QUERY,
        Action<SqlConnection>? configure = null,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.AddSqlServer(_ => connectionString, healthQuery, configure, name, failureStatus, tags, timeout);
    }

    public static IHealthChecksBuilder AddSqlServer(
        this IHealthChecksBuilder builder,
        Func<IServiceProvider, string> connectionStringFactory,
        string healthQuery = HEALTH_QUERY,
        Action<SqlConnection>? configure = null,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            sp =>
            {
                var options = new SqlServerHealthCheckOptions
                {
                    ConnectionString = connectionStringFactory(sp),
                    CommandText = healthQuery,
                    Configure = configure,
                };
                return new SqlServerHealthCheck(options);
            },
            failureStatus,
            tags,
            timeout));
    }

    public static IHealthChecksBuilder AddSqlServer(
        this IHealthChecksBuilder builder,
        SqlServerHealthCheckOptions options,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            _ => new SqlServerHealthCheck(options),
            failureStatus,
            tags,
            timeout));
    }
}
