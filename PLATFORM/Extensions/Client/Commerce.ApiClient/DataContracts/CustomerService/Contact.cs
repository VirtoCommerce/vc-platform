using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.CustomerService
{
    public class Contact
    {
        //private IDictionary<string, object> _properties = new Dictionary<string, object>();

        #region Public Properties

        public ICollection<Address> Addresses { get; set; }
        public ICollection<string> Emails { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DefaultLanguage { get; set; }
        public string FullName { get; set; }
        public string Id { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PreferredCommunication { get; set; }
        public string PreferredDelivery { get; set; }

        public ContactProperty[] Properties { get; set; }
        public string Salutation { get; set; }
        public string TaxpayerId { get; set; }
        public string TimeZone { get; set; }

        #endregion

        //public IDictionary<string, object> Properties
        //{
        //	get
        //	{
        //		return _properties;
        //	}
        //	set
        //	{
        //		_properties = value;
        //	}
        //}
    }
}
