using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public interface IUserApiKeySearchService : ISearchService<UserApiKeySearchCriteria, UserApiKeySearchResult, UserApiKey>
    {
        [Obsolete("Use SearchAsync()", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        Task<UserApiKeySearchResult> SearchUserApiKeysAsync(UserApiKeySearchCriteria criteria);
    }
}
