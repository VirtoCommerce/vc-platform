using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class AuditableEntity : Entity, IAuditable
    {
        #region IAuditable Members

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        #endregion
    }
}
