using System;
using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Contact : Member, IHasSecurityAccounts
    {
        public Contact()
        {
            SecurityAccounts = new List<ApplicationUserExtended>();
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }
        public DateTime? BirthDate { get; set; }
        public string TaxpayerId { get; set; }
        public string PreferredDelivery { get; set; }
        public string PreferredCommunication { get; set; }
        public string Salutation { get; set; }
        public ICollection<string> Organizations { get; set; }

        #region IHasSecurityAccounts Members
        /// <summary>
        /// All security accounts associated with this contact
        /// </summary>
        public ICollection<ApplicationUserExtended> SecurityAccounts { get; private set; } 
        #endregion
    }
}
