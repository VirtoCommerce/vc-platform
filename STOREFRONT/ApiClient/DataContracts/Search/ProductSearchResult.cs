namespace VirtoCommerce.ApiClient.DataContracts.Search
{
    public class ProductSearchResult : ResponseCollection<Product>
    {
        #region Public Properties

        public Facet[] Facets { get; set; }

        #endregion
    }
}
