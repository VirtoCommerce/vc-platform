using Omu.ValueInjecter;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Foundation.Importing.Repositories;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/import")]
    public class ImportController : ApiController
    {
        private readonly string _relativeDir = "Content/Uploads/ImportData/";

        private readonly IImportRepository _importRepository;
        //private readonly IDataManagementService _dataManagementService;

        public ImportController(IImportRepository importRepository/*, IDataManagementService dataManagementService*/)
        {
            _importRepository = importRepository;
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
        [Route("create")]
        public IHttpActionResult Put(webModel.ImportJob entry)
        {
            var coreEntry = entry.ToFoundation();
            _importRepository.Add(coreEntry);
            _importRepository.UnitOfWork.Commit();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("update")]
        public IHttpActionResult Post(webModel.ImportJob entry)
        {
            var coreEntry = entry.ToFoundation();
            var dbEntry = GetInternal(entry.Id);
            dbEntry.InjectFrom(coreEntry);

            _importRepository.UnitOfWork.Commit();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(webModel.ImportJob[]))]
        [HttpGet]
        [Route("list/{catalogId?}")]
        public IHttpActionResult List(string catalogId = null)
        {
            var dbEntries = _importRepository.ImportJobs
                .Where(x => catalogId == null || x.CatalogId.Equals(catalogId))
                .OrderBy(x => x.Name).ToArray();

            var retVal = dbEntries.Select(x => x.ToWebModel());
            return Ok(retVal);
        }

        [ResponseType(typeof(webModel.ImportJob))]
        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(string id)
        {
            var retVal = GetInternal(id);

            return Ok(retVal.ToWebModel());
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete([FromUri]string[] ids)
        {
            var entries = _importRepository.ImportJobs.Where(x => ids.Contains(x.ImportJobId)).ToList();

            entries.ForEach(_importRepository.Remove);
            _importRepository.UnitOfWork.Commit();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult UpdateMappingItems(webModel.ImportJob job)
        {
            // if (string.IsNullOrEmpty(job.EntityImporter))
            // TODO:
            // GetCsvColumns from csv file;
            // SetMappingItems for job;

            return Ok(job);
        }

        [HttpPost]
        public async Task<string> Run(string id, string sourceAssetId)
        {
            var retVal = Guid.NewGuid().ToString();

            // _importService.RunImportJob(jobEntity.ImportJob.ImportJobId, sourceAssetId));

            return await Task.FromResult(retVal);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult Cancel(string id)
        {
            // TODO

            return StatusCode(HttpStatusCode.NoContent);
        }

        #region private
        private Foundation.Importing.Model.ImportJob GetInternal(string id)
        {
            return _importRepository.ImportJobs.SingleOrDefault(x => x.ImportJobId.Equals(id));
        }
        #endregion
    }
}