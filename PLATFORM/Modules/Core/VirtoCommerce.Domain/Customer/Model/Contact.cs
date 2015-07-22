using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Contact : AuditableEntity, IHasDynamicProperties
    {

        public string FullName { get; set; }
        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }
        public DateTime? BirthDate { get; set; }
        public string TaxpayerId { get; set; }
        public string PreferredDelivery { get; set; }
        public string PreferredCommunication { get; set; }
        public string Salutation { get; set; }
        public ICollection<string> Organizations { get; set; }
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
