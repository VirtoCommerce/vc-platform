using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CoreModule.Web.Model
{
	public class FulfillmentCenter : Entity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int MaxReleasesPerPickBatch { get; set; }
		public int PickDelay { get; set; }
		public string DaytimePhoneNumber { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		public string StateProvince { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string PostalCode { get; set; }
	}
}
