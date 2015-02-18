#region

using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.Web.Core.DataContracts
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
