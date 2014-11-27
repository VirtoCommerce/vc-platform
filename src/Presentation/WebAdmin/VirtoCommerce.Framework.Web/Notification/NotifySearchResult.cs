using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Web.Notification
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
