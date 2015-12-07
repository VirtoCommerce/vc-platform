using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using VirtoCommerce.Platform.Core.Modularity;
using WebGrease.Extensions;
using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/platform/localization")]
    public class LocalizationController : ApiController
    {
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly string _localizationPath;
        public LocalizationController(IModuleManifestProvider manifestProvider)
        {
            _manifestProvider = manifestProvider;
        }

        /// <summary>
        /// Return all localization files by given locale
        /// </summary>
        /// <returns>json</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
        [AllowAnonymous]
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

        private string[] GetAllLocalizationFiles(string searchPattern) {
            var files = new List<string>();


            // Get platform localization files
            var platformPath = HostingEnvironment.MapPath("~").EnsureEndSeparator();
            var platformFileNames = GetLocalizationFilesByPath(platformPath, searchPattern);
            files.AddRange(platformFileNames);

            // Get modules localization files
            var modulesFileNames = new List<string>();
            foreach (var pair in _manifestProvider.GetModuleManifests())
            {
                var modulePath = Path.GetDirectoryName(pair.Key);
                var moduleFileNames = GetLocalizationFilesByPath(modulePath, searchPattern);
                files.AddRange(moduleFileNames);
            }

            // Get user defined localization files from App_Data/Localizations folder
            var userLocalizationPath = HostingEnvironment.MapPath("~/App_Data").EnsureEndSeparator();
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

        /// <summary>
        /// Return all aviable locales
        /// </summary>
        /// <returns>json</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("locales")]
        public string[] GetLocales()
        {
            var files = GetAllLocalizationFiles("*.json");
            var locales = files
                .Select(x=>Path.GetFileName(x))
                .Select(x => x.Substring(0, x.IndexOf('.'))).Distinct().ToArray();

            return locales;
        }

    }


}
