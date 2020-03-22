using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services
{
    public class UserApiKeySearchService : IUserApiKeySearchService
    {
        private readonly Func<ISecurityRepository> _repositoryFactory;

        public UserApiKeySearchService(Func<ISecurityRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<UserApiKeySearchResult> SearchUserApiKeysAsync(UserApiKeySearchCriteria criteria)
        {
            using (var repository = _repositoryFactory())
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria));
                }

                var result = AbstractTypeFactory<UserApiKeySearchResult>.TryCreateInstance();

                var query = repository.UserApiKeys.AsNoTracking();
                result.TotalCount = await query.CountAsync();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<UserApiKey>(x => x.ApiKey), SortDirection = SortDirection.Ascending } };
                }
                var apiKeysEntities = await query.OrderBySortInfos(sortInfos).Skip(criteria.Skip).Take(criteria.Take).ToArrayAsync();
                result.Results = apiKeysEntities.Select(x => x.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance())).ToArray();

                return result;
            }
        }
    }
}
