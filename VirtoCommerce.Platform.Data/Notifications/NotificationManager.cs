using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Logging;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationTemplateResolver _resolver;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly INotificationTemplateService _notificationTemplateService;
        private readonly List<Func<Notification>> _notifications = new List<Func<Notification>>();
        private readonly List<Type> _unregisteredNotifications = new List<Type>();

        private readonly ILog Logger = LogManager.GetLogger(typeof(NotificationManager));

        public NotificationManager(INotificationTemplateResolver resolver, Func<IPlatformRepository> repositoryFactory, INotificationTemplateService notificationTemplateService)
        {
            _resolver = resolver;
            _repositoryFactory = repositoryFactory;
            _notificationTemplateService = notificationTemplateService;
        }


        public void OverrideNotificationType<T>(Func<Notification> notificationFactory)
        {
            var replacedNotification = _notifications.FirstOrDefault(x => x().GetType() == typeof(T));
            if (replacedNotification != null)
            {
                var index = _notifications.IndexOf(replacedNotification);
                _notifications[index] = notificationFactory;
            }
        }

        public void RegisterNotificationType(Func<Notification> notificationFactory)
        {
            var notifications = GetNotifications();
            var notification = notificationFactory();
            if (!notifications.Any(x => notification.Type.EqualsInvariant(x.GetType().Name)))
            {
                _notifications.Add(notificationFactory);
            }
        }

        public void UnregisterNotificationType<T>()
        {
            if (!_unregisteredNotifications.Contains(typeof(T)))
            {
                _unregisteredNotifications.Add(typeof(T));
            }
        }

        public Notification[] GetNotifications()
        {
            return _notifications.Select(x => x()).ToArray();
        }

        SendNotificationResult INotificationManager.SendNotification(Notification notification)
        {
            var result = new SendNotificationResult();
            //Do not send unregistered notifications
            if (!_unregisteredNotifications.Any(x => x.IsAssignableFrom(notification.GetType())))
            {
                ResolveTemplate(notification);
                result = notification.SendNotification();
            }
            return result;
        }

        public void ScheduleSendNotification(Notification notification)
        {
            //Do not send unregistered notifications
            if (!_unregisteredNotifications.Any(x => x.IsAssignableFrom(notification.GetType())))
            {
                ResolveTemplate(notification);

                using (var repository = _repositoryFactory())
                {
                    var addedNotification = notification.ToDataModel();
                    repository.Add(addedNotification);
                    repository.UnitOfWork.Commit();
                }
            }
        }

        private void ResolveTemplate(Notification notification)
        {
            _resolver.ResolveTemplate(notification);
        }

        public Notification GetNotificationById(string id)
        {
            Notification retVal = null;
            using (var repository = _repositoryFactory())
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

        public Notification GetNewNotification(string type)
        {
            return GetNewNotification(type, null, null, null);
        }

        public Notification GetNewNotification(string type, string objectId, string objectTypeId, string language)
        {
            var notifications = GetNotifications();
            var retVal = notifications.FirstOrDefault(x => x.GetType().Name == type);
            if (retVal == null)
            {
                //try to find in derived types
                retVal = notifications.FirstOrDefault(x => x.GetType().GetTypeInheritanceChain().Select(y => y.Name).Contains(type));
            }
            if (retVal == null)
            {
                throw new InvalidOperationException($"Notification {type} not found. Please register this type by notificationManager.RegisterNotificationType before use");
            }

            retVal.ObjectId = objectId;
            retVal.ObjectTypeId = objectTypeId;
            retVal.Language = language;

            var notificationTypeName = retVal.GetType().Name;
            var template = _notificationTemplateService.GetByNotification(notificationTypeName, objectId, objectTypeId, language);

            if (template != null)
            {
                retVal.NotificationTemplate = template;
            }
            else if (retVal.NotificationTemplate == null)
            {
                retVal.NotificationTemplate = new NotificationTemplate();
            }

            if (retVal.NotificationTemplate != null && string.IsNullOrEmpty(retVal.NotificationTemplate.NotificationTypeId))
            {
                retVal.NotificationTemplate.NotificationTypeId = type;
            }

            return retVal;
        }

        public T GetNewNotification<T>(string objectId, string objectTypeId, string language) where T : Notification
        {
            return GetNewNotification(typeof(T).Name, objectId, objectTypeId, language) as T;
        }

        public void UpdateNotification(Notification notification)
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
                if (deletedEntity != null)
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
                retVal.Notifications = new List<Notification>();

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

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "CreatedDate", SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id);

                retVal.Notifications = query.Skip(criteria.Skip)
                                            .Take(criteria.Take)
                                            .ToArray()
                                            .Select(GetNotificationCoreModel)
                                            .ToList();
            }

            return retVal;
        }

        public void StopSendingNotifications(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var entity = repository.Notifications.FirstOrDefault(n => n.Id == id);
                    if (entity != null)
                    {
                        entity.IsActive = false;
                        repository.Update(entity);
                    }
                }

                repository.UnitOfWork.Commit();
            }
        }


        private Notification GetNotificationCoreModel(NotificationEntity entity)
        {
            var retVal = GetNewNotification(entity.Type);

            // Type may have been unregistered by now. 
            retVal?.InjectFrom(entity);

            if (retVal is EmailNotification emailNotification)
            {
                emailNotification.CC = entity.Сс?.Split(',');
                emailNotification.Bcc = entity.Bcс?.Split(',');
            }

            return retVal;
        }
    }
}
