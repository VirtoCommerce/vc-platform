using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public interface INotificationManager
	{
		void SendNotification(Notification notification);
		void RegisterNotification(Func<Notification> notification);
		Notification[] GetNotifications();

		void RegisterNotificationSendingGateway(Func<INotificationSendingGateway> notificationSendingGateway);
		INotificationSendingGateway[] GetNotificationSendingGateways();
	}
}
