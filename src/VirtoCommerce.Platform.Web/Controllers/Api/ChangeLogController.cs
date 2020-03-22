using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Web.Model;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform")]
    public class ChangeLogController : Controller
    {
        private static DateTime _lastTimestamp = DateTime.UtcNow;
        private readonly IChangeLogSearchService _changeLogSearchService;
        public ChangeLogController(IChangeLogSearchService changeLogSearchService)
        {
            _changeLogSearchService = changeLogSearchService;
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
            _lastTimestamp = DateTime.UtcNow;
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
        public async Task<ActionResult<LastModifiedResponse>> GetLastModifiedDate([FromQuery] string scope = null)
        {
            var criteria = new ChangeLogSearchCriteria
            {
                Take = 1
            };
            // Get latest change  from operation log
            var latestChange = (await _changeLogSearchService.SearchAsync(criteria)).Results.FirstOrDefault();

            var result = new LastModifiedResponse
            {
                Scope = scope,
                LastModifiedDate = new DateTime(Math.Max((latestChange?.ModifiedDate ?? _lastTimestamp).Ticks, _lastTimestamp.Ticks))
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("changelog/search")]
        public async Task<ActionResult<ChangeLogSearchResult>> SearchChanges([FromBody]ChangeLogSearchCriteria criteria)
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
