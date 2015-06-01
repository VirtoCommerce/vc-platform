namespace VirtoCommerce.ApiClient.DataContracts
{
    public class ProcessPaymentResult
    {
        public PaymentStatus NewPaymentStatus { get; set; }

        public PaymentMethodType PaymentMethodType { get; set; }

        public string HtmlForm { get; set; }

        public string RedirectUrl { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public string OuterId { get; set; }
    }
}