using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model.Notifications
{
	public class ImportNotification : JobNotificationBase
	{
		public ImportNotification(string creator)
			: base(creator)
		{
			NotifyType = "CatalogCsvImport";
		}
	}
}
