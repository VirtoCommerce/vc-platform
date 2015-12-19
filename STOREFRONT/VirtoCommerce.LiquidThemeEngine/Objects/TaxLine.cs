using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents tax line object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/tax_line
    /// </remarks>
    [DataContract]
    public class TaxLine : Drop
    {
        /// <summary>
        /// Returns the title of the tax.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Returns the amount of the tax.
        /// Use one of the money filters to return the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Returns the rate of the tax in decimal notation.
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Returns the rate of the tax in percentage format.
        /// </summary>
        [DataMember]
        public decimal RatePercentage
        {
            get
            {
                return Rate * 100;
            }
        }
    }
}