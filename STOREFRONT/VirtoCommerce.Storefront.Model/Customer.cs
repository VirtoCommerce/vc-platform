using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public string Email { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }

        public bool IsAnonymous { get; set; }

        public Address DefaultBillingAddress { get; set; }
        public Address DefaultShippingAddress { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<DynamicProperty> DynamicProperties { get; set; }
    }
}