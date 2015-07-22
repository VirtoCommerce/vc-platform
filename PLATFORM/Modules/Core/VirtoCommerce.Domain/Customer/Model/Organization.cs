using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Organization : AuditableEntity, IHasDynamicProperties
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessCategory { get; set; }
        public string OwnerId { get; set; }
        public string ParentId { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<string> Phones { get; set; }
        public ICollection<string> Emails { get; set; }
        public ICollection<Note> Notes { get; set; }

        #region IHasDynamicProperties Members
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

        #endregion
    }
}
