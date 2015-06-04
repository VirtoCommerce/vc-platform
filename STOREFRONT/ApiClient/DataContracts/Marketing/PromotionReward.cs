namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class PromotionReward
    {
        public bool IsValid { get; set; }

        public string Description { get; set; }

        public decimal CouponAmount { get; set; }

        public string Coupon { get; set; }

        public decimal? CouponMinOrderAmount { get; set; }

        public string PromotionId { get; set; }

        public Promotion Promotion { get; set; }

        public string RewardType { get; set; }

        public string AmountType { get; set; }

        public decimal Amount { get; set; }

        public int Quantity { get; set; }

        public string LineItemId { get; set; }

        public string ProductId { get; set; }

        public string CategoryId { get; set; }

        public string MeasureUnit { get; set; }

        public string ImageUrl { get; set; }
    }
}