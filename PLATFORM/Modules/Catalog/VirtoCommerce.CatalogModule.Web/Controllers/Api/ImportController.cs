using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Model.Notifications;
using VirtoCommerce.Platform.Core.Notification;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/importjobs")]
	public class ImportController : ApiController
	{
		//private readonly Func<IImportRepository> _importRepositoryFactory;
		//private readonly Func<IImportService> _importServiceFactory;
		//private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		//private readonly INotifier _notifier;

		//private static readonly ConcurrentQueue<webModel.ImportJob> _sheduledJobs = new ConcurrentQueue<webModel.ImportJob>();
		//private static readonly ConcurrentBag<webModel.ImportJob> _jobList = new ConcurrentBag<webModel.ImportJob>();
		//private static Task _runningTask = null;
		//private static readonly Object _lockObject = new Object();


		//public ImportController(Func<IImportRepository> importRepositoryFactory,
		//						Func<IImportService> importServiceFactory,
		//						Func<ICatalogRepository> catalogRepositoryFactory,
		//						INotifier notifier /*, IDataManagementService dataManagementService*/)
		//{
		//	_importRepositoryFactory = importRepositoryFactory;
		//	_importServiceFactory = importServiceFactory;
		//	_catalogRepositoryFactory = catalogRepositoryFactory;
		//	_notifier = notifier;
		//	//_dataManagementService = dataManagementService;
		//}


		///// <summary>
		///// GET /api/catalog/catalogs/apple/importjobs/getnew
		///// </summary>
		///// <param name="catalogId"></param>
		///// <returns></returns>
		//[HttpGet]
		//[ResponseType(typeof(webModel.ImportJob))]
		//[Route("~/api/catalog/importjobs/getnew")]
		//public IHttpActionResult GetNew()
		//{
		//	var retVal = new webModel.ImportJob
		//	{
		//		Name = "New import job",
		//		ColumnDelimiter = "?",
		//		ImportStep = 1
		//	};

		//	return Ok(retVal);
		//}

		///// <summary>
		///// POST /api/catalog/importjobs
		///// </summary>
		///// <param name="entry"></param>
		///// <returns></returns>
		//[HttpPost]
		//[ResponseType(typeof(void))]
		//[Route("")]
		//public IHttpActionResult Upsert(webModel.ImportJob entry)
		//{
		//	using (var repository = _importRepositoryFactory())
		//	{
		//		var dbEntryChanged = entry.ToFoundation();
		//		if (entry.Id != null)
		//		{
		//			var dbEntry = repository.ImportJobs.ExpandAll().Single(x => x.ImportJobId.Equals(entry.Id));
		//			if (dbEntry == null)
		//			{
		//				return NotFound();
		//			}
		//			dbEntryChanged.Patch(dbEntry);
		//		}
		//		else
		//		{
		//			dbEntryChanged.ImportJobId = Guid.NewGuid().ToString();
		//			repository.Add(dbEntryChanged);
		//		}
		//		try
		//		{
		//			repository.UnitOfWork.Commit();
		//		}
		//		catch(Exception ex)
		//		{
		//			ex.ThrowFaultException();
		//		}
		//	}
		//	return Ok();
		//}

		///// <summary>
		///// GET api/catalog/importjobs
		///// </summary>
		///// <param name="catalogId"></param>
		///// <returns></returns>
		//[ResponseType(typeof(webModel.ImportJob[]))]
		//[HttpGet]
		//[Route("")]
		//public IHttpActionResult List()
		//{
		//	foundation.ImportJob[] dbEntries;
		//	using (var repository = _importRepositoryFactory())
		//	{
		//		dbEntries = repository.ImportJobs.OrderBy(x => x.Name).ToArray();
		//	}

		//	var retVal = dbEntries.Select(x => x.ToWebModel()).ToArray();
		//	foreach(var job in retVal)
		//	{
		//		TryPopulateProgressInfo(job);
		//	}
		//	return Ok(retVal);
		//}

		///// <summary>
		///// GET api/catalog/importjobs/123
		///// </summary>
		///// <param name="id"></param>
		///// <returns></returns>
		//[ResponseType(typeof(webModel.ImportJob))]
		//[HttpGet]
		//[Route("{id}")]
		//public IHttpActionResult Get(string id)
		//{
		//	foundation.ImportJob job;
		//	using (var repository = _importRepositoryFactory())
		//	{
		//		job = repository.ImportJobs.ExpandAll().SingleOrDefault(x => x.ImportJobId.Equals(id));
		//	}

		//	var retVal = job.ToWebModel();

		//	//Load available columns
		//	var importService = _importServiceFactory();
		//	var csvColumns = importService.GetCsvColumns(retVal.TemplatePath, retVal.ColumnDelimiter);
		//	retVal.AvailableCsvColumns = csvColumns;
		//	TryPopulateProgressInfo(retVal);
		//	return Ok(retVal);
		//}

		///// <summary>
		///// DELETE api/catalog/importjobs/123
		///// </summary>
		///// <param name="ids"></param>
		///// <returns></returns>
		//[HttpDelete]
		//[ResponseType(typeof(void))]
		//[Route("")]
		//public IHttpActionResult Delete([FromUri]string[] ids)
		//{
		//	using (var repository = _importRepositoryFactory())
		//	{
		//		var entries = repository.ImportJobs.Where(x => ids.Contains(x.ImportJobId)).ToList();

		//		entries.ForEach(repository.Remove);
		//		repository.UnitOfWork.Commit();
		//	}

		//	return StatusCode(HttpStatusCode.NoContent);
		//}

		///// <summary>
		///// GET api/catalog/importjobs/getautomapping?path='c:\\sss.csv'&importType=product&delimiter=,
		///// </summary>
		///// <param name="templatePath"></param>
		///// <param name="importerType"></param>
		///// <param name="delimiter"></param>
		///// <returns></returns>
		//[ResponseType(typeof(webModel.MappingItem))]
		//[HttpGet]
		//[Route("getautomapping")]
		//public IHttpActionResult GetAutoMapping([FromUri]string path, [FromUri]string entityImporter, [FromUri]string delimiter = ",")
		//{
		//	var availableImporters = new foundation.EntityImporterBase[]
		//	{
		//		new foundation.ItemImporter(foundation.ImportEntityType.Product.ToString()),
		//		new foundation.CategoryImporter()
		//	};
		//	var tmpImporter = availableImporters.FirstOrDefault(x => x.Name == entityImporter);
		//	if (tmpImporter == null)
		//	{
		//		return BadRequest("Invalid EntityImporter.");
		//	}
		//	var importService = _importServiceFactory();

		//	//add system properties of the selected import type (importer)
		//	var retVal = tmpImporter.SystemProperties.Select(x => x.ToWebModel()).ToList();

		//	retVal = retVal.OrderBy(x => x.DisplayName).ToList();

		//	//Get a csv columns from imported file
		//	if (delimiter == "?")
		//		delimiter = importService.GetCsvColumnsAutomatically(path);
		//	var csvColumns = importService.GetCsvColumns(path, delimiter);

		//	//default columns mapping
		//	if (csvColumns.Any())
		//	{
		//		foreach (var csvColumn in csvColumns)
		//		{
		//			var mappingItem = retVal.FirstOrDefault(x => x.EntityColumnName.ToLower().Contains(csvColumn.ToLower()) ||
		//													csvColumn.ToLower().Contains(x.EntityColumnName.ToLower()));
		//			//if entity column name contains csv column name or visa versa - match entity property name to csv file column name
		//			if (mappingItem != null)
		//			{
		//				mappingItem.CsvColumnName = csvColumn;
		//				mappingItem.CustomValue = null;
		//			}
		//		}
		//	}
		//	return Ok(retVal);
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
		//public IHttpActionResult Run(string id, [FromUri] string templatePath)
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
		//		job.TemplatePath = templatePath;
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

		//			job.ImportService.ReportProgress = (result, id, name) => {
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

		//private static void TryPopulateProgressInfo(webModel.ImportJob job)
		//{
		//	var other = _sheduledJobs.Concat(_jobList).FirstOrDefault(x => x.Id == job.Id);
		//	if(other != null)
		//	{
		//		job.ProgressStatus = other.ProgressStatus;
		//		job.ProgressInfo = other.ProgressInfo;
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