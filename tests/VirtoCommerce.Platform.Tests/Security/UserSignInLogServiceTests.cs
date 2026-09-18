using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogServiceTests
{
    [Fact]
    public async Task SaveChangesAsync_AddsOneEntityPerRecordAndCommitsOnce()
    {
        var added = new List<UserSignInLogEntity>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>().BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        repository.Setup(x => x.Add(It.IsAny<UserSignInLogEntity>()))
            .Callback<UserSignInLogEntity>(added.Add);

        var service = new UserSignInLogService(ScopedServiceFactoryStub.Of(repository.Object));

        await service.SaveChanges(
        [
            new UserSignInLog { UserName = "a", SignInType = SignInType.Password, Succeeded = true },
            new UserSignInLog { UserName = "b", SignInType = SignInType.Password, Succeeded = false },
        ], TestContext.Current.CancellationToken);

        added.Should().HaveCount(2);
        added.Select(x => x.UserName).Should().BeEquivalentTo("a", "b");
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task SaveChangesAsync_EmptyBatch_DoesNotTouchTheRepository()
    {
        var repository = new Mock<ISecurityRepository>();
        var service = new UserSignInLogService(ScopedServiceFactoryStub.Of(repository.Object));

        await service.SaveChanges([], TestContext.Current.CancellationToken);

        repository.Verify(x => x.Add(It.IsAny<UserSignInLogEntity>()), Times.Never);
    }

    [Fact]
    public async Task BuildExpiredQuery_SelectsOnlyRowsBeforeTheCutoff()
    {
        var cutoff = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<UserSignInLogEntity>
        {
            new() { Id = "old", CreatedDate = cutoff.AddDays(-1) },
            new() { Id = "new", CreatedDate = cutoff.AddDays(1) },
        };

        var service = new TestableUserSignInLogService(CreateRepository(rows));

        var selected = await service.SelectExpired(cutoff, batchSize: 100).ToListAsync(TestContext.Current.CancellationToken);

        selected.Should().ContainSingle().Which.Id.Should().Be("old");
    }

    [Fact]
    public async Task BuildExpiredQuery_TakesOldestFirstUpToTheBatchSize()
    {
        var cutoff = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<UserSignInLogEntity>
        {
            new() { Id = "middle", CreatedDate = cutoff.AddDays(-2) },
            new() { Id = "oldest", CreatedDate = cutoff.AddDays(-3) },
            new() { Id = "newest", CreatedDate = cutoff.AddDays(-1) },
        };

        var service = new TestableUserSignInLogService(CreateRepository(rows));

        var selected = await service.SelectExpired(cutoff, batchSize: 2).ToListAsync(TestContext.Current.CancellationToken);

        // Oldest first, so repeated batches walk forward through the backlog.
        selected.Select(x => x.Id).Should().Equal("oldest", "middle");
    }

    private static ISecurityRepository CreateRepository(List<UserSignInLogEntity> rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(rows.BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
        return repository.Object;
    }

    private sealed class TestableUserSignInLogService : UserSignInLogService
    {
        private readonly ISecurityRepository _repository;

        public TestableUserSignInLogService(ISecurityRepository repository)
            : base(ScopedServiceFactoryStub.Of(repository))
        {
            _repository = repository;
        }

        public IQueryable<UserSignInLogEntity> SelectExpired(DateTime cutoff, int batchSize)
            => BuildExpiredQuery(_repository, cutoff, batchSize);
    }
}
