#region

using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    public class Product : CatalogItem
    {
        #region Public Properties

        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }

        #endregion
    }
}
