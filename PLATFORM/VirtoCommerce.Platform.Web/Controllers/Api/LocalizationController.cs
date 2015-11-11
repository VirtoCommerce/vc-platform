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
            _localizationPath = HostingEnvironment.MapPath("~/App_Data/Localizations/");
        }

        /// <summary>
        /// Get all localization files by given language
        /// </summary>
        /// <returns>json</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
        public JObject GetLocalizationFile(string lang = "en")
        {
            DirectoryInfo directory = new DirectoryInfo(_localizationPath);
            var files = directory.GetFiles().Where(x => x.Name.StartsWith(lang)).ToList();

            // move custom file to the end of file list
            //var custom = string.Format("{0}.custom.json", lang);
            //var customFile = files.FirstOrDefault(x => x.Name.ToLower() == custom);
            //if (customFile != null)
            //{
            //    files.Remove(customFile);
            //    files.Add(customFile);
            //}

            var result = new JObject();
            foreach (var file in files)
            {
                var part = JObject.Parse(File.ReadAllText(file.FullName));
                result.Merge(part, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
            }

            return result; 
        }

    }


}
