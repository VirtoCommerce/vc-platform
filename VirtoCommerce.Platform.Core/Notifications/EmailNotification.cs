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
	}
}
