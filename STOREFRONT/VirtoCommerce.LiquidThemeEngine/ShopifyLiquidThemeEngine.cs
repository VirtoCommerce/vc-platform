using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.LiquidThemeEngine.Operators;
using VirtoCommerce.LiquidThemeEngine.Tags;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;


namespace VirtoCommerce.LiquidThemeEngine
{
    /// <summary>
    /// Shopify compliant theme folder structure and all methods for rendering
    /// assets - storages for css, images and other assets
    /// config - contains theme configuration
    /// layout - master pages and layouts
    /// locales - localization resources
    /// snippets - snippets - partial views
    /// templates - view templates
    /// </summary>
    public class ShopifyLiquidThemeEngine : IFileSystem
    {
        private const string _defaultMasterView = "theme";
        private const string _liquidTemplateFormat = "{0}.liquid";
        private static Regex _templateRegex = new Regex(@"[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private string _themesRelativeUrl;
        private string _themesAssetsRelativeUrl;
        private Func<WorkContext> _workContextFactory;
        private IStorefrontUrlBuilder _storeFrontUrlBuilder;

        public ShopifyLiquidThemeEngine(Func<WorkContext> workContextFactory, IStorefrontUrlBuilder storeFrontUrlBuilder, string themesRealtiveUrl, string themesAssetsRelativeUrl)
        {
            _workContextFactory = workContextFactory;
            _storeFrontUrlBuilder = storeFrontUrlBuilder;
            _themesRelativeUrl = themesRealtiveUrl;
            _themesAssetsRelativeUrl = themesAssetsRelativeUrl;

            Liquid.UseRubyDateFormat = true;
            // Register custom tags (Only need to do this once)
            Template.RegisterFilter(typeof(CommonFilters));
            Template.RegisterFilter(typeof(CommerceFilters));
            Template.RegisterFilter(typeof(TranslationFilter));
            Template.RegisterFilter(typeof(UrlFilters));
            Template.RegisterFilter(typeof(MoneyFilters));
            Template.RegisterFilter(typeof(HtmlFilters));
            Template.RegisterFilter(typeof(StringFilters));

            Condition.Operators["contains"] = (left, right) => ContainsMethod(left, right);

            Template.RegisterTag<Layout>("layout");
            var contextType = typeof(WorkContext);

            //Register WorkingContext properties as DropBased 
            foreach (var contextProperty in contextType.GetTypePropsRecursively((x) => x.PropertyType.Assembly == contextType.Assembly))
            {
                var dropType = contextProperty.PropertyType.IsArray ? contextProperty.PropertyType.GetElementType() : contextProperty.PropertyType;
                var allowedProperties = dropType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                .Select(x => x.Name)
                                                .ToArray();

                Template.RegisterSafeType(dropType, allowedProperties);
            }
         
        }

        /// <summary>
        /// Main work context
        /// </summary>
        public ShopifyThemeWorkContext WorkContext
        {
            get
            {
                return _workContextFactory() as ShopifyThemeWorkContext;
            }
        }

        /// <summary>
        /// Default master view name
        /// </summary>
        public string MasterViewName
        {
            get
            {
                return _defaultMasterView;
            }
        }
        /// <summary>
        /// Current theme name
        /// </summary>
        public string ThemeName
        {
            get
            {
                return WorkContext.CurrentStore.ThemeName ?? "default";
            }
        }

        /// <summary>
        /// Current theme relative url
        /// </summary>
        public string ThemeRelativeUrl
        {
            get
            {
                return _storeFrontUrlBuilder.ToAppRelative(_themesRelativeUrl + "/" + ThemeName, WorkContext.CurrentStore.Id, WorkContext.CurrentLanguage);
            }

        }

        /// <summary>
        /// Current theme local path
        /// </summary>
        public string ThemeLocalPath
        {
            get
            {
                return _storeFrontUrlBuilder.ToLocalPath(ThemeRelativeUrl);
            }
        }
        /// <summary>
        /// Theme asset url
        /// </summary>
        public string ThemeAssetsRelativeUrl
        {
            get
            {
                return _storeFrontUrlBuilder.ToAppRelative(_themesAssetsRelativeUrl, WorkContext.CurrentStore.Id, WorkContext.CurrentLanguage);
            }
        }
      

        #region IFileSystem members
        public string ReadTemplateFile(Context context, string templateName)
        {
            return ReadTemplateByName(templateName);
        }
        #endregion

        /// <summary>
        /// Read template by name
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string ReadTemplateByName(string templateName)
        {
            if (templateName == null || !_templateRegex.IsMatch(templateName))
                throw new FileSystemException("Error - Illegal template name '{0}'", templateName);

            var baseDirectory = new DirectoryInfo(ThemeLocalPath);
            var templateFile = baseDirectory.GetFiles(String.Format(_liquidTemplateFormat, templateName), SearchOption.AllDirectories).FirstOrDefault();

            if (templateFile == null)
                throw new FileSystemException("Error - No such template {0} . Looked in the following locations:<br />{1}", templateName, ThemeName);

            return File.ReadAllText(templateFile.FullName);
        }

        /// <summary>
        /// Render template by name and with passed context (parameters)
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string RenderTemplateByName(string templateName, Dictionary<string, object> parameters)
        {
            if (String.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("templateName");
            }
            string retVal = null;

            var templateContent = ReadTemplateByName(templateName);
            retVal = RenderTemplate(templateContent, parameters);
            return retVal;
        }

        public string RenderTemplate(string templateContent, Dictionary<string, object> parameters)
        {
            if (String.IsNullOrEmpty(templateContent))
            {
                return templateContent;
            }
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            string retVal = null;

            Template.FileSystem = this;

            var renderParams = new RenderParameters()
            {
                LocalVariables = Hash.FromDictionary(parameters)
            };
      
            var parsedTemplate = Template.Parse(templateContent);
            retVal = parsedTemplate.RenderWithTracing(renderParams);
            return retVal;
        }

        /// <summary>
        /// Read shopify theme settings from 'config' folder
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DefaultableDictionary GetSettings(string defaultValue = null)
        {
            DefaultableDictionary retVal = new DefaultableDictionary(defaultValue);
            var settingsFilePath = Path.Combine(ThemeLocalPath, "config\\settings_data.json");
            if (File.Exists(settingsFilePath))
            {
                var settings = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(settingsFilePath));
                // now get settings for current theme and add it as a settings parameter
                var currentSettings = settings["current"];
                if (!(currentSettings is JObject))
                {
                    currentSettings = settings["presets"][currentSettings.ToString()] as JObject;
                }

                if (currentSettings != null && currentSettings is JObject)
                {
                    var dict = currentSettings.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);
                    retVal = new DefaultableDictionary(dict, defaultValue);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Read localization resources 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public JObject ReadLocalization()
        {
            var culture = WorkContext.CurrentCulture;

            var localeDirectory = new DirectoryInfo(Path.Combine(ThemeLocalPath, "locales"));
            var localeFilePath = Path.Combine(localeDirectory.FullName, String.Format("{0}.json", culture.TwoLetterISOLanguageName));
            var localeDefaultPath = localeDirectory.GetFiles("*.default.json").Select(x => x.FullName).FirstOrDefault();

            JObject localeJson = null;
            JObject defaultJson = null;

            if (File.Exists(localeFilePath))
            {
                localeJson = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(localeFilePath));
            }
            if (File.Exists(localeDefaultPath))
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

        /// <summary>
        /// Get relative url for assets (assets folder)
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public string GetAssetAbsoluteUrl(string assetName)
        {
            return _storeFrontUrlBuilder.ToAbsolute(_themesAssetsRelativeUrl.TrimEnd('/') + "/" + assetName.TrimStart('/'), WorkContext.CurrentStore.Id, WorkContext.CurrentLanguage);
        }

        private static bool ContainsMethod(object left, object right)
        {

            if (left is string)
            {
                return right != null && ((string)left).Contains(right as string);
            }

            if (left is IList)
            {
                return ((IList)left).Contains(right);
            }

            return false;
        }

    }
}
