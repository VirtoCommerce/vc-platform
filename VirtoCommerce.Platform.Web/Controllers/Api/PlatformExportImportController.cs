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
        private const string _sampledataStateSetting = "VirtoCommerce:SampleDataState";

        private readonly IPlatformExportImportManager _platformExportManager;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ISettingsManager _settingsManager;
        private readonly IUserNameResolver _userNameResolver;
        private static readonly object _lockObject = new object();

        public PlatformExportImportController(IPlatformExportImportManager platformExportManager, IPushNotificationManager pushNotifier, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver, ISettingsManager settingManager, IUserNameResolver userNameResolver)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _blobStorageProvider = blobStorageProvider;
            _blobUrlResolver = blobUrlResolver;
            _settingsManager = settingManager;
            _userNameResolver = userNameResolver;
        }

        [HttpGet]
        [Route("sampledata/discover")]
        [ResponseType(typeof(SampleDataInfo[]))]
        public IHttpActionResult DiscoverSampleData()
        {
            var retVal = new List<SampleDataInfo>();
            if (!_settingsManager.GetValue("VirtoCommerce:SampleDataInstalled", false))
            {
                var sampleDataUrl = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:SampleDataUrl", string.Empty);
                if (!string.IsNullOrEmpty(sampleDataUrl))
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
                            sampleDataInfos = sampleDataInfos.Select(x => new { Version = SemanticVersion.Parse(x.PlatformVersion), x.Name, Data = x })
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
        [Route("sampledata/import")]
        [ResponseType(typeof(SampleDataImportPushNotification))]
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
                        _settingsManager.SetValue(_sampledataStateSetting, SampleDataState.Processing);
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
        [Route("sampledata/state")]
        [ResponseType(typeof(SampleDataState))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public IHttpActionResult GetSampleDataState()
        {
            var state = EnumUtility.SafeParse(_settingsManager.GetValue(_sampledataStateSetting, string.Empty), SampleDataState.Undefined);
            return Ok(state);
        }

        [HttpGet]
        [Route("export/manifest/new")]
        [ResponseType(typeof(PlatformExportManifest))]
        public IHttpActionResult GetNewExportManifest()
        {
            return Ok(_platformExportManager.GetNewExportManifest(_userNameResolver.GetCurrentUserName()));
        }

        [HttpGet]
        [Route("export/manifest/load")]
        [ResponseType(typeof(PlatformExportManifest))]
        public IHttpActionResult LoadExportManifest([FromUri]string fileUrl)
        {
            if(string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentNullException("fileUrl");
            }
            var localPath = HostingEnvironment.MapPath(fileUrl);
            PlatformExportManifest retVal;
            using (var stream = File.Open(localPath, FileMode.Open))
            {
                retVal = _platformExportManager.ReadExportManifest(stream);
            }
            return Ok(retVal);
        }

        [HttpPost]
        [Route("export")]
        [ResponseType(typeof(PushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.PlatformExport)]
        public IHttpActionResult ProcessExport(PlatformImportExportRequest exportRequest)
        {
            var notification = new PlatformExportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform export task",
                Description = "starting export...."
            };
            _pushNotifier.Upsert(notification);

            BackgroundJob.Enqueue(() => PlatformExportBackground(exportRequest, notification));

            return Ok(notification);
        }

        [HttpPost]
        [Route("import")]
        [ResponseType(typeof(PushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.PlatformImport)]
        public IHttpActionResult ProcessImport(PlatformImportExportRequest importRequest)
        {
            var notification = new PlatformImportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform import task",
                Description = "starting import...."
            };
            _pushNotifier.Upsert(notification);

            BackgroundJob.Enqueue(() => PlatformImportBackground(importRequest, notification));

            return Ok(notification);
        }

        public void SampleDataImportBackground(Uri url, string tmpPath, SampleDataImportPushNotification pushNotification)
        {
            Action<ExportImportProgressInfo> progressCallback = x =>
            {
                pushNotification.InjectFrom(x);
                _pushNotifier.Upsert(pushNotification);
            };
            try
            {
                pushNotification.Description = "Start downloading from " + url;
                _pushNotifier.Upsert(pushNotification);

                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }

                var tmpFilePath = Path.Combine(tmpPath, Path.GetFileName(url.ToString()));
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        pushNotification.Description = string.Format("Sample data {0} of {1} downloading...", args.BytesReceived.ToHumanReadableSize(), args.TotalBytesToReceive.ToHumanReadableSize());
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
                _settingsManager.SetValue(_sampledataStateSetting, SampleDataState.Completed);
                pushNotification.Finished = DateTime.UtcNow;
                _pushNotifier.Upsert(pushNotification);
            }
        }

        public void PlatformImportBackground(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification)
        {
            Action<ExportImportProgressInfo> progressCallback = x =>
            {
                pushNotification.InjectFrom(x);
                pushNotification.Errors = x.Errors;
                _pushNotifier.Upsert(pushNotification);
            };

            var now = DateTime.UtcNow;
            try
            {
                var localPath = HostingEnvironment.MapPath(importRequest.FileUrl);

                //Load source data only from local file system 
                using (var stream = File.Open(localPath, FileMode.Open))
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
            Action<ExportImportProgressInfo> progressCallback = x =>
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
