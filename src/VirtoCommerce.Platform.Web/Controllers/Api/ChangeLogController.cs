using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
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
        [Authorize(PlatformConstants.Security.Permissions.ChangesRead)]
        [ProducesResponseType<LastModifiedResponse>(StatusCodes.Status200OK)]
        public ActionResult<LastModifiedResponse> GetLastModifiedDate([FromQuery] string scope = null)
        {
            var result = AbstractTypeFactory<LastModifiedResponse>.TryCreateInstance();
            result.Scope = scope;
            result.LastModifiedDate = _lastModifiedDateTime.LastModified.UtcDateTime;

            return result;
        }

        /// <summary>
        /// Get the entity names changed since the given date.
        /// Used for cache invalidation in external platform clients.
        /// </summary>
        [HttpPost]
        [Route("~/api/changes/changed-entities")]
        [Authorize(PlatformConstants.Security.Permissions.ChangesRead)]
        [ProducesResponseType<ChangedEntitiesResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ChangedEntitiesResponse> GetChangedEntities([FromBody] ChangedEntitiesRequest changedEntitiesRequest)
        {
            if (changedEntitiesRequest is null)
            {
                return BadRequest("The request body is required.");
            }

            var modifiedSince = changedEntitiesRequest.ModifiedSince;

            var result = AbstractTypeFactory<ChangedEntitiesResponse>.TryCreateInstance();
            result.Entities = (changedEntitiesRequest.EntityNames ?? [])
                .OfType<string>()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(CreateChangedEntity)
                .Where(x => x.ModifiedDate >= modifiedSince)
                .ToList();

            return result;
        }

        private ChangedEntity CreateChangedEntity(string entityName)
        {
            var changedEntity = AbstractTypeFactory<ChangedEntity>.TryCreateInstance();
            changedEntity.Name = entityName;
            changedEntity.ModifiedDate = _lastChangesService.GetLastModifiedDate(entityName).UtcDateTime;

            return changedEntity;
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
        public async Task<ActionResult<OperationLog[]>> SearchChanges([FromBody] ChangeLogSearchCriteria criteria)
        {
            var result = await _changeLogSearchService.SearchAsync(criteria);

            return Ok(result.Results);
        }

        [HttpPost]
        [Route("changelog/v2/search")]
        public async Task<ActionResult<ChangeLogSearchResult>> SearchChangesV2([FromBody] ChangeLogSearchCriteria criteria)
        {
            var result = await _changeLogSearchService.SearchAsync(criteria);

            return Ok(result);
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
