using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class UserDetail : Entity
    {
        public IList<string> Permissions { get; set; } = new List<string>();
        public string UserName { get; set; }
        public bool IsAdministrator { get; set; }
        public bool PasswordExpired { get; set; }
        public int DaysTillPasswordExpiry { get; set; }
        public string AuthenticationMethod { get; set; }
        [Obsolete("Use IsPasswordSignIn instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public bool IsSsoAuthenticationMethod => !IsPasswordSignIn;
        public bool IsPasswordSignIn { get; set; }
        public string MemberId { get; set; }
        public bool CanAccessAdminUI { get; set; }
    }
}
