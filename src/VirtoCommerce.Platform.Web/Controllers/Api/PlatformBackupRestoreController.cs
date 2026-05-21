using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;

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
        private readonly ILogger<PlatformBackupRestoreController> _log;

        public PlatformBackupRestoreController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options,
            ILogger<PlatformBackupRestoreController> log)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _platformOptions = options.Value;
            _log = log;
        }

        [Obsolete("Use the constructor that accepts ILogger<PlatformBackupRestoreController>.",
            DiagnosticId = "VC0014",
            UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public PlatformBackupRestoreController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options)
            : this(platformExportManager, pushNotifier, userNameResolver, options, log: null)
        {
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

            var jobId = BackgroundJob.Enqueue(() => PlatformBackupBackgroundAsync(exportRequest, notification, null, CancellationToken.None));
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

            var jobId = BackgroundJob.Enqueue(() => PlatformRestoreBackgroundAsync(importRequest, notification, null, CancellationToken.None));
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

        public async Task PlatformRestoreBackgroundAsync(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification, PerformContext context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(pushNotification);

            void ProgressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            var now = DateTime.UtcNow;
            try
            {
                var localPath = GetSafeFullPath(_platformOptions.LocalUploadFolderPath, importRequest.FileUrl);

                //Load source data only from local file system
                using var stream = new FileStream(localPath, FileMode.Open);
                var manifest = importRequest.ToManifest();
                manifest.Created = now;
                await _platformExportManager.ImportAsync(stream, manifest, ProgressCallback, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Also catches Hangfire.JobAbortedException, which derives from OperationCanceledException.
                _log?.LogWarning("Platform restore job {JobId} started by {User} was cancelled.",
                    context?.BackgroundJob?.Id, pushNotification?.Creator);
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

        public async Task PlatformBackupBackgroundAsync(PlatformImportExportRequest exportRequest, PlatformExportPushNotification pushNotification, PerformContext context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(pushNotification);

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
                using var stream = System.IO.File.OpenWrite(localTmpPath);
                var manifest = exportRequest.ToManifest();
                await _platformExportManager.ExportAsync(stream, manifest, ProgressCallback, cancellationToken);
                pushNotification.DownloadUrl = $"api/platform/export/download/{fileName}";
            }
            catch (OperationCanceledException)
            {
                // Also catches Hangfire.JobAbortedException, which derives from OperationCanceledException.
                _log?.LogWarning("Platform backup job {JobId} started by {User} was cancelled.",
                    context?.BackgroundJob?.Id, pushNotification?.Creator);
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

        // Shim for in-flight queue items: ShutdownToken only - UI delete won't cancel jobs on this path.
        [Obsolete("Hangfire compatibility shim for legacy queue items. Use the overload with CancellationToken.",
            DiagnosticId = "VC0014",
            UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public Task PlatformRestoreBackgroundAsync(PlatformImportExportRequest importRequest, PlatformImportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
            => PlatformRestoreBackgroundAsync(importRequest, pushNotification, context, cancellationToken?.ShutdownToken ?? CancellationToken.None);

        [Obsolete("Hangfire compatibility shim for legacy queue items. Use the overload with CancellationToken.",
            DiagnosticId = "VC0014",
            UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public Task PlatformBackupBackgroundAsync(PlatformImportExportRequest exportRequest, PlatformExportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
            => PlatformBackupBackgroundAsync(exportRequest, pushNotification, context, cancellationToken?.ShutdownToken ?? CancellationToken.None);

        private static string GetSafeFullPath(string basePath, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new PlatformException("Path is empty.");
            }

            // Only allow a file name (single path component) from user input.
            if (Path.IsPathRooted(relativePath) ||
                !string.Equals(Path.GetFileName(relativePath), relativePath, StringComparison.Ordinal) ||
                relativePath.Contains("..", StringComparison.Ordinal))
            {
                throw new PlatformException($"Invalid path {relativePath}");
            }

            var baseFullPath = Path.GetFullPath(basePath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var result = Path.GetFullPath(Path.Combine(baseFullPath, relativePath));
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            if (!result.StartsWith(baseFullPath + Path.DirectorySeparatorChar, comparison))
            {
                throw new PlatformException($"Invalid path {relativePath}");
            }

            return result;
        }
    }
}
