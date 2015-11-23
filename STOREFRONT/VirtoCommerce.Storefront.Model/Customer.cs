using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Customer : Entity
    {
        public Customer()
        {
            Addresses = new List<Address>();
            DynamicProperties = new List<DynamicProperty>();
        }

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

        //Returns the number of addresses associated with a customer.
        public int AddressesCount
        {
            get
            {
                return this.Addresses == null ? 0 : this.Addresses.Count;
            }
        }

        /// <summary>
        /// Returns the default customer_address.
        /// </summary>
        public Address DefaultAddress { get; set; }

        /// <summary>
        /// Returns true if user registered  returns false if it anonynous. 
        /// </summary>
        public bool HasAccount { get; set; }

        /// <summary>
        /// Returns the list of tags associated with the customer.
        /// </summary>
        public ICollection<string> Tags { get; set; }
    }
}
