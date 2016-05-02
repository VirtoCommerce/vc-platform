using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using PagedList;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents customer object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/customer
    /// </remarks>
    [DataContract]
    public class Customer : Drop
    {
        /// <summary>
        /// Return customer username
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Returns the email address of the customer.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Returns the full name of the customer.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Returns the first name of the customer.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Returns the last name of the customer.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Returns the middle name of the customer.
        /// </summary>
        [DataMember]
        public string MiddleName { get; set; }

        /// <summary>
        /// Returns the customer timezone.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Returns customer default language
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Returns true if the email associated with an order is also tied to a Customer Account. 
        /// Returns false if it is not. Helpful in email templates. In the theme, that will always be true.
        /// </summary>
        public bool HasAccount { get; set; }

        /// <summary>
        /// Returns true if the customer accepts marketing, returns false if the customer does not.
        /// </summary>
        [DataMember]
        public bool AcceptsMarketing { get; set; }

        /// <summary>
        /// Returns the default customer_address.
        /// </summary>
        public Address DefaultAddress { get; set; }

        /// <summary>
        /// Returns the default billing address
        /// </summary>
        public Address DefaultBillingAddress { get; set; }

        /// <summary>
        /// Returns the default shipping address
        /// </summary>
        public Address DefaultShippingAddress { get; set; }

        /// <summary>
        /// Returns an array of all addresses associated with a customer.
        /// See customer_address for a full list of available attributes.
        /// </summary>
        public IMutablePagedList<Address> Addresses { get; set; }

        /// <summary>
        /// Returns the number of addresses associated with a customer.
        /// </summary>
        [DataMember]
        public int AddressesCount { get { return Addresses.GetTotalCount(); } }

        /// <summary>
        /// Returns the list of tags associated with the customer.
        /// </summary>
        [DataMember]
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// Returns the list of customer dynamic properties
        /// </summary>
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Returns an array of all orders placed by the customer.
        /// </summary>
        public IMutablePagedList<Order> Orders { get; set; }

        /// <summary>
        /// Returns the total number of orders a customer has placed.
        /// </summary>
        [DataMember]
        public int OrdersCount { get { return Orders.GetTotalCount(); } }

        public IMutablePagedList<QuoteRequest> QuoteRequests { get; set; }

        /// <summary>
        /// The user ID of an operator who has loggen in on behalf of a customer
        /// </summary>
        public string OperatorUserId { get; set; }
        /// <summary>
        /// The user name of an operator who has loggen in on behalf of a customer
        /// </summary>
        public string OperatorUserName { get; set; }
    }
}
