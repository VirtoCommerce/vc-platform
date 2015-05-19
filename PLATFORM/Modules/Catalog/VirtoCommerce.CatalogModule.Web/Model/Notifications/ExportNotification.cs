using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model.Notifications
{
	public class ExportNotification : NotifyEvent
	{
		public ExportNotification()
			:base("")
		{
			Errors = new List<string>();
		}
		public string DownloadUrl { get; set; }
		public DateTime? Finished { get; set; }
		public long TotalCount { get; set; }
		public long ProcessedCount { get; set; }
		public long ErrorCount { get; set; }
		public ICollection<string> Errors { get; set; }
	}
}