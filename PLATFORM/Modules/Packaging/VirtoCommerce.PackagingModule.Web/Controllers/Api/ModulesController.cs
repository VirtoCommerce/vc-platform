using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.PackagingModule.Services;
using VirtoCommerce.PackagingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Data.Asset;
using moduleModel = VirtoCommerce.PackagingModule.Model;
using webModel = VirtoCommerce.PackagingModule.Web.Model;

namespace VirtoCommerce.PackagingModule.Web.Controllers.Api
{
    [RoutePrefix("api/modules")]
    public class ModulesController : ApiController
    {
        private readonly string _packagesPath;
        private readonly IPackageService _packageService;
        private static readonly ConcurrentQueue<webModel.ModuleWorkerJob> _sheduledJobs = new ConcurrentQueue<webModel.ModuleWorkerJob>();
        private static readonly ConcurrentBag<webModel.ModuleWorkerJob> _jobList = new ConcurrentBag<webModel.ModuleWorkerJob>();
        private static Task _runningTask = null;
        private static readonly Object _lockObject = new Object();

        public ModulesController(IPackageService packageService, string packagesPath)
        {
            _packageService = packageService;
            _packagesPath = packagesPath;
        }

        // GET: api/modules
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleDescriptor[]))]
        [Route("")]
        public IHttpActionResult GetModules()
        {
            var retVal = _packageService.GetModules().Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        // GET: api/modules/121
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleDescriptor))]
        [Route("{id}")]
        public IHttpActionResult GetModuleById(string id)
        {
            var retVal = _packageService.GetModules().FirstOrDefault(x => x.Id == id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }

        // POST: api/modules
        [HttpPost]
        [ResponseType(typeof(webModel.ModuleDescriptor))]
        [Route("")]
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
                    var allInstalledModules = _packageService.GetModules().Select(x => x.Id);
                    //check unresolved dependencies 
                    if (descriptor.Dependencies != null)
                    {
                        retVal.ValidationErrors = descriptor.Dependencies.Except(allInstalledModules).Select(x => "Unresolved dependency: " + x).ToList();
                    }
                    //Check module already installed
                    if (allInstalledModules.Contains(descriptor.Id))
                    {
                        retVal.ValidationErrors.Add("Already installed");
                    }
                    retVal.FileName = file.LocalFileName;
                    return Ok(retVal);
                }
            }
            return NotFound();
        }

        // GET: api/modules/install?fileName=''
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleWorkerJob))]
        [Route("install")]
        public IHttpActionResult InstallModule([FromUri]string fileName)
        {
            var descriptor = _packageService.OpenPackage(Path.Combine(_packagesPath, fileName));
            if (descriptor != null)
            {
                var retVal = SheduleJob(descriptor.ToWebModel(), webModel.ModuleAction.Install);
                return Ok(retVal);
            }
            return InternalServerError();
        }

        // GET: api/modules/121/update
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleWorkerJob))]
        [Route("{id}/update")]
        public IHttpActionResult UpdateModule(string id)
        {
            var descriptor = _packageService.GetModules().FirstOrDefault(x => x.Id == id);
            if (descriptor != null)
            {
                var retVal = SheduleJob(descriptor.ToWebModel(), webModel.ModuleAction.Update);
                return Ok(retVal);
            }
            return InternalServerError();
        }

        // GET: api/modules/121/uninstall
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleWorkerJob))]
        [Route("{id}/uninstall")]
        public IHttpActionResult UninstallModule(string id)
        {
            var descriptor = _packageService.GetModules().FirstOrDefault(x => x.Id == id);
            if (descriptor != null)
            {
                var retVal = SheduleJob(descriptor.ToWebModel(), webModel.ModuleAction.Uninstall);
                return Ok(retVal);
            }
            return InternalServerError();
        }

        // GET: api/modules/jobs/111
        [HttpGet]
        [ResponseType(typeof(webModel.ModuleWorkerJob))]
        [Route("jobs/{id}")]
        public IHttpActionResult GetJob(string id)
        {
            var job = _jobList.FirstOrDefault(x => x.Id == id);
            if (job != null)
            {
                return Ok(job);
            }
            return NotFound();
        }

        // GET: api/modules/restart
        [HttpGet]
        [Route("restart")]
        public IHttpActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return Ok();
        }


        private webModel.ModuleWorkerJob SheduleJob(webModel.ModuleDescriptor descriptor, webModel.ModuleAction action)
        {
            var retVal = new webModel.ModuleWorkerJob(_packageService, descriptor, action);

            _sheduledJobs.Enqueue(retVal);

            if (_runningTask == null || _runningTask.IsCompleted)
            {
                lock (_lockObject)
                {
                    if (_runningTask == null || _runningTask.IsCompleted)
                    {
                        _runningTask = Task.Run(() => { DoWork(); }, retVal.CancellationToken);
                    }
                }
            }

            return retVal;
        }

        private static void DoWork()
        {
            while (_sheduledJobs.Any())
            {
                webModel.ModuleWorkerJob job;

                if (_sheduledJobs.TryDequeue(out job))
                {
                    try
                    {
                        _jobList.Add(job);
                        job.Started = DateTime.UtcNow;
                        var reportProgress = new Progress<moduleModel.ProgressMessage>((x) => { job.ProgressLog.Add(x.ToWebModel()); });

                        if (job.Action == webModel.ModuleAction.Install)
                        {
                            job.PackageService.Install(job.ModuleDescriptor.Id, job.ModuleDescriptor.Version, reportProgress);
                        }
                        else if (job.Action == webModel.ModuleAction.Update)
                        {
                            job.PackageService.Update(job.ModuleDescriptor.Id, job.ModuleDescriptor.Version, reportProgress);
                        }
                        else if (job.Action == webModel.ModuleAction.Uninstall)
                        {
                            job.PackageService.Uninstall(job.ModuleDescriptor.Id, reportProgress);
                        }
                    }
                    catch (Exception ex)
                    {
                        job.ProgressLog.Add(new webModel.ProgressMessage { Message = ex.ToString(), Level = moduleModel.ProgressMessageLevel.Error.ToString() });
                    }

                    job.Completed = DateTime.UtcNow;
                }
            }
        }
    }
}
