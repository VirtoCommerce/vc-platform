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
        private readonly string _uploadsUrl = Startup.VirtualRoot + "/App_Data/Uploads/";
        private readonly IModuleCatalog _moduleCatalog;
        private readonly IModuleInstaller _moduleInstaller;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;

        public ModulesController(IModuleCatalog moduleCatalog, IModuleInstaller moduleInstaller, IPushNotificationManager pushNotifier, IUserNameResolver userNameResolver)
        {
            _moduleCatalog = moduleCatalog;
            _moduleInstaller = moduleInstaller;
            _pushNotifier = pushNotifier;
            _userNameResolver = userNameResolver;
        }

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult GetModules()
        {
            var retVal = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().OrderBy(x=>x.Id).ThenBy(x=>x.Version)
                                       .Select(x => x.ToWebModel())
                                       .ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Get all dependent modules for module
        /// </summary>
        /// <param name="modules">modules</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdependents")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult GetDependingModules(webModel.ModuleDescriptor[] modules)
        {
            var retVal = new List<ManifestModuleInfo>();
            foreach (var module in modules)
            {
                retVal.AddRange(_moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                                   .Where(x => x.IsInstalled)
                                                   .Where(x => x.DependsOn.Contains(module.Id)));
            }
            return Ok(retVal.Distinct().Select(x=>x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// Returns a flat expanded  list of modules that depend on passed modules
        /// </summary>
        /// <param name="moduleDescriptors">modules descriptors</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getmissingdependencies")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult GetMissingDependencies(webModel.ModuleDescriptor[] moduleDescriptors)
        {
            var modules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Join(moduleDescriptors, x => x.Identity, y => y.Identity, (x, y) => x);
       
            var retVal = _moduleCatalog.CompleteListWithDependencies(modules).OfType<ManifestModuleInfo>()
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
            var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            var path = VirtualPathUtility.ToAbsolute(_uploadsUrl + fileName);

            using (var packageStream = File.Open(path, FileMode.Open))
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
                            _moduleCatalog.AddModule(module);
                        }
                        module.Ref = path;
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
                    notification.ProgressLog.Add(m.ToWebModel());
                    _pushNotifier.Upsert(notification);
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
                notification.Finished = DateTime.UtcNow;
                _pushNotifier.Upsert(notification);
            }
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
                    notification.ProgressLog.Add(new webModel.ProgressMessage { Level = ProgressMessageLevel.Info.ToString(), Message = "Starting uninstalling..." });
                    break;
            }

            _pushNotifier.Upsert(notification);

            BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification));

            return notification;
        }
    }
}
