using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;
using webModel = VirtoCommerce.Platform.Web.Model.ExportImport;
using VirtoCommerce.Platform.Web.Converters.Packaging;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Web.Model.ExportImport;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Model.ExportImport.NotificationEvent;
using VirtoCommerce.Platform.Core.Asset;
using System.Reflection;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using System.IO;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
	[RoutePrefix("api/platform")]
	public class PlatformExportImportController : ApiController
	{
		private readonly IPlatformExportImportManager _platformExportManager;
		private readonly INotifier _eventNotifier;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;
		private readonly IPackageService _packageService;

		public PlatformExportImportController(IPlatformExportImportManager platformExportManager, INotifier eventNotifier, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver, IPackageService packageService)
		{
			_platformExportManager = platformExportManager;
			_eventNotifier = eventNotifier;
			_blobStorageProvider = blobStorageProvider;
			_blobUrlResolver = blobUrlResolver;
			_packageService = packageService;
		}

		 [HttpGet]
		 [ResponseType(typeof(VirtoCommerce.Platform.Web.Model.Packaging.ModuleDescriptor[]))]
		 [Route("export/modules")]
		 public IHttpActionResult GetAllowExportModules()
		 {
			 return Ok(InnerGetModulesWithInterface(typeof(ISupportExportModule)).Select(x => x.ToWebModel()).ToArray());
		 }

		 [HttpGet]
		 [ResponseType(typeof(webModel.ImportInfo))]
		 [Route("import/info")]
		 public IHttpActionResult GetImportInformation([FromUri]string fileUrl)
		 {
			 var retVal = new webModel.ImportInfo();
			 using (var stream = _blobStorageProvider.OpenReadOnly(fileUrl))
			 {
				 retVal.ExportManifest = _platformExportManager.ReadPlatformExportManifest(stream);
				 //First check platform compatibility
				 if (!SemanticVersion.Parse(retVal.ExportManifest.PlatformVersion).IsCompatibleWith(PlatformVersion.CurrentVersion))
				 {
					 throw new InvalidDataException("Imported platform version " + retVal.ExportManifest.PlatformVersion + " not compatible with current " + PlatformVersion.CurrentVersion.ToString());
				 }
				 foreach (var exportModuleInfo in retVal.ExportManifest.Modules)
				 {
					 var installedModule = _packageService.GetModules().FirstOrDefault(x => exportModuleInfo.ModuleId == x.Id);
					 if (installedModule != null)
					 {
						 var moduleDescriptor = installedModule.ToWebModel();
						 retVal.Modules.Add(moduleDescriptor);
						 //Check compatibility
						 if (!SemanticVersion.Parse(moduleDescriptor.Version).IsCompatibleWith(SemanticVersion.Parse(installedModule.Version)))
						 {
							 moduleDescriptor.ValidationErrors.Add("Imported module version " + moduleDescriptor.Version + " not compatible with installed " + installedModule.Version);
						 }
					 }
				 }
			 }
			 return Ok(retVal);
		 }

		 [HttpPost]
		 [ResponseType(typeof(NotifyEvent))]
		 [Route("export")]
		 public IHttpActionResult ProcessExport(PlatformExportImportRequest exportRequest)
		 {
			 var notification = new ExportImportProgressNotificationEvent(CurrentPrincipal.GetCurrentUserName())
			 {
				 Title = "Platform export task",
				 Description = "starting export...."
			 };
			 _eventNotifier.Upsert(notification);
			 var now = DateTime.UtcNow;

			 BackgroundJob.Enqueue(() => PlatformExportBackground(exportRequest, PlatformVersion.CurrentVersion.ToString(), CurrentPrincipal.GetCurrentUserName(), notification));

			 return Ok(notification);
		 }

		 [HttpPost]
		 [ResponseType(typeof(NotifyEvent))]
		 [Route("import")]
		 public IHttpActionResult ProcessImport(PlatformExportImportRequest importRequest)
		 {
			 var notification = new ExportImportProgressNotificationEvent(CurrentPrincipal.GetCurrentUserName())
			 {
				 Title = "Platform import task",
				 Description = "starting import...."
			 };
			 _eventNotifier.Upsert(notification);
			 var now = DateTime.UtcNow;

			 BackgroundJob.Enqueue(() => PlatformImportBackground(importRequest, notification));

			 return Ok(notification);
		 }

		 public void PlatformImportBackground(PlatformExportImportRequest importRequest, ExportImportProgressNotificationEvent notifyEvent)
		 {
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 notifyEvent.InjectFrom(x);
				 _eventNotifier.Upsert(notifyEvent);
			 };

			 var now = DateTime.UtcNow;
			 try
			 {
				 var importedModules = InnerGetModulesWithInterface(typeof(ISupportImportModule)).Where(x => importRequest.Modules.Contains(x.Id)).ToArray();
				 using (var stream = _blobStorageProvider.OpenReadOnly(importRequest.FileUrl))
				 {
					 var options = new PlatformExportImportOptions
					 {
						 Modules = importedModules,
						 PlatformSecurity = importRequest.PlatformSecurity,
						 PlatformSettings = importRequest.PlatformSettings,
					 };
					 _platformExportManager.Import(stream, options, progressCallback);
				 }
			 }
			 catch (Exception ex)
			 {
				 notifyEvent.Errors.Add(ex.ExpandExceptionMessage());
			 }
			 finally
			 {
				 notifyEvent.Finished = DateTime.UtcNow;
				 _eventNotifier.Upsert(notifyEvent);
			 }

		 }

		 public void PlatformExportBackground(PlatformExportImportRequest exportRequest, string platformVersion, string author, ExportImportProgressNotificationEvent notifyEvent)
		 {
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 notifyEvent.InjectFrom(x);
				 _eventNotifier.Upsert(notifyEvent);
			 };

			 var now = DateTime.UtcNow;
			 try
			 {
				 var exportedModules = InnerGetModulesWithInterface(typeof(ISupportExportModule)).Where(x => exportRequest.Modules.Contains(x.Id)).ToArray();
				 using (var stream = new MemoryStream())
				 {
					 var options = new PlatformExportImportOptions
					 {
						 Modules = exportedModules,
						 PlatformSecurity = exportRequest.PlatformSecurity,
						 PlatformSettings = exportRequest.PlatformSettings,
						 PlatformVersion = SemanticVersion.Parse(platformVersion),
						 Author = author
					 };

					 _platformExportManager.Export(stream, options, progressCallback);
					 stream.Seek(0, SeekOrigin.Begin);
					 //Upload result  to blob storage
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
				 }
			 }
			 catch(Exception ex)
			 {
				 notifyEvent.Errors.Add(ex.ExpandExceptionMessage()); 
			 }
			 finally
			 {
				 notifyEvent.Finished = DateTime.UtcNow;
				 _eventNotifier.Upsert(notifyEvent);
			 }

		 }

		 private ModuleDescriptor[] InnerGetModulesWithInterface(Type interfaceType)
		 {
			 var retVal = _packageService.GetModules().Where(x => x.ModuleInfo.ModuleInstance != null)
										 .Where(x => x.ModuleInfo.ModuleInstance.GetType().GetInterfaces().Contains(interfaceType))
										 .ToArray();
			 return retVal;
		 }
	}
}