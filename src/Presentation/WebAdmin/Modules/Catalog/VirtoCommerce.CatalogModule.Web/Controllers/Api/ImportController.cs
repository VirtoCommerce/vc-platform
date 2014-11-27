using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.DataManagement.Services;
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

        // GET: api/import/new
        [HttpGet]
        [ResponseType(typeof(webModel.ImportJob))]
        [Route("new/{catalogId}")]
        public IHttpActionResult New(string catalogId)
        {
            // var category = _importRepository.ImportJobs.fir  etById(categoryId);
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

        // POST: api/import/post
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(webModel.ImportJob property)
        {
            //if (property.IsNew)
            //{
            //    _propertyService.Create(moduleProperty);
            //}
            //else
            //{
            //    _propertyService.Update(new moduleModel.Property[] { moduleProperty });
            //}

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(webModel.ImportJob[]))]
        [HttpGet]
        [Route("list/{catalogId?}")]
        public IHttpActionResult List(string catalogId = null)
        {
            var retVal = _importRepository.ImportJobs.Where(x => catalogId == null || x.CatalogId.Equals(catalogId)).ToArray();
            return Ok(retVal);
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete([FromUri]string[] ids)
        {
            //get
            //_importRepository.Remove( id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult UpdateMappingItems(webModel.ImportJob job)
        {
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

    }
}