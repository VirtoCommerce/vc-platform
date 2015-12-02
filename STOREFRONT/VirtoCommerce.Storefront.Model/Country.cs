using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Regions { get; set; }
    }
}
