using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
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
        [HttpPost]
        [Route("activateByCode")]
        [ResponseType(typeof(License))]
        public async Task<IHttpActionResult> ActivateByCode([FromBody]string activationCode)
        {
            License license = null;

            using (var httpClient = new HttpClient())
            {
                var activationUrl = new Uri("https://virtocommerce.com/admin/api/licenses/activate/" + activationCode);
                var httpResponse = await httpClient.GetAsync(activationUrl);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var rawLicense = await httpResponse.Content.ReadAsStringAsync();
                    license = License.Parse(rawLicense);
                }
            }

            return Ok(license);
        }

        [HttpPost]
        [Route("activateByFile")]
        [ResponseType(typeof(License))]
        public async Task<IHttpActionResult> ActivateByFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "MIME multipart content expected"));

            License license = null;

            var streamProvider = await Request.Content.ReadAsMultipartAsync();
            var httpContent = streamProvider.Contents.FirstOrDefault();

            if (httpContent != null)
            {
                var rawLicense = await httpContent.ReadAsStringAsync();
                license = License.Parse(rawLicense);
            }

            return Ok(license);
        }

        [HttpPost]
        [Route("activateLicense")]
        [ResponseType(typeof(License))]
        public IHttpActionResult ActivateLicense(License license)
        {
            license = License.Parse(license?.RawLicense);

            if (license != null)
            {
                var licenseFilePath = HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data/VirtoCommerce.lic");
                File.WriteAllText(licenseFilePath, license.RawLicense);
            }

            return Ok(license);
        }
    }
}
