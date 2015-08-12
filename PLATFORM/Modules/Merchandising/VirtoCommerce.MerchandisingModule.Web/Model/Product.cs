using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Product : CatalogItem
    {
        /// <summary>
        /// Gets or sets the collection of product variations
        /// </summary>
        /// <value>
        /// Array of ProductVariation objects
        /// </value>
        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }
    }
}