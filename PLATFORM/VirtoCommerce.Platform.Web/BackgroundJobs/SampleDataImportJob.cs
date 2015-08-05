using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.Model.ExportImport.PushNotifications;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.BackgroundJobs
{
	public class SampleDataImportJob
	{
		private readonly IPlatformExportImportManager _exportImportManager;
		private readonly IPushNotificationManager _pushNotifier;

		public SampleDataImportJob(IPlatformExportImportManager exportImportManager, IPushNotificationManager pushNotifier)
		{
			_exportImportManager = exportImportManager;
			_pushNotifier = pushNotifier;
		}

		public void ImportSampleData(string filePath, SampleDataImportPushNotification pushNotification)
		{
			Action<ExportImportProgressInfo> progressCallback = (x) =>
			{
				pushNotification.InjectFrom(x);
				_pushNotifier.Upsert(pushNotification);
			};
			try
			{
				using (var stream = File.Open(filePath, FileMode.Open))
				{
					var manifest = _exportImportManager.ReadExportManifest(stream);
					if (manifest != null)
					{
						_exportImportManager.Import(stream, manifest, progressCallback);
					}
				}
			}
			catch (Exception ex)
			{
				pushNotification.Errors.Add(ex.ExpandExceptionMessage());
			}
			finally
			{
				pushNotification.Finished = DateTime.UtcNow;
				_pushNotifier.Upsert(pushNotification);

			}

		}
	}
}