namespace VirtoCommerce.Platform.Core.PushNotification
{
	public interface IPushNotificationManager
	{
		void Upsert(PushNotification notification);
		PushNotifcationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

	}
}
