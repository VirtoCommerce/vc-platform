namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class Discount
    {
        #region Public Properties

        public Coupon Coupon { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public decimal? DiscountAmount { get; set; }

        public string Id { get; set; }

        public string PromotionId { get; set; }

        #endregion
    }
}
