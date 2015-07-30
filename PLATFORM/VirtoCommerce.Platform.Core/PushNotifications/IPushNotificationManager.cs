namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public interface IPushNotificationManager
    {
        void Upsert(PushNotification notification);
        PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

    }
}
