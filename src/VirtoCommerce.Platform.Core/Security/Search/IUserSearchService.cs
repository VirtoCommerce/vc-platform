using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public interface IUserSearchService
    {
        Task<UserSearchResult> SearchUsersAsync(UserSearchCriteria criteria);

    }
}
