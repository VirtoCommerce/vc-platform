using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents the shipping method object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/shipping_method
    /// </remarks>
    [DataContract]
    public class ShippingMethod : Drop
    {
        /// <summary>
        /// Returns the handle of the shipping method.
        /// The price of the shipping rate is appended to the end of the handle.
        /// </summary>
        [DataMember]
        public string Handle { get; set; }

        /// <summary>
        /// Returns the price of the shipping method.
        /// Use a money filter to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Returns the title of the shipping method.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Returns the value of tax total
        /// </summary>
        [DataMember]
        public decimal TaxTotal { get; set; }

        /// <summary>
        /// Returns the value of tax type
        /// </summary>
        [DataMember]
        public string TaxType { get; set; }
    }
}