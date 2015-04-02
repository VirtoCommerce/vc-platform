#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;
using System;

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
                bool isAvailable = false;

                if (string.IsNullOrEmpty(this.InventoryManagement))
                {
                    isAvailable = true;
                }
                else
                {
                    if (this.InventoryPolicy.Equals("deny", StringComparison.OrdinalIgnoreCase))
                    {
                        if (this.InventoryQuantity > 0)
                        {
                            isAvailable = true;
                        }
                    }
                    else if (this.InventoryPolicy.Equals("continue", StringComparison.OrdinalIgnoreCase))
                    {
                        isAvailable = true;
                    }
                }

                return isAvailable;
            }
        }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        public decimal CompareAtPrice { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string FeaturedImage
        {
            get
            {
                return this.Image != null ? this.Image.Src : null;
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
        public string Option1 { get; set; }

        [DataMember]
        public string Option2 { get; set; }

        [DataMember]
        public string Option3 { get; set; }

        [DataMember]
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
        public int Weight { get; set; }

        [DataMember]
        public string WeightUnit { get; set; }

        [DataMember]
        public string WeightInUnit { get; set; }
    }
}