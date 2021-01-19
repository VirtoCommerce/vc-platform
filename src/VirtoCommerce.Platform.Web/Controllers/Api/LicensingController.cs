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
        [Route("getTrialExpirationDate")]
        public async Task<ActionResult<TrialState>> GetTrialExpirationDate()
        {
            var trialExpirationDate = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name);
            return trialExpirationDate.Value switch
            {
                DateTime dateTime when dateTime == DateTime.MaxValue => Ok(TrialState.Registered),
                DateTime dateTime => Ok(new TrialState(dateTime)),
                _ => Ok(TrialState.Empty)
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("continueTrial")]
        public async Task<ActionResult> ContinueTrial([FromBody] TrialProlongation request)
        {
            if (!DateTime.TryParse(request.NextTime, out var result))
            {
                return BadRequest();
            }

            var trialExpirationDate = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name);
            trialExpirationDate.Value = result;
            await _settingsManager.SaveObjectSettingsAsync(new[] { trialExpirationDate });

            return Ok();
        }

        private async Task DisableTrial()
        {
            var trialExpirationDate = await _settingsManager.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name);
            trialExpirationDate.Value = DateTime.MaxValue;
            await _settingsManager.SaveObjectSettingsAsync(new[] { trialExpirationDate });
        }

        public class TrialState
        {
            public DateTime? ExpirationDate { get; protected set; }
            public bool ClientPassRegistration { get; protected set; }

            protected TrialState()
            {
            }

            public TrialState(DateTime expirationDate)
            {
                ExpirationDate = expirationDate;
            }

            public static TrialState Registered => new TrialState { ClientPassRegistration = true };
            public static TrialState Empty => new TrialState();
        }

        public class TrialProlongation
        {
            public string NextTime { get; set; }
        }
    }
}
