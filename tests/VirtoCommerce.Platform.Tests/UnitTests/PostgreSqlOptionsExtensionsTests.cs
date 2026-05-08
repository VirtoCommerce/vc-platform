using Npgsql;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Hangfire.Extensions;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests;

public class PostgreSqlOptionsExtensionsTests
{
    private readonly PostgreSqlOptions _defaultOptions = new()
    {
        HangfireCommandTimeout = 120,
        MinPoolSize = 5,
        HangfireMaxPoolSize = 50,
        ConnectionLifetime = 300,
        KeepAlive = 60,
        TcpKeepAliveInterval = 10,
        TcpKeepAliveTime = 30,
    };

    [Fact]
    public void EnhanceConnectionString_ShouldAddMissingParameters()
    {
        // Arrange
        const string connectionString = "DATABASE=test;USER ID=postgres;PASSWORD=postgres;HOST=localhost;PORT=5432";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert
        Assert.Equal(120, builder.CommandTimeout);
        Assert.Equal(5, builder.MinPoolSize);
        Assert.Equal(50, builder.MaxPoolSize);
        Assert.Equal(300, builder.ConnectionLifetime);
        Assert.Equal(60, builder.KeepAlive);
        Assert.True(builder.TcpKeepAlive);
        Assert.Equal(10, builder.TcpKeepAliveInterval);
        Assert.Equal(30, builder.TcpKeepAliveTime);
    }

    [Fact]
    public void EnhanceConnectionString_ShouldPreserveExistingCommandTimeout()
    {
        // Arrange
        const string connectionString = "DATABASE=test;HOST=localhost;COMMANDTIMEOUT=60";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert - should keep original value (60), not use options value (120)
        Assert.Equal(60, builder.CommandTimeout);
    }

    [Theory]
    [InlineData("Command Timeout=45")]
    [InlineData("CommandTimeout=45")]
    [InlineData("COMMANDTIMEOUT=45")]
    public void EnhanceConnectionString_ShouldPreserveCommandTimeoutWithDifferentCasing(string timeoutParam)
    {
        // Arrange
        var connectionString = $"DATABASE=test;HOST=localhost;{timeoutParam}";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert
        Assert.Equal(45, builder.CommandTimeout);
    }

    [Theory]
    [InlineData("MinPoolSize=10;MaxPoolSize=200")]
    [InlineData("Minimum Pool Size=10;Maximum Pool Size=200")]
    [InlineData("MINPOOLSIZE=10;MAXPOOLSIZE=200")]
    public void EnhanceConnectionString_ShouldPreservePoolSizeWithDifferentCasing(string poolParams)
    {
        // Arrange
        var connectionString = $"DATABASE=test;HOST=localhost;{poolParams}";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert
        Assert.Equal(10, builder.MinPoolSize);
        Assert.Equal(200, builder.MaxPoolSize);
    }

    [Fact]
    public void EnhanceConnectionString_ShouldPreserveAllExistingParameters()
    {
        // Arrange
        const string connectionString = "DATABASE=test;USER ID=postgres;PASSWORD=postgres;HOST=localhost;PORT=5432;" +
                                        "TIMEOUT=30;POOLING=True;MINPOOLSIZE=2;MAXPOOLSIZE=100;COMMANDTIMEOUT=60;" +
                                        "Connection Lifetime=600;Keepalive=120";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert - all values from connection string should be preserved
        Assert.Equal(60, builder.CommandTimeout);
        Assert.Equal(2, builder.MinPoolSize);
        Assert.Equal(100, builder.MaxPoolSize);
        Assert.Equal(600, builder.ConnectionLifetime);
        Assert.Equal(120, builder.KeepAlive);
        Assert.True(builder.Pooling);
    }

    [Fact]
    public void EnhanceConnectionString_ShouldHandleNullConnectionString()
    {
        // Act
        var result = _defaultOptions.EnhanceConnectionString(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EnhanceConnectionString_ShouldHandleEmptyConnectionString()
    {
        // Act
        var result = _defaultOptions.EnhanceConnectionString(string.Empty);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EnhanceConnectionString_ShouldPreserveDefaultValueIfExplicitlySet()
    {
        // Arrange - CommandTimeout default is 30, but we explicitly set it to 30
        const string connectionString = "DATABASE=test;HOST=localhost;CommandTimeout=30";

        // Act
        var result = _defaultOptions.EnhanceConnectionString(connectionString);
        var builder = new NpgsqlConnectionStringBuilder(result);

        // Assert - should keep 30 from connection string, not 120 from options
        Assert.Equal(30, builder.CommandTimeout);
    }
}
