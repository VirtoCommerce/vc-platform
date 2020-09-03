using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class LoginRequest : ValueObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
