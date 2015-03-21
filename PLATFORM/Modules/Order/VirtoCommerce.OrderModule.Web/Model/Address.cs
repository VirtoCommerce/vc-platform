using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class Address
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public AddressType AddressType { get; set; }
		public string Organization { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Zip { get; set; }
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}
}