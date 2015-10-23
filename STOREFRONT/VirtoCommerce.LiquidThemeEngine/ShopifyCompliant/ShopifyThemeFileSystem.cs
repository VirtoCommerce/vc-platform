using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using DotLiquid.ViewEngine.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotLiquid.ViewEngine.FileSystems
{
    /// <summary>
    /// Shopify theme folder structure
    /// assets - storages for css, images and other assets
    /// config - contains theme configuration
    /// layout - master pages and layouts
    /// locales - localization resources
    /// snippets - snippets - partial views
    /// templates - view templates
    /// </summary>
    public class ShopifyThemeLiquidFileSystem : IFileSystem
    {
        private readonly string _themeBasePath;
        private readonly string _language;
        private const string _liquidTemplateFormat = "{0}.liquid";
        private static Regex _templateRegex = new Regex(@"[a-zA-Z0-9]+$", RegexOptions.Compiled);

        public ShopifyThemeLiquidFileSystem(string themeBasePath, string language) 
        {
            _themeBasePath = themeBasePath;
            _language = language;
        }

        #region IFileSystem members
        public string ReadTemplateFile(Context context, string templateName)
        {
            return ReadTemplateByName((string)context[templateName]);
        }
        #endregion

        public string ReadTemplateByName(string templateName)
        {
            if (templateName == null || !_templateRegex.IsMatch(templateName))
                throw new FileSystemException("Error - Illegal template name '{0}'", templateName);


            var baseDirectory = new DirectoryInfo(_themeBasePath);
            var templateFile = baseDirectory.GetFiles(String.Format(_liquidTemplateFormat, templateName), SearchOption.AllDirectories).FirstOrDefault();

            if (templateFile == null)
                throw new FileSystemException("Error - No such template {0} . Looked in the following locations:<br />{1}", templateName, _themeBasePath);

            return File.ReadAllText(templateFile.FullName);
        }

        /// <summary>
        /// Read localization resources 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public JObject ReadLocalization()
        {
            var culture =  _language.TryGetCultureInfo();

            var localeDirectory = new DirectoryInfo(Path.Combine(_themeBasePath, "locales" ));
            var localeFilePath = Path.Combine(localeDirectory.FullName, String.Format("{0}.json", culture.TwoLetterISOLanguageName));
            var localeDefaultPath = localeDirectory.GetFiles("*.default.json").Select(x => x.FullName).FirstOrDefault();

            JObject localeJson = null;
            JObject defaultJson = null;

            if (File.Exists(localeFilePath))
            {
                localeJson = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(localeFilePath));
            }
            if(File.Exists(localeDefaultPath))
            {
                defaultJson = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(localeDefaultPath));
            }
            //Need merge default and requested localization json to resulting object
            var retVal = defaultJson ?? localeJson;
            if (defaultJson != null && localeJson != null)
            {
                retVal.Merge(localeJson, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            }

            return retVal;
        }
     
    }
}
