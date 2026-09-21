using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.SignInLog;

public class UserSignInLogSearchService : IUserSignInLogSearchService
{
    private const int TopN = 10;

    // A start date is client-supplied and unvalidated. Without a cap, a request starting in 1990
    // would gap-fill ~13,000 buckets and serialise every one of them.
    private const int MaxTimelinePoints = 500;

    /// <summary>
    /// What the "On behalf" tile counts: sessions actually opened on behalf of a customer. A revert
    /// ends a session rather than opening one, and a denied attempt opens nothing, so counting either
    /// would put the tile out of step with the list it drills into. Declared once so the period and
    /// its comparison window cannot drift apart.
    /// </summary>
    private static readonly Expression<Func<UserSignInLogEntity, bool>> ImpersonationSession =
        x => x.SignInType == SignInType.Impersonation && x.Succeeded;

    private readonly IScopedServiceFactory<ISecurityRepository> _repositoryFactory;
    private readonly ISettingsManager _settingsManager;

    [Obsolete("Use the constructor that takes IScopedServiceFactory<T> instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public UserSignInLogSearchService(Func<ISecurityRepository> repositoryFactory, ISettingsManager settingsManager)
        : this(new DelegateScopedServiceFactory<ISecurityRepository>(repositoryFactory), settingsManager)
    {
    }

    [ActivatorUtilitiesConstructor]
    public UserSignInLogSearchService(IScopedServiceFactory<ISecurityRepository> repositoryFactory, ISettingsManager settingsManager)
    {
        _repositoryFactory = repositoryFactory;
        _settingsManager = settingsManager;
    }

    public virtual async Task<UserSignInLogSearchResult> SearchAsync(UserSignInLogSearchCriteria criteria, bool clone = true)
    {
        var result = AbstractTypeFactory<UserSignInLogSearchResult>.TryCreateInstance();

        using var scopedRepository = _repositoryFactory.Create();
        var repository = scopedRepository.Service;

        var query = BuildQuery(repository, criteria);

        result.TotalCount = await query.CountAsync();

        if (criteria.Take > 0)
        {
            var entities = await query
                .OrderBySortInfos(GetSortInfos(criteria))
                .Skip(criteria.Skip)
                .Take(criteria.Take)
                .ToListAsync();

            result.Results = entities
                .Select(x => x.ToModel(AbstractTypeFactory<UserSignInLog>.TryCreateInstance()))
                .ToList();
        }

        return result;
    }

    public virtual async Task<UserSignInLogStats> GetStats(UserSignInLogSearchCriteria criteria)
    {
        using var scopedRepository = _repositoryFactory.Create();
        var repository = scopedRepository.Service;

        var unwindowed = BuildFilterQuery(repository, criteria);
        var query = ApplyWindow(unwindowed, criteria.StartDate, criteria.EndDate);
        var failed = query.Where(x => !x.Succeeded);

        var result = new UserSignInLogStats
        {
            RecordingEnabled = await _settingsManager.GetValueAsync<bool>(
                PlatformConstants.Settings.Security.SignInLogEnabled),
            TotalCount = await query.CountAsync(),
            FailedCount = await failed.CountAsync(),
            DistinctUserCount = await DistinctSignedInUsers(query),
            ImpersonationCount = await query.CountAsync(ImpersonationSession),
            TopFailedIpAddresses = await Top(failed.Where(x => x.IpAddress != null), x => x.IpAddress),
            TopFailedUserNames = await Top(failed.Where(x => x.UserName != null), x => x.UserName),
            FailureReasonBreakdown = await Top(failed.Where(x => x.FailureReason != null), x => x.FailureReason),
            SignInsByOrganization = await Top(query.Where(x => x.OrganizationName != null), x => x.OrganizationName),
            SignInsByStore = await Top(query.Where(x => x.StoreId != null), x => x.StoreId),
        };

        await AddPreviousPeriod(result, unwindowed, criteria);
        await AddTimeline(result, query, criteria);

        return result;
    }

    /// <summary>
    /// Buckets the window so the chart can show when things happened, not just how many. Skipped
    /// without a start date: "all time" has no bounded window to divide.
    /// </summary>
    protected virtual async Task AddTimeline(
        UserSignInLogStats result,
        IQueryable<UserSignInLogEntity> query,
        UserSignInLogSearchCriteria criteria)
    {
        if (criteria.StartDate == null)
        {
            return;
        }

        var start = criteria.StartDate.Value;
        var end = criteria.EndDate ?? DateTime.UtcNow;
        var duration = end - start;

        if (duration <= TimeSpan.Zero)
        {
            return;
        }

        var granularity = PickGranularity(duration);

        result.TimelineGranularity = granularity;
        result.Timeline = await BuildTimeline(query, start, end, granularity);
    }

    /// <summary>
    /// Bucket size that keeps the point count readable at blade width for every offered period:
    /// 30m and 1h by minute, 6h in ten-minute steps, a day by hour, anything longer by day.
    /// </summary>
    protected static string PickGranularity(TimeSpan duration)
    {
        if (duration <= TimeSpan.FromHours(1))
        {
            return TimelineGranularity.Minute;
        }

        if (duration <= TimeSpan.FromHours(12))
        {
            return TimelineGranularity.TenMinutes;
        }

        if (duration <= TimeSpan.FromDays(3))
        {
            return TimelineGranularity.Hour;
        }

        return TimelineGranularity.Day;
    }

    protected virtual async Task<IList<UserSignInLogTimelinePoint>> BuildTimeline(
        IQueryable<UserSignInLogEntity> query,
        DateTime start,
        DateTime end,
        string granularity)
    {
        // Grouping happens in SQL on date parts, which every supported provider translates.
        // Only the grouped rows come back, never the underlying records.
        var buckets = granularity switch
        {
            TimelineGranularity.Minute => await query
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month, x.CreatedDate.Day, x.CreatedDate.Hour, x.CreatedDate.Minute, x.Succeeded })
                .Select(g => new BucketRow(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.Minute, g.Key.Succeeded, g.Count()))
                .ToListAsync(),

            TimelineGranularity.TenMinutes => await query
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month, x.CreatedDate.Day, x.CreatedDate.Hour, Slot = x.CreatedDate.Minute / 10, x.Succeeded })
                .Select(g => new BucketRow(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.Slot * 10, g.Key.Succeeded, g.Count()))
                .ToListAsync(),

            TimelineGranularity.Hour => await query
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month, x.CreatedDate.Day, x.CreatedDate.Hour, x.Succeeded })
                .Select(g => new BucketRow(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, g.Key.Succeeded, g.Count()))
                .ToListAsync(),

            _ => await query
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month, x.CreatedDate.Day, x.Succeeded })
                .Select(g => new BucketRow(g.Key.Year, g.Key.Month, g.Key.Day, 0, 0, g.Key.Succeeded, g.Count()))
                .ToListAsync(),
        };

        var step = BucketSize(granularity);
        var counted = buckets.ToLookup(x => new DateTime(x.Year, x.Month, x.Day, x.Hour, x.Minute, 0, DateTimeKind.Utc));

        var points = new List<UserSignInLogTimelinePoint>();

        // Gap-fill so a quiet stretch reads as zeroes instead of the chart closing the hole up.
        for (var cursor = Truncate(start, granularity);
             cursor <= end && points.Count < MaxTimelinePoints;
             cursor = cursor.Add(step))
        {
            var rows = counted[cursor];

            points.Add(new UserSignInLogTimelinePoint
            {
                Timestamp = cursor,
                SucceededCount = rows.Where(x => x.Succeeded).Sum(x => x.Count),
                FailedCount = rows.Where(x => !x.Succeeded).Sum(x => x.Count),
            });
        }

        return points;
    }

    protected static TimeSpan BucketSize(string granularity) => granularity switch
    {
        TimelineGranularity.Minute => TimeSpan.FromMinutes(1),
        TimelineGranularity.TenMinutes => TimeSpan.FromMinutes(10),
        TimelineGranularity.Hour => TimeSpan.FromHours(1),
        _ => TimeSpan.FromDays(1),
    };

    protected static DateTime Truncate(DateTime value, string granularity) => granularity switch
    {
        TimelineGranularity.Minute => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0, DateTimeKind.Utc),
        TimelineGranularity.TenMinutes => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute / 10 * 10, 0, DateTimeKind.Utc),
        TimelineGranularity.Hour => new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0, DateTimeKind.Utc),
        _ => new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, DateTimeKind.Utc),
    };

    private sealed record BucketRow(int Year, int Month, int Day, int Hour, int Minute, bool Succeeded, int Count);

    /// <summary>
    /// Counts the same-length window immediately before the requested one. Skipped entirely when the
    /// criteria carry no start date, because "all time" has nothing before it.
    /// </summary>
    protected virtual async Task AddPreviousPeriod(
        UserSignInLogStats result,
        IQueryable<UserSignInLogEntity> unwindowed,
        UserSignInLogSearchCriteria criteria)
    {
        if (criteria.StartDate == null)
        {
            return;
        }

        var start = criteria.StartDate.Value;
        var duration = (criteria.EndDate ?? DateTime.UtcNow) - start;

        if (duration <= TimeSpan.Zero)
        {
            return;
        }

        var previous = ApplyWindow(unwindowed, start - duration, start);

        result.PreviousTotalCount = await previous.CountAsync();
        result.PreviousFailedCount = await previous.CountAsync(x => !x.Succeeded);
        result.PreviousDistinctUserCount = await DistinctSignedInUsers(previous);
        result.PreviousImpersonationCount = await previous.CountAsync(ImpersonationSession);
    }

    protected static IQueryable<UserSignInLogEntity> ApplyWindow(
        IQueryable<UserSignInLogEntity> query,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (startDate != null)
        {
            query = query.Where(x => x.CreatedDate >= startDate);
        }

        if (endDate != null)
        {
            query = query.Where(x => x.CreatedDate <= endDate);
        }

        return query;
    }

    /// <summary>
    /// Newest first unless the caller asks otherwise - an audit log is read from the most recent entry back.
    /// </summary>
    protected virtual IList<SortInfo> GetSortInfos(UserSignInLogSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo { SortColumn = nameof(UserSignInLog.CreatedDate), SortDirection = SortDirection.Descending }
            ];
        }

        return sortInfos;
    }

    protected virtual IQueryable<UserSignInLogEntity> BuildQuery(ISecurityRepository repository, UserSignInLogSearchCriteria criteria)
    {
        return ApplyWindow(BuildFilterQuery(repository, criteria), criteria.StartDate, criteria.EndDate);
    }

    /// <summary>Every predicate except the date window, which callers apply themselves.</summary>
    protected virtual IQueryable<UserSignInLogEntity> BuildFilterQuery(ISecurityRepository repository, UserSignInLogSearchCriteria criteria)
    {
        var query = repository.UserSignInLogs;

        if (!string.IsNullOrEmpty(criteria.UserId))
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (!string.IsNullOrEmpty(criteria.UserName))
        {
            query = query.Where(x => x.UserName == criteria.UserName);
        }

        if (criteria.Succeeded != null)
        {
            query = query.Where(x => x.Succeeded == criteria.Succeeded);
        }

        if (!criteria.SignInTypes.IsNullOrEmpty())
        {
            query = query.Where(x => criteria.SignInTypes.Contains(x.SignInType));
        }

        if (!criteria.FailureReasons.IsNullOrEmpty())
        {
            query = query.Where(x => criteria.FailureReasons.Contains(x.FailureReason));
        }

        if (!string.IsNullOrEmpty(criteria.IpAddress))
        {
            query = query.Where(x => x.IpAddress == criteria.IpAddress);
        }

        if (criteria.WithoutStore != null)
        {
            query = criteria.WithoutStore == true
                ? query.Where(x => x.StoreId == null)
                : query.Where(x => x.StoreId != null);
        }
        else if (!string.IsNullOrEmpty(criteria.StoreId))
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (!string.IsNullOrEmpty(criteria.OrganizationId))
        {
            query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
        }

        if (!string.IsNullOrEmpty(criteria.Keyword))
        {
            query = query.Where(x => x.UserName != null && x.UserName.Contains(criteria.Keyword) ||
                                     x.OperatorUserName != null && x.OperatorUserName.Contains(criteria.Keyword) ||
                                     x.IpAddress != null && x.IpAddress.Contains(criteria.Keyword));
        }

        return query;
    }

    /// <summary>
    /// Distinct accounts that actually signed in — successful attempts only, and only where the
    /// account exists. A failed attempt says nothing about who was using the store, and an unknown
    /// user name has no account to count.
    /// </summary>
    private static Task<int> DistinctSignedInUsers(IQueryable<UserSignInLogEntity> query)
    {
        return query
            .Where(x => x.Succeeded && x.UserId != null)
            .Select(x => x.UserId)
            .Distinct()
            .CountAsync();
    }

    private static async Task<IList<UserSignInLogStatsEntry>> Top(
        IQueryable<UserSignInLogEntity> query,
        Expression<Func<UserSignInLogEntity, string>> selector)
    {
        var grouped = await query
            .GroupBy(selector)
            .Select(g => new UserSignInLogStatsEntry { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(TopN)
            .ToListAsync();

        return grouped;
    }
}
