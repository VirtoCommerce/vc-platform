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

        [Obsolete("Use SearchAsync()", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public Task<UserApiKeySearchResult> SearchUserApiKeysAsync(UserApiKeySearchCriteria criteria)
        {
            return SearchAsync(criteria, clone: true);
        }

        public async Task<UserApiKeySearchResult> SearchAsync(UserApiKeySearchCriteria criteria, bool clone = true)
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
