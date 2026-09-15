using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.Search;

public interface IUserSignInLogSearchService : ISearchService<UserSignInLogSearchCriteria, UserSignInLogSearchResult, UserSignInLog>
{
    Task<UserSignInLogStats> GetStatsAsync(UserSignInLogSearchCriteria criteria);
}
