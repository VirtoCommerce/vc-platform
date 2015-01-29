using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient.DataContracts.Search
{
    public class ProductSearchResult : ResponseCollection<Product>
    {
        public Facet[] Facets { get; set; }
    }
}
