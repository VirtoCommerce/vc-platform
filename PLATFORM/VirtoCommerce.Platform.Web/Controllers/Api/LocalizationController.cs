using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/platform/localization")]
    public class LocalizationController : ApiController
    {
        private readonly string _localizationPath;
        public LocalizationController()
        {
            _localizationPath = HostingEnvironment.MapPath("~/App_Data/Localization/");
        }

        /// <summary>
        /// Get all localization files by given language
        /// </summary>
        /// <returns>json</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("file")]
        [AllowAnonymous]
        public JObject GetLocalizationFile(string lang = "en")
        {
            DirectoryInfo directory = new DirectoryInfo(_localizationPath);
            var file = directory.GetFiles().FirstOrDefault(x => x.Name == string.Format("{0}.json", lang));
            
            var result = JObject.Parse(File.ReadAllText(file.FullName));

            return result; 
        }

        /// <summary>
        /// Get all localization files by given language
        /// </summary>
        /// <returns>json</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("locales")]
        public string[] GetLocales()
        {
            DirectoryInfo directory = new DirectoryInfo(_localizationPath);
            var locales = directory.GetFiles().Select(x => x.Name.Substring(0, x.Name.IndexOf('.'))).Distinct().ToArray();

            return locales;
        }

    }


}
