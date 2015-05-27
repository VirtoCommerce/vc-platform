namespace VirtoCommerce.ApiClient.DataContracts
{
    public class PostProcessPaymentResult
    {
        public PaymentStatus NewPaymentStatus { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public string ReturnUrl { get; set; }
    }
}