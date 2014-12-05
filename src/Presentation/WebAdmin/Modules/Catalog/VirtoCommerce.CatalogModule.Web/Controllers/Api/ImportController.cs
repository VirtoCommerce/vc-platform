using System;
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
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Framework.Web.Notification;
using foundation = VirtoCommerce.Foundation.Importing.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/import")]
    public class ImportController : ApiController
    {
        private readonly Func<IImportRepository> _importRepositoryFactory;
        private readonly Func<IImportService> _importServiceFactory;
        private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
        private readonly INotifier _notifier;

        private readonly Object thisLock = new Object();
        static readonly Queue<webModel.ImportJobRun> _runningJobs = new Queue<webModel.ImportJobRun>();
        static webModel.ImportJobRun runningJob;

		public ImportController([Dependency("Catalog")]Func<IImportRepository> importRepositoryFactory,
								[Dependency("Catalog")]Func<IImportService> importServiceFactory,
								[Dependency("Catalog")]Func<IFoundationCatalogRepository> catalogRepositoryFactory,
								INotifier notifier /*, IDataManagementService dataManagementService*/)
        {
            _importRepositoryFactory = importRepositoryFactory;
            _importServiceFactory = importServiceFactory;
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _notifier = notifier;
            //_dataManagementService = dataManagementService;
        }


        [HttpGet]
        [ResponseType(typeof(webModel.ImportJob))]
        [Route("new/{catalogId?}")]
        public IHttpActionResult New(string catalogId = null)
        {
            var retVal = new webModel.ImportJob
            {
                Id = Guid.NewGuid().ToString(),
                Name = "New import job",
                CatalogId = catalogId,
                ColumnDelimiter = "?",
                ImportStep = 1
            };

            return Ok(retVal);
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("update")]
        public IHttpActionResult Put(webModel.ImportJob entry)
        {
            using (var repository = _importRepositoryFactory())
            {
                var dbEntry = repository.ImportJobs.ExpandAll().Single(x => x.ImportJobId.Equals(entry.Id));
                if (dbEntry == null)
                {
                    throw new NullReferenceException("dbEntry");
                }

                var dbEntryChanged = entry.ToFoundation();
                dbEntryChanged.Patch(dbEntry);

                repository.UnitOfWork.Commit();

            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("create")]
        public IHttpActionResult Post(webModel.ImportJob entry)
        {
            var coreEntry = entry.ToFoundation();
            using (var repository = _importRepositoryFactory())
            {
                repository.Add(coreEntry);
                repository.UnitOfWork.Commit();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(webModel.ImportJob[]))]
        [HttpGet]
        [Route("list/{catalogId?}")]
        public IHttpActionResult List(string catalogId = null)
        {
            foundation.ImportJob[] dbEntries;
            using (var repository = _importRepositoryFactory())
            {
                dbEntries = repository.ImportJobs
                    .Where(x => catalogId == null || x.CatalogId.Equals(catalogId))
                    .OrderBy(x => x.Name).ToArray();
            }

            var retVal = dbEntries.Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        [ResponseType(typeof(webModel.ImportJob))]
        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(string id)
        {
            foundation.ImportJob job;
            using (var repository = _importRepositoryFactory())
            {
                job = repository.ImportJobs.ExpandAll().SingleOrDefault(x => x.ImportJobId.Equals(id));
            }

            var retVal = job.ToWebModel();

            //Load available columns
            try
            {
                var importService = _importServiceFactory();
                var csvColumns = importService.GetCsvColumns(retVal.TemplatePath, retVal.ColumnDelimiter);
                retVal.AvailableCsvColumns = csvColumns;
            }
            catch (Exception)
            {
                //cannot load csv file
            }

            return Ok(retVal);
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete([FromUri]string[] ids)
        {
            using (var repository = _importRepositoryFactory())
            {
                var entries = repository.ImportJobs.Where(x => ids.Contains(x.ImportJobId)).ToList();

                entries.ForEach(repository.Remove);
                repository.UnitOfWork.Commit();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(webModel.ImportJob))]
        [HttpPost]
        public IHttpActionResult UpdateMappingItems(webModel.ImportJob job)
        {
            // parameter validation
            if (string.IsNullOrEmpty(job.EntityImporter))
            {
                return BadRequest("EntityImporter is required.");
            }

            var availableImporters = new foundation.EntityImporterBase[]
            {
                new foundation.ItemImporter(foundation.ImportEntityType.Product.ToString()),
                new foundation.CategoryImporter()
            };

            var tmpImporter = availableImporters.FirstOrDefault(x => x.Name == job.EntityImporter);
            if (tmpImporter == null)
            {
                return BadRequest("Invalid EntityImporter.");
            }
            if (string.IsNullOrEmpty(job.CatalogId))
            {
                return BadRequest("CatalogId is required.");
            }
            if (string.IsNullOrEmpty(job.TemplatePath))
            {
                return BadRequest("TemplatePath is required.");
            }

            if (string.IsNullOrEmpty(job.ColumnDelimiter))
            {
                return BadRequest("Column Delimiter is required.");
            }

            var importService = _importServiceFactory();

            if (job.ColumnDelimiter == "?")
                job.ColumnDelimiter = importService.GetCsvColumnsAutomatically(job.TemplatePath);
            var csvColumns = importService.GetCsvColumns(job.TemplatePath, job.ColumnDelimiter);
            job.AvailableCsvColumns = csvColumns;

            //add system properties of the selected import type (importer)
            var newList = new ObservableCollection<webModel.MappingItem>();
            tmpImporter.SystemProperties.ToList().ForEach(sysProp => newList.Add(new webModel.MappingItem
            {
                EntityColumnName = sysProp.Name,
                IsSystemProperty = true,
                IsRequired = sysProp.IsRequiredProperty,
                DisplayName = sysProp.IsRequiredProperty ? string.Format("* {0}", sysProp.DisplayName) : sysProp.DisplayName,
                ImportJobId = job.Id,
                CustomValue = !string.IsNullOrEmpty(sysProp.DefaultValue) ? sysProp.DefaultValue : null
            }));

            job.PropertiesMap = new ObservableCollection<webModel.MappingItem>(newList.OrderBy(item => item.DisplayName));
            //add custom properties (if any in the selected property set
            if (job.PropertySetId != null)
            {
                //get available locales for the catalog
                var catalogRepository = _catalogRepositoryFactory();
                var localesQuery = catalogRepository.Catalogs
                                    .OfType<Catalog>()
                                    .Where(x => x.CatalogId == job.CatalogId)
                                    .Expand(x => x.CatalogLanguages)
                                    .SingleOrDefault();

                var locales = new List<CatalogLanguage>();
                if (localesQuery != null)
                    locales = localesQuery.CatalogLanguages.ToList();

                //get property set properties
                var ps = catalogRepository.PropertySets
                                    .Where(x => x.PropertySetId == job.PropertySetId)
                                    .Expand("PropertySetProperties/Property")
                                    .FirstOrDefault();
                if (ps != null && ps.PropertySetProperties != null)
                {
                    var props = ps.PropertySetProperties;
                    newList = new ObservableCollection<webModel.MappingItem>();
                    props.ToList().ForEach(prop =>
                    {
                        if (prop.Property.IsLocaleDependant)
                        {
                            locales.ForEach(
                                locale =>
                                newList.Add(new webModel.MappingItem
                                {
                                    EntityColumnName = prop.Property.Name,
                                    DisplayName = prop.Property.IsRequired ? string.Format("* {0}", prop.Property.Name) : prop.Property.Name,
                                    IsRequired = prop.Property.IsRequired,
                                    Locale = locale.Language,
                                    IsSystemProperty = false,
                                    ImportJobId = job.Id
                                }));
                        }
                        else
                        {
                            newList.Add(new webModel.MappingItem
                            {
                                EntityColumnName = prop.Property.Name,
                                IsSystemProperty = false,
                                DisplayName = prop.Property.IsRequired ? string.Format("* {0}", prop.Property.Name) : prop.Property.Name,
                                IsRequired = prop.Property.IsRequired,
                                ImportJobId = job.Id
                            });
                        }
                    });
                    job.PropertiesMap.Add(newList.OrderBy(item => item.DisplayName));
                }
            }

            //default columns mapping
            if (job.AvailableCsvColumns != null && job.AvailableCsvColumns.Any())
            {
                job.PropertiesMap.ToList().ForEach(col => job.AvailableCsvColumns.ToList().ForEach(csvcolumn =>
                {
                    //if entity column name contains csv column name or visa versa - match entity property name to csv file column name
                    if (col.EntityColumnName.ToLower().Contains(csvcolumn.ToLower()) ||
                        csvcolumn.ToLower().Contains(col.EntityColumnName.ToLower()))
                    {
                        job.PropertiesMap.First(x => x.EntityColumnName == col.EntityColumnName).CsvColumnName = csvcolumn;
                        job.PropertiesMap.First(x => x.EntityColumnName == col.EntityColumnName).CustomValue = null;
                    }
                }));
            }

            return Ok(job);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("run")]
        public IHttpActionResult Run(webModel.ImportJob job)
        {
            var importService = _importServiceFactory();
            importService.ReportProgress = LogProgress;

            var jobHandle = new webModel.ImportJobRun
            {
                jobId = job.Id,
                jobName = job.Name,
                assetPath = job.TemplatePath,
                cancellationTokenSource = new CancellationTokenSource()
            };

            LogProgressStart(jobHandle);

            lock (thisLock)
            {
                _runningJobs.Enqueue(jobHandle);
            }

            if (runningJob == null)
            {
                // null HttpContext.Current workaround
                var ctx = HttpContext.Current;

                // import runner task
                Task.Run(() =>
                {
                    HttpContext.Current = ctx;
                    while (_runningJobs.Any())
                    {
                        lock (thisLock)
                        {
                            // can't Dequeue as cancellation needs it.
                            runningJob = _runningJobs.Peek();
                        }
                        try
                        {
                            if (!runningJob.cancellationTokenSource.IsCancellationRequested)
                            {
                                importService.ServiceRunnerId = runningJob.id;
                                importService.CancellationToken = runningJob.cancellationTokenSource.Token;
                                importService.RunImportJob(runningJob.jobId, runningJob.assetPath);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            var res = importService.GetImportResult(runningJob.jobId);
                            LogProgressCancel(res, runningJob);
                        }
                        catch (Exception e)
                        {
                            var progressEntity = new foundation.ImportResult
                            {
                                ErrorsCount = 1,
                                Errors = new List<string>(),
                                Stopped = DateTime.Now
                            };
                            progressEntity.Errors.Add(e.Message + Environment.NewLine + e);
                            LogProgress(progressEntity, runningJob.id, runningJob.jobName);
                        }
                        finally
                        {
                            lock (thisLock)
                            {
                                _runningJobs.Dequeue();
                            }
                        }
                    }
                    lock (thisLock)
                    {
                        runningJob = null;
                    }
                });
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult Cancel(string id)
        {
            var job = _runningJobs.FirstOrDefault(x => x.id == id);
            if (job != null && !job.cancellationTokenSource.IsCancellationRequested)
            {
                job.cancellationTokenSource.Cancel();

                lock (thisLock)
                {
                    if (runningJob != job)
                    {
                        Task.Run(() => LogProgressCancel(null, job));
                    }
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        #region private
        private void LogProgressStart(webModel.ImportJobRun jobRun)
        {
            var notify = new NotifyEvent
            {
                Id = jobRun.id,
                Title = string.Format("Import job '{0}' submitted for processing.", jobRun.jobName),
                Description = string.Format("Import job '{0}' submitted for processing.", jobRun.jobName),
                Status = NotifyStatus.Pending,
                NotifyType = NotifyType.LongRunningTask,
                Created = DateTime.Now
            };

            _notifier.Create(notify);
        }

        private void LogProgressCancel(foundation.ImportResult result, webModel.ImportJobRun jobRun)
        {
            var notify = new NotifyEvent
            {
                Id = jobRun.id,
                Title = string.Format("Import job '{0}' processing was canceled.", jobRun.jobName),
                Description = string.Format("Import job '{0}' processing was canceled.", jobRun.jobName),
                Status = NotifyStatus.Aborted,
                NotifyType = NotifyType.LongRunningTask,
                FinishDate = DateTime.Now
            };

            if (result != null && (result.ProcessedRecordsCount + result.ErrorsCount) > 0)
            {
                notify.Description += string.Format(" Processed records: {0}", result.ProcessedRecordsCount + result.ErrorsCount);
                if (result.ErrorsCount > 0)
                {
                    notify.Description += Environment.NewLine + "Errors:" + Environment.NewLine + string.Join(Environment.NewLine, result.Errors.Cast<string>());
                }
            }

            _notifier.Update(notify);
        }

        private void LogProgress(foundation.ImportResult result, string id, string jobName)
        {
            var notify = new NotifyEvent
            {
                Id = id,
                Title = string.Format(result.IsFinished ? "Import job '{0}' complete" : "Import job '{0}' progress", jobName),
                Description = string.Format("Processed records: {0}", result.ProcessedRecordsCount + result.ErrorsCount),
                Status = !result.IsStarted ? NotifyStatus.Pending : (result.IsRunning ? NotifyStatus.Running : (result.IsFinished ? NotifyStatus.Finished : NotifyStatus.Aborted)),
                FinishDate = result.Stopped
            };

            if (result.ErrorsCount > 0)
            {
                notify.Description += Environment.NewLine + "Errors:" + Environment.NewLine + string.Join(Environment.NewLine, result.Errors.Cast<string>());
            }

            _notifier.Update(notify);
        }
        #endregion
    }
}