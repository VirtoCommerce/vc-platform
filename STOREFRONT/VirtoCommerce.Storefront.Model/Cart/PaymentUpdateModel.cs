namespace VirtoCommerce.Storefront.Model.Cart
{
    public class PaymentUpdateModel
    {
        public string Id { get; set; }

        public string OuterId { get; set; }

        public string PaymentGatewayCode { get; set; }

        public decimal Amount { get; set; }

        public Address BillingAddress { get; set; }
    }
}