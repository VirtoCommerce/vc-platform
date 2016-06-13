using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Web.Model.ExportImport.PushNotifications
{
	public class PlatformExportPushNotification : PlatformExportImportPushNotification
	{
		public PlatformExportPushNotification(string creator)
			: base(creator)
		{
		}

		[JsonProperty("downloadUrl")]
		public string DownloadUrl { get; set; }
	}
}