using System.Collections.Generic;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class SearchResult
    {
        public SearchResult()
        {
            Products = new List<CatalogProduct>();
            Categories = new List<Category>();
            Catalogs = new List<Catalog>();
        }

        public int ProductsTotalCount { get; set; }
        /// <summary>
        /// Type used in search result and represent properties search result aggregation 
        /// </summary>
        public ICollection<CatalogProduct> Products { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }

        /// <summary>
        /// Represent aggregations for product properties
        /// </summary>
        public Aggregation[] Aggregations { get; set; }
    }
}
