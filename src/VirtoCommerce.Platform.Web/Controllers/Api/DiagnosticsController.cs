// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Diagnostics;
using VirtoCommerce.Platform.Web.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/diagnostics")]
    public class DiagnosticsController : Controller
    {
        private readonly IModuleCatalog _moduleCatalog;
        private readonly LicenseProvider _licenseProvider;

        public DiagnosticsController(IModuleCatalog moduleCatalog, LicenseProvider licenseProvider)
        {
            _moduleCatalog = moduleCatalog;
            _licenseProvider = licenseProvider;

        }
        [HttpGet]
        [Route("systeminfo")]
        public ActionResult<License> GetSystemInfo()
        {
            var platformVersion = PlatformVersion.CurrentVersion.ToString();
            var license = _licenseProvider.GetLicense();

            var installedModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).OrderBy(x => x.Id)
                                       .Select(x => new ModuleDescriptor(x))
                                       .ToArray();

            var result = new SystemInfo()
            {
                PlatformVersion = platformVersion,
                License = license,
                InstalledModules = installedModules
            };
            return Ok(result);
        }
    }
}
