namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represents bank card information
    /// </summary>
    public class BankCardInfo
    {
        /// <summary>
        /// Gets or sets bank card number
        /// </summary>
        public string BankCardNumber { get; set; }

        /// <summary>
        /// Gets or sets bank card type
        /// </summary>
        public string BankCardType { get; set; }

        /// <summary>
        /// Gets or sets bank card expiration month
        /// </summary>
        public int? BankCardMonth { get; set; }

        /// <summary>
        /// Gets or sets bank card expiration year
        /// </summary>
        public int? BankCardYear { get; set; }

        /// <summary>
        /// Gets or sets bank card CVV
        /// </summary>
        public string BankCardCVV2 { get; set; }

        /// <summary>
        /// Gets or sets the cardholder name
        /// </summary>
        public string CardholderName { get; set; }
    }
}