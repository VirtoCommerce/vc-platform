using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public class PushNotificationOptions
    {
        public string ScalabilityMode { get; set; } = "None";
        [Url, Required]
        public string HubUrl { get; set; }

    }
}
