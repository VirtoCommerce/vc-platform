using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
