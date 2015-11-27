using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string[] Regions { get; set; }
    }
}
