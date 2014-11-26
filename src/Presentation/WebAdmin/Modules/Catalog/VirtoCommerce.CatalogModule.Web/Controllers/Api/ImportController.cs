using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.DataManagement.Services;
using VirtoCommerce.Foundation.Importing.Repositories;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class ImportController : ApiController
    {
        private readonly string _relativeDir = "Content/Uploads/ImportData/";

        private readonly IImportRepository _importRepository;
        private readonly IDataManagementService _dataManagementService;

        public ImportController(IImportRepository importRepository, IDataManagementService dataManagementService)
        {
            _importRepository = importRepository;
            _dataManagementService = dataManagementService;
        }

        // GET: api/import/getnewimportjob
        [HttpGet]
        [ResponseType(typeof(webModel.ImportJob))]
        public IHttpActionResult GetNewImportJob()
        {
            // var category = _importRepository.ImportJobs.fir  etById(categoryId);
            var retVal = new webModel.ImportJob
            {
                Id = Guid.NewGuid().ToString(),
                Name = "new importJob"
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

        // GET: api/import/getimportjobs
        [ResponseType(typeof(webModel.ImportJob[]))]
        public IHttpActionResult GetImportJobs()
        {
            var retVal = new webModel.ImportJob[] { };
            return Ok(retVal);
        }

        // DELETE: api/import/id
        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string id)
        {
            //get
            //_importRepository.Remove( id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/import/GetCsvColumns
        [ResponseType(typeof(string))]
        [HttpGet]
        public IHttpActionResult UpdateMappingItems(webModel.ImportJob job)
        {
            // TODO:
            // GetCsvColumns from csv file;
            // SetMappingItems for job;

            return Ok(job);
        }

        [HttpPost]
        public async Task<string> RunImportJob(string id, string sourceAssetId)
        {
            var retVal = Guid.NewGuid().ToString();

            // _importService.RunImportJob(jobEntity.ImportJob.ImportJobId, sourceAssetId));

            return retVal;
        }

    }
}