namespace VirtoCommerce.Platform.Data.Notifications
{
    public class AspsmsSmsGatewayOptions : SmsGatewayOptions
    {
        /// <summary>
        /// ASPSMS Json REST API endpoint
        /// </summary>
        public string JsonApiUri { get; set; }
    }
}
