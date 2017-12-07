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
        private const string LocalizationFilesFormat = ".json";
        private const string LocalizationFilesFolder = "Localizations";
        private const string InternationalizationFilesFormat = ".js";
        private const string InternationalizationFilesFolder = "Scripts\\i18n\\angular";

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
            var searchPattern = string.Format("{0}.*{1}", lang, LocalizationFilesFormat);
            var files = GetAllLocalizationFiles(searchPattern, LocalizationFilesFolder);

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
        [AllowAnonymous]
        [ResponseType(typeof(string[]))]
        public IHttpActionResult GetLocales()
        {
            var files = GetAllLocalizationFiles("*" + LocalizationFilesFormat, LocalizationFilesFolder);
            var locales = files
                .Select(Path.GetFileName)
                .Select(x => x.Substring(0, x.IndexOf('.'))).Distinct().ToArray();

            return Ok(locales);
        }

        /// <summary>
        /// Return all available regional formats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("regionalformats")]
        [AllowAnonymous]
        [ResponseType(typeof(string[]))]
        public IHttpActionResult GetRegionalFormats()
        {
            var files = GetAllInternationalizationFiles("*" + InternationalizationFilesFormat, InternationalizationFilesFolder);
            var formats = files
                .Select(Path.GetFileName)
                .Select(x =>
                {
                    var startIndexOfCode = x.IndexOf("_") + 1;
                    var endIndexOfCode = x.IndexOf(".");
                    return x.Substring(startIndexOfCode, endIndexOfCode - startIndexOfCode);
                }).Distinct().ToArray();

            return Ok(formats);
        }

        private string[] GetAllLocalizationFiles(string searchPattern, string localizationsFolder)
        {
            var files = new List<string>();

            // Get platform localization files
            var platformPath = HostingEnvironment.MapPath(Startup.VirtualRoot).EnsureEndSeparator();
            var platformFileNames = GetFilesByPath(platformPath, searchPattern, localizationsFolder);
            files.AddRange(platformFileNames);

            // Get modules localization files ordered by dependency.
            var allModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().ToArray();
            var manifestModules = _moduleCatalog.CompleteListWithDependencies(allModules)
                .Where(x => x.State == ModuleState.Initialized)
                .OfType<ManifestModuleInfo>();

            foreach (var module in manifestModules)
            {
                var moduleFileNames = GetFilesByPath(module.FullPhysicalPath, searchPattern, localizationsFolder);
                files.AddRange(moduleFileNames);
            }

            // Get user defined localization files from App_Data/Localizations folder
            var userLocalizationPath = HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data").EnsureEndSeparator();
            var userFileNames = GetFilesByPath(userLocalizationPath, searchPattern, localizationsFolder);
            files.AddRange(userFileNames);
            return files.ToArray();
        }

        private string[] GetAllInternationalizationFiles(string searchPattern, string internationalizationsFolder)
        {
            var files = new List<string>();

            // Get platform internationalization files
            var platformPath = HostingEnvironment.MapPath(Startup.VirtualRoot).EnsureEndSeparator();
            var platformFileNames = GetFilesByPath(platformPath, searchPattern, internationalizationsFolder);
            files.AddRange(platformFileNames);

            return files.ToArray();
        }

        private string[] GetFilesByPath(string path, string searchPattern, string subfolder)
        {
            var sourceDirectoryPath = Path.Combine(path, subfolder).EnsureEndSeparator();

            return Directory.Exists(sourceDirectoryPath)
                ? Directory.EnumerateFiles(sourceDirectoryPath, searchPattern, SearchOption.AllDirectories).ToArray()
                : new string[0];
        }
    }
}
