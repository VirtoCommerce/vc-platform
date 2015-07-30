using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class SearchNotificationsResult
	{
		public ICollection<Notification> Notifications { get; set; }
		public int TotalCount { get; set; }
	}
}
