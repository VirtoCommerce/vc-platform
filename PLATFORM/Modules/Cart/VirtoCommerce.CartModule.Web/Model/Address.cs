using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CartModule.Web.Model
{
    public class Address : ValueObject<Address>
    {
        /// <summary>
        /// Gets or sets the value of address type
        /// </summary>
        /// <value>
        /// Billind, Shipping
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public AddressType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of organization name
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the value of country code
        /// </summary>
        /// <value>
        /// Country code in ISO 3166-1 alpha-3 format
        /// </value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the value of country name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the value of city name
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the value of postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the value of zip code
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the value of address line 1
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the value of address line 2
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the value of region code
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// Gets or sets the value of region name
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the value of first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the value of middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the value of last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the value of phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the value of E-mail address
        /// </summary>
        public string Email { get; set; }
    }
}