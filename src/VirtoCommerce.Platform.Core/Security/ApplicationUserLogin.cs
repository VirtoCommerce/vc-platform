using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUserLogin : ValueObject
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
    }
}
