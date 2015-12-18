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
        public Variant()
        {
          
        }

        public string CategoryId { get; set; }

        public string CatalogId { get; set; }

        /// <summary>
        /// Returns true if the variant is available to be purchased, or false if it not. 
        /// In order for a variant to be available, its variant.inventory_quantity must be greater than zero or variant.inventory_policy must be set to continue. 
        /// A variant with no variant.inventory_management is also considered available.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Returns the variant's barcode.
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Returns the variant's compare at price. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal CompareAtPrice { get; set; }

        public string Id { get; set; }

        public Image FeaturedImage { get; set; }
        public Image Image { get; set; }

        /// <summary>
        /// Returns the variant's inventory tracking service.
        /// </summary>
        public string InventoryManagement { get; set; }

        /// <summary>
        /// Returns the string continue if the "Allow users to purchase this item, even if it is no longer in stock."
        /// </summary>
        public string InventoryPolicy { get; set; }

        /// <summary>
        /// Returns the variant's inventory quantity.
        /// </summary>
        public long InventoryQuantity { get; set; }

        public string[] Options { get; set; }

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

        /// <summary>
        /// Returns the variant's price. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal Price { get; set; }

        public bool Selected { get; set; }

        /// <summary>
        /// Returns the variant's SKU.
        /// </summary>
        public string Sku { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public decimal Weight { get; set; }

        public string WeightUnit { get; set; }

        public string WeightInUnit { get; set; }
    }
}
