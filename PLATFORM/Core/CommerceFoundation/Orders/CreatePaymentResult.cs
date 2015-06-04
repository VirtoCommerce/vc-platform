using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Orders
{
    [DataContract]
    public class CreatePaymentResult
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool IsSuccess { get; set; }

		[DataMember]
		public string TransactionId { get; set; }

		public CreatePaymentResult(bool success)
        {
			IsSuccess = success;
        }

        public CreatePaymentResult()
        {
        }
    }
}
