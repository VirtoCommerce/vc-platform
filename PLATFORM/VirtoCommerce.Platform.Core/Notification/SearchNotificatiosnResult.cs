using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class SearchNotificatiosnResult
	{
		private Collection<Notification> _notifications;
		public Collection<Notification> Notifications { get { return _notifications ?? (_notifications = new Collection<Notification>()); } }

		public int TotalCount { get; set; }
	}
}
