#region
using VirtoCommerce.Web.Core.DataContracts;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Search
{
    #region
    
    #endregion

    public class ProductSearchResult : ResponseCollection<Product>
    {
        #region Public Properties
        public Facet[] Facets { get; set; }
        #endregion
    }
}