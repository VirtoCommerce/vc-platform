using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Security.Search;

public interface IUserSessionsSearchService : ISearchService<UserSessionSearchCriteria, UserSessionSearchResult, UserSession>
{
}

