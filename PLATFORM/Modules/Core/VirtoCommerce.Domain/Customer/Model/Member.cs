using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Customer.Model
{
    public abstract class Member : AuditableEntity, IHasDynamicProperties
    {
        public Member()
        {
            MemberType = this.GetType().Name;
        }
        public string Name { get; set; }
        public string MemberType { get; set; }
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
