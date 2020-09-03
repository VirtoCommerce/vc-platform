using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Web.Extensions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/localization")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LocalizationController : Controller
    {
        private const string InternationalizationFilesFormat = ".js";
        private static readonly string InternationalizationFilesFolder = $"js{Path.DirectorySeparatorChar}i18n{Path.DirectorySeparatorChar}angular";

        private readonly IWebHostEnvironment _hostingEnv;
        private readonly ITranslationService _translationService;

        public LocalizationController(IWebHostEnvironment hostingEnv, ITranslationService translationService)
        {
            _hostingEnv = hostingEnv;
            _translationService = translationService;
        }

        /// <summary>
        /// Return localization resource
        /// </summary>
        /// <param name="lang">Language of localization resource (en by default)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult<JObject> GetLocalization(string lang = null)
        {
            var result = _translationService.GetTranslationDataForLanguage(lang);

            return Ok(result);
        }

        /// <summary>
        /// Return all available locales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("locales")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public ActionResult<string[]> GetLocales()
        {
            var locales = _translationService.GetListOfInstalledLanguages();

            return Ok(locales);
        }

        /// <summary>
        /// Return all available regional formats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("regionalformats")]
        [AllowAnonymous]
        public ActionResult<string[]> GetRegionalFormats()
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


        private string[] GetAllInternationalizationFiles(string searchPattern, string internationalizationsFolder)
        {
            var files = new List<string>();

            // Get platform internationalization files
            var platformPath = _hostingEnv.MapPath("~/");
            var platformFileNames = GetFilesByPath(platformPath, searchPattern, internationalizationsFolder);
            files.AddRange(platformFileNames);

            return files.ToArray();
        }

        private string[] GetFilesByPath(string path, string searchPattern, string subfolder)
        {
            var sourceDirectoryPath = Path.Combine(path, subfolder);

            return Directory.Exists(sourceDirectoryPath)
                ? Directory.EnumerateFiles(sourceDirectoryPath, searchPattern, SearchOption.AllDirectories).ToArray()
                : new string[0];
        }
    }
}
