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
        private readonly string _uploadsPath;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly IUserNameResolver _userNameResolver;

        public ModulesController(IPackageService packageService, string uploadsPath, IPushNotificationManager pushNotifier, IUserNameResolver userNameResolver)
        {
            _packageService = packageService;
            _uploadsPath = uploadsPath;
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

            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }

            var streamProvider = new CustomMultipartFormDataStreamProvider(_uploadsPath);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            var file = streamProvider.FileData.FirstOrDefault();
            if (file != null)
            {
                var descriptor = _packageService.OpenPackage(file.LocalFileName);
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
            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = webModel.ModuleAction.Install,
                PackageFilePath = Path.Combine(_uploadsPath, fileName),
            };
            var result = ScheduleJob(options);
            return Ok(result);
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
            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = webModel.ModuleAction.Update,
                PackageId = id,
                PackageFilePath = Path.Combine(_uploadsPath, fileName)
            };
            var result = ScheduleJob(options);
            return Ok(result);
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
            var options = new webModel.ModuleBackgroundJobOptions
            {
                Action = webModel.ModuleAction.Uninstall,
                PackageId = id
            };
            var result = ScheduleJob(options);
            return Ok(result);
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

        [ApiExplorerSettings(IgnoreApi = true)]
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
                        _packageService.Install(options.PackageFilePath, reportProgress);
                        break;
                    case webModel.ModuleAction.Update:
                        _packageService.Update(options.PackageId, options.PackageFilePath, reportProgress);
                        break;
                    case webModel.ModuleAction.Uninstall:
                        _packageService.Uninstall(options.PackageId, reportProgress);
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
