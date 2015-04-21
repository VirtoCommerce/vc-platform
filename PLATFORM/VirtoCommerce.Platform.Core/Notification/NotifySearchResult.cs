using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class NotifySearchResult
	{
		public NotifySearchResult()
        {
			NotifyEvents = new List<NotifyEvent>();
        }
        public int TotalCount { get; set; }
		public int NewCount { get; set; }
		public List<NotifyEvent> NotifyEvents { get; set; }
	}
}
