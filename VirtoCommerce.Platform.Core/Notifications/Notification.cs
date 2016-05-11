using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public abstract class Notification : AuditableEntity
    {
        private readonly INotificationSendingGateway _notificationSendingGateway;

        protected Notification(INotificationSendingGateway notificationSendingGateway)
        {
            _notificationSendingGateway = notificationSendingGateway;
            MaxAttemptCount = 10;
            Type = GetType().Name;
        }

        public string DisplayName { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Must be made sending
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Notification was successfully sent
        /// </summary>
        public bool IsSuccessSend { get; set; }

        public string Type { get; set; }

        public string ObjectId { get; set; }

        /// <summary>
        /// Subject of notification
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Sender info (e-mail, phone number and etc.) of notification
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Recipient info (e-mail, phone number and etc.) of notification
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// Number of current attempt
        /// </summary>
        public int AttemptCount { get; set; }

        /// <summary>
        /// Maximum number of attempts to send a message
        /// </summary>
        public int MaxAttemptCount { get; set; }

        /// <summary>
        /// Last fail attempt error message
        /// </summary>
        public string LastFailAttemptMessage { get; set; }

        public string SendingGateway { get; set; }

        /// <summary>
        /// Date of last fail attempt
        /// </summary>
        public DateTime? LastFailAttemptDate { get; set; }

        /// <summary>
        /// Date of start sending notification
        /// </summary>
        public DateTime? StartSendingDate { get; set; }

        /// <summary>
        /// Date of success sent result
        /// </summary>
        public DateTime? SentDate { get; set; }

        public string ObjectTypeId { get; set; }

        public string Language { get; set; }

        public INotificationSendingGateway NotificationSendingGateway { get { return _notificationSendingGateway; } }

        public NotificationTemplate NotificationTemplate { get; set; }

        public virtual SendNotificationResult SendNotification()
        {
            var result = NotificationSendingGateway.SendNotification(this);

            return result;
        }
    }
}
