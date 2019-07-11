using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public abstract class EmailNotification : Notification
    {
        protected EmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway)
        {
        }

        public IList<string> CC { get; set; } = new List<string>();
        public IList<string> Bcc { get; set; } = new List<string>();
    }
}
