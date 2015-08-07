using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Web.Model.ExportImport.PushNotifications
{
	public class SampleDataImportPushNotification : PlatformExportImportPushNotification
	{
		public SampleDataImportPushNotification(string creator)
			: base(creator)
		{
		}

	}
}