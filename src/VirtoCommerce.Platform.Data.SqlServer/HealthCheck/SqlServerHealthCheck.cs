using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VirtoCommerce.Platform.Data.SqlServer.HealthCheck;

// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.SqlServer/SqlServerHealthCheck.cs
public class SqlServerHealthCheck : IHealthCheck
{
    private readonly SqlServerHealthCheckOptions _options;

    public SqlServerHealthCheck(SqlServerHealthCheckOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = new SqlConnection(_options.ConnectionString);

            _options.Configure?.Invoke(connection);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            await using var command = connection.CreateCommand();
            command.CommandText = _options.CommandText;
            var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

            return _options.HealthCheckResultBuilder?.Invoke(result) ?? HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
