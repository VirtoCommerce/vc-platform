using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Product : CatalogItem
    {
        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }
    }
}
