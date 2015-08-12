using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Web.Converters.Packaging;
using webModel = VirtoCommerce.Platform.Web.Model.Packaging;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/modules")]
    [CheckPermission(Permission = PredefinedPermissions.ModuleQuery)]
    public class ModulesController : ApiController
    {
        private readonly IPackageService _packageService;
        private readonly string _packagesPath;
        private readonly IPushNotificationManager _pushNotifier;

        public ModulesController(IPackageService packageService, string packagesPath, IPushNotificationManager pushNotifier)
        {
            _packageService = packageService;
            _packagesPath = packagesPath;
            _pushNotifier = pushNotifier;
        }

        /// <summary>
        /// Get installed modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        public IHttpActionResult GetModules()
        {
            var retVal = _packageService.GetModules().Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Get module details
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(webModel.ModuleDescriptor))]
        public IHttpActionResult GetModuleById(string id)
        {
            var retVal = _packageService.GetModules().FirstOrDefault(x => x.Id == id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }

        /// <summary>
        /// Upload module package for installation or update
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(webModel.ModuleDescriptor))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public async Task<IHttpActionResult> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (!Directory.Exists(_packagesPath))
            {
                Directory.CreateDirectory(_packagesPath);
            }

            var streamProvider = new CustomMultipartFormDataStreamProvider(_packagesPath);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            var file = streamProvider.FileData.FirstOrDefault();
            if (file != null)
            {
                var descriptor = _packageService.OpenPackage(Path.Combine(_packagesPath, file.LocalFileName));
                if (descriptor != null)
                {
                    var retVal = descriptor.ToWebModel();
                    retVal.FileName = file.LocalFileName;

                    var dependencyErrors = _packageService.GetDependencyErrors(descriptor);
                    retVal.ValidationErrors.AddRange(dependencyErrors);

                    return Ok(retVal);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Install module from uploaded file
        /// </summary>
        /// <param name="fileName">Module package file name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("install")]
        [ResponseType(typeof(webModel.ModulePushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult InstallModule(string fileName)
        {
            var package = _packageService.OpenPackage(Path.Combine(_packagesPath, fileName));

            if (package != null)
            {
                var result = ScheduleJob(webModel.ModuleAction.Install, package);
                return Ok(result);
            }

            return InternalServerError();
        }

        /// <summary>
        /// Update module from uploaded file
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <param name="fileName">Module package file name.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/update")]
        [ResponseType(typeof(webModel.ModulePushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult UpdateModule(string id, string fileName)
        {
            var module = _packageService.GetModules().FirstOrDefault(m => m.Id == id);

            if (module != null)
            {
                var package = _packageService.OpenPackage(Path.Combine(_packagesPath, fileName));

                if (package != null && package.Id == module.Id)
                {
                    var result = ScheduleJob(webModel.ModuleAction.Update, package);
                    return Ok(result);
                }
            }

            return InternalServerError();
        }

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <param name="id">Module ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/uninstall")]
        [ResponseType(typeof(webModel.ModulePushNotification))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult UninstallModule(string id)
        {
            var module = _packageService.GetModules().FirstOrDefault(m => m.Id == id);

            if (module != null)
            {
                var result = ScheduleJob(webModel.ModuleAction.Uninstall, module);
                return Ok(result);
            }

            return InternalServerError();
        }

        /// <summary>
        /// Restart web application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("restart")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return StatusCode(HttpStatusCode.NoContent);
        }

        public void ModuleBackgroundJob(webModel.ModuleBackgroundJobOptions options, webModel.ModulePushNotification notification)
        {
            try
            {
                notification.Started = DateTime.UtcNow;

                var reportProgress = new Progress<ProgressMessage>(m =>
                {
                    notification.ProgressLog.Add(m.ToWebModel());
                    _pushNotifier.Upsert(notification);
                });

                switch (options.Action)
                {
                    case webModel.ModuleAction.Install:
                        _packageService.Install(options.ModuleId, options.Version, reportProgress);
                        break;
                    case webModel.ModuleAction.Update:
                        _packageService.Update(options.ModuleId, options.Version, reportProgress);
                        break;
                    case webModel.ModuleAction.Uninstall:
                        _packageService.Uninstall(options.ModuleId, reportProgress);
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


        private webModel.ModulePushNotification ScheduleJob(webModel.ModuleAction action, ModuleIdentity module)
        {
            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = action,
                ModuleId = module.Id,
                Version = module.Version,
            };

            var notification = new webModel.ModulePushNotification(CurrentPrincipal.GetCurrentUserName());

            switch (action)
            {
                case webModel.ModuleAction.Install:
                    notification.Title = "Install Module";
                    break;
                case webModel.ModuleAction.Update:
                    notification.Title = "Update Module";
                    break;
                case webModel.ModuleAction.Uninstall:
                    notification.Title = "Uninstall Module";
                    break;
            }

            _pushNotifier.Upsert(notification);

            BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification));

            return notification;
        }
    }
}
