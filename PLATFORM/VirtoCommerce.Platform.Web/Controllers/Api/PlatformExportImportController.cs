using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Web.Model.ExportImport;
using VirtoCommerce.Platform.Web.Model.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Web.Converters.ExportImport;
using VirtoCommerce.Platform.Core.Settings;
using System.Configuration;
using System.Web.Hosting;
using System.Net;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
	[RoutePrefix("api/platform")]
    [ApiExplorerSettings(IgnoreApi = true)]
	public class PlatformExportImportController : ApiController
	{
		private readonly IPlatformExportImportManager _platformExportManager;
		private readonly IPushNotificationManager _pushNotifier;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;
		private readonly ISettingsManager _settingsManager;

		public PlatformExportImportController(IPlatformExportImportManager platformExportManager, IPushNotificationManager pushNotifier, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver, ISettingsManager settingManager)
		{
			_platformExportManager = platformExportManager;
			_pushNotifier = pushNotifier;
			_blobStorageProvider = blobStorageProvider;
			_blobUrlResolver = blobUrlResolver;
			_settingsManager = settingManager;
		}

		[HttpGet]
		[ResponseType(typeof(SampleDataImportPushNotification))]
		[Route("sampledata/import")]
		public IHttpActionResult TryToImportSampleData()
		{
			//Sample data initialization
			var sampleDataPath = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:SampleDataPath", string.Empty);
			if (!String.IsNullOrEmpty(sampleDataPath) && !_settingsManager.GetValue("VirtoCommerce:SampleDataInstalled", false))
			{
				_settingsManager.SetValue("VirtoCommerce:SampleDataInstalled", true);

				var pushNotification = new SampleDataImportPushNotification("System");
				_pushNotifier.Upsert(pushNotification);
				BackgroundJob.Enqueue(() => SampleDataImportBackground(HostingEnvironment.MapPath(sampleDataPath), pushNotification));

				return Ok(pushNotification);
			}
			return StatusCode(HttpStatusCode.NoContent);
		}

		 [HttpGet]
		 [ResponseType(typeof(PlatformExportManifest))]
		 [Route("export/manifest/new")]
		 public IHttpActionResult GetNewExportManifest()
		 {
			 return Ok(_platformExportManager.GetNewExportManifest());
		 }

		 [HttpGet]
		 [ResponseType(typeof(PlatformExportManifest))]
		 [Route("export/manifest/load")]
		 public IHttpActionResult LoadExportManifest([FromUri]string fileUrl)
		 {
			 PlatformExportManifest retVal = null;
			 using (var stream = _blobStorageProvider.OpenReadOnly(fileUrl))
			 {
				 retVal = _platformExportManager.ReadExportManifest(stream);
			 }
			 return Ok(retVal);
		 }

		 [HttpPost]
		 [ResponseType(typeof(PushNotification))]
		 [Route("export")]
		 public IHttpActionResult ProcessExport(PlatformImportExportRequest exportRequest)
		 {
			 var notification = new PlatformExportPushNotification(CurrentPrincipal.GetCurrentUserName())
			 {
				 Title = "Platform export task",
				 Description = "starting export...."
			 };
			 _pushNotifier.Upsert(notification);
			 var now = DateTime.UtcNow;

			 BackgroundJob.Enqueue(() => PlatformExportBackground(exportRequest, notification));

			 return Ok(notification);
		 }

		 [HttpPost]
		 [ResponseType(typeof(PushNotification))]
		 [Route("import")]
		 public IHttpActionResult ProcessImport(PlatformImportExportRequest importRequest)
		 {
			 var notification = new PlatformImportPushNotification(CurrentPrincipal.GetCurrentUserName())
			 {
				 Title = "Platform import task",
				 Description = "starting import...."
			 };
			 _pushNotifier.Upsert(notification);
			 var now = DateTime.UtcNow;

			 BackgroundJob.Enqueue(() => PlatformImportBackground(importRequest, notification));

			 return Ok(notification);
		 }

		 public void SampleDataImportBackground(string filePath, SampleDataImportPushNotification pushNotification)
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
					 var manifest = _platformExportManager.ReadExportManifest(stream);
					 if (manifest != null)
					 {
						 _platformExportManager.Import(stream, manifest, progressCallback);
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

		 public void PlatformImportBackground(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification)
		 {
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 pushNotification.InjectFrom(x);
				 pushNotification.Errors = x.Errors;
				 _pushNotifier.Upsert(pushNotification);
			 };

			 var now = DateTime.UtcNow;
			 try
			 {
				 using (var stream = _blobStorageProvider.OpenReadOnly(importRequest.FileUrl))
				 {
					 var manifest = importRequest.ToManifest();
					 manifest.Created = now;
					 _platformExportManager.Import(stream, manifest, progressCallback);
				 }
				 pushNotification.Description = "Import finished";
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

		 public void PlatformExportBackground(PlatformImportExportRequest exportRequest, PlatformExportPushNotification pushNotification)
		 {
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 pushNotification.InjectFrom(x);
				 pushNotification.Errors = x.Errors;
				 _pushNotifier.Upsert(pushNotification);
			 };

			 try
			 {
				 using (var stream = new MemoryStream())
				 {
					 var manifest = exportRequest.ToManifest();
					 _platformExportManager.Export(stream, manifest, progressCallback);
					 stream.Seek(0, SeekOrigin.Begin);
					 //Upload result  to blob storage
					 var uploadInfo = new UploadStreamInfo
					 {
                         FileName = string.Format("Export (UTC {0}).zip", DateTime.UtcNow.ToString("yy-MM-dd hh-mm")),
						 FileByteStream = stream,
						 FolderName = "tmp"
					 };
					 var blobKey = _blobStorageProvider.Upload(uploadInfo);
					 //Get a download url
					 pushNotification.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobKey);
					 pushNotification.Description = "Export finished";
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