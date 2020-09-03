using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public interface IRoleSearchService
    {
        Task<RoleSearchResult> SearchRolesAsync(RoleSearchCriteria criteria);
    }
}
