using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public class NotificationTemplate : AuditableEntity
    {
        /// <summary>
        /// Id of object that create notification
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Id of notification type
        /// </summary>
        public string NotificationTypeId { get; set; }

        public string Sender { get; set; }
        public string Recipient { get; set; }
        /// <summary>
        /// Subject of notification
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Body of notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Engine that resolve this template
        /// </summary>
        public string TemplateEngine { get; set; }

        public string ObjectTypeId { get; set; }
        public string Language { get; set; }
        public bool IsDefault { get; set; }
    }
}
