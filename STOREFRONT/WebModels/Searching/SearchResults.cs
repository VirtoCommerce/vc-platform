using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.Searching
{
    public class SearchResults<T> : ItemCollection<T>
    {
        public SearchResults(IEnumerable<T> collections)
            : base(collections)
        {
        }

        public FacetFilter[] Facets { get; set; }
    }
}
