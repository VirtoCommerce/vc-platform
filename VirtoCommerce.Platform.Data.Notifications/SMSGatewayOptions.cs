namespace VirtoCommerce.Platform.Data.Notifications
{
    public class SmsGatewayOptions
    {
        /// <summary>
        /// Sms gateway account Id 
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// Sms gateway account password or auth token
        /// </summary>
        public string AccountPassword { get; set; }
        /// <summary>
        /// Phone number or name (if supported by gateway) used for sending sms
        /// </summary>
        public string Sender { get; set; }
    }
}
