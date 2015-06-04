using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Model
{
	public class SearchResult
	{
		public SearchResult()
        {
            PropertyValues = new List<PropertyValue>();
            Products = new List<CatalogProduct>();
            Categories = new List<Category>();
            Catalogs = new List<Catalog>();
        }
        public int TotalCount { get; set; }

        public List<PropertyValue> PropertyValues { get; set; }
		public List<CatalogProduct> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Catalog> Catalogs { get; set; }
	}
}
