using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
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

    [Fact]
    public void Dispose_ResolvedInOwnScope_DisposesScopeWithRepository()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDbContext<TestDbContext>(options => options.UseSqlServer(ConnectionString));
        services.AddScoped<DisposableSpy>();
        services.AddTransient<SpyingRepository>();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });

        var repository = provider.ResolveInOwnScope<SpyingRepository>();
        var spy = repository.Spy;
        spy.IsDisposed.Should().BeFalse();

        // Act — the second call must be a no-op: disposing the owned scope disposes the repository again.
        repository.Dispose();
        repository.Dispose();

        // Assert
        spy.IsDisposed.Should().BeTrue();
        repository.DbContext.Should().BeNull();
    }

    public class TestDbContext : DbContext
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

    public class DisposableSpy : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public class SpyingRepository : DbContextRepositoryBase<TestDbContext>
    {
        public SpyingRepository(TestDbContext dbContext, DisposableSpy spy)
            : base(dbContext)
        {
            Spy = spy;
        }

        public DisposableSpy Spy { get; }
    }
}
