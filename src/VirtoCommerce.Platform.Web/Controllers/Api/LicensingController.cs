using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Web.Licensing;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/licensing")]
    [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LicensingController : Controller
    {
        private readonly PlatformOptions _platformOptions;

        public LicensingController(IOptions<PlatformOptions> platformOptions)
        {
            _platformOptions = platformOptions.Value;
        }

        [HttpPost]
        [Route("activateByCode")]
        public async Task<ActionResult<License>> ActivateByCode([FromBody] string activationCode)
        {
            if (!Regex.IsMatch(activationCode, "^([a-zA-Z_0-9-])+$"))
            {
                return BadRequest(new { Message = $"Activation code \"{activationCode}\" is invalid" });
            }

            License license = null;

            using (var httpClient = new HttpClient())
            {
                var activationUrl = new Uri(_platformOptions.LicenseActivationUrl + activationCode);
                var httpResponse = await httpClient.GetAsync(activationUrl);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var rawLicense = await httpResponse.Content.ReadAsStringAsync();
                    license = License.Parse(rawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));
                }
            }

            return Ok(license);
        }

        [HttpPost]
        [Route("activateByFile")]
        public async Task<ActionResult<License>> ActivateByFile(IFormFile file)
        {
            License license = null;
            var rawLicense = string.Empty;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                rawLicense = await reader.ReadToEndAsync();
            }

            if (!string.IsNullOrEmpty(rawLicense))
            {
                license = License.Parse(rawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));
            }

            return Ok(license);
        }

        [HttpPost]
        [Route("activateLicense")]
        public ActionResult<License> ActivateLicense([FromBody] License license)
        {
            license = License.Parse(license?.RawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));

            if (license != null)
            {
                var licenseFilePath = Path.GetFullPath(_platformOptions.LicenseFilePath);
                System.IO.File.WriteAllText(licenseFilePath, license.RawLicense);
            }

            return Ok(license);
        }
    }
}
