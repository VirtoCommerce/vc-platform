namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ProductSearchResult : ResponseCollection<Product>
    {
        /// <summary>
        /// Gets or sets the collection of facets
        /// </summary>
        /// <value>
        /// Array of Facet objects
        /// </value>
        public Facet[] Facets { get; set; }
    }
}