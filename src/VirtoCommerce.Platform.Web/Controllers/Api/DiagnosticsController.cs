// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Diagnostics;
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/diagnostics")]
    [ApiController]
    [Authorize]
    public class DiagnosticsController : ControllerBase
    {
        private readonly LicenseProvider _licenseProvider;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiagnosticsController(LicenseProvider licenseProvider, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _licenseProvider = licenseProvider;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("systeminfo")]
        public ActionResult<SystemInfo> GetSystemInfo()
        {
            var platformVersion = PlatformVersion.CurrentVersion.ToString();
            var license = _licenseProvider.GetLicense();

            var databaseProvider = _configuration.GetValue("DatabaseProvider", "SqlServer");


            var installedModules = ModuleBootstrapper.Instance.GetInstalledModules().OrderBy(x => x.Id)
                                       .Select(x => new ModuleDescriptor(x))
                                       .ToArray();

            var result = new SystemInfo
            {
                PlatformVersion = platformVersion,
                License = license,
                InstalledModules = installedModules,
                Version = Environment.Version.ToString(),
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                DatabaseProvider = databaseProvider,
                EnvironmentName = _webHostEnvironment.IsDevelopment()
                    ? Environments.Development
                    : Environments.Production,
                RuntimeIdentifier = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier(),
            };

            return Ok(result);
        }

        /// <summary>
        /// Get installed modules with errors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("errors")]
        [AllowAnonymous]
        public ActionResult<ModuleDescriptor[]> GetModulesErrors()
        {
            var result = ModuleBootstrapper.Instance.GetFailedModules()
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version)
                .Select(x => new ModuleDescriptor(x))
                .ToArray();

            if (!_webHostEnvironment.IsDevelopment() && result.Length != 0)
            {
                var errorDescription = "To enable the details of this specific error message, please switch environment to Development mode.";
                var errors = new[] { errorDescription };

                result.Apply(x =>
                {
                    x.ValidationErrors = errors;
                });
            }

            return Ok(result);
        }
    }
}
