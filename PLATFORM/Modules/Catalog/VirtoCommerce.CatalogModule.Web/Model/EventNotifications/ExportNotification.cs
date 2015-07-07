using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model.EventNotifications
{
	public class ExportNotification : JobNotificationBase
	{
		public ExportNotification(string creator)
			: base(creator)
		{
			NotifyType = "CatalogCsvExport";
		}

		[JsonProperty("downloadUrl")]
		public string DownloadUrl { get; set; }
		
	}
}