using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserSearchResult
    {
        public UserSearchResult()
        {
            Users = new List<ApplicationUserExtended>();
        }
        public int TotalCount { get; set; }

        public List<ApplicationUserExtended> Users { get; set; }
    }
}