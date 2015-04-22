using System;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class NotifySearchCriteria
	{
		public NotifySearchCriteria()
		{
			Count = 20;
		}
		public bool OnlyNew { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int Start { get; set; }
		public int Count { get; set; }
		public string OrderBy { get; set; }
		
	}
}
