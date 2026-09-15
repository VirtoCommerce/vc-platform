using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

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

        var service = new UserSignInLogService(() => repository.Object);

        await service.SaveChangesAsync(
        [
            new UserSignInLog { UserName = "a", SignInType = SignInType.Password, Succeeded = true },
            new UserSignInLog { UserName = "b", SignInType = SignInType.Password, Succeeded = false },
        ]);

        added.Should().HaveCount(2);
        added.Select(x => x.UserName).Should().BeEquivalentTo("a", "b");
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task SaveChangesAsync_EmptyBatch_DoesNotTouchTheRepository()
    {
        var repository = new Mock<ISecurityRepository>();
        var service = new UserSignInLogService(() => repository.Object);

        await service.SaveChangesAsync([]);

        repository.Verify(x => x.Add(It.IsAny<UserSignInLogEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteOlderThanAsync_RemovesOnlyRowsBeforeTheCutoff()
    {
        var cutoff = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var removed = new List<UserSignInLogEntity>();
        var rows = new List<UserSignInLogEntity>
        {
            new() { Id = "old", CreatedDate = cutoff.AddDays(-1) },
            new() { Id = "new", CreatedDate = cutoff.AddDays(1) },
        };

        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(rows.BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
        repository.Setup(x => x.Remove(It.IsAny<UserSignInLogEntity>()))
            .Callback<UserSignInLogEntity>(removed.Add);

        var service = new UserSignInLogService(() => repository.Object);

        var deleted = await service.DeleteOlderThanAsync(cutoff, batchSize: 100);

        deleted.Should().Be(1);
        removed.Should().ContainSingle().Which.Id.Should().Be("old");
    }

    [Fact]
    public async Task DeleteOlderThanAsync_NothingExpired_DoesNotCommit()
    {
        var cutoff = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>
        {
            new() { Id = "new", CreatedDate = cutoff.AddDays(1) },
        }.BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);

        var service = new UserSignInLogService(() => repository.Object);

        var deleted = await service.DeleteOlderThanAsync(cutoff, batchSize: 100);

        deleted.Should().Be(0);
        unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
    }
}
