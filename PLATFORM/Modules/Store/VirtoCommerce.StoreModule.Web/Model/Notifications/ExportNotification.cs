using Newtonsoft.Json;

namespace VirtoCommerce.StoreModule.Web.Model.Notifications
{
	public class ExportNotification : JobNotificationBase
	{
		public ExportNotification(string creator)
			: base(creator)
		{
			NotifyType = "StoreExport";
		}

		[JsonProperty("downloadUrl")]
		public string DownloadUrl { get; set; }
		
	}
}