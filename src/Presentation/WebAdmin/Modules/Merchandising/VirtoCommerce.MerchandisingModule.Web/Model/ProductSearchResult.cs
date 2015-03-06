namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ProductSearchResult : ResponseCollection<Product>
    {
        #region Public Properties

        public Facet[] Facets { get; set; }

        #endregion
    }
}
