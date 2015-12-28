using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents product variant object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/variant
    /// </remarks>
    [DataContract]
    public class Variant : Drop
    {
        public Variant()
        {
        }

        /// <summary>
        /// Returns the value of variant category ID
        /// </summary>
        [DataMember]
        public string CategoryId { get; set; }

        /// <summary>
        /// Returns the calue of variant catalog ID
        /// </summary>
        [DataMember]
        public string CatalogId { get; set; }

        /// <summary>
        /// Returns true if the variant is available to be purchased, or false if it not. 
        /// In order for a variant to be available, its variant.inventory_quantity must be
        /// greater than zero or variant.inventory_policy must be set to continue. 
        /// A variant with no variant.inventory_management is also considered available.
        /// </summary>
        [DataMember]
        public bool Available { get; set; }

        /// <summary>
        /// Returns the variant's barcode.
        /// </summary>
        [DataMember]
        public string Barcode { get; set; }

        /// <summary>
        /// Returns the variant's compare at price. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal CompareAtPrice { get; set; }

        /// <summary>
        /// Returns the variant's unique id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Returns the image object associated to the variant.
        /// </summary>
        [DataMember]
        public Image FeaturedImage { get; set; }

        public Image Image { get; set; }

        /// <summary>
        /// Returns the variant's inventory tracking service.
        /// </summary>
        [DataMember]
        public string InventoryManagement { get; set; }

        /// <summary>
        /// Returns the string continue if the "Allow users to purchase this item,
        /// even if it is no longer in stock."
        /// </summary>
        [DataMember]
        public string InventoryPolicy { get; set; }

        /// <summary>
        /// Returns the variant's inventory quantity.
        /// </summary>
        [DataMember]
        public long InventoryQuantity { get; set; }

        /// <summary>
        /// Returns the array of product variant options
        /// </summary>
        [DataMember]
        public string[] Options { get; set; }

        /// <summary>
        /// Returns the value of the variant's first option.
        /// </summary>
        [DataMember]
        public string Option1
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 1 ? this.Options[0] : null;
            }
        }

        /// <summary>
        /// Returns the value of the variant's second option.
        /// </summary>
        [DataMember]
        public string Option2
        {
            get
            {
                if (this.Options == null) return null;
                return this.Options.Length >= 2 ? this.Options[1] : null;
            }
        }

        /// <summary>
        /// Returns the value of the variant's third option.
        /// </summary>
        [DataMember]
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
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Returns true if the variant is currently selected by the ?variant= URL parameter.
        /// Returns false if the variant is not selected by a URL parameter.
        /// </summary>
        [DataMember]
        public bool Selected { get; set; }

        /// <summary>
        /// Returns the variant's SKU.
        /// </summary>
        [DataMember]
        public string Sku { get; set; }

        /// <summary>
        /// Returns the concatenation of all the variant's option values, joined by a /.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Returns product variant URL
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Returns the variant's weight in grams. Use the weight_with_unit filter to convert it
        /// to the shop's weight format or the weight unit configured on the variant.
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }

        /// <summary>
        /// Returns the unit for the weight configured on the variant. Works well paired with
        /// the weight_in_unit attribute and the weight_with_unit filter.
        /// </summary>
        [DataMember]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Returns the weight of the product converted to the unit configured on the variant.
        /// Works well paired with the weight_unit attribute and the weight_with_unit filter.
        /// </summary>
        [DataMember]
        public string WeightInUnit { get; set; }
    }
}