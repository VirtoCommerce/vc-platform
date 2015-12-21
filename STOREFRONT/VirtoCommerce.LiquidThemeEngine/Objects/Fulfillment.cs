using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents fulfillment object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/fulfillment
    /// </remarks>
    public class Fulfillment : Drop
    {
        /// <summary>
        /// Returns the name of the fulfillment service.
        /// </summary>
        public string TrackingCompany { get; set; }

        /// <summary>
        /// Returns the tracking number for a fulfillment if it exists.
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Returns the URL for a tracking number.
        /// </summary>
        public string TrackingUrl { get; set; }
    }
}