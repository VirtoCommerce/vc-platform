using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Hangfire;

using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class PlatformExportImportController : Controller
    {
        private readonly IPlatformExportImportManager _platformExportManager;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly ISettingsManager _settingsManager;
        private readonly IUserNameResolver _userNameResolver;
        private readonly PlatformOptions _platformOptions;

        private static readonly object _lockObject = new object();

        public PlatformExportImportController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            ISettingsManager settingManager,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _settingsManager = settingManager;
            _userNameResolver = userNameResolver;
            _platformOptions = options.Value;
        }

        [HttpGet]
        [Route("sampledata/discover")]
        [AllowAnonymous]
        public ActionResult<SampleDataInfo[]> DiscoverSampleData()
        {
            return Ok(InnerDiscoverSampleData().ToArray());
        }

        [HttpPost]
        [Route("sampledata/autoinstall")]
        [Authorize(Permissions.PlatformImport)]
        public ActionResult<SampleDataImportPushNotification> TryToAutoInstallSampleData()
        {
            var sampleData = InnerDiscoverSampleData().FirstOrDefault(x => !x.Url.IsNullOrEmpty());
            if (sampleData != null)
            {
                return ImportSampleData(sampleData.Url);
            }

            return Ok();
        }

        [HttpPost]
        [Route("sampledata/import")]
        [Authorize(Permissions.PlatformImport)]
        public ActionResult<SampleDataImportPushNotification> ImportSampleData([FromQuery] string url = null)
        {
            lock (_lockObject)
            {
                var sampleDataState = EnumUtility.SafeParse(_settingsManager.GetValue(PlatformConstants.Settings.Setup.SampleDataState.Name, SampleDataState.Undefined.ToString()), SampleDataState.Undefined);
                if (sampleDataState == SampleDataState.Undefined && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    _settingsManager.SetValue(PlatformConstants.Settings.Setup.SampleDataState.Name, SampleDataState.Processing);
                    var pushNotification = new SampleDataImportPushNotification(User.Identity.Name);
                    _pushNotifier.Send(pushNotification);
                    var jobId = BackgroundJob.Enqueue(() => SampleDataImportBackgroundAsync(new Uri(url), pushNotification, JobCancellationToken.Null, null));
                    pushNotification.JobId = jobId;

                    return Ok(pushNotification);
                }
            }

            return Ok();
        }

        /// <summary>
        /// This method used for azure automatically deployment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sampledata/state")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public ActionResult<SampleDataState> GetSampleDataState()
        {
            var state = EnumUtility.SafeParse(_settingsManager.GetValue(PlatformConstants.Settings.Setup.SampleDataState.Name, string.Empty), SampleDataState.Undefined);
            return Ok(state);
        }

        [HttpGet]
        [Route("export/manifest/new")]
        public ActionResult<PlatformExportManifest> GetNewExportManifest()
        {
            return Ok(_platformExportManager.GetNewExportManifest(_userNameResolver.GetCurrentUserName()));
        }

        [HttpGet]
        [Route("export/manifest/load")]
        public ActionResult<PlatformExportManifest> LoadExportManifest([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentNullException(nameof(fileUrl));
            }

            var uploadFolderPath = Path.GetFullPath(_platformOptions.LocalUploadFolderPath);

            var localPath = Path.Combine(uploadFolderPath, fileUrl);
            if (!localPath.StartsWith(uploadFolderPath))
            {
                throw new PlatformException($"Invalid path {localPath}");
            }

            PlatformExportManifest retVal;
            using (var stream = new FileStream(localPath, FileMode.Open))
            {
                retVal = _platformExportManager.ReadExportManifest(stream);
            }
            return Ok(retVal);
        }

        [HttpPost]
        [Route("export")]
        [Authorize(Permissions.PlatformImport)]
        public ActionResult<PlatformExportPushNotification> ProcessExport([FromBody] PlatformImportExportRequest exportRequest)
        {
            var notification = new PlatformExportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform export task",
                Description = "starting export...."
            };
            _pushNotifier.Send(notification);

            var jobId = BackgroundJob.Enqueue(() => PlatformExportBackgroundAsync(exportRequest, notification, JobCancellationToken.Null, null));
            notification.JobId = jobId;
            return Ok(notification);
        }

        [HttpPost]
        [Route("import")]
        [Authorize(Permissions.PlatformImport)]
        public ActionResult<PlatformImportPushNotification> ProcessImport([FromBody] PlatformImportExportRequest importRequest)
        {
            var notification = new PlatformImportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform import task",
                Description = "starting import...."
            };
            _pushNotifier.Send(notification);

            var jobId = BackgroundJob.Enqueue(() => PlatformImportBackgroundAsync(importRequest, notification, JobCancellationToken.Null, null));
            notification.JobId = jobId;

            return Ok(notification);
        }

        [HttpPost]
        [Route("exortimport/tasks/{jobId}/cancel")]
        public ActionResult Cancel([FromRoute] string jobId)
        {
            BackgroundJob.Delete(jobId);
            return Ok();
        }

        [HttpGet]
        [Route("export/download/{fileName}")]
        [Authorize(Permissions.PlatformExport)]
        public ActionResult DownloadExportFile([FromRoute] string fileName)
        {
            var localTmpFolder = Path.GetFullPath(Path.Combine(_platformOptions.DefaultExportFolder));
            var localPath = Path.Combine(localTmpFolder, Path.GetFileName(fileName));

            //Load source data only from local file system
            using (var stream = System.IO.File.Open(localPath, FileMode.Open))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(localPath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                return PhysicalFile(localPath, contentType);
            }
        }

        private IEnumerable<SampleDataInfo> InnerDiscoverSampleData()
        {
            var sampleDataUrl = _platformOptions.SampleDataUrl;
            if (string.IsNullOrEmpty(sampleDataUrl))
            {
                return Enumerable.Empty<SampleDataInfo>();
            }

            //Direct file mode
            if (sampleDataUrl.EndsWith(".zip"))
            {
                return new List<SampleDataInfo>
                {
                    new SampleDataInfo { Url = sampleDataUrl }
                };
            }

            //Discovery mode
            var manifestUrl = sampleDataUrl + "\\manifest.json";
            using (var client = new WebClient())
            using (var stream = client.OpenRead(new Uri(manifestUrl)))
            {
                //Add empty template
                var result = new List<SampleDataInfo>
                {
                    new SampleDataInfo { Name = "Empty" }
                };

                //Need filter unsupported versions and take one most new sample data
                var sampleDataInfos = stream.DeserializeJson<List<SampleDataInfo>>()
                    .Select(x => new
                    {
                        Version = SemanticVersion.Parse(x.PlatformVersion),
                        x.Name,
                        Data = x
                    })
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

                result.AddRange(sampleDataInfos);

                return result;
            }
        }

        public async Task SampleDataImportBackgroundAsync(Uri url, SampleDataImportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
        {
            void progressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            try
            {
                pushNotification.Description = "Start downloading from " + url;

                await _pushNotifier.SendAsync(pushNotification);

                var tmpPath = Path.GetFullPath(_platformOptions.LocalUploadFolderPath);
                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }

                var tmpFilePath = Path.Combine(tmpPath, Path.GetFileName(url.ToString()));
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += async (sender, args) =>
                    {
                        pushNotification.Description = string.Format("Sample data {0} of {1} downloading...", args.BytesReceived.ToHumanReadableSize(), args.TotalBytesToReceive.ToHumanReadableSize());
                        await _pushNotifier.SendAsync(pushNotification);
                    };
                    var task = client.DownloadFileTaskAsync(url, tmpFilePath);
                    task.Wait();
                }
                using (var stream = new FileStream(tmpFilePath, FileMode.Open))
                {
                    var manifest = _platformExportManager.ReadExportManifest(stream);
                    if (manifest != null)
                    {
                        await _platformExportManager.ImportAsync(stream, manifest, progressCallback, new JobCancellationTokenWrapper(cancellationToken));
                    }
                }
            }
            catch (JobAbortedException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                _settingsManager.SetValue(PlatformConstants.Settings.Setup.SampleDataState.Name, SampleDataState.Completed);
                pushNotification.Description = "Import finished";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        public async Task PlatformImportBackgroundAsync(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
        {
            void progressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            var now = DateTime.UtcNow;
            try
            {
                var cancellationTokenWrapper = new JobCancellationTokenWrapper(cancellationToken);

                var uploadFolderFullPath = Path.GetFullPath(_platformOptions.LocalUploadFolderPath);
                // VP-5353: Checking that the file is inside LocalUploadFolderPath
                var localPath = Path.Combine(uploadFolderFullPath, importRequest.FileUrl);

                if (!localPath.StartsWith(uploadFolderFullPath))
                {
                    throw new PlatformException($"Invalid path {localPath}");
                }

                //Load source data only from local file system
                using (var stream = new FileStream(localPath, FileMode.Open))
                {
                    var manifest = importRequest.ToManifest();
                    manifest.Created = now;
                    await _platformExportManager.ImportAsync(stream, manifest, progressCallback, cancellationTokenWrapper);
                }
            }
            catch (JobAbortedException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                pushNotification.Description = "Import finished";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        public async Task PlatformExportBackgroundAsync(PlatformImportExportRequest exportRequest, PlatformExportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
        {
            void progressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            try
            {
                var fileName = string.Format(_platformOptions.DefaultExportFileName, DateTime.UtcNow);
                var localTmpFolder = Path.GetFullPath(Path.Combine(_platformOptions.DefaultExportFolder));
                var localTmpPath = Path.Combine(localTmpFolder, Path.GetFileName(fileName));

                if (!Directory.Exists(localTmpFolder))
                {
                    Directory.CreateDirectory(localTmpFolder);
                }

                if (System.IO.File.Exists(localTmpPath))
                {
                    System.IO.File.Delete(localTmpPath);
                }

                //Import first to local tmp folder because Azure blob storage doesn't support some special file access mode
                using (var stream = System.IO.File.OpenWrite(localTmpPath))
                {
                    var manifest = exportRequest.ToManifest();
                    await _platformExportManager.ExportAsync(stream, manifest, progressCallback, new JobCancellationTokenWrapper(cancellationToken));
                    pushNotification.DownloadUrl = $"api/platform/export/download/{fileName}";
                }
            }
            catch (JobAbortedException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                pushNotification.Description = "Export finished";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }
    }
}
