using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
	public class DefaultSmsNotificationSendingGateway : ISmsNotificationSendingGateway
	{
		public SendNotificationResult SendNotification(Core.Notifications.Notification notification)
		{
			throw new NotImplementedException();
		}

		public bool ValidateNotification(Core.Notifications.Notification notification)
		{
			throw new NotImplementedException();
		}
	}
}
