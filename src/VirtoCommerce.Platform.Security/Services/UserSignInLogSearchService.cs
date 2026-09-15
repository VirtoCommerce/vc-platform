using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services;

public class UserSignInLogSearchService : IUserSignInLogSearchService
{
    private const int TopN = 10;

    private readonly Func<ISecurityRepository> _repositoryFactory;

    public UserSignInLogSearchService(Func<ISecurityRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public virtual async Task<UserSignInLogSearchResult> SearchAsync(UserSignInLogSearchCriteria criteria, bool clone = true)
    {
        var result = AbstractTypeFactory<UserSignInLogSearchResult>.TryCreateInstance();

        using var repository = _repositoryFactory();

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

    public virtual async Task<UserSignInLogStats> GetStatsAsync(UserSignInLogSearchCriteria criteria)
    {
        using var repository = _repositoryFactory();

        var unwindowed = BuildFilterQuery(repository, criteria);
        var query = ApplyWindow(unwindowed, criteria.StartDate, criteria.EndDate);
        var failed = query.Where(x => !x.Succeeded);

        var result = new UserSignInLogStats
        {
            TotalCount = await query.CountAsync(),
            FailedCount = await failed.CountAsync(),
            DistinctUserCount = await DistinctSignedInUsersAsync(query),
            ImpersonationCount = await query.CountAsync(x => x.SignInType == SignInType.Impersonation),
            TopFailedIpAddresses = await TopAsync(failed.Where(x => x.IpAddress != null), x => x.IpAddress),
            TopFailedUserNames = await TopAsync(failed.Where(x => x.UserName != null), x => x.UserName),
            FailureReasonBreakdown = await TopAsync(failed.Where(x => x.FailureReason != null), x => x.FailureReason),
            SignInsByOrganization = await TopAsync(query.Where(x => x.OrganizationName != null), x => x.OrganizationName),
        };

        await AddPreviousPeriodAsync(result, unwindowed, criteria);

        return result;
    }

    /// <summary>
    /// Counts the same-length window immediately before the requested one. Skipped entirely when the
    /// criteria carry no start date, because "all time" has nothing before it.
    /// </summary>
    protected virtual async Task AddPreviousPeriodAsync(
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
        result.PreviousDistinctUserCount = await DistinctSignedInUsersAsync(previous);
        result.PreviousImpersonationCount = await previous.CountAsync(x => x.SignInType == SignInType.Impersonation);
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

        if (!string.IsNullOrEmpty(criteria.StoreId))
        {
            query = query.Where(x => x.StoreId == criteria.StoreId);
        }

        if (!string.IsNullOrEmpty(criteria.OrganizationId))
        {
            query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
        }

        if (!string.IsNullOrEmpty(criteria.Keyword))
        {
            query = query.Where(x => x.UserName.Contains(criteria.Keyword) || x.IpAddress.Contains(criteria.Keyword));
        }

        return query;
    }

    /// <summary>
    /// Distinct accounts that actually signed in — successful attempts only, and only where the
    /// account exists. A failed attempt says nothing about who was using the store, and an unknown
    /// user name has no account to count.
    /// </summary>
    private static Task<int> DistinctSignedInUsersAsync(IQueryable<UserSignInLogEntity> query)
    {
        return query
            .Where(x => x.Succeeded && x.UserId != null)
            .Select(x => x.UserId)
            .Distinct()
            .CountAsync();
    }

    private static async Task<IList<UserSignInLogStatsEntry>> TopAsync(
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
