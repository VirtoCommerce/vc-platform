using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Assets
{

    public abstract class BlobEntry : AuditableEntity, ICloneable
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string RelativeUrl { get; set; }

        #region ICloneable members

        public virtual object Clone()
        {
            return MemberwiseClone() as BlobEntry;
        }

        #endregion
    }
}
