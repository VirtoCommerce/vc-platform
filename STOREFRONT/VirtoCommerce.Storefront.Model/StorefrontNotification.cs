namespace VirtoCommerce.Storefront.Model
{
    public class StorefrontNotification
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public StorefrontNotificationType Type { get; set; }
    }
}