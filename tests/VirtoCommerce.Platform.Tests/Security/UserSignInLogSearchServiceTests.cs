using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

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
    public async Task SearchAsync_Keyword_FindsRowsByTheOperatorTheGridShows()
    {
        var service = CreateService(
            Row("onbehalf", userId: "customer", signInType: SignInType.Impersonation, operatorUserName: "support@virto.com"),
            // No operator at all, which is the ordinary case and the one that used to throw.
            Row("unrelated", userId: "someone-else"));

        // The grid leads with the operator on these rows, so the name an administrator can see has to
        // be a name they can search for.
        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { Keyword = "support@", Take = 20 });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("onbehalf");
    }

    [Fact]
    public async Task SearchAsync_FiltersByStoreId()
    {
        var service = CreateService(
            Row("b2b", storeId: "B2B-store"),
            Row("none"));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { StoreId = "B2B-store", Take = 20 });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("b2b");
    }

    [Fact]
    public async Task SearchAsync_WithoutStore_FindsTheRowsNoStoreIdCanReach()
    {
        var service = CreateService(
            Row("b2b", storeId: "B2B-store"),
            Row("backoffice"));

        // An empty StoreId already means "any store", so back-office sign-ins and failed attempts
        // against unknown user names are otherwise unfindable.
        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { WithoutStore = true, Take = 20 });

        result.Results.Should().ContainSingle().Which.Id.Should().Be("backoffice");
    }

    [Fact]
    public async Task SearchAsync_WithoutStoreFalse_FindsOnlyRowsThatBelongToAStore()
    {
        var service = CreateService(
            Row("b2b", storeId: "B2B-store"),
            Row("electronics", storeId: "Electronics"),
            Row("backoffice"));

        // The mirror of WithoutStore = true, and the reason the flag is tri-state rather than a bool:
        // "storefront traffic only" would otherwise mean naming every store id in the criteria.
        var result = await service.SearchAsync(new UserSignInLogSearchCriteria { WithoutStore = false, Take = 20 });

        result.TotalCount.Should().Be(2);
        result.Results.Select(x => x.Id).Should().BeEquivalentTo("b2b", "electronics");
    }

    [Fact]
    public async Task SearchAsync_WithoutStore_OverridesAStoreIdSetAlongsideIt()
    {
        var service = CreateService(
            Row("b2b", storeId: "B2B-store"),
            Row("backoffice"));

        var result = await service.SearchAsync(new UserSignInLogSearchCriteria
        {
            WithoutStore = true,
            StoreId = "B2B-store",
            Take = 20,
        });

        // The two are mutually exclusive by definition; combining them must not return nothing.
        result.Results.Should().ContainSingle().Which.Id.Should().Be("backoffice");
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

        var stats = await service.GetStats(new UserSignInLogSearchCriteria { StartDate = _now.AddDays(-1) });

        stats.TotalCount.Should().Be(5);
        stats.FailedCount.Should().Be(3);
        stats.ImpersonationCount.Should().Be(1);
        stats.DistinctUserCount.Should().Be(1);

        var topIp = stats.TopFailedIpAddresses.First();
        topIp.Key.Should().Be("203.0.113.7");
        topIp.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetStatsAsync_DistinctUsersCountsSuccessfulSignInsOnlyAndDeduplicates()
    {
        var service = CreateService(
            Row("a", succeeded: true, userId: "user-1"),
            Row("b", succeeded: true, userId: "user-1"),
            Row("c", succeeded: true, userId: "user-2"),
            // Failed attempts say nothing about who used the store.
            Row("d", succeeded: false, userId: "user-3"),
            // An unknown user name has no account to count.
            Row("e", succeeded: false, userId: null));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria());

        stats.DistinctUserCount.Should().Be(2);
    }

    [Fact]
    public async Task GetStatsAsync_ReportsWhenRecordingIsTurnedOff()
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>().BuildMock());

        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetObjectSettingAsync(
                PlatformConstants.Settings.Security.SignInLogEnabled.Name, null, null))
            .ReturnsAsync(new ObjectSettingEntry { Value = false });

        var service = new UserSignInLogSearchService(() => repository.Object, settings.Object);

        var stats = await service.GetStats(new UserSignInLogSearchCriteria());

        // Zeros alone cannot tell "nothing happened" from "nothing is being recorded".
        stats.RecordingEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task GetStatsAsync_BreaksDownFailureReasons()
    {
        var service = CreateService(
            Row("a", succeeded: false, failureReason: SignInFailureReason.InvalidPassword),
            Row("b", succeeded: false, failureReason: SignInFailureReason.InvalidPassword),
            Row("c", succeeded: false, failureReason: SignInFailureReason.LockedOut));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria());

        stats.FailureReasonBreakdown.Should().HaveCount(2);
        stats.FailureReasonBreakdown.First().Key.Should().Be(SignInFailureReason.InvalidPassword);
        stats.FailureReasonBreakdown.First().Count.Should().Be(2);
    }

    [Fact]
    public async Task GetStatsAsync_CountsTheSameLengthWindowImmediatelyBefore()
    {
        var service = CreateService(
            // Current window: the last 24 hours before _now.
            Row("cur1", succeeded: true, createdDate: _now.AddHours(-1)),
            Row("cur2", succeeded: false, createdDate: _now.AddHours(-2)),
            // Previous window: the 24 hours before that.
            Row("prev1", succeeded: true, createdDate: _now.AddHours(-26)),
            Row("prev2", succeeded: false, createdDate: _now.AddHours(-30)),
            Row("prev3", succeeded: false, createdDate: _now.AddHours(-40)),
            // Older than both windows - must not be counted.
            Row("ancient", succeeded: false, createdDate: _now.AddDays(-9)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            StartDate = _now.AddHours(-24),
            EndDate = _now,
        });

        stats.TotalCount.Should().Be(2);
        stats.FailedCount.Should().Be(1);
        stats.PreviousTotalCount.Should().Be(3);
        stats.PreviousFailedCount.Should().Be(2);
    }

    [Fact]
    public async Task GetStatsAsync_NoStartDate_LeavesPreviousPeriodUnset()
    {
        var service = CreateService(Row("a"), Row("b"));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria());

        // "All time" has nothing before it.
        stats.PreviousTotalCount.Should().BeNull();
        stats.PreviousFailedCount.Should().BeNull();
    }

    [Fact]
    public async Task GetStatsAsync_PreviousPeriodKeepsTheNonDateFilters()
    {
        var service = CreateService(
            Row("cur", succeeded: false, userId: "user-1", createdDate: _now.AddHours(-1)),
            Row("prev-same-user", succeeded: false, userId: "user-1", createdDate: _now.AddHours(-30)),
            Row("prev-other-user", succeeded: false, userId: "user-2", createdDate: _now.AddHours(-30)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            UserId = "user-1",
            StartDate = _now.AddHours(-24),
            EndDate = _now,
        });

        stats.TotalCount.Should().Be(1);
        // The other user's row sits in the previous window but must stay filtered out.
        stats.PreviousTotalCount.Should().Be(1);
    }

    [Theory]
    [InlineData(30, TimelineGranularity.Minute)]      // 30 minutes
    [InlineData(60, TimelineGranularity.Minute)]      // 1 hour
    [InlineData(360, TimelineGranularity.TenMinutes)] // 6 hours
    [InlineData(1440, TimelineGranularity.Hour)]      // 24 hours
    [InlineData(43200, TimelineGranularity.Day)]      // 30 days
    public async Task GetStatsAsync_PicksBucketSizeFromTheWindow(int windowMinutes, string expected)
    {
        var service = CreateService(Row("a", createdDate: _now.AddMinutes(-1)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            StartDate = _now.AddMinutes(-windowMinutes),
            EndDate = _now,
        });

        stats.TimelineGranularity.Should().Be(expected);
    }

    [Fact]
    public async Task GetStatsAsync_TimelineSplitsSuccessesFromFailuresPerBucket()
    {
        var at = new DateTime(2026, 9, 15, 11, 0, 0, DateTimeKind.Utc);

        var service = CreateService(
            Row("s1", succeeded: true, createdDate: at.AddMinutes(1)),
            Row("s2", succeeded: true, createdDate: at.AddMinutes(1)),
            Row("f1", succeeded: false, createdDate: at.AddMinutes(1)),
            Row("f2", succeeded: false, createdDate: at.AddMinutes(3)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            StartDate = at,
            EndDate = at.AddMinutes(5),
        });

        var busy = stats.Timeline.Single(x => x.Timestamp == at.AddMinutes(1));
        busy.SucceededCount.Should().Be(2);
        busy.FailedCount.Should().Be(1);

        stats.Timeline.Single(x => x.Timestamp == at.AddMinutes(3)).FailedCount.Should().Be(1);
    }

    [Fact]
    public async Task GetStatsAsync_TimelineGapFillsQuietBuckets()
    {
        var at = new DateTime(2026, 9, 15, 11, 0, 0, DateTimeKind.Utc);

        var service = CreateService(Row("only", succeeded: true, createdDate: at.AddMinutes(2)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            StartDate = at,
            EndDate = at.AddMinutes(4),
        });

        // A quiet stretch must read as zeroes, not as a hole the chart closes up.
        stats.Timeline.Should().HaveCount(5);
        stats.Timeline.Select(x => x.SucceededCount).Should().Equal(0, 0, 1, 0, 0);
    }

    [Fact]
    public async Task GetStatsAsync_NoStartDate_ProducesNoTimeline()
    {
        var service = CreateService(Row("a"), Row("b"));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria());

        stats.Timeline.Should().BeEmpty();
        stats.TimelineGranularity.Should().BeNull();
    }

    [Fact]
    public async Task GetStatsAsync_TimelineKeepsTheNonDateFilters()
    {
        var at = new DateTime(2026, 9, 15, 11, 0, 0, DateTimeKind.Utc);

        var service = CreateService(
            Row("mine", succeeded: true, userId: "user-1", createdDate: at.AddMinutes(1)),
            Row("theirs", succeeded: true, userId: "user-2", createdDate: at.AddMinutes(1)));

        var stats = await service.GetStats(new UserSignInLogSearchCriteria
        {
            UserId = "user-1",
            StartDate = at,
            EndDate = at.AddMinutes(3),
        });

        stats.Timeline.Sum(x => x.SucceededCount).Should().Be(1);
    }

    private static UserSignInLogEntity Row(
        string id,
        string userId = null,
        bool succeeded = true,
        string signInType = SignInType.Password,
        string ip = null,
        string failureReason = null,
        string storeId = null,
        string operatorUserName = null,
        DateTime? createdDate = null)
        => new()
        {
            Id = id,
            UserId = userId,
            UserName = userId ?? "anonymous",
            Succeeded = succeeded,
            SignInType = signInType,
            StoreId = storeId,
            OperatorUserName = operatorUserName,
            IpAddress = ip,
            FailureReason = failureReason,
            CreatedDate = createdDate ?? _now,
        };

    private static UserSignInLogSearchService CreateService(params UserSignInLogEntity[] rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>(rows).BuildMock());

        // GetValueAsync is an extension method; GetObjectSettingAsync is the mockable seam beneath it.
        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetObjectSettingAsync(
                PlatformConstants.Settings.Security.SignInLogEnabled.Name, null, null))
            .ReturnsAsync(new ObjectSettingEntry { Value = true });

        return new UserSignInLogSearchService(() => repository.Object, settings.Object);
    }
}
