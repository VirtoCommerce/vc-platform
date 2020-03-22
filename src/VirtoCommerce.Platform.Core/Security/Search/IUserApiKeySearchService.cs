using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public interface IUserApiKeySearchService
    {
        Task<UserApiKeySearchResult> SearchUserApiKeysAsync(UserApiKeySearchCriteria criteria);
    }
}
