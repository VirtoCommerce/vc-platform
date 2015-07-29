using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.PushNotification
{
	public class PushNotifcationSearchResult
	{
		public PushNotifcationSearchResult()
        {
			NotifyEvents = new List<PushNotification>();
        }
        public int TotalCount { get; set; }
		public int NewCount { get; set; }
		public List<PushNotification> NotifyEvents { get; set; }
	}
}
