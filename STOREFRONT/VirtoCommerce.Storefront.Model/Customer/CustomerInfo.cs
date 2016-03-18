using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Model.Customer
{
    /// <summary>
    /// Represent customer information structure 
    /// </summary>
    public class CustomerInfo : Entity
    {
        public CustomerInfo()
        {
            Addresses = new List<Address>();
            DynamicProperties = new List<DynamicProperty>();
        }
        /// <summary>
        /// Security account Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Security account user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Returns the email address of the customer.
        /// </summary>
        public string Email { get; set; }

        public string FullName { get; set; }
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

        public Address DefaultBillingAddress { get; set; }
        public Address DefaultShippingAddress { get; set; }

        /// <summary>
        /// Returns an array of all addresses associated with a customer.
        /// </summary>
        public ICollection<Address> Addresses { get; set; }
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Returns true if the customer accepts marketing, returns false if the customer does not.
        /// </summary>
        public bool AcceptsMarketing { get; set; }


        /// <summary>
        /// Returns the default customer_address.
        /// </summary>
        public Address DefaultAddress { get; set; }

        /// <summary>
        /// Returns true if user registered  returns false if it anonynous. 
        /// </summary>
        public bool IsRegisteredUser { get; set; }

        /// <summary>
        /// Returns the list of tags associated with the customer.
        /// </summary>
        public ICollection<string> Tags { get; set; }

        [IgnoreDataMember]
        public IMutablePagedList<CustomerOrder> Orders { get; set; }
        [IgnoreDataMember]
        public IMutablePagedList<QuoteRequest> QuoteRequests { get; set; }

        /// <summary>
        /// The user ID of an operator who has loggen in on behalf of a customer
        /// </summary>
        public string OperatorUserId { get; set; }
        /// <summary>
        /// The user name of an operator who has loggen in on behalf of a customer
        /// </summary>
        public string OperatorUserName { get; set; }

        /// <summary>
        /// List of stores which user can sign in
        /// </summary>
        public IEnumerable<string> AllowedStores { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "user#{0} {1} {2}", Id ?? "undef", UserName ?? "undef", IsRegisteredUser ? "registered" : "anonymous");
        }
    }
}
