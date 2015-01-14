using Newtonsoft.Json;

namespace VirtoCommerce.Web.Core.DataContracts
{
    public class Product : CatalogItem
    {
        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }
    }
}
