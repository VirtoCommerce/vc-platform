using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using CsvHelper;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.CatalogModule.Web.Model.EventNotifications;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog")]
	public class CatalogModuleExportImportController : ApiController
	{
		private readonly ICatalogService _catalogService;
		private readonly INotifier _notifier;
		private readonly ISettingsManager _settingsManager;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly CsvCatalogExporter _csvExporter;
		private readonly CsvCatalogImporter _csvImporter;
		private readonly IBlobUrlResolver _blobUrlResolver;

		public CatalogModuleExportImportController(ICatalogService catalogService, INotifier notifier, ISettingsManager settingsManager, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver, CsvCatalogExporter csvExporter, CsvCatalogImporter csvImporter)
		{
			_catalogService = catalogService;
			_notifier = notifier;
			_settingsManager = settingsManager;
			_blobStorageProvider = blobStorageProvider;
			_csvExporter = csvExporter;
			_csvImporter = csvImporter;
			_blobUrlResolver = blobUrlResolver;
		}

		/// <summary>
		/// GET api/catalog/export/sony
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[ResponseType(typeof(ExportNotification))]
		[HttpPost]
		[Route("export")]
		public IHttpActionResult DoExport(CsvExportInfo exportInfo)
		{
			var notification = new ExportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Catalog export task",
				Description = "starting export...."
			};
			_notifier.Upsert(notification);

		
			BackgroundJob.Enqueue(() => BackgroundExport(exportInfo, notification));

			return Ok(notification);

		}


		/// <summary>
		/// GET api/catalog/import/mapping?path='c:\\sss.csv'&importType=product&delimiter=,
		/// </summary>
		/// <param name="templatePath"></param>
		/// <param name="importerType"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		[ResponseType(typeof(CsvProductMappingConfiguration))]
		[HttpGet]
		[Route("import/mappingconfiguration")]
		public IHttpActionResult GetMappingConfiguration([FromUri]string fileUrl, [FromUri]string delimiter = ";")
		{
			var retVal = CsvProductMappingConfiguration.GetDefaultConfiguration();

			retVal.Delimiter = delimiter;

			//Read csv headers and try to auto map fields by name
			using (var reader = new CsvReader(new StreamReader(_blobStorageProvider.OpenReadOnly(fileUrl))))
			{
				reader.Configuration.Delimiter = delimiter;
				if(reader.Read())
				{
					retVal.AutoMap(reader.FieldHeaders);
				}
			}

			return Ok(retVal);
		}


		/// <summary>
		/// GET api/catalog/import/sony
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[ResponseType(typeof(ImportNotification))]
		[HttpPost]
		[Route("import")]
		public IHttpActionResult DoImport(CsvImportInfo importInfo)
		{
			var notification = new ImportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Import catalog from CSV",
				Description = "starting import...."
			};
			_notifier.Upsert(notification);

			BackgroundJob.Enqueue(() => BackgroundImport(importInfo, notification));

			return Ok(notification);
		}


		/// <summary>
		///  GET api/catalog/importjobs/123/cancel
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}/cancel")]
		[ResponseType(typeof(void))]
		public IHttpActionResult Cancel(string id)
		{
			return StatusCode(HttpStatusCode.NoContent);
			//var job = _jobList.FirstOrDefault(x => x.Id == id);
			//if (job != null && job.CanBeCanceled)
			//{
			//	job.CancellationToken.Cancel();
			//}

			//return StatusCode(HttpStatusCode.NoContent);
		}
		public void BackgroundImport(CsvImportInfo importInfo, ImportNotification notifyEvent)
		{
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 notifyEvent.InjectFrom(x);
				 _notifier.Upsert(notifyEvent);
			 };

			 using (var stream = _blobStorageProvider.OpenReadOnly(importInfo.FileUrl))
			{
				try
				{
					_csvImporter.DoImport(stream, importInfo, progressCallback);
				}
				catch(Exception ex)
				{
					notifyEvent.Description = "Export error";
					notifyEvent.ErrorCount++;
					notifyEvent.Errors.Add(ex.ToString());
				}
				finally
				{
					notifyEvent.Finished = DateTime.UtcNow;
					notifyEvent.Description = "Import finished" + (notifyEvent.Errors.Any() ? " with errors" : " successfully");
					_notifier.Upsert(notifyEvent);
				}
			}
		}

		public void BackgroundExport(CsvExportInfo exportInfo, ExportNotification notifyEvent)
		{
			var curencySetting = _settingsManager.GetSettingByName("VirtoCommerce.Core.General.Currencies");
			var defaultCurrency = EnumUtility.SafeParse<CurrencyCodes>(curencySetting.DefaultValue, CurrencyCodes.USD);
			exportInfo.Currency = exportInfo.Currency ?? defaultCurrency;
			var catalog = _catalogService.GetById(exportInfo.CatalogId);
			if (catalog == null)
			{
				throw new NullReferenceException("catalog");
			}
			
			 Action<ExportImportProgressInfo> progressCallback = (x) =>
			 {
				 notifyEvent.InjectFrom(x);
				 _notifier.Upsert(notifyEvent);
			 };

			 using (var stream = new MemoryStream())
			 {
				 try
				 {
					 exportInfo.Configuration = CsvProductMappingConfiguration.GetDefaultConfiguration();
					 _csvExporter.DoExport(stream, exportInfo, progressCallback);
					 
					 stream.Position = 0;
					 //Upload result csv to blob storage
					 var uploadInfo = new UploadStreamInfo
					 {
						 FileName = "Catalog-" + catalog.Name + "-export.csv",
						 FileByteStream = stream,
						 FolderName = "temp"
					 };
					 var blobKey = _blobStorageProvider.Upload(uploadInfo);
					 //Get a download url
					 notifyEvent.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobKey);
					 notifyEvent.Description = "Export finished";
				 }
				 catch (Exception ex)
				 {
					 notifyEvent.Description = "Export failed";
					 notifyEvent.Errors.Add(ex.ExpandExceptionMessage());
				 }
				 finally
				 {
					 notifyEvent.Finished = DateTime.UtcNow;
					 _notifier.Upsert(notifyEvent);
				 }
			 }
			
		}

	}
}