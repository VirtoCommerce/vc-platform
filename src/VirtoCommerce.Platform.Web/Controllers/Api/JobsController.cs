using System.Linq;
using Hangfire;
using Hangfire.States;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/jobs")]
    [Authorize(PlatformConstants.Security.Permissions.BackgroundJobsManage)]
    public class JobsController : Controller
    {
        private static readonly string[] _finalStates = { DeletedState.StateName, FailedState.StateName, SucceededState.StateName };

        /// <summary>
        /// Get background job status
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Job> GetStatus(string id)
        {
            var result = GetJob(id);
            return Ok(result);
        }

        private static Job GetJob(string jobId)
        {
            var result = new Job { Id = jobId };

            var state = JobStorage.Current.GetConnection().GetStateData(jobId);

            if (state != null)
            {
                result.State = state.Name;
            }

            result.Completed = (state == null || _finalStates.Contains(result.State));

            return result;
        }
    }
}
