using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class DefaultEmailNotificationSendingGateway : INotificationSendingGateway
	{
		public void SendNotification(Core.Notification.Notification notification)
		{
			throw new NotImplementedException();
		}

		public NotificationSendingGatewayType Type { get { return NotificationSendingGatewayType.Email; } }
	}
}
