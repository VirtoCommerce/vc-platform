using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Product : CatalogItem
    {
        #region Public Properties

        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }

        #endregion
    }
}
