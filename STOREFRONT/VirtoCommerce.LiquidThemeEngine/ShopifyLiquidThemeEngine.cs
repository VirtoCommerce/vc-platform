using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Filters;
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
        private const string _defaultThemeName = "default";
        private const string _defaultMasterView = "theme";
        private const string _liquidTemplateFormat = "{0}.liquid";
        private static readonly string[] _templatesDiscoveryFolders = { "templates", "snippets", "layout", "assets" };
        private static readonly Regex _templateRegex = new Regex(@"[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private readonly string _themesLocalPath;
        private readonly string _themesAssetsRelativeUrl;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly Func<IStorefrontUrlBuilder> _storeFrontUrlBuilderFactory;

        public ShopifyLiquidThemeEngine(Func<WorkContext> workContextFactory, Func<IStorefrontUrlBuilder> storeFrontUrlBuilderFactory, string themesLocalPath, string themesAssetsRelativeUrl)
        {
            _workContextFactory = workContextFactory;
            _storeFrontUrlBuilderFactory = storeFrontUrlBuilderFactory;
            _themesLocalPath = themesLocalPath;
            _themesAssetsRelativeUrl = themesAssetsRelativeUrl;

            Liquid.UseRubyDateFormat = true;
            // Register custom tags (Only need to do this once)
            Template.RegisterFilter(typeof(CommonFilters));
            Template.RegisterFilter(typeof(CommerceFilters));
            Template.RegisterFilter(typeof(TranslationFilter));
            Template.RegisterFilter(typeof(UrlFilters));
            Template.RegisterFilter(typeof(DateFilters));
            Template.RegisterFilter(typeof(MoneyFilters));
            Template.RegisterFilter(typeof(HtmlFilters));
            Template.RegisterFilter(typeof(StringFilters));

            Condition.Operators["contains"] = CommonOperators.ContainsMethod;

            Template.RegisterTag<LayoutTag>("layout");
            Template.RegisterTag<FormTag>("form");
            Template.RegisterTag<PaginateTag>("paginate");
        }

        /// <summary>
        /// Main work context
        /// </summary>
        public WorkContext WorkContext
        {
            get
            {
                return _workContextFactory();
            }
        }
        /// <summary>
        /// Store url builder
        /// </summary>
        public IStorefrontUrlBuilder UrlBuilder
        {
            get
            {
                return _storeFrontUrlBuilderFactory();
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
        public string CurrentThemeName
        {
            get
            {
                return WorkContext.CurrentStore.ThemeName ?? _defaultThemeName;
            }
        }

      
        /// <summary>
        /// Current theme local path
        /// </summary>
        private string CurrentThemeLocalPath
        {
            get
            {
                return Path.Combine(_themesLocalPath, WorkContext.CurrentStore.Name, CurrentThemeName);
            }
        }
        /// <summary>
        /// Default theme local path
        /// </summary>
        private string DefaultThemeLocalPath
        {
            get
            {
                return Path.Combine(_themesLocalPath, _defaultThemeName);
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

            var curentThemediscoveryPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(CurrentThemeLocalPath, x, String.Format(_liquidTemplateFormat, templateName)));
            //First try to find template in current theme folder
            var existTemplatePath = curentThemediscoveryPaths.FirstOrDefault(x => File.Exists(x));
            if(existTemplatePath == null && DefaultThemeLocalPath != CurrentThemeLocalPath)
            {
                //Then try to find in default theme
                var defaultThemeDiscoveyPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(DefaultThemeLocalPath, x, String.Format(_liquidTemplateFormat, templateName)));
                existTemplatePath = defaultThemeDiscoveyPaths.FirstOrDefault(x => File.Exists(x));
            }

            if(existTemplatePath != null)
            {
                return File.ReadAllText(existTemplatePath);
            }
          
            throw new FileSystemException("Error - No such template {0} . Looked in the following locations:<br />{1}", templateName, CurrentThemeName);
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

            var templateContent = ReadTemplateByName(templateName);
            var retVal = RenderTemplate(templateContent, parameters);
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

            Template.FileSystem = this;

            var renderParams = new RenderParameters()
            {
                LocalVariables = Hash.FromDictionary(parameters)
            };

            var parsedTemplate = Template.Parse(templateContent);
            var retVal = parsedTemplate.RenderWithTracing(renderParams);
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

            var resultSettings = InnerGetSettings(DefaultThemeLocalPath);
            if(DefaultThemeLocalPath != CurrentThemeLocalPath)
            {
                var currentThemeSettings = InnerGetSettings(CurrentThemeLocalPath);
                resultSettings.Merge(currentThemeSettings, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            }

            if (resultSettings != null)
            {
                var dict = resultSettings.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);
                retVal = new DefaultableDictionary(dict, defaultValue);
            }

            return retVal;
        }

        private JObject InnerGetSettings(string themePath)
        {
            JObject retVal = null;
            var settingsFilePath = Path.Combine(themePath, "config\\settings_data.json");
            if (File.Exists(settingsFilePath))
            {
                var settings = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(settingsFilePath));
                // now get settings for current theme and add it as a settings parameter
                retVal = settings["current"] as JObject;
                if (retVal == null)
                {
                    //is setting preset name need return it as active
                    retVal = settings["presets"][settings["current"].ToString()] as JObject;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Read localization resources 
        /// </summary>
        /// <returns></returns>
        public JObject ReadLocalization()
        {
            //Load first localization from default theme
            var retVal = InnerReadLocalization(DefaultThemeLocalPath, WorkContext.CurrentLanguage);
            if(DefaultThemeLocalPath != CurrentThemeLocalPath)
            {
                //Next need merge current theme localization with default
                var currentThemeLocalization = InnerReadLocalization(CurrentThemeLocalPath, WorkContext.CurrentLanguage);
                retVal.Merge(currentThemeLocalization, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
            }
            return retVal;
        }

        private JObject InnerReadLocalization(string themePath, Language language)
        {
            var localeDirectory = new DirectoryInfo(Path.Combine(themePath, "locales"));
            var localeFilePath = Path.Combine(localeDirectory.FullName, string.Concat(language.TwoLetterLanguageName, ".json"));
            var localeDefaultPath = localeDirectory.GetFiles("*.default.json").Select(x => x.FullName).FirstOrDefault();

            JObject localeJson = null;
            JObject defaultJson = null;

            if (File.Exists(localeFilePath))
            {
                localeJson = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(localeFilePath));
            }

            if (localeDefaultPath != null && File.Exists(localeDefaultPath))
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
            return UrlBuilder.ToAppAbsolute(_themesAssetsRelativeUrl.TrimEnd('/') + "/" + assetName.TrimStart('/'), WorkContext.CurrentStore, WorkContext.CurrentLanguage);
        }

    }
}
