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
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Licensing;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/licensing")]
    [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LicensingController : Controller
    {
        private readonly PlatformOptions _platformOptions;
        private readonly ISettingsManager _settingsManager;

        public LicensingController(IOptions<PlatformOptions> platformOptions, ISettingsManager settingsManager)
        {
            _platformOptions = platformOptions.Value;
            _settingsManager = settingsManager;
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

            if (license != null)
            {
                await DisableTrial();
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

            if (license != null)
            {
                await DisableTrial();
            }

            return Ok(license);
        }

        [HttpPost]
        [Route("activateLicense")]
        public async Task<ActionResult<License>> ActivateLicense([FromBody] License license)
        {
            license = License.Parse(license?.RawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));

            if (license != null)
            {
                var licenseFilePath = Path.GetFullPath(_platformOptions.LicenseFilePath);
                System.IO.File.WriteAllText(licenseFilePath, license.RawLicense);
            }

            if (license != null)
            {
                await DisableTrial();
            }

            return Ok(license);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("checkTrialExpiration")]
        public async Task<ActionResult<bool>> CheckTrialExpiration()
        {
            var trialExpirationDate = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name);

            // First login, check if delay setup
            if (trialExpirationDate.Value is null)
            {
                var delay = _platformOptions.RegistrationDelay;
                // If delay is zero or delay format is wrong we need show registration immediately.
                if (delay <= TimeSpan.Zero)
                {
                    return Ok(true);
                }

                // If not we need to delay registration by this time
                trialExpirationDate.Value = DateTime.UtcNow.Add(delay);
                await _settingsManager.SaveObjectSettingsAsync(new[] { trialExpirationDate });

                return Ok(false);
            }

            // Trial period is end, show registration
            if ((DateTime)trialExpirationDate.Value < DateTime.UtcNow)
            {
                return Ok(true);
            }

            // Trial period is not expired
            return Ok(false);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("continueTrial")]
        public async Task<ActionResult> ContinueTrial()
        {
            var trialExpirationDate = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name);

            var expirationPeriod = _platformOptions.RegistrationExpiration;
            if (expirationPeriod <= TimeSpan.Zero)
            {
                expirationPeriod = TimeSpan.FromDays(30);
            }

            trialExpirationDate.Value = DateTime.UtcNow.Add(expirationPeriod);

            await _settingsManager.SaveObjectSettingsAsync(new[] { trialExpirationDate });

            return Ok();
        }

        private async Task DisableTrial()
        {
            var clientPassRegistration = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.ClientPassRegistration.Name);
            clientPassRegistration.Value = true;
            await _settingsManager.SaveObjectSettingsAsync(new[] { clientPassRegistration });
        }
    }
}
