using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class CurrentUser : ICurrentUser
    {
        public string UserName { get; set; }
    }
}
