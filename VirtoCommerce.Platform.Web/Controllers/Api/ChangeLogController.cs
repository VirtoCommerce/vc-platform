using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/changelog")]
    public class ChangeLogController : ApiController
    {
        private readonly IChangeLogService _changeLog;
        public ChangeLogController(IChangeLogService changeLog)
        {
            _changeLog = changeLog;
        }

        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(OperationLog[]))]
        public IHttpActionResult SearchObjectChangeHistory([FromBody]TenantIdentity tenant)
        {
            var result = _changeLog.FindObjectChangeHistory(tenant.TenantId, tenant.TenantType).ToArray();
            return Ok(result);
        }

        [HttpGet]
        [Route("{type}/changes")]
        [ResponseType(typeof(OperationLog[]))]
        public IHttpActionResult SearchTypeChangeHistory(string type, [FromUri] DateTime? start = null, [FromUri] DateTime? end = null)
        {
            var result = _changeLog.FindChangeHistory(type, start, end).ToArray();
            return Ok(result);
        }
    }
}
