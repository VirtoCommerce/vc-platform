using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Web.Licensing;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/licensing")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LicensingController : ApiController
    {
        private static readonly LicenseService _licenseService = new LicenseService();

        [HttpPost]
        [Route("activateByCode")]
        [ResponseType(typeof(License))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public async Task<IHttpActionResult> ActivateByCode(string activationCode)
        {
            License license;

            using (var webClient = new WebClient())
            {
                var activationUrl = new Uri(string.Join("http://localhost/admin/api/licenses/getLicenseFile/", activationCode));
                var content = await webClient.DownloadStringTaskAsync(activationUrl);
                license = _licenseService.SaveLicenseIfValid(content);
            }

            return Ok(license);
        }

        /// <summary>
        /// Activate platform by file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("activateByFile")]
        [ResponseType(typeof(License))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public async Task<IHttpActionResult> ActivateByFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "MIME multipart content expected"));

            License license = null;

            var streamProvider = await Request.Content.ReadAsMultipartAsync();
            var httpContent = streamProvider.Contents.FirstOrDefault();

            if (httpContent != null)
            {
                var content = await httpContent.ReadAsStringAsync();
                license = _licenseService.SaveLicenseIfValid(content);
            }

            return Ok(license);
        }
    }
}
