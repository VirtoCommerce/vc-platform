using System;
using System.IO;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Hangfire;

using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class PlatformBackupRestoreController : Controller
    {
        private readonly IPlatformExportImportManager _platformExportManager;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;
        private readonly PlatformOptions _platformOptions;

        public PlatformBackupRestoreController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _platformOptions = options.Value;
        }


        [HttpPost]
        [Route("export")]
        [Authorize(Permissions.PlatformExport)]
        public ActionResult<PlatformExportPushNotification> ProcessExport([FromBody] PlatformImportExportRequest exportRequest)
        {
            var notification = new PlatformExportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform backup task",
                Description = "starting backup..."
            };
            _pushNotifier.Send(notification);

            var jobId = BackgroundJob.Enqueue(() => PlatformBackupBackgroundAsync(exportRequest, notification, JobCancellationToken.Null, null));
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
                Title = "Platform restore task",
                Description = "starting restore..."
            };
            _pushNotifier.Send(notification);

            var jobId = BackgroundJob.Enqueue(() => PlatformRestoreBackgroundAsync(importRequest, notification, JobCancellationToken.Null, null));
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
            var localPath = GetSafeFullPath(_platformOptions.DefaultExportFolder, fileName);

            //Load source data only from local file system
            using (System.IO.File.Open(localPath, FileMode.Open))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(localPath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                return PhysicalFile(localPath, contentType);
            }
        }


        public async Task PlatformRestoreBackgroundAsync(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
        {
            void ProgressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            var now = DateTime.UtcNow;
            try
            {
                var cancellationTokenWrapper = new JobCancellationTokenWrapper(cancellationToken);

                var localPath = GetSafeFullPath(_platformOptions.LocalUploadFolderPath, importRequest.FileUrl);

                //Load source data only from local file system
                using (var stream = new FileStream(localPath, FileMode.Open))
                {
                    var manifest = importRequest.ToManifest();
                    manifest.Created = now;
                    await _platformExportManager.ImportAsync(stream, manifest, ProgressCallback, cancellationTokenWrapper);
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
                pushNotification.Description = "Platform restore process completed successfully.";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        public async Task PlatformBackupBackgroundAsync(PlatformImportExportRequest exportRequest, PlatformExportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
        {
            void ProgressCallback(ExportImportProgressInfo x)
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
                    await _platformExportManager.ExportAsync(stream, manifest, ProgressCallback, new JobCancellationTokenWrapper(cancellationToken));
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
                pushNotification.Description = "Platform backup process completed successfully.";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        private static string GetSafeFullPath(string basePath, string relativePath)
        {
            var baseFullPath = Path.GetFullPath(basePath);
            var result = Path.GetFullPath(Path.Combine(baseFullPath, relativePath));

            if (!result.StartsWith(baseFullPath + Path.DirectorySeparatorChar))
            {
                throw new PlatformException($"Invalid path {relativePath}");
            }

            return result;
        }
    }
}
