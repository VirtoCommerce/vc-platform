namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class Payment
    {
        public string Id { get; set; }

        public string OuterId { get; set; }

        public string PaymentGatewayCode { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public Address BillingAddress { get; set; }
    }
}