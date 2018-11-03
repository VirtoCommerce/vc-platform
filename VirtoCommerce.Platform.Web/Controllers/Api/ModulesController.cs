using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Web.Assets;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Web.Converters.Modularity;
using webModel = VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/modules")]
    [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
    public class ModulesController : ApiController
    {
        private const string _autoInstallStateSetting = "VirtoCommerce.ModulesAutoInstallState";

        private readonly string _uploadsUrl = Startup.VirtualRoot + "/App_Data/Uploads/";
        private readonly IModuleCatalog _moduleCatalog;
        private readonly IModuleInstaller _moduleInstaller;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;
        private readonly ISettingsManager _settingsManager;
        private static readonly object _lockObject = new object();

        public ModulesController(IModuleCatalog moduleCatalog, IModuleInstaller moduleInstaller, IPushNotificationManager pushNotifier, IUserNameResolver userNameResolver, ISettingsManager settingsManager)
        {
            _moduleCatalog = moduleCatalog;
            _moduleInstaller = moduleInstaller;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// Reload  modules
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reload")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
        public IHttpActionResult ReloadModules()
        {
            _moduleCatalog.Reload();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
        public IHttpActionResult GetModules()
        {
            EnsureModulesCatalogInitialized();

            var retVal = _moduleCatalog.Modules
                                       .OfType<ManifestModuleInfo>().OrderBy(x => x.Id).ThenBy(x => x.Version)
                                       .Select(x => x.ToWebModel())
                                       .ToArray();

            return Ok(retVal);
        }

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdependents")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
        public IHttpActionResult GetDependingModules(webModel.ModuleDescriptor[] moduleDescriptors)
        {
            EnsureModulesCatalogInitialized();

            var modules = _moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Join(moduleDescriptors, x => x.Identity, y => y.Identity, (x, y) => x)
                .ToList();

            var retVal = GetDependingModulesRecursive(modules).Distinct()
                                                              .Except(modules)
                                                              .Select(x => x.ToWebModel())
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
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
        public IHttpActionResult GetMissingDependencies(webModel.ModuleDescriptor[] moduleDescriptors)
        {
            EnsureModulesCatalogInitialized();
            var modules = _moduleCatalog.Modules
                                        .OfType<ManifestModuleInfo>().Join(moduleDescriptors, x => x.Identity, y => y.Identity, (x, y) => x)
                                        .ToList();

            var retVal = _moduleCatalog.CompleteListWithDependencies(modules)
                                       .OfType<ManifestModuleInfo>()
                                       .Where(x => !x.IsInstalled)
                                       .Except(modules)
                                       .Select(x => x.ToWebModel())
                                       .ToArray();

            return Ok(retVal);
        }

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("localstorage")]
        [ResponseType(typeof(webModel.ModuleDescriptor))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public async Task<IHttpActionResult> UploadModuleArchive()
        {
            EnsureModulesCatalogInitialized();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }

            webModel.ModuleDescriptor retVal = null;
            var uploadsPath = HostingEnvironment.MapPath(_uploadsUrl);
            var streamProvider = new CustomMultipartFormDataStreamProvider(uploadsPath);

            await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
            });

            var fileData = streamProvider.FileData.FirstOrDefault();

            using (var packageStream = File.Open(fileData.LocalFileName, FileMode.Open))
            using (var package = new ZipArchive(packageStream, ZipArchiveMode.Read))
            {
                var entry = package.GetEntry("module.manifest");
                if (entry != null)
                {
                    using (var manifestStream = entry.Open())
                    {
                        var manifest = ManifestReader.Read(manifestStream);
                        var module = new ManifestModuleInfo(manifest);
                        var alreadyExistModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.Equals(module));
                        if (alreadyExistModule != null)
                        {
                            module = alreadyExistModule;
                        }
                        else
                        {
                            //Force dependency validation for new module
                            _moduleCatalog.CompleteListWithDependencies(new[] { module }).ToList().Clear();
                            _moduleCatalog.AddModule(module);
                        }

                        module.Ref = fileData.LocalFileName;
                        retVal = module.ToWebModel();
                    }
                }
            }

            return Ok(retVal);
        }

        /// <summary>
        /// Install modules 
        /// </summary>
        /// <param name="modules">modules for install</param>
        /// <returns></returns>
        [HttpPost]
        [Route("install")]
        [ResponseType(typeof(webModel.ModulePushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult InstallModules(webModel.ModuleDescriptor[] modules)
        {
            EnsureModulesCatalogInitialized();

            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = webModel.ModuleAction.Install,
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
        [ResponseType(typeof(webModel.ModulePushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult UninstallModule(webModel.ModuleDescriptor[] modules)
        {
            EnsureModulesCatalogInitialized();

            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = webModel.ModuleAction.Uninstall,
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
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Auto-install modules with specified groups
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("autoinstall")]
        [ResponseType(typeof(webModel.ModuleAutoInstallPushNotification))]
        public IHttpActionResult TryToAutoInstallModules()
        {
            var notification = new webModel.ModuleAutoInstallPushNotification(User.Identity.Name)
            {
                Title = "Modules installation",
                //set completed by default
                Finished = DateTime.UtcNow
            };


            if (!_settingsManager.GetValue("VirtoCommerce.ModulesAutoInstalled", false))
            {
                lock (_lockObject)
                {
                    if (!_settingsManager.GetValue("VirtoCommerce.ModulesAutoInstalled", false))
                    {
                        var moduleBundles = ConfigurationHelper.SplitAppSettingsStringValue("VirtoCommerce:AutoInstallModuleBundles");
                        if (!moduleBundles.IsNullOrEmpty())
                        {
                            _settingsManager.SetValue("VirtoCommerce.ModulesAutoInstalled", true);
                            _settingsManager.SetValue(_autoInstallStateSetting, webModel.AutoInstallState.Processing);

                            EnsureModulesCatalogInitialized();

                            var modules = new List<ManifestModuleInfo>();
                            var moduleVersionGroups = _moduleCatalog.Modules
                                .OfType<ManifestModuleInfo>()
                                .Where(x => x.Groups.Intersect(moduleBundles, StringComparer.OrdinalIgnoreCase).Any())
                                .GroupBy(x => x.Id);

                            //Need install only latest versions
                            foreach (var moduleVersionGroup in moduleVersionGroups)
                            {
                                var alreadyInstalledModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.IsInstalled && x.Id.EqualsInvariant(moduleVersionGroup.Key));
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

                            var modulesWithDependencies = _moduleCatalog.CompleteListWithDependencies(modules)
                                .OfType<ManifestModuleInfo>()
                                .Where(x => !x.IsInstalled)
                                .Select(x => x.ToWebModel())
                                .ToArray();

                            if (modulesWithDependencies.Any())
                            {
                                var options = new webModel.ModuleBackgroundJobOptions
                                {
                                    Action = webModel.ModuleAction.Install,
                                    Modules = modulesWithDependencies
                                };
                                //reset finished date
                                notification.Finished = null;
                                BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification));
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
        /// <returns></returns>
        [HttpGet]
        [Route("autoinstall/state")]
        [ResponseType(typeof(string))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public IHttpActionResult GetAutoInstallState()
        {
            var state = EnumUtility.SafeParse(_settingsManager.GetValue(_autoInstallStateSetting, string.Empty), webModel.AutoInstallState.Undefined);
            return Ok(state);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ModuleBackgroundJob(webModel.ModuleBackgroundJobOptions options, webModel.ModulePushNotification notification)
        {
            try
            {
                notification.Started = DateTime.UtcNow;
                var moduleInfos = _moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                     .Where(x => options.Modules.Any(y => y.Identity.Equals(x.Identity)))
                                     .ToArray();
                var reportProgress = new Progress<ProgressMessage>(m =>
                {
                    lock (_lockObject)
                    {
                        notification.ProgressLog.Add(m.ToWebModel());
                        _pushNotifier.Upsert(notification);
                    }
                });

                switch (options.Action)
                {
                    case webModel.ModuleAction.Install:
                        _moduleInstaller.Install(moduleInfos, reportProgress);
                        break;
                    case webModel.ModuleAction.Uninstall:
                        _moduleInstaller.Uninstall(moduleInfos, reportProgress);
                        break;
                }
            }
            catch (Exception ex)
            {
                notification.ProgressLog.Add(new webModel.ProgressMessage
                {
                    Level = ProgressMessageLevel.Error.ToString(),
                    Message = ex.ExpandExceptionMessage(),
                });
            }
            finally
            {
                _settingsManager.SetValue(_autoInstallStateSetting, webModel.AutoInstallState.Completed);

                notification.Finished = DateTime.UtcNow;
                notification.ProgressLog.Add(new webModel.ProgressMessage
                {
                    Level = ProgressMessageLevel.Info.ToString(),
                    Message = "Installation finished.",
                });
                _pushNotifier.Upsert(notification);
            }
        }

        private void EnsureModulesCatalogInitialized()
        {
            _moduleCatalog.Initialize();
        }

        private IEnumerable<ManifestModuleInfo> GetDependingModulesRecursive(IEnumerable<ManifestModuleInfo> modules)
        {
            var retVal = new List<ManifestModuleInfo>();
            foreach (var module in modules)
            {
                retVal.Add(module);
                var dependingModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                                             .Where(x => x.IsInstalled)
                                                             .Where(x => x.DependsOn.Contains(module.Id, StringComparer.OrdinalIgnoreCase))
                                                             .ToList();
                if (dependingModules.Any())
                {
                    retVal.AddRange(GetDependingModulesRecursive(dependingModules));
                }
            }
            return retVal;
        }

        private webModel.ModulePushNotification ScheduleJob(webModel.ModuleBackgroundJobOptions options)
        {
            var notification = new webModel.ModulePushNotification(_userNameResolver.GetCurrentUserName());

            switch (options.Action)
            {
                case webModel.ModuleAction.Install:
                    notification.Title = "Install Module";
                    notification.ProgressLog.Add(new webModel.ProgressMessage { Level = ProgressMessageLevel.Info.ToString(), Message = "Starting installation..." });
                    break;
                case webModel.ModuleAction.Uninstall:
                    notification.Title = "Uninstall Module";
                    notification.ProgressLog.Add(new webModel.ProgressMessage { Level = ProgressMessageLevel.Info.ToString(), Message = "Starting uninstall..." });
                    break;
            }

            _pushNotifier.Upsert(notification);

            BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification));

            return notification;
        }
    }
}
