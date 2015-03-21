using System.Collections.Generic;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class ItemsSearchResult
    {
        public ItemsSearchResult()
        {
            PropertyValues = new List<PropertyValue>();
            Items = new List<Product>();
            Categories = new List<Category>();
            Catalogs = new List<Catalog>();
        }
        public int TotalCount { get; set; }

        public ICollection<PropertyValue> PropertyValues { get; set; }
        public ICollection<Product> Items { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }
    }
}