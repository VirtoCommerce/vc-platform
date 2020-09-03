using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public class UserSearchResult : GenericSearchResult<ApplicationUser>
    {
        public IList<ApplicationUser> Users => Results;
    }
}
