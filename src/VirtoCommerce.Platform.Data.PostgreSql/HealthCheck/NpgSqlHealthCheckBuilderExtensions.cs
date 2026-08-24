using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace VirtoCommerce.Platform.Data.PostgreSql.HealthCheck;

/// <summary>
/// Extension methods to configure <see cref="NpgSqlHealthCheck"/>.
/// </summary>
public static class NpgSqlHealthCheckBuilderExtensions
{
    private const string NAME = "npgsql";
    internal const string HEALTH_QUERY = "SELECT 1;";
    internal const string VERSION_QUERY = "SHOW server_version_num;";

    /// <summary>
    /// Minimum supported PostgreSQL <c>server_version_num</c> (180000 = PostgreSQL 18.0).
    /// User and role search use <c>LIKE</c> over a case-insensitive (nondeterministic) collation,
    /// which is only supported by PostgreSQL 18 and later. Earlier versions reject it with
    /// SqlState 0A000 'nondeterministic collations are not supported for LIKE'.
    /// </summary>
    public const int MinSupportedServerVersionNum = 180000;

    /// <summary>
    /// Add a health check for Postgres databases.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="connectionString">The Postgres connection string to be used.</param>
    /// <param name="healthQuery">The query to be used in check.</param>
    /// <param name="configure">An optional action to allow additional Npgsql specific configuration.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'npgsql' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    public static IHealthChecksBuilder AddNpgSql(
        this IHealthChecksBuilder builder,
        string connectionString,
        string healthQuery = HEALTH_QUERY,
        Action<NpgsqlConnection>? configure = null,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.AddNpgSql(new NpgSqlHealthCheckOptions(connectionString)
        {
            CommandText = healthQuery,
            Configure = configure
        }, name, failureStatus, tags, timeout);
    }

    /// <summary>
    /// Add a health check for Postgres databases.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="connectionStringFactory">A factory to build the Postgres connection string to use.</param>
    /// <param name="healthQuery">The query to be used in check.</param>
    /// <param name="configure">An optional action to allow additional Npgsql specific configuration.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'npgsql' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    public static IHealthChecksBuilder AddNpgSql(
        this IHealthChecksBuilder builder,
        Func<IServiceProvider, string> connectionStringFactory,
        string healthQuery = HEALTH_QUERY,
        Action<NpgsqlConnection>? configure = null,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        // This instance is captured in lambda closure, so it can be reused (perf)
        NpgSqlHealthCheckOptions options = new()
        {
            CommandText = healthQuery,
            Configure = configure,
        };

        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            sp =>
            {
                options.ConnectionString ??= connectionStringFactory.Invoke(sp);

                return new NpgSqlHealthCheck(options);
            },
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Add a health check for Postgres databases.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="dbDataSourceFactory">
    /// An optional factory to obtain <see cref="NpgsqlDataSource" /> instance.
    /// When not provided, <see cref="NpgsqlDataSource" /> is simply resolved from <see cref="IServiceProvider"/>.
    /// </param>
    /// <param name="healthQuery">The query to be used in check.</param>
    /// <param name="configure">An optional action to allow additional Npgsql specific configuration.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'npgsql' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    /// <remarks>
    /// Depending on how the <see cref="NpgsqlDataSource" /> was configured, the connections it hands out may be pooled.
    /// That is why it should be the exact same <see cref="NpgsqlDataSource" /> that is used by other parts of your app.
    /// </remarks>
    public static IHealthChecksBuilder AddNpgSql(
        this IHealthChecksBuilder builder,
        Func<IServiceProvider, NpgsqlDataSource>? dbDataSourceFactory = null,
        string healthQuery = HEALTH_QUERY,
        Action<NpgsqlConnection>? configure = null,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        // This instance is captured in lambda closure, so it can be reused (perf)
        NpgSqlHealthCheckOptions options = new()
        {
            CommandText = healthQuery,
            Configure = configure,
        };

        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            sp =>
            {
                options.DataSource ??= dbDataSourceFactory?.Invoke(sp) ?? sp.GetRequiredService<NpgsqlDataSource>();

                return new NpgSqlHealthCheck(options);
            },
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Add a health check for Postgres databases.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="options">Options for health check.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'npgsql' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    public static IHealthChecksBuilder AddNpgSql(
        this IHealthChecksBuilder builder,
        NpgSqlHealthCheckOptions options,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {

        return builder.Add(new HealthCheckRegistration(
            name ?? NAME,
            _ => new NpgSqlHealthCheck(options),
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Add a health check for Postgres databases that also verifies the server version is supported.
    /// Reports unhealthy when the server is older than <paramref name="minServerVersionNum"/>
    /// (defaults to <see cref="MinSupportedServerVersionNum"/>).
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="connectionString">The Postgres connection string to be used.</param>
    /// <param name="minServerVersionNum">The minimum supported <c>server_version_num</c>. Optional.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'npgsql' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    public static IHealthChecksBuilder AddNpgSqlVersionCheck(
        this IHealthChecksBuilder builder,
        string connectionString,
        int minServerVersionNum = MinSupportedServerVersionNum,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.AddNpgSql(new NpgSqlHealthCheckOptions(connectionString)
        {
            // server_version_num is an integer, e.g. "170004" => 17.4, "180000" => 18.0
            CommandText = VERSION_QUERY,
            HealthCheckResultBuilder = result => BuildVersionHealthCheckResult(result, minServerVersionNum),
        }, name, failureStatus, tags, timeout);
    }

    private static HealthCheckResult BuildVersionHealthCheckResult(object? result, int minServerVersionNum)
    {
        var versionNum = int.TryParse(result?.ToString(), out var v) ? v : 0;

        return versionNum >= minServerVersionNum
            ? HealthCheckResult.Healthy($"PostgreSQL OK")
            : HealthCheckResult.Unhealthy($"PostgreSQL {versionNum} is not supported. Virto Commerce requires PostgreSQL 18+.");
    }
}
