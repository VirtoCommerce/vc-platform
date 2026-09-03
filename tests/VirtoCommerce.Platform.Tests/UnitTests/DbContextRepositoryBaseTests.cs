using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests;

public class DbContextRepositoryBaseTests
{
    // No connection is opened by any of these tests: the command timeout lives on the facade.
    private const string ConnectionString = "Server=localhost;Database=Test;Connect Timeout=300;";

    [Fact]
    public void Constructor_CommandTimeoutConfigured_KeepsConfiguredValue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(ConnectionString, sqlServer => sqlServer.CommandTimeout(42))
            .Options;

        // Act
        using var repository = new TestRepository(new TestDbContext(options));

        // Assert
        repository.DbContext.Database.GetCommandTimeout().Should().Be(42);
    }

    [Fact]
    public void Constructor_NoCommandTimeoutConfigured_PropagatesConnectionTimeout()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        // Act
        using var repository = new TestRepository(new TestDbContext(options));

        // Assert
        repository.DbContext.Database.GetCommandTimeout().Should().Be(300);
    }

    [Fact]
    public void Constructor_InfiniteCommandTimeoutConfigured_KeepsIt()
    {
        // Arrange — EF Core encodes an infinite command timeout as 0, not null, so widening
        // the constructor's guard to "null or 0" would silently cap it.
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(ConnectionString, sqlServer => sqlServer.CommandTimeout(0))
            .Options;

        // Act
        using var repository = new TestRepository(new TestDbContext(options));

        // Assert
        repository.DbContext.Database.GetCommandTimeout().Should().Be(0);
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }
    }

    private class TestRepository : DbContextRepositoryBase<TestDbContext>
    {
        public TestRepository(TestDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
