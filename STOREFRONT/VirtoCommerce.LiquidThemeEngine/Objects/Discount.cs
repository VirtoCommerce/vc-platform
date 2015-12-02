using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Discount : Drop
    {
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string Id { get; set; }
        public decimal Savings { get; set; }
        public string Type { get; set; }
        public string PromotionId { get; set; }
        public string Coupon { get; set; }
    }
}
