#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Variant : Drop
    {
        #region Public Properties
        [DataMember]
        public bool Available { get; set; }

        [DataMember]
        public string Barcode { get; set; }

        [DataMember]
        public decimal CompareAtPrice
        {
            get
            {
                if (Prices != null && Prices.Any())
                {
                    var priceModels = Prices.Where(p => p.MinQuantity <= 1);
                    if (priceModels.Any())
                    {
                        var priceModel = priceModels.First();
                        if (priceModel != null) return priceModel.List;
                    }
                }

                return 0;
            }
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Image Image
        {
            get
            {
                if (this.Images != null && this.Images.Any())
                {
                    var primaryImage = this.Images.SingleOrDefault(i => i.Name.Equals("primaryimage"));
                    if (primaryImage != null) return primaryImage;

                    return this.Images.ElementAt(0);
                }

                return null;
            }
        }

        [DataMember]
        public string InventoryManagement { get; set; }

        [DataMember]
        public string InventoryPolicy { get; set; }

        [DataMember]
        public decimal InventoryQuantity { get; set; }

        [DataMember]
        public string Option1 { get; set; }

        [DataMember]
        public string Option2 { get; set; }

        [DataMember]
        public string Option3 { get; set; }

        [DataMember]
        public decimal Price
        {
            get
            {
                if (Prices != null && Prices.Any())
                {
                    var priceModels = Prices.Where(p => p.MinQuantity <= 1);
                    if (priceModels.Any())
                    {
                        var priceModel = priceModels.First();
                        return priceModel.Sale.HasValue ? priceModel.Sale.Value : priceModel.List;
                    }
                }

                return 0;
            }
        }

        [DataMember]
        public bool Selected { get; set; }

        [DataMember]
        public string Sku { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Weight { get; set; }

        [DataMember]
        public Price[] Prices { get; set; }

        [DataMember]
        public IEnumerable<Image> Images { get; set; }
        #endregion
    }
}