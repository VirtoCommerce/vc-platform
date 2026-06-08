using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.Modularity;
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
        // Purpose string for IDataProtector. Distinct enough that a key collision with another
        // platform component is unrealistic — the protected blob's only consumer is this controller.
        private const string DataProtectionPurpose = "PlatformBackup.ZipPassword";

        // 24 random bytes → 32-char URL-safe base64 string → ~192 bits of entropy.
        // Comfortable margin above what's practically brute-forceable for AES-256.
        private const int PasswordRandomByteCount = 24;

        private readonly IPlatformExportImportManager _platformExportManager;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;
        private readonly PlatformOptions _platformOptions;
        private readonly ILogger<PlatformBackupRestoreController> _logger;
        private readonly IDataProtector _protector;

        public PlatformBackupRestoreController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options,
            IDataProtectionProvider dataProtectionProvider,
            ILogger<PlatformBackupRestoreController> logger)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _platformOptions = options.Value;
            _protector = dataProtectionProvider.CreateProtector(DataProtectionPurpose);
            _logger = logger;
        }


        [HttpPost]
        [Route("export")]
        [Authorize(Permissions.PlatformExport)]
        public ActionResult<PlatformExportStartedResult> ProcessExport([FromBody] PlatformImportExportRequest exportRequest)
        {
            var notification = new PlatformExportPushNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Platform backup task",
                Description = "starting backup..."
            };
            _pushNotifier.Send(notification);

            // Generate the one-time password up front so it can be returned in this HTTP
            // response. The plaintext is shown to the user exactly once; the protected blob
            // is what gets persisted as a Hangfire job argument so the password never lands
            // unencrypted in Hangfire storage.
            string plainPassword = null;
            string protectedPassword = null;
            if (exportRequest.PasswordProtect)
            {
                plainPassword = GeneratePassword();
                protectedPassword = _protector.Protect(plainPassword);
            }

            var jobId = BackgroundJob.Enqueue(() => PlatformBackupBackgroundAsync(exportRequest, protectedPassword, notification, null, CancellationToken.None));
            notification.JobId = jobId;
            return Ok(new PlatformExportStartedResult
            {
                Notification = notification,
                Password = plainPassword,
            });
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

            // Protect the user-supplied password before handing it to Hangfire. Symmetric with
            // the export path: Hangfire-persisted argument is opaque ciphertext.
            var protectedPassword = string.IsNullOrEmpty(importRequest.Password)
                ? null
                : _protector.Protect(importRequest.Password);
            // Strip the plaintext from the request object before the Hangfire serializer sees it.
            importRequest.Password = null;

            var jobId = BackgroundJob.Enqueue(() => PlatformRestoreBackgroundAsync(importRequest, protectedPassword, notification, null, CancellationToken.None));
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


        public async Task PlatformRestoreBackgroundAsync(PlatformImportExportRequest importRequest, string protectedPassword, PlatformImportPushNotification pushNotification, PerformContext context, CancellationToken cancellationToken)
        {
            void ProgressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Patch(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            var now = DateTime.UtcNow;
            // Unprotect inside the job's local scope; the plaintext lives only for the duration
            // of ImportAsync and never enters any logged/persisted object.
            string plainPassword = null;
            try
            {
                plainPassword = string.IsNullOrEmpty(protectedPassword) ? null : _protector.Unprotect(protectedPassword);

                var localPath = GetSafeFullPath(_platformOptions.LocalUploadFolderPath, importRequest.FileUrl);

                //Load source data only from local file system
                using (var stream = new FileStream(localPath, FileMode.Open))
                {
                    var manifest = importRequest.ToManifest();
                    manifest.Created = now;
                    // Preserve the admin who initiated this restore so the user-import phase
                    // can skip overwriting their PasswordHash / SecurityStamp and avoid logging
                    // them out mid-restore.
                    manifest.CallerUserName = pushNotification.Creator;
                    manifest.Password = plainPassword;
                    await _platformExportManager.ImportAsync(stream, manifest, ProgressCallback, cancellationToken);
                    manifest.Password = null;
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Platform restore job {JobId} started by {User} was cancelled.",
                    context?.BackgroundJob?.Id, pushNotification.Creator);
            }
            catch (Exception ex)
            {
                var message = ex.ExpandExceptionMessage();
                pushNotification.Errors.Add(message);
                pushNotification.ProgressLog ??= [];
                pushNotification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Error, Message = message });
            }
            finally
            {
                // Defense in depth: ensure plaintext password reference is cleared before the
                // method returns so the GC can collect it promptly.
                plainPassword = null;
                pushNotification.Description = pushNotification.Errors.Count > 0
                    ? "Platform restore process completed with errors."
                    : "Platform restore process completed successfully.";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        public async Task PlatformBackupBackgroundAsync(PlatformImportExportRequest exportRequest, string protectedPassword, PlatformExportPushNotification pushNotification, PerformContext context, CancellationToken cancellationToken)
        {
            void ProgressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Patch(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            string plainPassword = null;
            try
            {
                plainPassword = string.IsNullOrEmpty(protectedPassword) ? null : _protector.Unprotect(protectedPassword);

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
                    manifest.Password = plainPassword;
                    await _platformExportManager.ExportAsync(stream, manifest, ProgressCallback, cancellationToken);
                    manifest.Password = null;
                    pushNotification.DownloadUrl = $"api/platform/export/download/{fileName}";
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Platform restore job {JobId} started by {User} was cancelled.",
                    context?.BackgroundJob?.Id, pushNotification.Creator);
            }
            catch (Exception ex)
            {
                var message = ex.ExpandExceptionMessage();
                pushNotification.Errors.Add(message);
                pushNotification.ProgressLog ??= [];
                pushNotification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Error, Message = message });
            }
            finally
            {
                plainPassword = null;
                pushNotification.Description = pushNotification.Errors.Count > 0
                    ? "Platform backup process completed with errors."
                    : "Platform backup process completed successfully.";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        private static string GetSafeFullPath(string basePath, string relativePath)
        {
            // Reject inputs that should never be accepted as a filename in the export/upload
            // folder before they reach `Path.Combine` / `Path.GetFullPath`. Two cases this
            // catches that the post-resolution StartsWith check below misses:
            //
            //  * Whitespace-only input: `Path.GetFullPath(baseFullPath + "\\   ")` normalizes
            //    the trailing whitespace away and returns `baseFullPath + "\\"`, which then
            //    passes the StartsWith guard and lets the caller File.Open() the directory
            //    itself (yielding a confusing FileNotFoundException instead of a clear 400).
            //
            //  * Embedded separators: `subdir/file.zip` resolves to a real path INSIDE the
            //    base folder, so the StartsWith guard happily accepts it. But the download
            //    endpoint's contract is "serve files DIRECTLY in the export folder, not
            //    arbitrary descendants" — nested access widens the attack surface (e.g. an
            //    operator who can write into a subfolder shouldn't be able to download from
            //    it via this API). Reject any path separator outright.
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new PlatformException("File name is required");
            }
            if (relativePath.IndexOfAny(['/', '\\']) >= 0)
            {
                throw new PlatformException($"Invalid path {relativePath}");
            }

            var baseFullPath = Path.GetFullPath(basePath);
            var result = Path.GetFullPath(Path.Combine(baseFullPath, relativePath));

            if (!result.StartsWith(baseFullPath + Path.DirectorySeparatorChar))
            {
                throw new PlatformException($"Invalid path {relativePath}");
            }

            return result;
        }

        private static string GeneratePassword()
        {
            // URL-safe base64: replace '+' with '-' and '/' with '_', strip padding. Result is
            // a 32-char [-A-Za-z0-9_] string that can be copy-pasted into any ZIP tool without
            // shell-escaping concerns.
            var bytes = RandomNumberGenerator.GetBytes(PasswordRandomByteCount);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
