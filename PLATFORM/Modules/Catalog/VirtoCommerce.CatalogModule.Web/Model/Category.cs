using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Category
    {
        public string ParentId { get; set; }

        public string Id { get; set; }

        public bool Virtual { get; set; }

        public string Code { get; set; }
		public string TaxType { get; set; }
		public Catalog Catalog { get; set; }
        public string CatalogId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
		public bool? IsActive { get; set; }
		public Dictionary<string, string> Parents { get; set; }
        public ICollection<Category> Children { get; set; }
		public ICollection<Property> Properties { get; set; }
		public ICollection<CategoryLink> Links { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
		public ICollection<Image> Images { get; set; }
    }
}