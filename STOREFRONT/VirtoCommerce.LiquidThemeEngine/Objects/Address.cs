using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents address object
    /// </summary>
    [DataContract]
    public class Address : Drop
    {
        /// <summary>
        /// Returns the values of the First Name and Last Name fields of the address.
        /// </summary>
        /// <remarks>
        /// https://docs.shopify.com/themes/liquid-documentation/objects/address
        /// </remarks>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Returns the value of the First Name field of the address.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Returns the value of the Last Name field of the address.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Returns the value of the Address1 field of the address.
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }

        /// <summary>
        /// Returns the value of the Address2 field of the address.
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }

        /// <summary>
        /// Returns the combined values of the Address1 and Address2 fields of the address.
        /// </summary>
        [DataMember]
        public string Street { get; set; }

        /// <summary>
        /// Returns the value of the Company field of the address.
        /// </summary>
        [DataMember]
        public string Company { get; set; }

        /// <summary>
        /// Returns the value of the City field of the address.
        /// </summary>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        /// Returns the value of the Province/State field of the address.
        /// </summary>
        [DataMember]
        public string Province { get; set; }

        /// <summary>
        /// Returns the abbreviated value of the Province/State field of the address.
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// Returns the value of the Postal/Zip field of the address.
        /// </summary>
        [DataMember]
        public string Zip { get; set; }

        /// <summary>
        /// Returns the value of the Country field of the address.
        /// </summary>
        [DataMember]
        public string Country { get; set; }

        /// <summary>
        /// Returns the value of the Country field of the address in ISO 3166-2 standard format.
        /// </summary>
        [DataMember]
        public string CountryCode { get; set; }

        /// <summary>
        /// Returns the value of the Phone field of the address.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        /// Returns the value of address ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        public string Method { get; set; }
    }
}