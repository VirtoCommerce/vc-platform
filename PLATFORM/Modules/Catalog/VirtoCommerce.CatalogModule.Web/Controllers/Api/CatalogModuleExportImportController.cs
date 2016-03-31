using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using CsvHelper;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.CatalogModule.Web.Model.PushNotifications;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog")]
    public class CatalogModuleExportImportController : CatalogBaseController
    {
        private readonly ICatalogService _catalogService;
        private readonly IPushNotificationManager _notifier;
        private readonly ICommerceService _commerceService;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly CsvCatalogExporter _csvExporter;
        private readonly CsvCatalogImporter _csvImporter;
        private readonly IUserNameResolver _userNameResolver;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleExportImportController(ICatalogService catalogService, IPushNotificationManager pushNotificationManager, ICommerceService commerceService,
                                                   IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver,
                                                   CsvCatalogExporter csvExporter, CsvCatalogImporter csvImporter, ISecurityService securityService, IPermissionScopeService permissionScopeService,
                                                   IUserNameResolver userNameResolver)
            : base(securityService, permissionScopeService)
        {
            _catalogService = catalogService;
            _notifier = pushNotificationManager;
            _commerceService = commerceService;
            _blobStorageProvider = blobStorageProvider;
            _csvExporter = csvExporter;
            _csvImporter = csvImporter;
            _userNameResolver = userNameResolver;
            _blobUrlResolver = blobUrlResolver;
        }

        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>Data export is an async process. An ExportNotification is returned for progress reporting.</remarks>
        /// <param name="exportInfo">The export configuration.</param>
        [ResponseType(typeof(ExportNotification))]
        [HttpPost]
        [Route("export")]
        public IHttpActionResult DoExport(CsvExportInfo exportInfo)
        {
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Export, exportInfo);

            var notification = new ExportNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Catalog export task",
                Description = "starting export...."
            };
            _notifier.Upsert(notification);


            BackgroundJob.Enqueue(() => BackgroundExport(exportInfo, notification));

            return Ok(notification);
        }


        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>Analyses the supplied file's structure and returns automatic column mapping.</remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns></returns>
        [ResponseType(typeof(CsvProductMappingConfiguration))]
        [HttpGet]
        [Route("import/mappingconfiguration")]
        public IHttpActionResult GetMappingConfiguration([FromUri]string fileUrl, [FromUri]string delimiter = ";")
        {
            var retVal = CsvProductMappingConfiguration.GetDefaultConfiguration();

            retVal.Delimiter = delimiter;

            //Read csv headers and try to auto map fields by name
            using (var reader = new CsvReader(new StreamReader(_blobStorageProvider.OpenRead(fileUrl))))
            {
                reader.Configuration.Delimiter = delimiter;
                if (reader.Read())
                {
                    retVal.AutoMap(reader.FieldHeaders);
                }
            }

            return Ok(retVal);
        }


        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>Data import is an async process. An ImportNotification is returned for progress reporting.</remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns></returns>
        [ResponseType(typeof(ImportNotification))]
        [HttpPost]
        [Route("import")]
        public IHttpActionResult DoImport(CsvImportInfo importInfo)
        {
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Import, importInfo);

            var notification = new ImportNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Import catalog from CSV",
                Description = "starting import...."
            };
            _notifier.Upsert(notification);

            BackgroundJob.Enqueue(() => BackgroundImport(importInfo, notification));

            return Ok(notification);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        // Only public methods can be invoked in the background. (Hangfire)
        public void BackgroundImport(CsvImportInfo importInfo, ImportNotification notifyEvent)
        {
            Action<ExportImportProgressInfo> progressCallback = (x) =>
            {
                notifyEvent.InjectFrom(x);
                _notifier.Upsert(notifyEvent);
            };

            using (var stream = _blobStorageProvider.OpenRead(importInfo.FileUrl))
            {
                try
                {
                    _csvImporter.DoImport(stream, importInfo, progressCallback);
                }
                catch (Exception ex)
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


        [ApiExplorerSettings(IgnoreApi = true)]
        // Only public methods can be invoked in the background. (Hangfire)
        public void BackgroundExport(CsvExportInfo exportInfo, ExportNotification notifyEvent)
        {
            var currencies = _commerceService.GetAllCurrencies();
            var defaultCurrency = currencies.First(x => x.IsPrimary);
            exportInfo.Currency = exportInfo.Currency ?? defaultCurrency.Code;
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
                    var blobRelativeUrl = "temp/Catalog-" + catalog.Name + "-export.csv";
                    //Upload result csv to blob storage
                    using (var blobStream = _blobStorageProvider.OpenWrite(blobRelativeUrl))
                    {
                        stream.CopyTo(blobStream);
                    }
                    //Get a download url
                    notifyEvent.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobRelativeUrl);
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