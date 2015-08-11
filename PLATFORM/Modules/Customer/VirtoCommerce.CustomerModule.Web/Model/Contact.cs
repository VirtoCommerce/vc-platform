using System;
using System.Collections.Generic;

namespace VirtoCommerce.CustomerModule.Web.Model
{
    public class Contact : Member
    {
        public Contact()
            : base("Contact")
        {
        }

        public override string DisplayName
        {
            get { return FullName; }
        }

        public string FullName { get; set; }
        public string TimeZone { get; set; }

        /// <summary>
        /// Language Culture Name (en-US,fr-FR  etc)
        /// </summary>
        public string DefaultLanguage { get; set; }
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Not documented
        /// </summary>
        public string TaxpayerId { get; set; }
        public string PreferredDelivery { get; set; }
        public string PreferredCommunication { get; set; }
        public string Salutation { get; set; }

        /// <summary>
        /// Not documented
        /// </summary>
        public ICollection<string> Organizations { get; set; }

	
	}
}
