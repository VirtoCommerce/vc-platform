using System.Collections.Generic;
using DotLiquid;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Customer : Drop
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }

        public bool HasAccount { get; set; }
        public bool AcceptsMarketing { get; set; }

        public Address DefaultAddress { get; set; }
        public Address DefaultBillingAddress { get; set; }
        public Address DefaultShippingAddress { get; set; }

        public IStorefrontPagedList<Address> Addresses { get; set; }
        public int AddressesCount { get; set; }

        public ICollection<string> Tags { get; set; }
        public ICollection<DynamicProperty> DynamicProperties { get; set; }
    }
}
