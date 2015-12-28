using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents shopping cart's line item
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/line_item
    /// </remarks>
    [DataContract]
    public class LineItem : Drop
    {
        /// <summary>
        /// Gets line item fulfillment info
        /// </summary>
        [DataMember]
        public Fulfillment Fulfillment { get; set; }

        /// <summary>
        /// Gets line item weight
        /// </summary>
        [DataMember]
        public decimal Grams { get; set; }

        /// <summary>
        /// Gets line item id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets line item image
        /// </summary>
        [DataMember]
        public Image Image { get; set; }

        /// <summary>
        /// Gets line item subtotal
        /// </summary>
        [DataMember]
        public decimal LinePrice { get; set; }

        /// <summary>
        /// Gets line item price
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets product associated with the line item
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Gets ID of the product that associated with the line item
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets line item custom information
        /// </summary>
        [DataMember]
        public IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets line item quantity
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets sign that line item requires shipping
        /// </summary>
        [DataMember]
        public bool RequiresShipping { get; set; }

        /// <summary>
        /// Gets line item SKU
        /// </summary>
        [DataMember]
        public string Sku { get; set; }

        /// <summary>
        /// Gets sign that line item includes taxes
        /// </summary>
        [DataMember]
        public bool Taxable { get; set; }

        /// <summary>
        /// Gets line item title
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets line item type
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets line item product URL
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Gets the product variant that associated with the line item
        /// </summary>
        [DataMember]
        public Variant Variant { get; set; }

        /// <summary>
        /// Gets the ID of product variant that associated with line item
        /// </summary>
        [DataMember]
        public string VariantId { get; set; }

        /// <summary>
        /// Gets line item product vendor
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }
    }
}