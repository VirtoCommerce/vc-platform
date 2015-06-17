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

		public void RegisterNotification(Func<Core.Notification.Notification> notification)
		{
			_notifications.Add(notification);
		}

		public Core.Notification.Notification[] GetNotifications()
		{
			return _notifications.Select(x => x()).ToArray();
		}

		SendNotificationResult INotificationManager.SendNotification(Core.Notification.Notification notification)
		{
			var template = _notificationTemplateService.GetByNotification(notification.Type, notification.ObjectId);
			notification.NotificationTemplate = template;

			_resolver.ResolveTemplate(notification);

			var result = notification.NotificationSendingGateway.SendNotification(notification);

			return result;
		}

		public void SheduleSendNotification(Core.Notification.Notification notification)
		{
			var template = _notificationTemplateService.GetByNotification(notification.Type, notification.ObjectId);
			notification.NotificationTemplate = template;

			_resolver.ResolveTemplate(notification);

			_repository.Add(notification.ToDataModel());
			_repository.UnitOfWork.Commit();
		}

		public Core.Notification.Notification GetNotificationById(string id)
		{
			throw new NotImplementedException();
		}

		public T GetNewNotification<T>(string type) where T : Core.Notification.Notification 
		{
			var notifications = GetNotifications();
			return (T) notifications.FirstOrDefault(x => x.GetType().FullName == Type.GetType(type).FullName);
		}

		public T GetNewNotification<T>() where T : Core.Notification.Notification
		{
			var notifications = GetNotifications();
			return (T)notifications.FirstOrDefault(x => x.GetType().FullName == typeof(T).FullName);
		}

		public void UpdateNotification(Core.Notification.Notification notifications)
		{
			throw new NotImplementedException();
		}

		public void DeleteNotification(string id)
		{
			throw new NotImplementedException();
		}

		public SearchNotificatiosnResult SearchNotifications(SearchNotificationCriteria criteria)
		{
			var retVal = new SearchNotificatiosnResult();

			var notifications = _repository.Notifications.Take(criteria.Take).Skip(criteria.Skip);
			foreach(var notification in notifications)
			{
				retVal.Notifications.Add(notification.ToCoreModel());
			}
			retVal.TotalCount = notifications.Count();

			return retVal;
		}
	}
}
