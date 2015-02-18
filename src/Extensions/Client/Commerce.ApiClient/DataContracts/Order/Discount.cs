namespace VirtoCommerce.ApiClient.DataContracts.Order
{
    public class Discount
    {
        public string Id { get; set; }

        public string PromotionId { get; set; }

        public string PromotionDescription { get; set; }

        public string Currency { get; set; }

        public decimal DiscountAmount { get; set; }

        public string CouponCode { get; set; }

        public string CouponInvalidDescription { get; set; }

        public string CustomerOrderId { get; set; }

        public string ShipmentId { get; set; }

        public string LineItemId { get; set; }
    }
}