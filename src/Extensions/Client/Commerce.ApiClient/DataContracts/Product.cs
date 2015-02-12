namespace VirtoCommerce.Web.Core.DataContracts
{
    #region

    using Newtonsoft.Json;

    #endregion

    public class Product : CatalogItem
    {
        #region Public Properties

        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }

        #endregion
    }
}