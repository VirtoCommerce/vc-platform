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
        public Category()
        {
            Images = new List<Image>();
        }
        public string CatalogId { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string TaxType { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public SeoInfo SeoInfo { get; set; }
        /// <summary>
        /// Category main image
        /// </summary>
        public Image PrimaryImage { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
