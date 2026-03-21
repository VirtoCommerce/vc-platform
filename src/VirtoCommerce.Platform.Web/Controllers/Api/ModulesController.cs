using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Helpers;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/modules")]
    [Authorize]
    public class ModulesController : Controller
    {
        private const string _managementIsDisabledMessage = "Module management is disabled.";

        private readonly IModuleManagementService _moduleService;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;
        private readonly ISettingsManager _settingsManager;
        private readonly PlatformOptions _platformOptions;
        private readonly ExternalModuleCatalogOptions _externalModuleCatalogOptions;
        private readonly LocalStorageModuleCatalogOptions _localStorageModuleCatalogOptions;
        private readonly IPlatformRestarter _platformRestarter;
        private readonly ILogger<ModulesController> _logger;
        private static readonly object _lockObject = new object();
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public ModulesController(
            IModuleManagementService moduleService,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            ISettingsManager settingsManager,
            IOptions<PlatformOptions> platformOptions,
            IOptions<ExternalModuleCatalogOptions> externalModuleCatalogOptions,
            IOptions<LocalStorageModuleCatalogOptions> localStorageModuleCatalogOptions,
            IPlatformRestarter platformRestarter,
            ILogger<ModulesController> logger)
        {
            _moduleService = moduleService;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _settingsManager = settingsManager;
            _platformOptions = platformOptions.Value;
            _externalModuleCatalogOptions = externalModuleCatalogOptions.Value;
            _localStorageModuleCatalogOptions = localStorageModuleCatalogOptions.Value;
            _platformRestarter = platformRestarter;
            _logger = logger;
        }

        /// <summary>
        /// Reload modules
        /// </summary>
        [HttpPost]
        [Route("reload")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ReloadModules()
        {
            _moduleService.ReloadModules();
            return NoContent();
        }

        /// <summary>
        /// Get installed modules
        /// </summary>
        [HttpGet]
        [Route("")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        [ProducesResponseType(typeof(ModuleDescriptor[]), StatusCodes.Status200OK)]
        public ActionResult<ModuleDescriptor[]> GetModules()
        {
            var allModules = _moduleService.GetModules()
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version)
                .Select(x => new ModuleDescriptor(x))
                .ToList();

            var localModules = ModuleRegistry.GetAllModules().ToDictionary(x => x.Id);

            foreach (var module in allModules.Where(x => !string.IsNullOrEmpty(x.IconUrl)))
            {
                module.IconUrl = localModules.TryGetValue(module.Id, out var localModule) && IconFileExists(localModule)
                    ? localModule.IconUrl
                    : null;
            }

            return Ok(allModules);
        }

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        [HttpPost]
        [Route("getdependents")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        [ProducesResponseType(typeof(ModuleDescriptor[]), StatusCodes.Status200OK)]
        public ActionResult<ModuleDescriptor[]> GetDependingModules([FromBody] ModuleDescriptor[] moduleDescriptors)
        {
            var moduleIds = moduleDescriptors.Select(x => x.Id).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var retVal = _moduleService.GetDependents(moduleIds).Distinct()
                .Where(x => !moduleIds.Contains(x.Id, StringComparer.OrdinalIgnoreCase))
                .Select(x => new ModuleDescriptor(x))
                .ToArray();

            return Ok(retVal);
        }

        /// <summary>
        /// Returns a flat expanded list of modules that depend on passed modules
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        [HttpPost]
        [Route("getmissingdependencies")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        [ProducesResponseType(typeof(ModuleDescriptor[]), StatusCodes.Status200OK)]
        public ActionResult<ModuleDescriptor[]> GetMissingDependencies([FromBody] ModuleDescriptor[] moduleDescriptors)
        {
            var moduleIds = moduleDescriptors.Select(x => x.Id).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var result = _moduleService.GetDependencies(moduleIds)
                .Where(x => !x.IsInstalled)
                .Where(x => !moduleIds.Contains(x.Id, StringComparer.OrdinalIgnoreCase))
                .Select(x => new ModuleDescriptor(x))
                .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        [HttpPost]
        [Route("localstorage")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModuleDescriptor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModuleDescriptor>> UploadModuleArchive()
        {
            if (!_localStorageModuleCatalogOptions.RefreshProbingFolderOnStart)
            {
                return BadRequest(_managementIsDisabledMessage);
            }

            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            var targetFilePath = await UploadFile(Request, Path.GetFullPath(_platformOptions.LocalUploadFolderPath));
            if (targetFilePath is null)
            {
                return BadRequest("Cannot read file");
            }

            var manifest = await LoadModuleManifestFromZipArchive(targetFilePath);
            if (manifest is null)
            {
                return BadRequest("Cannot read module manifest");
            }

            var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
            module.LoadFromManifest(manifest);

            module = _moduleService.AddUploadedModule(module);
            module.Ref = targetFilePath;

            return Ok(new ModuleDescriptor(module));
        }

        /// <summary>
        /// Install modules
        /// </summary>
        /// <param name="modules">modules for install</param>
        [HttpPost]
        [Route("install")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> InstallModules([FromBody] ModuleDescriptor[] modules)
        {
            var requests = modules.Select(m => new ModuleInstallRequest(m.Id, m.Version)).ToArray();
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Install, Modules = requests }));
        }

        /// <summary>
        /// Install modules using lightweight install requests
        /// </summary>
        /// <param name="modules">module install requests (id + optional version)</param>
        [HttpPost]
        [Route("install/modules")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> InstallModuleRequests([FromBody] ModuleInstallRequest[] modules)
        {
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Install, Modules = modules }));
        }

        /// <summary>
        /// Update modules
        /// </summary>
        /// <param name="modules">modules for update</param>
        [HttpPost]
        [Route("update")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> UpdateModules([FromBody] ModuleDescriptor[] modules)
        {
            var requests = modules.Select(m => new ModuleInstallRequest(m.Id, m.Version)).ToArray();
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Update, Modules = requests }));
        }

        /// <summary>
        /// Update modules using lightweight install requests
        /// </summary>
        /// <param name="modules">module install requests (id + optional version)</param>
        [HttpPost]
        [Route("update/modules")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> UpdateModuleRequests([FromBody] ModuleInstallRequest[] modules)
        {
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Update, Modules = modules }));
        }

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <param name="modules">modules</param>
        [HttpPost]
        [Route("uninstall")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> UninstallModule([FromBody] ModuleDescriptor[] modules)
        {
            var requests = modules.Select(m => new ModuleInstallRequest(m.Id, m.Version)).ToArray();
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Uninstall, Modules = requests }));
        }

        /// <summary>
        /// Uninstall modules using lightweight install requests
        /// </summary>
        /// <param name="modules">module install requests (id only, version ignored)</param>
        [HttpPost]
        [Route("uninstall/modules")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> UninstallModuleRequests([FromBody] ModuleInstallRequest[] modules)
        {
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Uninstall, Modules = modules }));
        }

        /// <summary>
        /// Restart web application
        /// </summary>
        [HttpPost]
        [Route("restart")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult Restart()
        {
            _platformRestarter.Restart();
            return NoContent();
        }

        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        [HttpPost]
        [Route("autoinstall")]
        [ProducesResponseType(typeof(ModuleAutoInstallPushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModuleAutoInstallPushNotification> TryToAutoInstallModules()
        {
            var notification = new ModuleAutoInstallPushNotification(User.Identity.Name)
            {
                Title = "Modules installation",
                Finished = DateTime.UtcNow
            };

            if (!_settingsManager.GetValue<bool>(PlatformConstants.Settings.Setup.ModulesAutoInstalled))
            {
                lock (_lockObject)
                {
                    if (!_settingsManager.GetValue<bool>(PlatformConstants.Settings.Setup.ModulesAutoInstalled))
                    {
                        var moduleBundles = _externalModuleCatalogOptions.AutoInstallModuleBundles;
                        if (!moduleBundles.IsNullOrEmpty())
                        {
                            _settingsManager.SetValue(PlatformConstants.Settings.Setup.ModulesAutoInstalled.Name, true);
                            _settingsManager.SetValue(PlatformConstants.Settings.Setup.ModulesAutoInstallState.Name, AutoInstallState.Processing);

                            if (!_moduleService.GetModules().Any(x => x.IsInstalled))
                            {
                                var autoInstallModules = _moduleService.GetAutoInstallModules(moduleBundles);
                                if (autoInstallModules.Any())
                                {
                                    var options = new ModuleBackgroundJobOptions
                                    {
                                        Action = ModuleAction.Install,
                                        Modules = autoInstallModules.Select(x => new ModuleInstallRequest(x.Id, x.Version.ToString())).ToArray()
                                    };
                                    notification.Finished = null;

                                    // can't use Hangfire.BackgroundJob.Enqueue(...), because Hangfire tables might be missing in new DB
                                    new Thread(() =>
                                    {
                                        Thread.CurrentThread.IsBackground = true;
                                        ModuleBackgroundJob(options, notification);
                                    }).Start();
                                }
                            }
                        }
                    }
                }
            }

            return Ok(notification);
        }

        /// <summary>
        /// This method used by azure automatically deployment scripts to check the installation status
        /// </summary>
        [HttpGet]
        [Route("autoinstall/state")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public ActionResult<string> GetAutoInstallState()
        {
            var state = EnumUtility.SafeParse(_settingsManager.GetValue<string>(PlatformConstants.Settings.Setup.ModulesAutoInstallState), AutoInstallState.Undefined);
            return Ok(state);
        }

        /// <summary>
        /// Get module loading order
        /// </summary>
        [HttpGet]
        [Route("loading-order")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        public ActionResult<string[]> GetModulesLoadingOrder()
        {
            var moduleIds = _moduleService.GetModules()
                .Where(x => x.IsInstalled)
                .Select(x => x.Id)
                .ToList();

            var loadingOrder = _moduleService.GetDependencies(moduleIds)
                .Select(x => x.Id)
                .ToArray();

            return Ok(loadingOrder);
        }

        /// <summary>
        /// Validate that a specific module version package exists at the download URL.
        /// </summary>
        /// <param name="moduleId">Module identifier</param>
        /// <param name="version">Version to validate</param>
        [HttpGet]
        [Route("{moduleId}/versions/{version}/validate")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ValidateModuleVersion(string moduleId, string version)
        {
            return Ok(await _moduleService.ValidateModuleVersionAsync(moduleId, version));
        }

        /// <summary>
        /// Install a specific version of a module.
        /// Validates the package URL, registers the custom version, and schedules installation.
        /// </summary>
        /// <param name="moduleId">Module identifier</param>
        /// <param name="version">Version to install</param>
        [HttpPost]
        [Route("{moduleId}/versions/{version}/install")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModulePushNotification>> InstallModuleVersion(string moduleId, string version)
        {
            var moduleInfo = await _moduleService.RegisterCustomModuleVersionAsync(moduleId, version);
            if (moduleInfo == null)
            {
                return NotFound();
            }

            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Install, Modules = [new ModuleInstallRequest(moduleId, version)] }));
        }

        /// <summary>
        /// Install the latest available version of a module.
        /// </summary>
        /// <param name="moduleId">Module identifier</param>
        [HttpPost]
        [Route("{moduleId}/install")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> InstallModule(string moduleId)
        {
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Install, Modules = [new ModuleInstallRequest(moduleId)] }));
        }


        /// <summary>
        /// Uninstall a module.
        /// </summary>
        /// <param name="moduleId">Module identifier</param>
        [HttpPost]
        [Route("{moduleId}/uninstall")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        [ProducesResponseType(typeof(ModulePushNotification), StatusCodes.Status200OK)]
        public ActionResult<ModulePushNotification> UninstallSingleModule(string moduleId)
        {
            return Ok(ScheduleJob(new ModuleBackgroundJobOptions { Action = ModuleAction.Uninstall, Modules = [new ModuleInstallRequest(moduleId)] }));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ModuleBackgroundJob(ModuleBackgroundJobOptions options, ModulePushNotification notification)
        {
            try
            {
                notification.Started = DateTime.UtcNow;

                if (_localStorageModuleCatalogOptions.RefreshProbingFolderOnStart)
                {
                    var reportProgress = new Progress<ProgressMessage>(m =>
                    {
                        lock (_lockObject)
                        {
                            notification.Description = m.Message;
                            notification.ProgressLog.Add(m);
                            _pushNotifier.Send(notification);
                        }
                    });

                    switch (options.Action)
                    {
                        case ModuleAction.Install:
                        case ModuleAction.Update:
                            _moduleService.InstallModules(options.Modules, reportProgress);
                            break;
                        case ModuleAction.Uninstall:
                            _moduleService.UninstallModules(options.Modules.Select(m => m.Id).ToList(), reportProgress);
                            break;
                    }
                }
                else
                {
                    notification.Finished = DateTime.UtcNow;
                    notification.Description = _managementIsDisabledMessage;
                    notification.ProgressLog.Add(new ProgressMessage
                    {
                        Level = ProgressMessageLevel.Error,
                        Message = notification.Description,
                    });
                    _pushNotifier.Send(notification);
                }
            }
            catch (Exception ex)
            {
                notification.ProgressLog.Add(new ProgressMessage
                {
                    Level = ProgressMessageLevel.Error,
                    Message = ex.ToString(),
                });
            }
            finally
            {
                _settingsManager.SetValue(PlatformConstants.Settings.Setup.ModulesAutoInstallState.Name, AutoInstallState.Completed);

                notification.Finished = DateTime.UtcNow;
                notification.Description = options.Action switch
                {
                    ModuleAction.Install => "Installation finished.",
                    ModuleAction.Update => "Updating finished.",
                    _ => "Uninstalling finished."
                };

                notification.ProgressLog.Add(new ProgressMessage
                {
                    Level = ProgressMessageLevel.Info,
                    Message = notification.Description,
                });

                _pushNotifier.Send(notification);
            }
        }

        private ModulePushNotification ScheduleJob(ModuleBackgroundJobOptions options)
        {
            var notification = new ModulePushNotification(_userNameResolver.GetCurrentUserName());

            switch (options.Action)
            {
                case ModuleAction.Install:
                    notification.Title = "Install Module";
                    notification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Info, Message = "Starting installation..." });
                    break;
                case ModuleAction.Uninstall:
                    notification.Title = "Uninstall Module";
                    notification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Info, Message = "Starting uninstall..." });
                    break;
                case ModuleAction.Update:
                    notification.Title = "Update Module";
                    notification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Info, Message = "Starting update..." });
                    break;
            }

            _pushNotifier.Send(notification);

            BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification));

            return notification;
        }

        private static bool IconFileExists(ManifestModuleInfo module)
        {
            var moduleIconUrl = module.IconUrl;
            if (!moduleIconUrl.StartsWith('/'))
            {
                moduleIconUrl = "/" + moduleIconUrl;
            }

            var basePath = new PathString($"/modules/$({module.Id})");
            var iconUrlPath = new PathString(moduleIconUrl);

            if (!iconUrlPath.StartsWithSegments(basePath, out var subPath) ||
                string.IsNullOrEmpty(subPath.Value) ||
                !Directory.Exists(module.FullPhysicalPath))
            {
                return false;
            }

            using var fileProvider = new PhysicalFileProvider(module.FullPhysicalPath);
            return fileProvider.GetFileInfo(subPath.Value).Exists;
        }

        private static async Task<string> UploadFile(HttpRequest request, string uploadFolderPath)
        {
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, request.Body);
            var section = await reader.ReadNextSectionAsync();

            if (section == null)
            {
                return null;
            }

            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition) ||
                !MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
            {
                return null;
            }

            var fileName = Path.GetFileName(contentDisposition.FileName.Value);
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var targetFilePath = Path.Combine(uploadFolderPath, fileName);

            await using var targetStream = System.IO.File.Create(targetFilePath);
            await section.Body.CopyToAsync(targetStream);

            return targetFilePath;
        }

        private async Task<ModuleManifest> LoadModuleManifestFromZipArchive(string path)
        {
            ModuleManifest manifest = null;

            try
            {
                await using var packageStream = System.IO.File.Open(path, FileMode.Open);
                using var package = new ZipArchive(packageStream, ZipArchiveMode.Read);

                var entry = package.GetEntry("module.manifest");
                if (entry != null)
                {
                    await using var manifestStream = entry.Open();
                    manifest = ManifestReader.Read(manifestStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read module manifest from {Path}", path);
            }

            return manifest;
        }
    }
}
