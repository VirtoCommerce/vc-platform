using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public interface IPushNotificationManager
    {
        void Send(PushNotification notification);
        Task SendAsync(PushNotification notification);
        /// <summary>
        /// You shouldn't expect notification manager will return notifications with the same type as you put them in Send or SendAsync
        /// </summary>
        PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

    }
}
