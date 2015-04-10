namespace VirtoCommerce.CoreModule.Web.Security.Models
{
    public class SecurityMessage
    {
        public string UserId { get; set; }
        public string Language { get; set; }
        public string StoreId { get; set; }
        public string MessageType { get; set; }
        public string SendingMethod { get; set; }
        public string CallbackUrl { get; set; }
    }
}