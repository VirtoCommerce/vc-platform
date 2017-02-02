using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Assets;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Web.Licensing;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/licensing")]
    public class LicensingController : ApiController
    {
        private readonly string _uploadsUrl = Startup.VirtualRoot + "/App_Data/";
        private const string LicenseActivationUrlTemplate = "https://virtocommerce.com/admin/api/licenses/getLicenseFile/{1}";

        private static readonly LicenseService _licenseService = new LicenseService();

        [HttpPost]
        [Route("activateByCode")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public IHttpActionResult ActivateByCode(string activationCode)
        {
            var getUrl = string.Format(LicenseActivationUrlTemplate, activationCode);


            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Activate platform by file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("activateByFile")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.ModuleManage)]
        public async Task<IHttpActionResult> UploadModuleArchive()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
            var uploadsPath = HostingEnvironment.MapPath(_uploadsUrl);
            var streamProvider = new CustomMultipartFormDataStreamProvider(uploadsPath);

            await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
            });

            



            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}