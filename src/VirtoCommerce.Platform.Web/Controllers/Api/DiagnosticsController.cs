// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Diagnostics;
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/diagnostics")]
    [Authorize]
    public class DiagnosticsController : Controller
    {
        private readonly IModuleCatalog _moduleCatalog;
        private readonly LicenseProvider _licenseProvider;
        private readonly IConfiguration _configuration;

        public DiagnosticsController(IModuleCatalog moduleCatalog, LicenseProvider licenseProvider, IConfiguration configuration)
        {
            _moduleCatalog = moduleCatalog;
            _licenseProvider = licenseProvider;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("systeminfo")]
        public ActionResult<SystemInfo> GetSystemInfo()
        {
            var platformVersion = PlatformVersion.CurrentVersion.ToString();
            var license = _licenseProvider.GetLicense();

            var databaseProvider = _configuration.GetValue("DatabaseProvider", "SqlServer");


            var installedModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).OrderBy(x => x.Id)
                                       .Select(x => new ModuleDescriptor(x))
                                       .ToArray();

            var result = new SystemInfo()
            {
                PlatformVersion = platformVersion,
                License = license,
                InstalledModules = installedModules,
                Version = Environment.Version.ToString(),
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                DatabaseProvider = databaseProvider
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

            var result = _moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                .Where(x => !x.Errors.IsNullOrEmpty())
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version)
                .Select(x => new ModuleDescriptor(x))
                .ToArray();

            return Ok(result);
        }
    }
}
