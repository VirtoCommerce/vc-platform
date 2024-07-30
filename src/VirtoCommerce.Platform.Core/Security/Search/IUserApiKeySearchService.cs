using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public interface IUserApiKeySearchService : ISearchService<UserApiKeySearchCriteria, UserApiKeySearchResult, UserApiKey>
    {
    }
}
