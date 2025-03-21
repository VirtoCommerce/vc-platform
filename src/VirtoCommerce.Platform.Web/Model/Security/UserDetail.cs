using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserDetail : Entity
    {
        public IList<string> Permissions { get; set; } = new List<string>();
        public string UserName { get; set; }
        public bool isAdministrator { get; set; }
        public bool PasswordExpired { get; set; }
        public int DaysTillPasswordExpiry { get; set; }
        public string AuthenticationMethod { get; set; }
        public bool IsSsoAuthenticationMethod { get; set; }
        public string MemberId { get; set; }
    }
}
