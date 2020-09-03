using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.Search
{
    public class RoleSearchResult : GenericSearchResult<Role>
    {
        public IList<Role> Roles => Results;
    }
}
