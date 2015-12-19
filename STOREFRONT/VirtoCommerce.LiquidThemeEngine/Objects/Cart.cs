using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents customer's shopping cart
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/cart
    /// </remarks>
    [DataContract]
    public class Cart : Drop
    {
        /// <summary>
        /// Gets an additional shopping cart information
        /// </summary>
        public MetaFieldNamespacesCollection Attributes { get; set; }

        /// <summary>
        /// Gets collection of shopping cart line items
        /// </summary>
        [DataMember]
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// Gets the number of shopping cart line items
        /// </summary>
        [DataMember]
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets the shopping cart note
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// Gets shopping cart total price
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets shopping cart total weight
        /// </summary>
        [DataMember]
        public decimal TotalWeight { get; set; }
    }
}