using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Fulfillment : Drop
    {
        public string TrackingCompany { get; set; }

        public string TrackingNumber { get; set; }

        public string TrackingUrl { get; set; }
    }
}