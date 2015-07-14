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
			var notificationType = GetNewNotification(notification().GetType().Name);

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

			var result = notification.SendNotification();

			return result;
		}

		public void SheduleSendNotification(Core.Notification.Notification notification)
		{
			ResolveTemplate(notification);

			using (var repository = _repositoryFactory())
			{
				var addedNotification = notification.ToDataModel();
				repository.Add(addedNotification);
				repository.UnitOfWork.Commit();
			}
		}

		private void ResolveTemplate(Core.Notification.Notification notification)
		{
			GetNewNotification(notification.GetType().Name, notification.ObjectId, notification.ObjectTypeId, notification.Language);
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
			return GetNewNotification(type, null, null, null);
		}

		public Core.Notification.Notification GetNewNotification(string type, string objectId, string objectTypeId, string language)
		{
			var notifications = GetNotifications();
			var retVal = notifications.FirstOrDefault(x => x.GetType().Name == type);
			if (retVal != null)
			{
				retVal.ObjectId = objectId;
				retVal.ObjectTypeId = objectTypeId;
				retVal.Language = language;
				if (retVal != null)
				{
					var template = _notificationTemplateService.GetByNotification(type, objectId, objectTypeId, language);
					if (template != null)
					{
						retVal.NotificationTemplate = template;
					}
					else if (retVal.NotificationTemplate == null)
					{
						retVal.NotificationTemplate = new NotificationTemplate();
					}
				}

				if (retVal.NotificationTemplate != null && string.IsNullOrEmpty(retVal.NotificationTemplate.NotificationTypeId))
				{
					retVal.NotificationTemplate.NotificationTypeId = type;
				}
			}

			return retVal;
		}

		public T GetNewNotification<T>(string objectId, string objectTypeId, string language) where T : Core.Notification.Notification
		{
			var notifications = GetNotifications();
			return GetNewNotification(typeof(T).Name, objectId, objectTypeId, language) as T;
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
				if (!criteria.IsActive)
				{
					var notifications = repository.Notifications.Where(n => n.ObjectId == criteria.ObjectId && n.ObjectTypeId == criteria.ObjectTypeId).OrderBy(n => n.CreatedDate).Skip(criteria.Skip).Take(criteria.Take);
					foreach (var notification in notifications)
					{
						retVal.Notifications.Add(GetNotificationCoreModel(notification));
					}
					retVal.TotalCount = repository.Notifications.Count(n => n.ObjectId == criteria.ObjectId && n.ObjectTypeId == criteria.ObjectTypeId);
				}
				else
				{
					var notifications = repository.Notifications.Where(n => n.IsActive && !n.IsSuccessSend && !n.SentDate.HasValue && (!n.StartSendingDate.HasValue || (n.StartSendingDate.HasValue && n.StartSendingDate.Value < DateTime.UtcNow))).OrderBy(n => n.CreatedBy).Take(criteria.Take);
					foreach(var notification in notifications)
					{
						retVal.Notifications.Add(GetNotificationCoreModel(notification));
					}
				}
			}

			return retVal;
		}

		public Core.Notification.Notification GetNotification(string id)
		{
			Core.Notification.Notification retVal = null;

			using(var repository = _repositoryFactory())
			{
				var notification = repository.Notifications.FirstOrDefault(n => n.Id == id);
				if(notification != null)
				{
					retVal = GetNotificationCoreModel(notification);
				}
			}

			return retVal;
		}

		public void StopSendingNotifications(string[] ids)
		{
			using(var repository = _repositoryFactory())
			{
				foreach(var id in ids)
				{
					var entity = repository.Notifications.FirstOrDefault(n => n.Id == id);
					if(entity != null)
					{
						entity.IsActive = false;
						repository.Update(entity);
					}
				}

				repository.UnitOfWork.Commit();
			}
		}

		private Core.Notification.Notification GetNotificationCoreModel(NotificationEntity entity)
		{
			var retVal = GetNewNotification(entity.Type);
			retVal.InjectFrom(entity);

			return retVal;
		}
	}
}
