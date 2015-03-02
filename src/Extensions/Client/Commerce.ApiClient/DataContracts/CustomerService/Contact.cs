using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.CustomerService
{
	public class Contact
	{
		//private IDictionary<string, object> _properties = new Dictionary<string, object>();

		public string Id { get; set; }

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		public string FullName { get; set; }
		public string TimeZone { get; set; }
		public string DefaultLanguage { get; set; }
		public DateTime? BirthDate { get; set; }
		public string TaxpayerId { get; set; }
		public string PreferredDelivery { get; set; }
		public string PreferredCommunication { get; set; }
		public string Salutation { get; set; }

        public ICollection<Address> Addresses { get; set; }

		public ContactProperty[] Properties { get; set; }

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
