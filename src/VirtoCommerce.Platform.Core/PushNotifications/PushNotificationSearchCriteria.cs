using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
	public class PushNotificationSearchCriteria : SearchCriteriaBase
	{
		public string[] Ids { get; set; }
		public bool OnlyNew { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		
	}
}
