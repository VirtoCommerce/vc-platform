using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Hangfire.States;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Model.Jobs;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/jobs")]
    [CheckPermission(Permission = PredefinedPermissions.BackgroundJobsManage)]
    public class JobsController : ApiController
    {
        private static readonly string[] _finalStates = { DeletedState.StateName, FailedState.StateName, SucceededState.StateName };

        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(Job))]
        public IHttpActionResult GetStatus(string id)
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
