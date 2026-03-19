using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VirtoCommerce.Platform.Data.SqlServer.HealthCheck;

// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.SqlServer/SqlServerHealthCheckOptions.cs
public class SqlServerHealthCheckOptions
{
    public string ConnectionString { get; set; } = null!;

    public string CommandText { get; set; } = SqlServerHealthCheckBuilderExtensions.HEALTH_QUERY;

    public Action<SqlConnection>? Configure { get; set; }

    public Func<object?, HealthCheckResult>? HealthCheckResultBuilder { get; set; }
}

