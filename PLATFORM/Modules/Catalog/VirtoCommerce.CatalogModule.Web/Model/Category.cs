using System.Collections.Generic;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Category
    {
        public string ParentId { get; set; }

        public string Id { get; set; }

        public bool Virtual { get; set; }

        public string Code { get; set; }
		public Catalog Catalog { get; set; }
        public string CatalogId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
		public Dictionary<string, string> Parents { get; set; }
        public List<Category> Children { get; set; }
		public List<Property> Properties { get; set; }
        public List<CategoryLink> Links { get; set; }
		public List<SeoInfo> SeoInfos { get; set; }
	
    }
}