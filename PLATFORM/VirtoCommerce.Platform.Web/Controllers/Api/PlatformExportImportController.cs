using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Web.Converters.ExportImport;
using VirtoCommerce.Platform.Web.Model.ExportImport;
using VirtoCommerce.Platform.Web.Model.ExportImport.PushNotifications;

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
        private static object _lockObject = new object();

        public PlatformExportImportController(IPlatformExportImportManager platformExportManager, IPushNotificationManager pushNotifier, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver, ISettingsManager settingManager)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _blobStorageProvider = blobStorageProvider;
            _blobUrlResolver = blobUrlResolver;
            _settingsManager = settingManager;
        }

        [HttpGet]
        [ResponseType(typeof(SampleDataInfo[]))]
        [Route("sampledata/discover")]
        public IHttpActionResult DiscoverSampleData()
        {
            var retVal = new List<SampleDataInfo>();
            if (!_settingsManager.GetValue("VirtoCommerce:SampleDataInstalled", false))
            {
                var sampleDataUrl = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:SampleDataUrl", string.Empty);
                if (!String.IsNullOrEmpty(sampleDataUrl))
                {
                    //Discovery mode
                    if (!sampleDataUrl.EndsWith(".zip"))
                    {
                        var manifestUrl = sampleDataUrl + "\\manifest.json";
                        using (var client = new WebClient())
                        using (var stream = client.OpenRead(new Uri(manifestUrl)))
                        {
                            //Add empty template
                            retVal.Add(new SampleDataInfo { Name = "Empty" });
                            var sampleDataInfos = stream.DeserializeJson<List<SampleDataInfo>>();
                            //Need filter unsupported versions and take one most new sample data
                            sampleDataInfos = sampleDataInfos.Select(x => new { Version = SemanticVersion.Parse(x.PlatformVersion), Name = x.Name, Data = x })
                                                             .Where(x => x.Version.IsCompatibleWith(PlatformVersion.CurrentVersion))
                                                             .GroupBy(x => x.Name)
                                                             .Select(x => x.OrderByDescending(y => y.Version).First().Data)
                                                             .ToList();
                            //Convert relative  sample data urls to absolute
                            foreach (var sampleDataInfo in sampleDataInfos)
                            {
                                if (!Uri.IsWellFormedUriString(sampleDataInfo.Url, UriKind.Absolute))
                                {
                                    var uri = new Uri(sampleDataUrl);
                                    sampleDataInfo.Url = new Uri(uri, uri.AbsolutePath + "/" + sampleDataInfo.Url).ToString();
                                }
                            }
                            retVal.AddRange(sampleDataInfos);

                        }
                    }
                    else
                    {
                        //Direct file mode
                        retVal.Add(new SampleDataInfo { Url = sampleDataUrl });
                    }
                }
            }
            return Ok(retVal);
        }

        [HttpPost]
        [ResponseType(typeof(SampleDataImportPushNotification))]
        [Route("sampledata/import")]
        public IHttpActionResult TryToImportSampleData([FromUri]string url = null)
        {
            lock (_lockObject)
            {
                if (!_settingsManager.GetValue("VirtoCommerce:SampleDataInstalled", false))
                {
                    _settingsManager.SetValue("VirtoCommerce:SampleDataInstalled", true);
                    //Sample data initialization
                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        var pushNotification = new SampleDataImportPushNotification("System");
                        _pushNotifier.Upsert(pushNotification);
                        BackgroundJob.Enqueue(() => SampleDataImportBackground(new Uri(url), HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data/Uploads/"), pushNotification));

                        return Ok(pushNotification);
                    }
                }
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
            using (var stream = _blobStorageProvider.OpenRead(fileUrl))
            {
                retVal = _platformExportManager.ReadExportManifest(stream);
            }
            return Ok(retVal);
        }

        [HttpPost]
        [ResponseType(typeof(PushNotification))]
        [Route("export")]
        [CheckPermission(Permission = PredefinedPermissions.PlatformExport)]
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
        [CheckPermission(Permission = PredefinedPermissions.PlatformImport)]
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

        public void SampleDataImportBackground(Uri url, string tmpPath, SampleDataImportPushNotification pushNotification)
        {
            Action<ExportImportProgressInfo> progressCallback = (x) =>
            {
                pushNotification.InjectFrom(x);
                _pushNotifier.Upsert(pushNotification);
            };
            try
            {
                pushNotification.Description = "Start downloading from " + url.ToString();
                _pushNotifier.Upsert(pushNotification);

                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }

                var fileName = System.IO.Path.GetFileName(url.ToString());
                var tmpFilePath = Path.Combine(tmpPath, System.IO.Path.GetFileName(url.ToString()));
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        pushNotification.Description = String.Format("Sample data {0} of {1} downloading...", args.BytesReceived.ToHumanReadableSize(), args.TotalBytesToReceive.ToHumanReadableSize());
                        _pushNotifier.Upsert(pushNotification);
                    };
                    var task = client.DownloadFileTaskAsync(url, tmpFilePath);
                    task.Wait();
                }
                using (var stream = File.Open(tmpFilePath, FileMode.Open))
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
                using (var stream = _blobStorageProvider.OpenRead(importRequest.FileUrl))
                {
                    var manifest = importRequest.ToManifest();
                    manifest.Created = now;
                    _platformExportManager.Import(stream, manifest, progressCallback);
                }
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                pushNotification.Description = "Import finished";
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
                    var relativeUrl = "tmp/exported_data.zip";
                    using (var targetStream = _blobStorageProvider.OpenWrite(relativeUrl))
                    {
                        stream.CopyTo(targetStream);
                    }
                    //Get a download url
                    pushNotification.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(relativeUrl);
                }
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                pushNotification.Description = "Export finished";
                pushNotification.Finished = DateTime.UtcNow;
                _pushNotifier.Upsert(pushNotification);
            }

        }

    }
}