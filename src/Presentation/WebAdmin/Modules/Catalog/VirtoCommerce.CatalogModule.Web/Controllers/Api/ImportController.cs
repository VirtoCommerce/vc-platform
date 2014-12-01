using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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

        public ImportController(Func<IImportRepository> importRepositoryFactory, Func<IImportService> importServiceFactory, Func<IFoundationCatalogRepository> catalogRepositoryFactory, INotifier notifier /*, IDataManagementService dataManagementService*/)
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

                dbEntry.EntityImporter = entry.EntityImporter;
                dbEntry.ColumnDelimiter = entry.ColumnDelimiter;
                dbEntry.CatalogId = entry.CatalogId;
                dbEntry.ImportStep = entry.ImportStep;
                dbEntry.ImportCount = entry.ImportCount;
                dbEntry.MaxErrorsCount = entry.MaxErrorsCount;
                dbEntry.Name = entry.Name;
                dbEntry.PropertySetId = entry.PropertySetId;
                dbEntry.StartIndex = entry.StartIndex;
                dbEntry.TemplatePath = entry.TemplatePath;

                foreach (var map in dbEntry.PropertiesMap.ToArray())
                {
                    var entryMap = entry.PropertiesMap.SingleOrDefault(x => x.Id == map.MappingItemId);

                    if (entryMap == null)
                    {
                        dbEntry.PropertiesMap.Remove(map);
                    }
                    else
                    {
                        map.CustomValue = entryMap.CustomValue;
                        map.CsvColumnName = entryMap.CsvColumnName;
                        map.DisplayName = entryMap.DisplayName;
                        map.EntityColumnName = entryMap.EntityColumnName;
                        map.IsRequired = entryMap.IsRequired;
                        map.IsSystemProperty = entryMap.IsSystemProperty;
                        map.Locale = entryMap.Locale;
                        map.StringFormat = entryMap.StringFormat;
                    }
                }

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

            var importService = _importServiceFactory();
            var retVal = job.ToWebModel();

            //Load available columns
            try
            {
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

        [ResponseType(typeof(NotifyEvent))]
        [HttpPost]
        [Route("run")]
        public IHttpActionResult Run(webModel.ImportJob job)
        {
            //var importService = new Services.ImportService(_importServiceFactory());
            var importService = (_importServiceFactory());

            var jobHandle = new webModel.ImportJobRun
            {
                jobId = job.Id,
                AssetPath = job.TemplatePath
                // task = new Task(() => importService.RunImportJob(job.Id, job.TemplatePath))
            };

            lock (thisLock)
            {
                _runningJobs.Enqueue(jobHandle);
            }

            if (runningJob == null)
            {
                // import runner task
                Task.Run(() =>
                {
                    while (_runningJobs.Any())
                    {
                        lock (thisLock)
                        {
                            runningJob = _runningJobs.Dequeue();
                        }
                        try
                        {
                            importService.RunImportJob(runningJob.jobId, runningJob.AssetPath);
                            //jj.task = new Task(() => importService.RunImportJob(job.Id, job.TemplatePath));
                            //jj.task.Start();
                            //jj.task.Wait();
                            LogProgress(importService);
                        }
                        catch (Exception e)
                        {
                            // jj.
                            // return "Error: " + e.Message;
                        }
                    }
                    lock (thisLock)
                    {
                        runningJob = null;
                    }
                });

                Task.Run(() =>
                {
                    Task.Delay(TimeSpan.FromMilliseconds(300));

                    while (runningJob != null)
                    {
                        LogProgress(importService);

                        //res.
                        //progress.Report(new ImportProgress
                        //{
                        //    ImportEntity = jobEntity,
                        //    ImportResult = res,
                        //    StatusId = id,
                        //    Processed = res == null ? 0 : res.ProcessedRecordsCount + res.ErrorsCount
                        //});

                        //if (res != null && res.IsFinished)
                        //    finished = true;

                        Task.Delay(TimeSpan.FromMilliseconds(150));
                    }
                });
            }

            var notify = new NotifyEvent { Id = jobHandle.id };
            notify = _notifier.Create(notify);

            return Ok(notify);
        }

        private void LogProgress(IImportService importService)
        {
            var res = importService.GetImportResult(runningJob.jobId);
            var notify = new NotifyEvent
            {
                Id = runningJob.id,
                Status = res.IsRunning ? NotifyStatus.Running : res.IsFinished ? NotifyStatus.Finished : NotifyStatus.Aborted,
            };
            _notifier.Update(notify);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult Cancel(string id)
        {
            // TODO

            return StatusCode(HttpStatusCode.NoContent);
        }

        #region private


        #endregion
    }
}