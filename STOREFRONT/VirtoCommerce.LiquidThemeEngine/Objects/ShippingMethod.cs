using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class ShippingMethod : Drop
    {
        public string Handle { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public decimal TaxTotal { get; set; }
        public string TaxType { get; set; }
    }
}
