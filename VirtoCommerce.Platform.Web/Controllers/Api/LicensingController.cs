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
    [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LicensingController : ApiController
    {
        private static readonly LicenseService _licenseService = new LicenseService();

        [HttpPost]
        [Route("activateByCode")]
        [ResponseType(typeof(LicenseResponse))]
        public async Task<IHttpActionResult> ActivateByCode([FromBody]string activationCode)
        {
            LicenseResponse response = null;

            using (var httpClient = new HttpClient())
            {
                var activationUrl = new Uri("http://localhost/admin/api/licenses/activate/" + activationCode);
                var httpResponse = await httpClient.GetAsync(activationUrl);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    response = ValidateLicense(content);
                }
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("activateByFile")]
        [ResponseType(typeof(LicenseResponse))]
        public async Task<IHttpActionResult> ActivateByFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "MIME multipart content expected"));

            LicenseResponse response = null;

            var streamProvider = await Request.Content.ReadAsMultipartAsync();
            var httpContent = streamProvider.Contents.FirstOrDefault();

            if (httpContent != null)
            {
                var content = await httpContent.ReadAsStringAsync();
                response = ValidateLicense(content);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("activateLicense")]
        [ResponseType(typeof(License))]
        public IHttpActionResult ActivateLicense([FromBody]string licenseContent)
        {
            var license = _licenseService.SaveLicenseIfValid(licenseContent);
            return Ok(license);
        }


        private static LicenseResponse ValidateLicense(string content)
        {
            LicenseResponse response = null;

            var license = _licenseService.Parse(content);
            if (license != null)
            {
                response = new LicenseResponse
                {
                    License = license,
                    Content = content,
                };
            }

            return response;
        }
    }
}
