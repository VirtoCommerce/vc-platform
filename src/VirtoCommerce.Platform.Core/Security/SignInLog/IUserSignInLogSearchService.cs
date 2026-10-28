using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

public interface IUserSignInLogSearchService : ISearchService<UserSignInLogSearchCriteria, UserSignInLogSearchResult, UserSignInLog>
{
    Task<UserSignInLogStats> GetStats(UserSignInLogSearchCriteria criteria);
}
