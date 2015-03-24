using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Address : ValueObject<Address>
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public AddressType Type { get; set; }
		public string Organization { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Zip { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string RegionId { get; set; }
		public string RegionName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}
}
