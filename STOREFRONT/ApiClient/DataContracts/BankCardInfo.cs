namespace VirtoCommerce.ApiClient.DataContracts
{
    public class BankCardInfo
    {
        public string BankCardNumber { get; set; }

        public string BankCardType { get; set; }

        public int BankCardMonth { get; set; }

        public int BankCardYear { get; set; }

        public string BankCardCVV2 { get; set; }
    }
}