#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;
using System;
using Newtonsoft.Json;
using VirtoCommerce.Web.Helpers;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Variant : Drop
    {
        [DataMember]
        public bool Available
        {
            get
            {
                var isAvailable = true;

                if (!string.IsNullOrEmpty(InventoryManagement))
                {
                    if (InventoryPolicy.Equals("deny", StringComparison.OrdinalIgnoreCase))
                    {
                        if (InventoryQuantity <= 0)
                        {
                            isAvailable = false;
                        }
                    }
                }

                if (Price == 0)
                {
                    isAvailable = false;
                }

                return isAvailable;
            }
        }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal CompareAtPrice { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Image FeaturedImage
        {
            get
            {
                return this.Image ?? null;
            }
        }

        [DataMember]
        public Image Image { get; set; }

        [DataMember]
        public string InventoryManagement { get; set; }

        [DataMember]
        public string InventoryPolicy { get; set; }

        [DataMember]
        public long InventoryQuantity { get; set; }

        [DataMember]
        public string[] Options { get; set; }

        [DataMember]
        public string Option1
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 1 ? this.Options[0] : null;
            }
        }

        [DataMember]
        public string Option2
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 2 ? this.Options[1] : null;
            }
        }

        [DataMember]
        public string Option3
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 3 ? this.Options[2] : null;
            }
        }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal Price { get; set; }

        [DataMember]
        public bool Selected { get; set; }

        [DataMember]
        public string Sku { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public decimal Weight { get; set; }

        [DataMember]
        public string WeightUnit { get; set; }

        [DataMember]
        public string WeightInUnit { get; set; }
     }
}