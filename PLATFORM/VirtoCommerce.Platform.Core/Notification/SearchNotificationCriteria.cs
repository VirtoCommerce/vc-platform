using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class SearchNotificationCriteria
	{
		public int Take { get; set; }
		public int Skip { get; set; }
		public string SortOrder { get; set; }
	}
}
