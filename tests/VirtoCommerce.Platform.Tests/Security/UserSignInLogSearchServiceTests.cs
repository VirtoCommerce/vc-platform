using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogSearchServiceTests
{
    private static readonly DateTime _now = new(2026, 9, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task SearchAsync_FiltersByUserId()
    {
        var service = CreateService(
            Row("r1", userId: "user-1"),
            Row("r2", userId: "user-2"));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { UserId = "user-1", Take = 20 });

        result.TotalCount.Should().Be(1);
        result.Results.Should().ContainSingle().Which.Id.Should().Be("r1");
    }

    [Fact]
    public async Task SearchAsync_FiltersBySucceeded()
    {
        var service = CreateService(
            Row("ok", succeeded: true),
            Row("bad", succeeded: false));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Succeeded = false, Take = 20 });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("bad");
    }

    [Fact]
    public async Task SearchAsync_FiltersBySignInTypeAndDateRange()
    {
        var service = CreateService(
            Row("recent", signInType: SignInType.Impersonation, createdDate: _now.AddHours(-1)),
            Row("old", signInType: SignInType.Impersonation, createdDate: _now.AddDays(-10)),
            Row("other", signInType: SignInType.Password, createdDate: _now.AddHours(-1)));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria
        {
            SignInTypes = [SignInType.Impersonation],
            StartDate = _now.AddDays(-1),
            Take = 20,
        });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("recent");
    }

    [Fact]
    public async Task SearchAsync_DefaultsToNewestFirst()
    {
        var service = CreateService(
            Row("older", createdDate: _now.AddHours(-5)),
            Row("newest", createdDate: _now.AddHours(-1)));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Take = 20 });

        result.Results.First().Id.Should().Be("newest");
    }

    [Fact]
    public async Task SearchAsync_TakeZero_ReturnsCountWithoutRows()
    {
        var service = CreateService(Row("a"), Row("b"));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Take = 0 });

        result.TotalCount.Should().Be(2);
        result.Results.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_HonoursExplicitSortOverTheDefault()
    {
        var service = CreateService(
            Row("older", createdDate: _now.AddHours(-5)),
            Row("newest", createdDate: _now.AddHours(-1)));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria
        {
            Sort = "createdDate:asc",
            Take = 20,
        });

        result.Results.First().Id.Should().Be("older");
    }

    [Fact]
    public async Task SearchAsync_PagesWithSkipAndTake()
    {
        var service = CreateService(
            Row("r1", createdDate: _now.AddHours(-1)),
            Row("r2", createdDate: _now.AddHours(-2)),
            Row("r3", createdDate: _now.AddHours(-3)),
            Row("r4", createdDate: _now.AddHours(-4)),
            Row("r5", createdDate: _now.AddHours(-5)));

        var page2 = await service.SearchAsync(new UserSignInLogSearchCriteria { Skip = 2, Take = 2 });

        // Total is the whole filtered set, not the page.
        page2.TotalCount.Should().Be(5);
        page2.Results.Select(x => x.Id).Should().Equal("r3", "r4");
    }

    [Fact]
    public async Task GetStatsAsync_CountsTotalsAndTopFailedIps()
    {
        var service = CreateService(
            Row("a", succeeded: true, userId: "user-1", createdDate: _now.AddHours(-1)),
            Row("b", succeeded: false, ip: "203.0.113.7", createdDate: _now.AddHours(-1)),
            Row("c", succeeded: false, ip: "203.0.113.7", createdDate: _now.AddHours(-2)),
            Row("d", succeeded: false, ip: "198.51.100.1", createdDate: _now.AddHours(-3)),
            Row("e", succeeded: true, signInType: SignInType.Impersonation, createdDate: _now.AddHours(-1)));

        var stats = await service.GetStatsAsync(new UserSignInLogSearchCriteria { StartDate = _now.AddDays(-1) });

        stats.TotalCount.Should().Be(5);
        stats.FailedCount.Should().Be(3);
        stats.ImpersonationCount.Should().Be(1);
        stats.DistinctUserCount.Should().Be(1);

        var topIp = stats.TopFailedIpAddresses.First();
        topIp.Key.Should().Be("203.0.113.7");
        topIp.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetStatsAsync_BreaksDownFailureReasons()
    {
        var service = CreateService(
            Row("a", succeeded: false, failureReason: SignInFailureReason.InvalidPassword),
            Row("b", succeeded: false, failureReason: SignInFailureReason.InvalidPassword),
            Row("c", succeeded: false, failureReason: SignInFailureReason.LockedOut));

        var stats = await service.GetStatsAsync(new UserSignInLogSearchCriteria());

        stats.FailureReasonBreakdown.Should().HaveCount(2);
        stats.FailureReasonBreakdown.First().Key.Should().Be(SignInFailureReason.InvalidPassword);
        stats.FailureReasonBreakdown.First().Count.Should().Be(2);
    }

    private static UserSignInLogEntity Row(
        string id,
        string userId = null,
        bool succeeded = true,
        string signInType = SignInType.Password,
        string ip = null,
        string failureReason = null,
        DateTime? createdDate = null)
        => new()
        {
            Id = id,
            UserId = userId,
            UserName = userId ?? "anonymous",
            Succeeded = succeeded,
            SignInType = signInType,
            IpAddress = ip,
            FailureReason = failureReason,
            CreatedDate = createdDate ?? _now,
        };

    private static UserSignInLogSearchService CreateService(params UserSignInLogEntity[] rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>(rows).BuildMock());
        return new UserSignInLogSearchService(() => repository.Object);
    }
}
