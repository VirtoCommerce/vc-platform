using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public interface IPushNotificationStorage
    {
        void SavePushNotification(PushNotification notification);
        Task SavePushNotificationAsync(PushNotification notification);
        PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria);
    }
}
