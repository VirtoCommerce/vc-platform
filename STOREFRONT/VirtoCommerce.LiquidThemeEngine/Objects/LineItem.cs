using DotLiquid;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents shopping cart's line item
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/line_item
    /// </remarks>
    public class LineItem : Drop
    {
        /// <summary>
        /// Gets line item fulfillment info
        /// </summary>
        public Fulfillment Fulfillment { get; set; }

        /// <summary>
        /// Gets line item weight
        /// </summary>
        public decimal Grams { get; set; }
        /// <summary>
        /// Gets line item id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets line item image
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets line item subtotal
        /// </summary>
        public decimal LinePrice { get; set; }

        /// <summary>
        /// Gets line item price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets product associated with the line item
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets ID of the product that associated with the line item
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets line item custom information
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get
            {
                // TODO: Populate from dynamic properties
                return null;
            }
        }

        /// <summary>
        /// Gets line item quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets sign that line item requires shipping
        /// </summary>
        public bool RequiresShipping { get; set; }

        /// <summary>
        /// Gets line item SKU
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// Gets sign that line item includes taxes
        /// </summary>
        public bool Taxable { get; set; }

        /// <summary>
        /// Gets line item title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets line item type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets line item product URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the product variant that associated with the line item
        /// </summary>
        public Variant Variant { get; set; }

        /// <summary>
        /// Gets the ID of product variant that associated with line item
        /// </summary>
        public string VariantId { get; set; }

        /// <summary>
        /// Gets line item product vendor
        /// </summary>
        public string Vendor { get; set; }
    }
}