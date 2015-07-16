using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Web.Model.ExportImport.NotificationEvent
{
	public class ExportImportProgressNotificationEvent : NotifyEvent
	{
		public ExportImportProgressNotificationEvent(string creator)
			: base(creator)
		{
			Errors = new List<string>();
		}
		[JsonProperty("finished")]
		public DateTime? Finished { get; set; }
		[JsonProperty("totalCount")]
		public long TotalCount { get; set; }
		[JsonProperty("processedCount")]
		public long ProcessedCount { get; set; }
		[JsonProperty("errorCount")]
		public long ErrorCount
		{
			get
			{
				return Errors != null ? Errors.Count() : 0;
			}
		}
		[JsonProperty("errors")]
		public ICollection<string> Errors { get; set; }
		[JsonProperty("downloadUrl")]
		public string DownloadUrl { get; set; }
	}
}
