using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Model.Notifications;
using VirtoCommerce.Platform.Core.Notification;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/exportjobs")]
    public class CatalogModuleExportController : ApiController
    {
		//private readonly Func<IImportRepository> _importRepositoryFactory;
		//private readonly Func<IImportService> _importServiceFactory;
		//private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		//private readonly INotifier _notifier;

		//private static readonly ConcurrentQueue<webModel.ImportJob> _sheduledJobs = new ConcurrentQueue<webModel.ImportJob>();
		//private static readonly ConcurrentBag<webModel.ImportJob> _jobList = new ConcurrentBag<webModel.ImportJob>();
		//private static Task _runningTask = null;
		//private static readonly Object _lockObject = new Object();
		//private IDataManagementService _dataManagementService;


		//public ExportController(Func<IImportRepository> importRepositoryFactory,
		//						Func<IImportService> importServiceFactory,
		//						Func<ICatalogRepository> catalogRepositoryFactory,
		//						INotifier notifier, IDataManagementService dataManagementService)
		//{
		//	_importRepositoryFactory = importRepositoryFactory;
		//	_importServiceFactory = importServiceFactory;
		//	_catalogRepositoryFactory = catalogRepositoryFactory;
		//	_notifier = notifier;
		//	_dataManagementService = dataManagementService;
		//}

		///// <summary>
		///// GET api/catalog/importjobs/123/run?path='c:\\sss.csv'
		///// </summary>
		///// <param name="id"></param>
		///// <param name="filePath"></param>
		///// <returns></returns>
		//[ResponseType(typeof(void))]
		//[HttpGet]
		//[Route("{id}/run")]
		//public IHttpActionResult Run(string id)
		//{
		//	using (var repository = _importRepositoryFactory())
		//	{
		//		var importJob = repository.ImportJobs.FirstOrDefault(x => x.ImportJobId == id);
		//		if (importJob == null)
		//		{
		//			return NotFound();
		//		}
		//		var job = importJob.ToWebModel();
		//		job.Notifier = _notifier;
		//		job.ImportService = _importServiceFactory();
		//		// job.TemplatePath = templatePath;
		//		job.CancellationToken = new CancellationTokenSource();
		//		job.NotifyEvent = new ImportNotifyEvent(job, User.Identity.Name);
		//		job.ProgressInfo = new webModel.JobProgressInfo();
		//		job.ProgressStatus = webModel.ProgressStatus.Pending;

		//		SheduleJob(job);
		//	}
		//	return Ok();

		//}

		///// <summary>
		/////  GET api/catalog/importjobs/123/cancel
		///// </summary>
		///// <param name="id"></param>
		///// <returns></returns>
		//[HttpGet]
		//[Route("{id}/cancel")]
		//[ResponseType(typeof(void))]
		//public IHttpActionResult Cancel(string id)
		//{
		//	var job = _jobList.FirstOrDefault(x => x.Id == id);
		//	if (job != null && job.CanBeCanceled)
		//	{
		//		job.CancellationToken.Cancel();
		//	}

		//	return StatusCode(HttpStatusCode.NoContent);
		//}

		//#region private
		//private static void SheduleJob(webModel.ImportJob job)
		//{
		//	_sheduledJobs.Enqueue(job);

		//	if (_runningTask == null || _runningTask.IsCompleted)
		//	{
		//		lock (_lockObject)
		//		{
		//			if (_runningTask == null || _runningTask.IsCompleted)
		//			{
		//				var context = HttpContext.Current;
		//				_runningTask = Task.Run(() => { DoWork(context); }, job.CancellationToken.Token);
		//			}
		//		}
		//	}
		//}

		//private static void DoWork(HttpContext context)
		//{
		//	HttpContext.Current = context;
		//	while (_sheduledJobs.Any())
		//	{
		//		webModel.ImportJob job;

		//		if (_sheduledJobs.TryDequeue(out job))
		//		{
		//			_jobList.Add(job);

		//			job.ProgressStatus = webModel.ProgressStatus.Running;
		//			job.NotifyEvent.IsRunning = true;
		//			job.ProgressInfo.Started = DateTime.UtcNow;

		//			job.ImportService.ReportProgress = (result, id, name) =>
		//			{
		//				LogProgress(job, result);
		//			};

		//			if (!job.CancellationToken.IsCancellationRequested)
		//			{
		//				job.ImportService.ServiceRunnerId = job.Id;
		//				job.ImportService.CancellationToken = job.CancellationToken.Token;

		//				var result = new VirtoCommerce.Foundation.Importing.Model.ImportResult();
		//				result.Errors = new List<string>();
		//				LogProgress(job, result);
		//				try
		//				{
		//					//job.ImportService.RunImportJob(job.Id, job.TemplatePath);

		//					result.Length = 6;
		//					LogProgress(job, result);

		//					for (int i = 0; i < result.Length; i++)
		//					{
		//						if (job.CancellationToken.IsCancellationRequested)
		//						{
		//							job.ProgressStatus = webModel.ProgressStatus.Aborted;
		//							break;
		//						}
		//						if (i % 2 == 0)
		//						{
		//							result.ErrorsCount++;
		//							result.Errors.Add("Null referncse object item");
		//							LogProgress(job, result);
		//						}
		//						else
		//						{
		//							result.ProcessedRecordsCount++;
		//							LogProgress(job, result);
		//						}
		//						Thread.Sleep(10000);
		//					}

		//				}
		//				catch (Exception e)
		//				{
		//					result.Errors.Add(e.ToString());
		//					//var result = job.ImportService.GetImportResult(job.Id);
		//					//result.Errors.Add(e.Message + Environment.NewLine + e);
		//					//job.NotifyEvent.LogProgress(result);
		//				}
		//				finally
		//				{
		//					job.ProgressStatus = webModel.ProgressStatus.Finished;
		//					job.ProgressInfo.Finished = DateTime.UtcNow;
		//					job.NotifyEvent.IsRunning = false;
		//					LogProgress(job, result);
		//				}
		//			}
		//		}
		//	}
		//}

		//private static void LogProgress(webModel.ImportJob job, VirtoCommerce.Foundation.Importing.Model.ImportResult result)
		//{
		//	//Update notification
		//	job.NotifyEvent.LogProgress(result);
		//	//update job progress

		//	job.ProgressInfo.TotalCount = result.Length;
		//	job.ProgressInfo.ProcessedCount = result.ProcessedRecordsCount;
		//	job.ProgressInfo.ErrorCount = result.ErrorsCount;
		//	job.ProgressInfo.Errors = result.Errors;
		//}

		//#endregion
    }
}