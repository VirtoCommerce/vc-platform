using System;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUserLogin : ICloneable
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
