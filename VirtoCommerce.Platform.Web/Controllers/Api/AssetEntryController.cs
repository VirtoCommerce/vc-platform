using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/assetentries")]
    public class AssetEntryController : ApiController
    {
        private readonly IAssetEntryService _assetService;
        private readonly IAssetEntrySearchService _assetSearchService;

        public AssetEntryController(IAssetEntryService assetService, IAssetEntrySearchService assetSearchService)
        {
            _assetService = assetService;
            _assetSearchService = assetSearchService;
        }

        /// <summary>
        /// Search for AssetEntries by AssetEntrySearchCriteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(AssetEntrySearchResult))]
        // [CheckPermission(Permission = PredefinedPermissions.AssetAccess)]
        public IHttpActionResult Search(AssetEntrySearchCriteria criteria)
        {
            var result = _assetSearchService.SearchAssetEntries(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get asset details by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(AssetEntry))]
        [CheckPermission(Permission = PredefinedPermissions.AssetRead)]
        public IHttpActionResult Get(string id)
        {
            var retVal = _assetService.GetByIds(new[] { id });
            if (retVal?.Any() == true)
            {
                return Ok(retVal.Single());
            }

            return NotFound();
        }

        /// <summary>
        /// Create / Update asset entry
        /// </summary>
        [HttpPut]
        [Route("")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.AssetUpdate)]
        public IHttpActionResult Update(AssetEntry item)
        {
            _assetService.SaveChanges(new[] { item });
            return Ok();
        }

        /// <summary>
        /// Delete asset entries by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.AssetDelete)]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            _assetService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
