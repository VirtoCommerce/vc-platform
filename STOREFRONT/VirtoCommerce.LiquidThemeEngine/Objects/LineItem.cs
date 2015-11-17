using DotLiquid;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents shopping cart's line item
    /// </summary>
    public class LineItem : Drop
    {
        private readonly Storefront.Model.Cart.LineItem _lineItem;

        public LineItem(Storefront.Model.Cart.LineItem lineItem)
        {
            _lineItem = lineItem;
        }

        /// <summary>
        /// Gets line item fulfillment info
        /// </summary>
        public Fulfillment Fulfillment
        {
            get
            {
                // TODO: Populate with fulfillment info
                return null;
            }
        }

        /// <summary>
        /// Gets line item weight
        /// </summary>
        public decimal Grams
        {
            get
            {
                return _lineItem.Weight.HasValue ? _lineItem.Weight.Value : 0M;
            }
        }

        /// <summary>
        /// Gets line item id
        /// </summary>
        public string Id
        {
            get
            {
                return _lineItem.Id;
            }
        }

        /// <summary>
        /// Gets line item image
        /// </summary>
        public Image Image
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets line item subtotal
        /// </summary>
        public decimal LinePrice
        {
            get
            {
                return _lineItem.ExtendedPrice;
            }
        }

        /// <summary>
        /// Gets line item price
        /// </summary>
        public decimal Price
        {
            get
            {
                return _lineItem.PlacedPrice;
            }
        }

        /// <summary>
        /// Gets product associated with the line item
        /// </summary>
        public Product Product
        {
            get
            {
                // TODO: Populate
                return null;
            }
        }

        /// <summary>
        /// Gets ID of the product that associated with the line item
        /// </summary>
        public string ProductId
        {
            get
            {
                // TODO: Populate
                return null;
            }
        }

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
        public int Quantity
        {
            get
            {
                return _lineItem.Quantity;
            }
        }

        /// <summary>
        /// Gets sign that line item requires shipping
        /// </summary>
        public bool RequiresShipping
        {
            get
            {
                return _lineItem.RequiredShipping;
            }
        }

        /// <summary>
        /// Gets line item SKU
        /// </summary>
        public string Sku
        {
            get
            {
                return _lineItem.Sku;
            }
        }

        /// <summary>
        /// Gets sign that line item includes taxes
        /// </summary>
        public bool Taxable
        {
            get
            {
                return _lineItem.TaxIncluded;
            }
        }

        /// <summary>
        /// Gets line item title
        /// </summary>
        public string Title
        {
            get
            {
                return _lineItem.Name;
            }
        }

        /// <summary>
        /// Gets line item type
        /// </summary>
        public string Type
        {
            get
            {
                // TODO: Populate
                return null;
            }
        }

        /// <summary>
        /// Gets line item product URL
        /// </summary>
        public string Url
        {
            get
            {
                // TODO: Populate
                return null;
            }
        }

        /// <summary>
        /// Gets the product variant that associated with the line item
        /// </summary>
        public Variant Variant
        {
            get
            {
                // TODO: populate
                return null;
            }
        }

        /// <summary>
        /// Gets the ID of product variant that associated with line item
        /// </summary>
        public string VariantId
        {
            get
            {
                // TODO: Populate
                return null;
            }
        }

        /// <summary>
        /// Gets line item product vendor
        /// </summary>
        public string Vendor
        {
            get
            {
                // TODO: Populate from dynamic properties
                return null;
            }
        }
    }
}