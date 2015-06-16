using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class NotificationManager : INotificationManager
	{
		private INotificationTemplateResolver _resolver;
		private IPlatformRepository _repository;

		public NotificationManager(INotificationTemplateResolver resolver, IPlatformRepository repository)
		{
			_resolver = resolver;
			_repository = repository;
		}

		private List<Func<Core.Notification.Notification>> _notifications = new List<Func<Core.Notification.Notification>>();
		private List<Func<INotificationSendingGateway>> _gateways = new List<Func<INotificationSendingGateway>>();

		public void SendNotification(Core.Notification.Notification notification)
		{
			_resolver.ResolveBody(notification);
			_resolver.ResolveSubject(notification);

			_repository.Add(notification.ToDataModel());
		}

		public void RegisterNotification(Func<Core.Notification.Notification> notification)
		{
			_notifications.Add(notification);
		}

		public Core.Notification.Notification[] GetNotifications()
		{
			return _notifications.Select(x => x()).ToArray();
		}

		public void RegisterNotificationSendingGateway(Func<INotificationSendingGateway> notificationSendingGateway)
		{
			_gateways.Add(notificationSendingGateway);
		}

		public INotificationSendingGateway[] GetNotificationSendingGateways()
		{
			return _gateways.Select(x => x()).ToArray();
		}
	}
}
