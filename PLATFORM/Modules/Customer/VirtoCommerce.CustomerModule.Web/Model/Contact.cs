using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class Contact : Member
	{
		public Contact()
			:base("Contact")
		{
		}
		public override string DisplayName
		{
			get { return FullName; }
		}
		public string FullName { get; set; }
		public string TimeZone { get; set; }
		public string DefaultLanguage { get; set; }
		public DateTime? BirthDate { get; set; }
		public string TaxpayerId { get; set; }
		public string PreferredDelivery { get; set; }
		public string PreferredCommunication { get; set; }
		public string Salutation { get; set; }

		public ICollection<string> Organizations { get; set; }
		public ICollection<Property> Properties { get; set; }
	
		

	}
}
