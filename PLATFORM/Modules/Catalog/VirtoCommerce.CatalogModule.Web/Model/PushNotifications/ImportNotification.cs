using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.CatalogModule.Web.Model.PushNotifications
{
    /// <summary>
    ///  Notification for catalog data import job.
    /// </summary>
	public class ImportNotification : JobNotificationBase
	{
		public ImportNotification(string creator)
			: base(creator)
		{
			NotifyType = "CatalogCsvImport";
		}
	}
}
