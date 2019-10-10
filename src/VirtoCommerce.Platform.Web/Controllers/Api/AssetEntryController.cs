using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Assets;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/assetentries")]
    public class AssetEntryController : Controller
    {
        private readonly IAssetEntryService _assetService;
        private readonly IAssetEntrySearchService _assetSearchService;

        public AssetEntryController(IAssetEntryService assetService, IAssetEntrySearchService assetSearchService)
        {
            _assetService = assetService;
            _assetSearchService = assetSearchService;
        }

        /// <summary>
        /// SearchAsync for AssetEntries by AssetEntrySearchCriteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [Authorize(PlatformConstants.Security.Permissions.AssetAccess)]
        public async Task<ActionResult<AssetEntrySearchResult>> Search([FromBody]AssetEntrySearchCriteria criteria)
        {
            var result = await _assetSearchService.SearchAssetEntriesAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get asset details by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [Authorize(PlatformConstants.Security.Permissions.AssetRead)]
        public async Task<ActionResult<AssetEntry>> Get([FromQuery]string id)
        {
            var retVal = await _assetService.GetByIdsAsync(new[] { id });
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
        [Authorize(PlatformConstants.Security.Permissions.AssetUpdate)]
        public async Task<ActionResult> Update([FromBody]AssetEntry item)
        {
            await _assetService.SaveChangesAsync(new[] { item });
            return NoContent();
        }

        /// <summary>
        /// Delete asset entries by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(PlatformConstants.Security.Permissions.AssetDelete)]
        public async Task<ActionResult> Delete([FromQuery] string[] ids)
        {
            await _assetService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
