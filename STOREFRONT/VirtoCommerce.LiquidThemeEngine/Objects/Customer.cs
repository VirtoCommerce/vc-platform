using System.Collections.Generic;
using DotLiquid;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/customer
    /// </summary>
    public class Customer : Drop
    {
        public string UserName { get; set; }
        /// <summary>
        /// Returns the email address of the customer.
        /// </summary>
        public string Email { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Returns the first name of the customer.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Returns the last name of the customer.
        /// </summary>
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }
        /// <summary>
        /// Returns true if the email associated with an order is also tied to a Customer Account. 
        /// Returns false if it is not. Helpful in email templates. In the theme, that will always be true.
        /// </summary>
        public bool HasAccount { get; set; }
        public bool AcceptsMarketing { get; set; }
        /// <summary>
        /// Returns the default customer_address.
        /// </summary>
        public Address DefaultAddress { get; set; }
        public Address DefaultBillingAddress { get; set; }
        public Address DefaultShippingAddress { get; set; }

        /// <summary>
        /// Returns an array of all addresses associated with a customer. See customer_address for a full list of available attributes.
        /// </summary>
        public IStorefrontPagedList<Address> Addresses { get; set; }
        public int AddressesCount { get; set; }

        public ICollection<string> Tags { get; set; }
        public ICollection<DynamicProperty> DynamicProperties { get; set; }
    }
}
