using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class Category : Entity
    {
        public string CatalogId { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string TaxType { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Parents { get; set; }
        public Category[] Children { get; set; }
        public SeoInfo[] SeoInfos { get; set; }
        public Image[] Images { get; set; }
        public Product[] Products { get; set; }
    }
}
