using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
	public class SearchNotificationsResult
	{
		public ICollection<Notification> Notifications { get; set; }
		public int TotalCount { get; set; }
	}
}