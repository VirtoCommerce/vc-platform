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
		private INotificationTemplateService _notificationTemplateService;

		public NotificationManager(INotificationTemplateResolver resolver, IPlatformRepository repository, INotificationTemplateService notificationTemplateService)
		{
			_resolver = resolver;
			_repository = repository;
			_notificationTemplateService = notificationTemplateService;
		}

		private List<Func<Core.Notification.Notification>> _notifications = new List<Func<Core.Notification.Notification>>();
		private List<Func<INotificationSendingGateway>> _gateways = new List<Func<INotificationSendingGateway>>();

		public void SendNotification(Core.Notification.Notification notification)
		{
			var template = _notificationTemplateService.GetByNotification(notification.GetType().FullName, notification.ObjectId);
			notification.NotificationTemplate = template;

			_resolver.ResolveTemplate(notification);

			notification.Id = Guid.NewGuid().ToString("N");
			_repository.Add(notification.ToDataModel());
			_repository.UnitOfWork.Commit();
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
