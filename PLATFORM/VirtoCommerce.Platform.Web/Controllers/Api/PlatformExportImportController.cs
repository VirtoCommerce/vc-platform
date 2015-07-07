using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.ImportExport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;
using webModel = VirtoCommerce.Platform.Web.Model.Packaging;
using VirtoCommerce.Platform.Web.Converters.Packaging;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Web.Model.ExportImport;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Model.ExportImport.NotificationEvent;
using VirtoCommerce.Platform.Core.Asset;
using System.Reflection;
using Hangfire;
using Omu.ValueInjecter;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
	[RoutePrefix("api")]
	public class PlatformExportImportController : ApiController
	{
		private readonly IPlatformExportImportManager _platformExportManager;
		private readonly INotifier _eventNotifier;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;
		public PlatformExportImportController(IPlatformExportImportManager platformExportManager, INotifier eventNotifier, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver)
		{
			_platformExportManager = platformExportManager;
			_eventNotifier = eventNotifier;
			_blobStorageProvider = blobStorageProvider;
			_blobUrlResolver = blobUrlResolver;
		}

		 [HttpGet]
		 [ResponseType(typeof(webModel.ModuleDescriptor[]))]
		 [Route("{exportImportAction}/modules")]
		 public IHttpActionResult GetModulesForAction(string exportImportAction)
		 {
			 if(exportImportAction == "export")
			 {
				 return Ok(_platformExportManager.GetSupportedExportModules());
			 }

			 return Ok(_platformExportManager.GetSupportedImportModules());
		 }

		 [HttpPost]
		 [ResponseType(typeof(NotifyEvent))]
		 [Route("export")]
		 public IHttpActionResult ProcessExport(PlatformExportRequest exportRequest)
		 {
			 var notification = new ExportImportProgressNotificationEvent(CurrentPrincipal.GetCurrentUserName())
			 {
				 Title = "Platform export task",
				 Description = "starting export...."
			 };
			 _eventNotifier.Upsert(notification);
			 var now = DateTime.UtcNow;

			 var assembly = Assembly.GetExecutingAssembly();
			 var platformVersion = String.Format("{0}.{1}", assembly.GetInformationalVersion(), assembly.GetFileVersion());

			 BackgroundJob.Enqueue(() => PlatformExportBackground(exportRequest, platformVersion, notification));

			 return Ok(notification);
		 }


		 public void PlatformExportBackground(PlatformExportRequest exportRequest, string platformVersion, ExportImportProgressNotificationEvent notifyEvent)
		 {
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 notifyEvent.InjectFrom(x);
				 notifyEvent.Description = x.Status;
				 _eventNotifier.Upsert(notifyEvent);
			 };

			 var now = DateTime.UtcNow;
			 using (var stream = _platformExportManager.Export(exportRequest.Modules, platformVersion, progressCallback))
			 {
				 //Upload result csv to blob storage
				 var uploadInfo = new UploadStreamInfo
				 {
					 FileName = "Platform-" + now.ToString("yyMMddhh") + "-export.zip",
					 FileByteStream = stream,
					 FolderName = "tmp"
				 };
				 var blobKey = _blobStorageProvider.Upload(uploadInfo);
				 //Get a download url
				 notifyEvent.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobKey);
				 notifyEvent.Description = "Export finished";
				 notifyEvent.Finished = DateTime.UtcNow;
				 _eventNotifier.Upsert(notifyEvent);
			 }

		 }
	}
}