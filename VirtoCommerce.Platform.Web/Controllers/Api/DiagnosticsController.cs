using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Converters.Modularity;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Model.Diagnostics;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/diagnostics")]
    public class DiagnosticsController : ApiController
    {
        private readonly IModuleCatalog _moduleCatalog;

        public DiagnosticsController(IModuleCatalog moduleCatalog)
        {
            _moduleCatalog = moduleCatalog;
        }

        [HttpGet]
        [Route("systeminfo")]
        [ResponseType(typeof(SystemInfo))]
        public IHttpActionResult GetSystemInfo()
        {
            var platformVersion = PlatformVersion.CurrentVersion.ToString();
            var license = LicenseProvider.LoadLicense(); //need to be refactored into platform v.3

            var installedModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).OrderBy(x => x.Id)
                                       .Select(x => x.ToWebModel())
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
