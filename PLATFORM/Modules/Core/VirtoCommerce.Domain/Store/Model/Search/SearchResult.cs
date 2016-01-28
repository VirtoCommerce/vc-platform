using System.Collections.Generic;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.Domain.Store.Model
{
    public class SearchResult
    {
        public SearchResult()
        {
            Stores = new List<Store>();
        }

        public int TotalCount { get; set; }
        /// <summary>
        /// Type used in search result and represent properties search result aggregation 
        /// </summary>
        public ICollection<Store> Stores { get; set; }
 

    }
}
