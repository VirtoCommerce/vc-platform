using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Practices.Unity;
using VirtoCommerce.PackagingModule.Services;
using webModel = VirtoCommerce.PackagingModule.Web.Model;
using VirtoCommerce.PackagingModule.Web.Converters;
using VirtoCommerce.Framework.Web.Common;
using System.Collections.Concurrent;
using System.IO;

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

		public ModulesController([Dependency("Package")]IPackageService packageService, [Dependency("Package")]string packagesPath)
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

		// POST: api/modules/upload
		[HttpPost]
		[ResponseType(typeof(webModel.ModuleDescriptor))]  
		[Route("")]
		public async Task<IHttpActionResult> Upload()
		{
			var streamProvider = await HttpRequestUploader.ReadDataAsync(Request, _packagesPath);

			var file = streamProvider.FileData.FirstOrDefault();
			if(file != null)
			{
				var retVal = _packageService.OpenPackage(Path.Combine(_packagesPath, file.LocalFileName));
				if (retVal != null)
				{
					return Ok(retVal.ToWebModel());
				}
			}
			return NotFound();
		}

		// GET: api/modules/121/install
		[HttpGet]
		[ResponseType(typeof(string))]
		[Route("{id}/install")]
		public IHttpActionResult InstallModule(string id)
		{
			var descriptor = _packageService.GetModules().FirstOrDefault(x => x.Id == id);
			if (descriptor != null)
			{
				var job = new webModel.ModuleWorkerJob(_packageService, descriptor.ToWebModel(), webModel.ModuleAction.Install);
				SheduleJob(job);
				return Ok(job.Id);
			}
			return NotFound();
		}

		// GET: api/modules/jobs/111
		[HttpGet]
		[ResponseType(typeof(string))]
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

		private static void SheduleJob(webModel.ModuleWorkerJob job)
		{
			_sheduledJobs.Enqueue(job);

			if (_runningTask == null || _runningTask.IsCompleted)
			{
				lock (_lockObject)
				{
					if (_runningTask == null || _runningTask.IsCompleted)
					{
						_runningTask = Task.Run(() => { DoWork(); }, job.CancellationToken);
					}
				}
			}
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
						//TODO: log
						if (job.Action == webModel.ModuleAction.Install)
						{
							job.PackageService.Install(job.ModuleDescriptor.Id, job.ModuleDescriptor.Version);
						}
						else if (job.Action == webModel.ModuleAction.Update)
						{
							job.PackageService.Update(job.ModuleDescriptor.Id, job.ModuleDescriptor.Version);
						}
						else if (job.Action == webModel.ModuleAction.Uninstall)
						{
							job.PackageService.Uninstall(job.ModuleDescriptor.Id);
						}
					}
					catch (Exception ex)
					{
						job.Errors.Add(ex.ToString());
					}
				
				}
			}
		}
	}
}
