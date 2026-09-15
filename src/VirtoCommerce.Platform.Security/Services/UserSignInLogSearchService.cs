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
                .OrderByDescending(x => x.CreatedDate)
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

        var query = BuildQuery(repository, criteria);
        var failed = query.Where(x => !x.Succeeded);

        return new UserSignInLogStats
        {
            TotalCount = await query.CountAsync(),
            FailedCount = await failed.CountAsync(),
            DistinctUserCount = await query.Where(x => x.UserId != null).Select(x => x.UserId).Distinct().CountAsync(),
            ImpersonationCount = await query.CountAsync(x => x.SignInType == SignInType.Impersonation),
            TopFailedIpAddresses = await TopAsync(failed.Where(x => x.IpAddress != null), x => x.IpAddress),
            TopFailedUserNames = await TopAsync(failed.Where(x => x.UserName != null), x => x.UserName),
            FailureReasonBreakdown = await TopAsync(failed.Where(x => x.FailureReason != null), x => x.FailureReason),
            SignInsByOrganization = await TopAsync(query.Where(x => x.OrganizationName != null), x => x.OrganizationName),
        };
    }

    protected virtual IQueryable<UserSignInLogEntity> BuildQuery(ISecurityRepository repository, UserSignInLogSearchCriteria criteria)
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

        if (criteria.StartDate != null)
        {
            query = query.Where(x => x.CreatedDate >= criteria.StartDate);
        }

        if (criteria.EndDate != null)
        {
            query = query.Where(x => x.CreatedDate <= criteria.EndDate);
        }

        if (!string.IsNullOrEmpty(criteria.Keyword))
        {
            query = query.Where(x => x.UserName.Contains(criteria.Keyword) || x.IpAddress.Contains(criteria.Keyword));
        }

        return query;
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
