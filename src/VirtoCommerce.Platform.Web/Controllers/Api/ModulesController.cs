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
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/modules")]
    [Authorize]
    public class ModulesController : Controller
    {
        private const string _managementIsDisabledMessage = "Module management is disabled.";

        private readonly IExternalModuleCatalog _externalModuleCatalog;
        private readonly IModuleInstaller _moduleInstaller;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;
        private readonly ISettingsManager _settingsManager;
        private readonly PlatformOptions _platformOptions;
        private readonly ExternalModuleCatalogOptions _externalModuleCatalogOptions;
        private readonly LocalStorageModuleCatalogOptions _localStorageModuleCatalogOptions;
        private readonly IPlatformRestarter _platformRestarter;
        private static readonly object _lockObject = new object();
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly ILocalModuleCatalog _localModuleCatalog;

        public ModulesController(
            IExternalModuleCatalog externalModuleCatalog,
            IModuleInstaller moduleInstaller,
            IPushNotificationManager pushNotifier,
            IUserNameResolver userNameResolver,
            ISettingsManager settingsManager,
            IOptions<PlatformOptions> platformOptions,
            IOptions<ExternalModuleCatalogOptions> externalModuleCatalogOptions,
            IOptions<LocalStorageModuleCatalogOptions> localStorageModuleCatalogOptions,
            IPlatformRestarter platformRestarter,
            ILocalModuleCatalog localModuleCatalog)
        {
            _externalModuleCatalog = externalModuleCatalog;
            _moduleInstaller = moduleInstaller;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _settingsManager = settingsManager;
            _platformOptions = platformOptions.Value;
            _externalModuleCatalogOptions = externalModuleCatalogOptions.Value;
            _localStorageModuleCatalogOptions = localStorageModuleCatalogOptions.Value;
            _platformRestarter = platformRestarter;
            _localModuleCatalog = localModuleCatalog;
        }

        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reload")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ReloadModules()
        {
            _externalModuleCatalog.Reload();
            return NoContent();
        }

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        public ActionResult<ModuleDescriptor[]> GetModules()
        {
            EnsureModulesCatalogInitialized();

            var allModules = _externalModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version)
                .Select(x => new ModuleDescriptor(x))
                .ToList();

            _localModuleCatalog.Initialize();
            var localModules = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>().ToDictionary(x => x.Id);

            foreach (var module in allModules.Where(x => !string.IsNullOrEmpty(x.IconUrl)))
            {
                module.IconUrl = localModules.TryGetValue(module.Id, out var localModule) && IconFileExists(localModule)
                    ? localModule.IconUrl
                    : null;
            }

            return Ok(allModules);
        }

        private static bool IconFileExists(ManifestModuleInfo module)
        {
            // PathString should start from "/"
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

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdependents")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        public ActionResult<ModuleDescriptor[]> GetDependingModules([FromBody] ModuleDescriptor[] moduleDescriptors)
        {
            EnsureModulesCatalogInitialized();

            var modules = _externalModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Join(moduleDescriptors, x => x.Identity, y => y.Identity, (x, y) => x)
                .ToList();

            var retVal = GetDependingModulesRecursive(modules).Distinct()
                                                              .Except(modules)
                                                              .Select(x => new ModuleDescriptor(x))
                                                              .ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getmissingdependencies")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleQuery)]
        public ActionResult<ModuleDescriptor[]> GetMissingDependencies([FromBody] ModuleDescriptor[] moduleDescriptors)
        {
            EnsureModulesCatalogInitialized();
            var modules = _externalModuleCatalog.Modules
                                        .OfType<ManifestModuleInfo>().Join(moduleDescriptors, x => x.Identity, y => y.Identity, (x, y) => x)
                                        .ToList();

            var result = _externalModuleCatalog.CompleteListWithDependencies(modules)
                                       .OfType<ManifestModuleInfo>()
                                       .Where(x => !x.IsInstalled)
                                       .Except(modules)
                                       .Select(x => new ModuleDescriptor(x))
                                       .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("localstorage")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        public async Task<ActionResult<ModuleDescriptor>> UploadModuleArchive()
        {
            EnsureModulesCatalogInitialized();

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
            var existingModule = _externalModuleCatalog.Modules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.Equals(module));

            if (existingModule != null)
            {
                module = existingModule;
            }
            else
            {
                //Force dependency validation for new module
                _externalModuleCatalog.CompleteListWithDependencies([module]).ToList().Clear();
                _externalModuleCatalog.AddModule(module);
            }

            module.Ref = targetFilePath;
            var result = new ModuleDescriptor(module);

            return Ok(result);
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

        private static async Task<ModuleManifest> LoadModuleManifestFromZipArchive(string path)
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
            catch
            {
                // Suppress any exceptions
            }

            return manifest;
        }

        /// <summary>
        /// Install modules 
        /// </summary>
        /// <param name="modules">modules for install</param>
        /// <returns></returns>
        [HttpPost]
        [Route("install")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        public ActionResult<ModulePushNotification> InstallModules([FromBody] ModuleDescriptor[] modules)
        {
            EnsureModulesCatalogInitialized();

            var options = new ModuleBackgroundJobOptions
            {
                Action = ModuleAction.Install,
                Modules = modules
            };
            var result = ScheduleJob(options);
            return Ok(result);
        }

        /// <summary>
        /// Update modules 
        /// </summary>
        /// <param name="modules">modules for update</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        public ActionResult<ModulePushNotification> UpdateModules([FromBody] ModuleDescriptor[] modules)
        {
            EnsureModulesCatalogInitialized();

            var options = new ModuleBackgroundJobOptions
            {
                Action = ModuleAction.Update,
                Modules = modules
            };
            var result = ScheduleJob(options);
            return Ok(result);
        }

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <param name="modules">modules</param>
        /// <returns></returns>
        [HttpPost]
        [Route("uninstall")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        public ActionResult<ModulePushNotification> UninstallModule([FromBody] ModuleDescriptor[] modules)
        {
            EnsureModulesCatalogInitialized();

            var options = new ModuleBackgroundJobOptions
            {
                Action = ModuleAction.Uninstall,
                Modules = modules
            };
            var result = ScheduleJob(options);
            return Ok(result);
        }

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        [Route("autoinstall")]
        public ActionResult<ModuleAutoInstallPushNotification> TryToAutoInstallModules()
        {
            var notification = new ModuleAutoInstallPushNotification(User.Identity.Name)
            {
                Title = "Modules installation",
                //set completed by default
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

                            EnsureModulesCatalogInitialized();

                            // Skip Auto Installation if some modules already installed manually
                            if (!_externalModuleCatalog.Modules.OfType<ManifestModuleInfo>().Any(x => x.IsInstalled))
                            {
                                InstallModulesFromBundles(moduleBundles, notification);
                            }
                        }
                    }
                }
            }
            return Ok(notification);
        }

        private void InstallModulesFromBundles(string[] moduleBundles, ModuleAutoInstallPushNotification notification)
        {
            var modules = new List<ManifestModuleInfo>();
            var moduleVersionGroups = _externalModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.Groups.Intersect(moduleBundles, StringComparer.OrdinalIgnoreCase).Any())
                .GroupBy(x => x.Id);

            //Need install only latest versions
            foreach (var moduleVersionGroup in moduleVersionGroups)
            {
                var alreadyInstalledModule = _externalModuleCatalog.Modules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.IsInstalled && x.Id.EqualsIgnoreCase(moduleVersionGroup.Key));
                //skip already installed modules
                if (alreadyInstalledModule == null)
                {
                    var latestVersion = moduleVersionGroup.OrderBy(x => x.Version).LastOrDefault();
                    if (latestVersion != null)
                    {
                        modules.Add(latestVersion);
                    }
                }
            }

            var modulesWithDependencies = _externalModuleCatalog.CompleteListWithDependencies(modules)
                .OfType<ManifestModuleInfo>()
                .Where(x => !x.IsInstalled)
                .Select(x => new ModuleDescriptor(x))
                .ToArray();

            if (modulesWithDependencies.Any())
            {
                var options = new ModuleBackgroundJobOptions
                {
                    Action = ModuleAction.Install,
                    Modules = modulesWithDependencies
                };
                //reset finished date
                notification.Finished = null;

                // can't use Hangfire.BackgroundJob.Enqueue(...), because Hangfire tables might be missing in new DB
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    ModuleBackgroundJob(options, notification);
                }).Start();
            }
        }

        /// <summary>
        /// This method used by azure automatically deployment scripts to check the installation status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("autoinstall/state")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public ActionResult<string> GetAutoInstallState()
        {
            var state = EnumUtility.SafeParse(_settingsManager.GetValue<string>(PlatformConstants.Settings.Setup.ModulesAutoInstallState), AutoInstallState.Undefined);
            return Ok(state);
        }

        [HttpGet]
        [Route("loading-order")]
        [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
        public ActionResult<string[]> GetModulesLoadingOrder()
        {
            EnsureModulesCatalogInitialized();

            var modules = _externalModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.IsInstalled)
                .ToArray();

            var loadingOrder = _externalModuleCatalog.CompleteListWithDependencies(modules)
                .OfType<ManifestModuleInfo>()
                .Select(x => x.Id)
                .ToArray();

            return Ok(loadingOrder);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ModuleBackgroundJob(ModuleBackgroundJobOptions options, ModulePushNotification notification)
        {
            try
            {
                notification.Started = DateTime.UtcNow;

                if (_localStorageModuleCatalogOptions.RefreshProbingFolderOnStart)
                {
                    var moduleInfos = _externalModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                        .Where(x => options.Modules.Any(y => y.Identity.Equals(x.Identity)))
                        .ToArray();
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
                            _moduleInstaller.Install(moduleInfos, reportProgress);
                            break;
                        case ModuleAction.Uninstall:
                            _moduleInstaller.Uninstall(moduleInfos, reportProgress);
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

        private void EnsureModulesCatalogInitialized()
        {
            _externalModuleCatalog.Initialize();
        }

        private IEnumerable<ManifestModuleInfo> GetDependingModulesRecursive(IEnumerable<ManifestModuleInfo> modules)
        {
            var retVal = new List<ManifestModuleInfo>();
            foreach (var module in modules)
            {
                retVal.Add(module);
                var dependingModules = _externalModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                                                             .Where(x => x.IsInstalled)
                                                             .Where(x => x.DependsOn.ContainsIgnoreCase(module.Id))
                                                             .ToList();
                if (dependingModules.Any())
                {
                    retVal.AddRange(GetDependingModulesRecursive(dependingModules));
                }
            }
            return retVal;
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
    }
}
