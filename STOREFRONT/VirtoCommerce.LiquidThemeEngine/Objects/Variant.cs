using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/variant
    /// </summary>
    public class Variant : Drop
    {
        private readonly Storefront.Model.Catalog.Product _product;
        private readonly WorkContext _workContext;
        private readonly string _mainProductId;
        private readonly IStorefrontUrlBuilder _urlBuilder;

        public Variant(Storefront.Model.Catalog.Product product, WorkContext workContext, IStorefrontUrlBuilder urlBuilder, string mainProductId)
        {
            _product = product;
            _workContext = workContext;
            _mainProductId = mainProductId;
            _urlBuilder = urlBuilder;
        }

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

        public string Barcode
        {
            get
            {
                string retVal = null;
                return retVal;
            }
        }

        public decimal CompareAtPrice
        {
            get
            {
                return Price;
            }
        }

        public string Id
        {
            get
            {
                return _product.Id;
            }
        }

        public Image FeaturedImage
        {
            get
            {
                return Image;
            }
        }

        public Image Image
        {
            get
            {
                if(_product.Images != null && _product.PrimaryImage != null)
                {
                    return new Image(_product.PrimaryImage);
                }
                return null;
            }
        }

        public string InventoryManagement
        {
            get
            {
                string retVal = null;

                if ((_product.IsBuyable ?? false) && (_product.TrackInventory ?? false) && _product.Inventory != null)
                {
                    var inventory = _product.Inventory;
                    if(inventory != null)
                    {
                        retVal = inventory.FulfillmentCenterId;
                    }
                }

                return retVal;
            }
        }

        public string InventoryPolicy
        {
            get
            {
                string retVal = null;

                if ((_product.IsBuyable ?? false) && (_product.TrackInventory ?? false) && _product.Inventory != null)
                {
                    var inventory = _product.Inventory;
                    if (inventory != null)
                    {
                        retVal = "deny";
                    }

                    if (inventory.AllowBackorder ?? false && inventory.BackorderAvailabilityDate.HasValue &&
                            inventory.BackorderAvailabilityDate.Value > DateTime.UtcNow)
                    {
                        retVal = "continue";
                    }

                    if (inventory.AllowPreorder ?? false && inventory.PreorderAvailabilityDate.HasValue &&
                        inventory.PreorderAvailabilityDate.Value > DateTime.UtcNow)
                    {
                        retVal = "continue";
                    }
                }

                return retVal;
            }
        }

        public long InventoryQuantity
        {
            get
            {
                long retVal = 0;

                if ((_product.IsBuyable ?? false) && (_product.TrackInventory ?? false) && _product.Inventory != null)
                {
                    var inventory = _product.Inventory;
                    if (inventory != null)
                    {
                        retVal = inventory.InStockQuantity.Value - inventory.ReservedQuantity.Value;
                    }
                }

                return retVal;
            }
        }

        public string[] Options
        {
            get
            {
                var options = new List<string>();
                if(_product.Properties != null)
                {
                    foreach(var property in _product.Properties)
                    {
                        if(property.Type == "Variation" && property.Values != null && property.Values.Any())
                        {
                            options.Add(string.Join(";", property.Values.Select(v => v.Value)));
                        }
                    }
                }
                return options.ToArray();
            }
        }

        public string Option1
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 1 ? this.Options[0] : null;
            }
        }

        public string Option2
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 2 ? this.Options[1] : null;
            }
        }

        public string Option3
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 3 ? this.Options[2] : null;
            }
        }

        public decimal Price
        {
            get
            {
                var retVal = _product.Price.SalePrice.Amount;
                return retVal;
            }
        }

        public bool Selected
        {
            get
            {
                //TODO: Check that variant in querystring
                return false;
            }
        }

        public string Sku
        {
            get
            {
                var retVal = _product.Sku;
                var property = _product.Properties.FirstOrDefault(p => p.Name == "sku");
                if(property != null)
                {
                    var values = property.Values;
                    if(values.Any())
                    {
                        retVal = (string)values.First().Value;
                    }
                }

                return retVal;
            }
        }

        public string Title
        {
            get
            {
                return _product.Name;
            }
        }

        public string Url
        {
            get
            {
                return _urlBuilder.ToAppAbsolute(_workContext, string.Format("~/products/{0}?variant={1}", _mainProductId, _product.Id), _workContext.CurrentStore, _workContext.CurrentLanguage);
            }
        }

        public decimal Weight
        {
            get
            {
                
                return (decimal)(_product.Weight ?? 0);
            }
        }

        public string WeightUnit
        {
            get
            {
                return _product.WeightUnit;
            }
        }

        public string WeightInUnit
        {
            get
            {
                //TODO No info about this
                return null;
            }
        }
    }
}
