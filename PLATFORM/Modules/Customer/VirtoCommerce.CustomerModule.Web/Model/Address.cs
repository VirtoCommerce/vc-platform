using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class Address
	{
        /// <summary>
        /// Type of address.
        /// </summary>
        /// <value>1: Billing, 2: Shipping, 3: BillingAndShipping</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public AddressType AddressType { get; set; }

        /// <summary>
        /// Not documented
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Not documented
        /// </summary>
		public string Organization { get; set; }
        
        /// <summary>
        /// ISO 3166-1 alpha-3
        /// </summary>
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Zip { get; set; }
		public string Line1 { get; set; }
        public string Line2 { get; set; }
        /// <summary>
        /// Code of Region (AL - Alabama)
        /// </summary>
		public string RegionId { get; set; }
		public string RegionName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}
}
