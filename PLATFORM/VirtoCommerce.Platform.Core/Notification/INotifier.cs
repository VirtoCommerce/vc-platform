namespace VirtoCommerce.Platform.Core.Notification
{
	public interface INotifier
	{
		void Upsert(NotifyEvent notify);
		NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria);

	}
}
