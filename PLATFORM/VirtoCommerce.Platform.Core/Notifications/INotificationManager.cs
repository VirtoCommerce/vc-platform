using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public interface INotificationManager
    {
        SendNotificationResult SendNotification(Notification notification);
        void ScheduleSendNotification(Notification notification);
        Notification GetNotificationById(string id);
        void StopSendingNotifications(string[] ids);
        T GetNewNotification<T>() where T : Core.Notifications.Notification;
        Core.Notifications.Notification GetNewNotification(string type);
        T GetNewNotification<T>(string objectId, string objectTypeId, string language) where T : Core.Notifications.Notification;
        Core.Notifications.Notification GetNewNotification(string type, string objectId, string objectTypeId, string language);
        void UpdateNotification(Notification notification);
        void DeleteNotification(string id);
        SearchNotificationsResult SearchNotifications(SearchNotificationCriteria criteria);
        void RegisterNotificationType(Func<Notification> notification);
        Notification[] GetNotifications();
    }
}
