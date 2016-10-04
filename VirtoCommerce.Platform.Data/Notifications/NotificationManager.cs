using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Repositories;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace VirtoCommerce.Platform.Data.Notifications
{
	public class NotificationManager : INotificationManager
	{
		private INotificationTemplateResolver _resolver;
		private Func<IPlatformRepository> _repositoryFactory;
		private INotificationTemplateService _notificationTemplateService;
        private List<Func<Core.Notifications.Notification>> _notifications = new List<Func<Core.Notifications.Notification>>();
        private List<Func<INotificationSendingGateway>> _gateways = new List<Func<INotificationSendingGateway>>();

        public NotificationManager(INotificationTemplateResolver resolver, Func<IPlatformRepository> repositoryFactory, INotificationTemplateService notificationTemplateService)
		{
			_resolver = resolver;
			_repositoryFactory = repositoryFactory;
			_notificationTemplateService = notificationTemplateService;
		}

		
        public void OverrideNotificationType<T>(Func<Notification> notificationFactory)
        {
            var replacedNotification = _notifications.FirstOrDefault(x => x().GetType() == typeof(T));
            if(replacedNotification != null)
            {
                var index = _notifications.IndexOf(replacedNotification);
                _notifications[index] = notificationFactory;
            }
        }

        public void RegisterNotificationType(Func<Core.Notifications.Notification> notificationFactory)
		{
            var notifications = GetNotifications();
            var notification = notificationFactory();
            if (!notifications.Any(x => String.Equals(x.GetType().Name, notification.Type, StringComparison.InvariantCultureIgnoreCase)))
			{
				_notifications.Add(notificationFactory);
			}
		}

		public Core.Notifications.Notification[] GetNotifications()
		{
			return _notifications.Select(x => x()).ToArray();
		}

		SendNotificationResult INotificationManager.SendNotification(Core.Notifications.Notification notification)
		{
			ResolveTemplate(notification);

			var result = notification.SendNotification();

			return result;
		}

		public void ScheduleSendNotification(Core.Notifications.Notification notification)
		{
			ResolveTemplate(notification);

			using (var repository = _repositoryFactory())
			{
				var addedNotification = notification.ToDataModel();
				repository.Add(addedNotification);
				repository.UnitOfWork.Commit();
			}
		}

		private void ResolveTemplate(Core.Notifications.Notification notification)
		{
			_resolver.ResolveTemplate(notification);
		}

		public Core.Notifications.Notification GetNotificationById(string id)
		{
			Core.Notifications.Notification retVal = null;
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

        public T GetNewNotification<T>() where T : Notification
        {
            return GetNewNotification<T>(null, null, null);
        }

        public Core.Notifications.Notification GetNewNotification(string type)
		{
			return GetNewNotification(type, null, null, null);
		}

        public Core.Notifications.Notification GetNewNotification(string type, string objectId, string objectTypeId, string language)
        {
            var notifications = GetNotifications();
            var retVal = notifications.FirstOrDefault(x => x.GetType().Name == type);
            if (retVal == null)
            {
                //try to find in derived types
                retVal = notifications.FirstOrDefault(x => x.GetType().GetTypeInheritanceChain().Select(y => y.Name).Contains(type));
            }
            if(retVal == null)
            {
                throw new NullReferenceException("Notification  " + type + " not found. Please register this type by notificationManager.RegisterNotificationType before use");
            }

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

            return retVal;
        }

        public T GetNewNotification<T>(string objectId, string objectTypeId, string language) where T : Core.Notifications.Notification
		{
			return GetNewNotification(typeof(T).Name, objectId, objectTypeId, language) as T;
		}

		public void UpdateNotification(Core.Notifications.Notification notification)
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
                retVal.Notifications = new List<Core.Notifications.Notification>();

                var query = repository.Notifications;

                if (!string.IsNullOrEmpty(criteria.ObjectId))
                {
                    query = query.Where(n => n.ObjectId == criteria.ObjectId);
                }
                if (!string.IsNullOrEmpty(criteria.ObjectTypeId))
                {
                    query = query.Where(n => n.ObjectTypeId == criteria.ObjectTypeId);
                }
                
                if (criteria.IsActive)
                {                   
                    query = query.Where(n => n.IsActive && n.SentDate == null && (n.LastFailAttemptDate == null || DbFunctions.AddHours(n.LastFailAttemptDate, criteria.RepeatHoursIntervalForFail) < DateTime.UtcNow)
                                             && (n.StartSendingDate == null || n.StartSendingDate < DateTime.UtcNow));
                }
                retVal.TotalCount = query.Count();
                retVal.Notifications = query.OrderBy(n => n.CreatedDate)
                                            .Skip(criteria.Skip)
                                            .Take(criteria.Take)
                                            .ToArray()
                                            .Select(x => GetNotificationCoreModel(x))
                                            .ToList();
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


        private Core.Notifications.Notification GetNotificationCoreModel(NotificationEntity entity)
		{
			var retVal = GetNewNotification(entity.Type);
			retVal.InjectFrom(entity);

			return retVal;
		}
    }
}
