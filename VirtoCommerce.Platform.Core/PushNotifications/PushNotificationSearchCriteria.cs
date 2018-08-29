using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
	public class PushNotificationSearchCriteria : ValueObject
	{
		public PushNotificationSearchCriteria()
		{
			Count = 20;
		}
		public string[] Ids { get; set; }
		public bool OnlyNew { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int Start { get; set; }
		public int Count { get; set; }
		public string OrderBy { get; set; }
	}
}
