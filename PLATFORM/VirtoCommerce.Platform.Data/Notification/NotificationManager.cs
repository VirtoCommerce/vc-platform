using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Data.Repositories;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class NotificationManager : INotificationManager
	{
		private INotificationTemplateResolver _resolver;
		private Func<IPlatformRepository> _repositoryFactory;
		private INotificationTemplateService _notificationTemplateService;

		public NotificationManager(INotificationTemplateResolver resolver, Func<IPlatformRepository> repositoryFactory, INotificationTemplateService notificationTemplateService)
		{
			_resolver = resolver;
			_repositoryFactory = repositoryFactory;
			_notificationTemplateService = notificationTemplateService;
		}

		private List<Func<Core.Notification.Notification>> _notifications = new List<Func<Core.Notification.Notification>>();
		private List<Func<INotificationSendingGateway>> _gateways = new List<Func<INotificationSendingGateway>>();

		public void RegisterNotificationType(Func<Core.Notification.Notification> notification)
		{
			var notificationType = GetNewNotification(notification().Type);

			if (notificationType == null)
			{
				_notifications.Add(notification);
			}
		}

		public Core.Notification.Notification[] GetNotifications()
		{
			return _notifications.Select(x => x()).ToArray();
		}

		SendNotificationResult INotificationManager.SendNotification(Core.Notification.Notification notification)
		{
			ResolveTemplate(notification);

			var result = notification.NotificationSendingGateway.SendNotification(notification);

			return result;
		}

		public void SheduleSendNotification(Core.Notification.Notification notification)
		{
			ResolveTemplate(notification);

			using (var repository = _repositoryFactory())
			{
				notification.Id = Guid.NewGuid().ToString("N");
				repository.Add(notification.ToDataModel());
				repository.UnitOfWork.Commit();
			}
		}

		private void ResolveTemplate(Core.Notification.Notification notification)
		{
			var template = _notificationTemplateService.GetByNotification(notification.Type, notification.ObjectId);
			notification.NotificationTemplate = template;

			_resolver.ResolveTemplate(notification);
		}

		public Core.Notification.Notification GetNotificationById(string id)
		{
			Core.Notification.Notification retVal = null;
			using(var repository = _repositoryFactory())
			{
				var notificationEntity = repository.Notifications.FirstOrDefault(x => x.Id == id);
				if (notificationEntity != null)
				{
					retVal = GetNotificationCoreModel(notificationEntity);
				}
			}

			return retVal;
		}

		public Core.Notification.Notification GetNewNotification(string type)
		{
			var notifications = GetNotifications();
			return notifications.FirstOrDefault(x => x.Type == type);
		}

		public T GetNewNotification<T>() where T : Core.Notification.Notification
		{
			var notifications = GetNotifications();
			return (T)notifications.FirstOrDefault(x => x.GetType().Name == typeof(T).Name);
		}

		public void UpdateNotification(Core.Notification.Notification notification)
		{
			using (var repository = _repositoryFactory())
			{
				repository.Update(notification.ToDataModel());
				repository.UnitOfWork.Commit();
			}
		}

		public void DeleteNotification(string id)
		{
			using (var repository = _repositoryFactory())
			{
				var deletedEntity = repository.Notifications.FirstOrDefault(x => x.Id == id);
				if(deletedEntity != null)
				{
					repository.Remove(deletedEntity);
					repository.UnitOfWork.Commit();
				}
			}
		}

		public SearchNotificationsResult SearchNotifications(SearchNotificationCriteria criteria)
		{
			var retVal = new SearchNotificationsResult();

			using (var repository = _repositoryFactory())
			{
				retVal.Notifications = new List<Core.Notification.Notification>();
				var notifications = repository.Notifications.Take(criteria.Take).Skip(criteria.Skip);
				foreach (var notification in notifications)
				{
					retVal.Notifications.Add(GetNotificationCoreModel(notification));
				}
				retVal.TotalCount = notifications.Count();
			}

			return retVal;
		}

		private Core.Notification.Notification GetNotificationCoreModel(NotificationEntity entity)
		{
			var retVal = GetNewNotification(entity.Type);
			retVal.InjectFrom(entity);

			return retVal;
		}
	}
}
