using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class ChangeLogSearchService : IChangeLogSearchService
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public ChangeLogSearchService(Func<IPlatformRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
        }

        public virtual async Task<ChangeLogSearchResult> SearchAsync(ChangeLogSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<ChangeLogSearchResult>.TryCreateInstance();

            using (var repository = _repositoryFactory())
            {
                repository.DisableChangesTracking();

                var sortInfos = GetSortInfos(criteria);
                var query = GetQuery(repository, criteria, sortInfos);

                result.TotalCount = await query.CountAsync();
                if (criteria.Take > 0)
                {
                    result.Results = (await query.Skip(criteria.Skip).Take(criteria.Take).ToArrayAsync()).Select(x => x.ToModel(AbstractTypeFactory<OperationLog>.TryCreateInstance())).ToList();
                }
            }
            return result;
        }

        protected virtual IQueryable<OperationLogEntity> GetQuery(IPlatformRepository repository, ChangeLogSearchCriteria criteria, IEnumerable<SortInfo> sortInfos)
        {
            var query = repository.OperationLogs.Where(x => (criteria.StartDate == null || x.ModifiedDate >= criteria.StartDate) && (criteria.EndDate == null || x.ModifiedDate <= criteria.EndDate));
            if (!criteria.ObjectIds.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ObjectIds.Contains(x.ObjectId));
            }
            if (!criteria.ObjectTypes.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ObjectTypes.Contains(x.ObjectType));
            }

            return query.OrderBySortInfos(sortInfos);
        }

        protected virtual IList<SortInfo> GetSortInfos(ChangeLogSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[]
                {
                    new SortInfo { SortColumn = nameof(OperationLog.ModifiedDate), SortDirection = SortDirection.Descending }
                };
            }
            return sortInfos;
        }
    }
}
