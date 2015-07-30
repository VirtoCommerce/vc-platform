namespace VirtoCommerce.Platform.Core.PushNotification
{
    public interface IPushNotificationManager
    {
        void Upsert(PushNotification notification);
        PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

    }
}
