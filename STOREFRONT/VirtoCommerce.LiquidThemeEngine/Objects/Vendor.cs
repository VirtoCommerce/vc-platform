using DotLiquid;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Vendor : Drop
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SiteUrl { get; set; }

        public string LogoUrl { get; set; }

        public string GroupName { get; set; }

        public IMutablePagedList<Address> Addresses { get; set; }

        public ICollection<DynamicProperty> DynamicProperties { get; set; }
    }
}