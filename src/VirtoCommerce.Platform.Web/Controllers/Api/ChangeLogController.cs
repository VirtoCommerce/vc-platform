using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform")]
    [Authorize]
    public class ChangeLogController : Controller
    {
        private readonly IChangeLogSearchService _changeLogSearchService;
        private readonly ILastModifiedDateTime _lastModifiedDateTime;
        private readonly ILastChangesService _lastChangesService;

        public ChangeLogController(IChangeLogSearchService changeLogSearchService, ILastModifiedDateTime lastModifiedDateTime, ILastChangesService lastChangesService)
        {
            _changeLogSearchService = changeLogSearchService;
            _lastModifiedDateTime = lastModifiedDateTime;
            _lastChangesService = lastChangesService;
        }

        /// <summary>
        /// Force set changes last modified date
        /// </summary>
        /// <param name="forceRequest">Force changes request</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/changes/force")]
        [Authorize(PlatformConstants.Security.Permissions.ResetCache)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ForceChanges(ForceChangesRequest forceRequest)
        {
            _lastModifiedDateTime.Reset();
            return NoContent();
        }

        [HttpPost]
        [Route("~/api/platform-cache/reset")]
        [Authorize(PlatformConstants.Security.Permissions.ResetCache)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ResetPlatformCache()
        {
            GlobalCacheRegion.ExpireRegion();

            return NoContent();
        }

        /// <summary>
        /// Get last modified date for given scope
        /// Used for signal of what something changed and for cache invalidation in external platform clients
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/changes/lastmodifieddate")]
        [AllowAnonymous]
        public ActionResult<LastModifiedResponse> GetLastModifiedDate([FromQuery] string scope = null)
        {
            var result = new LastModifiedResponse
            {
                Scope = scope,
                LastModifiedDate = _lastModifiedDateTime.LastModified.UtcDateTime
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("~/api/changes/changed-entities")]
        [AllowAnonymous]
        public ActionResult<ChangedEntitiesResponse> GetChangedEntities([FromBody] ChangedEntitiesRequest changedEntitiesRequest)
        {
            var result = new ChangedEntitiesResponse()
            {
                Entities = changedEntitiesRequest.EntityNames
                    .Select(x => new ChangedEntity { Name = x, ModifiedDate = _lastChangesService.GetLastModifiedDate(x).UtcDateTime })
                    .Where(x => x.ModifiedDate >= changedEntitiesRequest.ModifiedSince)
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("~/api/changes/changed-entities/reset")]
        [Authorize(PlatformConstants.Security.Permissions.ResetCache)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ResetChangedEntities([FromBody] string[] entityNames)
        {
            foreach (var entityName in entityNames)
            {
                _lastChangesService.Reset(entityName);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("changelog/search")]
        public async Task<ActionResult<ChangeLogSearchResult>> SearchChanges([FromBody] ChangeLogSearchCriteria criteria)
        {
            // Get changes count from operation log
            var result = await _changeLogSearchService.SearchAsync(criteria);

            return Ok(result.Results);
        }

        [HttpGet]
        [Route("changelog/{type}/changes")]
        public async Task<ActionResult<OperationLog[]>> SearchTypeChangeHistory(string type, [FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var criteria = new ChangeLogSearchCriteria
            {
                ObjectType = type,
                StartDate = start,
                EndDate = end
            };
            // Get changes count from operation log
            var result = await _changeLogSearchService.SearchAsync(criteria);
            return Ok(result.Results);
        }
    }
}
