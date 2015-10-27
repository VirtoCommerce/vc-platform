using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class SampleNotification : EmailNotification
    {
        public SampleNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

        [NotificationParameter("Sample string property")]
        public string SampleStringProperty { get; set; }

        [NotificationParameter("Sample dictionary property")]
        public IDictionary SampleDictionaryProperty { get; set; }
    }
}
