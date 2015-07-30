using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class SearchNotificationCriteria
	{
		public int Take { get; set; }
		public int Skip { get; set; }
		public string SortOrder { get; set; }
		public string ObjectId { get; set; }
		public string ObjectTypeId { get; set; }
		public bool IsActive { get; set; }
	}
}
