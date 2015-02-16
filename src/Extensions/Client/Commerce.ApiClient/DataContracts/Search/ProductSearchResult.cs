namespace VirtoCommerce.ApiClient.DataContracts.Search
{
    #region

    using VirtoCommerce.Web.Core.DataContracts;

    #endregion

    public class ProductSearchResult : ResponseCollection<Product>
    {
        #region Public Properties

        public Facet[] Facets { get; set; }

        #endregion
    }
}