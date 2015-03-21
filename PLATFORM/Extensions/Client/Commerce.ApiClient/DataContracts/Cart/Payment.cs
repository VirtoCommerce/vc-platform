namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class Payment
    {
        #region Public Properties

        public decimal? Amount { get; set; }

        public Address BillingAddress { get; set; }

        public string Currency { get; set; }

        public string Id { get; set; }

        public string OuterId { get; set; }

        public string PaymentGatewayCode { get; set; }

        #endregion
    }
}
