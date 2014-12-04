using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Product : CatalogItem
    {
        [JsonProperty("variations")]
        public ProductVariation[] Variations { get; set; }
    }
}
