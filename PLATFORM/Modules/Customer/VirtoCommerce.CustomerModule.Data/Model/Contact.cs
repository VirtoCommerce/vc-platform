using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class Contact : Member
    {
        public Contact()
        {
            BirthDate = DateTime.Now;
			ContactPropertyValues = new ObservableCollection<ContactPropertyValue>();
        }

        #region UserProfile members
        [StringLength(128)]
        [Required]
		public string FullName { get; set; }

        [StringLength(32)]
		public string TimeZone { get; set; }

        [StringLength(32)]
		public string DefaultLanguage { get; set; }

		public DateTime? BirthDate { get; set; }

        [StringLength(64)]
		public string TaxpayerId { get; set; }

        [StringLength(64)]
		public string PreferredDelivery { get; set; }

        [StringLength(64)]
		public string PreferredCommunication { get; set; }

		public byte[] Photo { get; set; }

        [StringLength(256)]
		public string Salutation { get; set; }

        #endregion

        #region NavigationProperties
		public ObservableCollection<ContactPropertyValue> ContactPropertyValues { get; set; }

        #endregion
    }
}
