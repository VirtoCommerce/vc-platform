using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using WebGrease.Extensions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/localization")]
    public class LocalizationController : ApiController
    {
        private readonly IModuleCatalog _moduleCatalog;
        public LocalizationController(IModuleCatalog moduleCatalog)
        {
            _moduleCatalog = moduleCatalog;
        }

        /// <summary>
        /// Return localization resource
        /// </summary>
        /// <param name="lang">Language of localization resource (en by default)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(object))] // Produces invalid response type in generated client
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JObject GetLocalization(string lang = "en")
        {
            var searchPattern = string.Format("{0}.*.json", lang);
            var files = GetAllLocalizationFiles(searchPattern);

            var result = new JObject();
            foreach (var file in files)
            {
                var part = JObject.Parse(File.ReadAllText(file));
                result.Merge(part, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            }
            return result;
        }

        /// <summary>
        /// Return all available locales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("locales")]
        [ResponseType(typeof(string[]))]
        public IHttpActionResult GetLocales()
        {
            var files = GetAllLocalizationFiles("*.json");
            var locales = files
                .Select(Path.GetFileName)
                .Select(x => x.Substring(0, x.IndexOf('.'))).Distinct().ToArray();

            return Ok(locales);
        }


        private string[] GetAllLocalizationFiles(string searchPattern)
        {
            var files = new List<string>();

            // Get platform localization files
            var platformPath = HostingEnvironment.MapPath(Startup.VirtualRoot).EnsureEndSeparator();
            var platformFileNames = GetLocalizationFilesByPath(platformPath, searchPattern);
            files.AddRange(platformFileNames);

            // Get modules localization files
            foreach (var module in _moduleCatalog.Modules.OfType<ManifestModuleInfo>())
            {
                  var moduleFileNames = GetLocalizationFilesByPath(module.FullPhysicalPath, searchPattern);
                files.AddRange(moduleFileNames);
            }

            // Get user defined localization files from App_Data/Localizations folder
            var userLocalizationPath = HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data").EnsureEndSeparator();
            var userFileNames = GetLocalizationFilesByPath(userLocalizationPath, searchPattern);
            files.AddRange(userFileNames);
            return files.ToArray();
        }

        private string[] GetLocalizationFilesByPath(string path, string searchPattern, string localizationSubfolder = "Localizations")
        {
            var sourceDirectoryPath = Path.Combine(path, localizationSubfolder).EnsureEndSeparator();

            return Directory.Exists(sourceDirectoryPath)
                ? Directory.EnumerateFiles(sourceDirectoryPath, searchPattern, SearchOption.AllDirectories).ToArray()
                : new string[0];
        }
    }
}
