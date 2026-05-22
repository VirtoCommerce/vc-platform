using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PlatformExportImportController> _log;

        private static readonly object _lockObject = new();

        [ActivatorUtilitiesConstructor]
        public PlatformExportImportController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            ISettingsManager settingManager,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options,
            IHttpClientFactory httpClientFactory,
            ILogger<PlatformExportImportController> log)
        {
            _platformExportManager = platformExportManager;
            _pushNotifier = pushNotifier;
            _settingsManager = settingManager;
            _userNameResolver = userNameResolver;
            _httpClientFactory = httpClientFactory;
            _platformOptions = options.Value;
            _log = log;
        }

        [Obsolete("Use the constructor that accepts ILogger<PlatformExportImportController>.",
            DiagnosticId = "VC0014",
            UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public PlatformExportImportController(
            IPlatformExportImportManager platformExportManager,
            IPushNotificationManager pushNotifier,
            ISettingsManager settingManager,
            IUserNameResolver userNameResolver,
            IOptions<PlatformOptions> options,
            IHttpClientFactory httpClientFactory)
            : this(platformExportManager, pushNotifier, settingManager, userNameResolver, options, httpClientFactory, log: null)
        {
        }

        [HttpGet]
        [Route("sampledata/discover")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<SampleDataInfo>>> DiscoverSampleData()
        {
            return Ok(await InnerDiscoverSampleDataAsync());
        }

        [HttpPost]
        [Route("sampledata/autoinstall")]
        [Authorize(Permissions.PlatformImport)]
        public async Task<ActionResult<SampleDataImportPushNotification>> TryToAutoInstallSampleData()
        {
            var sampleData = (await InnerDiscoverSampleDataAsync()).FirstOrDefault(x => !x.Url.IsNullOrEmpty());
            if (sampleData != null)
            {
                return Ok(StartImportSampleData(sampleData.Name));
            }

            return Ok();
        }

        [HttpPost]
        [Route("sampledata/import")]
        [Authorize(Permissions.PlatformImport)]
        public async Task<ActionResult<SampleDataImportPushNotification>> ImportSampleData([FromQuery] string name = null, [FromQuery] string url = null)
        {
            var sampleDataList = await InnerDiscoverSampleDataAsync();

            SampleDataInfo sampleData = null;

            if (!string.IsNullOrEmpty(name))
            {
                sampleData = sampleDataList.FirstOrDefault(x => x.Name == name);
            }
            else if (!string.IsNullOrEmpty(url))
            {
                sampleData = sampleDataList.FirstOrDefault(x => x.Url == url);
            }

            if (sampleData != null)
            {
                return Ok(StartImportSampleData(sampleData.Name));
            }

            return Ok();
        }

        private SampleDataImportPushNotification StartImportSampleData(string name)
        {
            SampleDataImportPushNotification pushNotification = null;

            lock (_lockObject)
            {
                var sampleDataState = EnumUtility.SafeParse(_settingsManager.GetValue<string>(PlatformConstants.Settings.Setup.SampleDataState), SampleDataState.Undefined);
                if (sampleDataState == SampleDataState.Undefined)
                {
                    _settingsManager.SetValue(PlatformConstants.Settings.Setup.SampleDataState.Name, SampleDataState.Processing);

                    pushNotification = new SampleDataImportPushNotification(User.Identity?.Name);
                    pushNotification.Title = "Sample data import process";

                    _pushNotifier.Send(pushNotification);
                    var jobId = BackgroundJob.Enqueue(() => SampleDataImportBackgroundAsync(name, pushNotification, null, CancellationToken.None));
                    pushNotification.JobId = jobId;
                }
            }

            return pushNotification;
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
            var state = EnumUtility.SafeParse(_settingsManager.GetValue<string>(PlatformConstants.Settings.Setup.SampleDataState), SampleDataState.Undefined);
            return Ok(state);
        }

        [HttpGet]
        [Route("export/manifest/new")]
        [Authorize(Permissions.PlatformExport)]
        public ActionResult<PlatformExportManifest> GetNewExportManifest()
        {
            return Ok(_platformExportManager.GetNewExportManifest(_userNameResolver.GetCurrentUserName()));
        }

        [HttpGet]
        [Route("export/manifest/load")]
        [Authorize(Permissions.PlatformImport)]
        public ActionResult<PlatformExportManifest> LoadExportManifest([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new ArgumentNullException(nameof(fileUrl));
            }

            var localPath = GetSafeFullPath(_platformOptions.LocalUploadFolderPath, fileUrl);

            PlatformExportManifest retVal;
            using (var stream = new FileStream(localPath, FileMode.Open))
            {
                retVal = _platformExportManager.ReadExportManifest(stream);
            }
            return Ok(retVal);
        }

        private async Task<IList<SampleDataInfo>> InnerDiscoverSampleDataAsync()
        {
            var sampleDataUrl = _platformOptions.SampleDataUrl;
            if (string.IsNullOrEmpty(sampleDataUrl))
            {
                return [];
            }

            //Direct file mode
            if (sampleDataUrl.EndsWith(".zip"))
            {
                return new List<SampleDataInfo>
                {
                    new()
                    {
                        Name = Path.GetFileNameWithoutExtension(sampleDataUrl),
                        Url = sampleDataUrl,
                    },
                };
            }

            //Discovery mode
            var manifestUrl = sampleDataUrl + "/manifest.json";
            var httpClient = _httpClientFactory.CreateClient();
            await using var stream = await httpClient.GetStreamAsync(new Uri(manifestUrl));
            //Add empty template
            var result = new List<SampleDataInfo>
            {
                new() { Name = "Empty" }
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

        public async Task SampleDataImportBackgroundAsync(string name, SampleDataImportPushNotification pushNotification, PerformContext context, CancellationToken cancellationToken)
        {
            void progressCallback(ExportImportProgressInfo x)
            {
                pushNotification.Path(x);
                pushNotification.JobId = context.BackgroundJob.Id;
                _pushNotifier.Send(pushNotification);
            }

            var wasCancelled = false;
            try
            {
                var url = (await InnerDiscoverSampleDataAsync()).FirstOrDefault(x => x.Name == name)?.Url;
                if (url is null)
                {
                    return;
                }

                pushNotification.Description = "Start downloading from " + url;

                await _pushNotifier.SendAsync(pushNotification);

                var tmpPath = Path.GetFullPath(_platformOptions.LocalUploadFolderPath);
                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }

                var tmpFilePath = Path.Combine(tmpPath, Path.GetFileName(url));

                await DownloadFileAsync(new Uri(url), tmpFilePath, async (bytesReceived, bytesTotal) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var message = $"Sample data {bytesReceived.ToHumanReadableSize()} of {bytesTotal.ToHumanReadableSize()} downloading...";
                    if (message != pushNotification.Description)
                    {
                        pushNotification.Description = message;
                        await _pushNotifier.SendAsync(pushNotification);
                    }
                });

                using var stream = new FileStream(tmpFilePath, FileMode.Open);
                var manifest = _platformExportManager.ReadExportManifest(stream);
                if (manifest != null)
                {
                    await _platformExportManager.ImportAsync(stream, manifest, progressCallback, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Also catches Hangfire.JobAbortedException, which derives from OperationCanceledException.
                wasCancelled = true;
                _log?.LogWarning("Sample data import job {JobId} started by {User} was cancelled.",
                    context?.BackgroundJob?.Id, pushNotification?.Creator);
            }
            catch (Exception ex)
            {
                pushNotification.Errors.Add(ex.ExpandExceptionMessage());
            }
            finally
            {
                await _settingsManager.SetValueAsync(PlatformConstants.Settings.Setup.SampleDataState.Name, SampleDataState.Completed);
                pushNotification.Description = wasCancelled
                    ? "Sample data import process was cancelled."
                    : "Sample data import process completed successfully.";
                pushNotification.Finished = DateTime.UtcNow;
                await _pushNotifier.SendAsync(pushNotification);
            }
        }

        // Shim for in-flight queue items: ShutdownToken only — UI delete won't cancel jobs on this path.
        [Obsolete("Hangfire compatibility shim for legacy queue items. Use the overload with CancellationToken.",
            DiagnosticId = "VC0014",
            UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public Task SampleDataImportBackgroundAsync(string name, SampleDataImportPushNotification pushNotification, IJobCancellationToken cancellationToken, PerformContext context)
            => SampleDataImportBackgroundAsync(name, pushNotification, context, cancellationToken?.ShutdownToken ?? CancellationToken.None);

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

        private async Task DownloadFileAsync(Uri uri, string filePath, Func<long, long, Task> progress)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var contentLength = response.Content.Headers.ContentLength ?? -1L;
            var bytesReceived = 0L;
            var bytesTotal = contentLength < 0 ? 0L : contentLength;

            const int defaultBufferSize = 65536;
            var bufferSize = contentLength < 0 || contentLength > defaultBufferSize ? defaultBufferSize : (int)contentLength;
            var buffer = new byte[bufferSize];

            await using var readStream = await response.Content.ReadAsStreamAsync();
            await using var writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous);

            while (true)
            {
                var bytesRead = await readStream.ReadAsync(new Memory<byte>(buffer)).ConfigureAwait(false);

                if (bytesRead == 0)
                {
                    break;
                }

                bytesReceived += bytesRead;

                if (contentLength < 0)
                {
                    bytesTotal = bytesReceived;
                }

                await writeStream.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead)).ConfigureAwait(false);

                await progress(bytesReceived, bytesTotal);
            }
        }
    }
}
