using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class SearchResult
	{
		public SearchResult()
        {
            Products = new List<CatalogProduct>();
            Categories = new List<Category>();
            Catalogs = new List<Catalog>();
            PropertyValueBuckets = new List<SearchAgregationItem<PropertyValue>>();
            PriceRangeBuckets = new List<SearchAggregationRangeItem<string>>();
        }
        public int TotalCount { get; set; }
        /// <summary>
        /// Type used in search result and represent properties search result aggregation 
        /// </summary>
        public ICollection<CatalogProduct> Products { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }

        /// <summary>
        /// Represent facets for product properties
        /// </summary>
        public ICollection<SearchAgregationItem<PropertyValue>> PropertyValueBuckets { get; set; }
        /// <summary>
        /// Represent prices ranges availabe for search
        /// </summary>
        public ICollection<SearchAggregationRangeItem<string>> PriceRangeBuckets { get; set; }
    }
}
