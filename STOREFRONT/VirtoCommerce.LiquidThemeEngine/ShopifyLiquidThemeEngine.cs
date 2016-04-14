using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using DotLiquid.ViewEngine.Exceptions;
using LibSassNetProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Operators;
using VirtoCommerce.LiquidThemeEngine.Tags;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

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
    public class ShopifyLiquidThemeEngine : IFileSystem, ILiquidThemeEngine
    {
        private const string _globalThemeName = "default";
        private const string _defaultMasterView = "theme";
        private const string _liquidTemplateFormat = "{0}.liquid";
        private static readonly string[] _templatesDiscoveryFolders = { "templates", "snippets", "layout", "assets" };
        private static readonly Regex _templateRegex = new Regex(@"[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private readonly string _themesAssetsRelativeUrl;
        private readonly string _globalThemeAssetsRelativeUrl;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly Func<IStorefrontUrlBuilder> _storeFrontUrlBuilderFactory;
        private readonly ILocalCacheManager _cacheManager;
        private readonly SassCompilerProxy _saasCompiler = new SassCompilerProxy();
        private readonly IContentBlobProvider _themeBlobProvider;
        private readonly IContentBlobProvider _globalThemeBlobProvider;

        public ShopifyLiquidThemeEngine(ILocalCacheManager cacheManager, Func<WorkContext> workContextFactory, Func<IStorefrontUrlBuilder> storeFrontUrlBuilderFactory, IContentBlobProvider themeBlobProvider, IContentBlobProvider globalThemeBlobProvider, string themesAssetsRelativeUrl, string globalThemeAssetsRelativeUrl)
        {
            _workContextFactory = workContextFactory;
            _storeFrontUrlBuilderFactory = storeFrontUrlBuilderFactory;
            _themesAssetsRelativeUrl = themesAssetsRelativeUrl;
            _globalThemeAssetsRelativeUrl = globalThemeAssetsRelativeUrl;
            _cacheManager = cacheManager;
            _themeBlobProvider = themeBlobProvider;
            _globalThemeBlobProvider = globalThemeBlobProvider;

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

            //Observe themes content system changes to invalidate cache if changes occur
            _themeBlobProvider.Changed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
            _themeBlobProvider.Renamed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
            _globalThemeBlobProvider.Changed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
            _globalThemeBlobProvider.Renamed += (sender, args) =>
            {
                _cacheManager.Clear();
            };
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
                return string.IsNullOrEmpty(WorkContext.CurrentStore.ThemeName) ? _globalThemeName : WorkContext.CurrentStore.ThemeName;
            }
        }


        /// <summary>
        /// Current theme base path
        /// </summary>
        private string CurrentThemePath
        {
            get
            {
                return Path.Combine(WorkContext.CurrentStore.Id, CurrentThemeName);
            }
        }

        #region IFileSystem members
        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = ResolveTemplatePath(templateName);
            return ReadTemplateByPath(templatePath);
        }
        #endregion

        #region ILiquidThemeEngine Members
        /// <summary>
        /// Return stream for requested  asset file  (used for search current and base themes assets)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream GetAssetStream(string fileName, bool searchInGlobalThemeOnly = false)
        {
            Stream retVal = null;
            var fileExtensions = System.IO.Path.GetExtension(fileName);
            string currentThemePath = null;
            string globalThemePath = _globalThemeBlobProvider.Search("assets", fileName, true).FirstOrDefault();
            if (!searchInGlobalThemeOnly)
            {
                //try to search in current store theme 
                if (_themeBlobProvider.PathExists(CurrentThemePath + "\\assets"))
                {
                    currentThemePath = _themeBlobProvider.Search(CurrentThemePath + "\\assets", fileName, true).FirstOrDefault();
                }
            }

            //We find requested asset need return resulting stream
            if (currentThemePath != null)
            {
                retVal = _themeBlobProvider.OpenRead(currentThemePath);
            }
            else if (globalThemePath != null)
            {
                retVal = _globalThemeBlobProvider.OpenRead(globalThemePath);
            }
            else
            {
                //Otherwise it may be liquid template 
                fileName = fileName.Replace(".scss.css", ".scss");
                var settings = GetSettings("''");
                //Try to parse liquid asset resource
                var themeAssetPath = ResolveTemplatePath(fileName, searchInGlobalThemeOnly);
                if (themeAssetPath != null)
                {
                    var templateContent = ReadTemplateByPath(themeAssetPath);
                    var content = RenderTemplate(templateContent, new Dictionary<string, object>() { { "settings", settings } });

                    if (fileName.EndsWith(".scss"))
                    {
                        try
                        {
                            //handle scss resources
                            content = _saasCompiler.Compile(content);
                        }
                        catch (Exception ex)
                        {
                            throw new SaasCompileException(fileName, content, ex);
                        }
                    }
                    if (content != null)
                    {
                        retVal = new MemoryStream(Encoding.UTF8.GetBytes(content));
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// resolve  template path by it name
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string ResolveTemplatePath(string templateName, bool searchInGlobalThemeOnly = false)
        {
            if (WorkContext.CurrentStore == null)
                return null;

            var liquidTemplateFileName = String.Format(_liquidTemplateFormat, templateName);
            var curentThemediscoveryPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(CurrentThemePath, x, liquidTemplateFileName));
            //First try to find template in current theme folder
            var retVal = curentThemediscoveryPaths.FirstOrDefault(x => _themeBlobProvider.PathExists(x));
            if (searchInGlobalThemeOnly || retVal == null)
            {
                //Then try to find in global theme
                var globalThemeDiscoveyPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(x, liquidTemplateFileName));
                retVal = globalThemeDiscoveyPaths.FirstOrDefault(x => _globalThemeBlobProvider.PathExists(x));
            }
            return retVal;
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
            var templatePath = ResolveTemplatePath(templateName);
            var templateContent = ReadTemplateByPath(templatePath);
            var retVal = RenderTemplate(templateContent, parameters);
            return retVal;
        }

        /// <summary>
        /// Render template by content and parameters
        /// </summary>
        /// <param name="templateContent"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

            var parsedTemplate = _cacheManager.Get(GetCacheKey("ParseTemplate", templateContent.GetHashCode().ToString()), "LiquidTheme", () => { return Template.Parse(templateContent); });

            var retVal = parsedTemplate.RenderWithTracing(renderParams);

            //Copy key values which were generated in rendering to out parameters
            if (parameters != null && parsedTemplate.Registers != null)
            {
                foreach (var registerPair in parsedTemplate.Registers)
                {
                    parameters[registerPair.Key] = registerPair.Value;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Read shopify theme settings from 'config' folder
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public IDictionary GetSettings(string defaultValue = null)
        {
            return _cacheManager.Get(GetCacheKey("GetSettings", defaultValue), "LiquidThemeRegion", () =>
            {
                DefaultableDictionary retVal = new DefaultableDictionary(defaultValue);
                //Read first settings from global theme
                var resultSettings = InnerGetSettings(_globalThemeBlobProvider, "");
                //Then load from current theme
                var currentThemeSettings = InnerGetSettings(_themeBlobProvider, CurrentThemePath);
                if (currentThemeSettings != null)
                {
                    if (resultSettings == null) // if there is no default settings, use just current theme
                    {
                        resultSettings = currentThemeSettings;
                    }
                    else
                    {
                        resultSettings.Merge(currentThemeSettings, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                    }
                }


                if (resultSettings != null)
                {
                    var dict = resultSettings.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);
                    retVal = new DefaultableDictionary(dict, defaultValue);
                }

                return retVal;
            });
        }


        /// <summary>
        /// Read localization resources 
        /// </summary>
        /// <returns></returns>
        public JObject ReadLocalization()
        {
            return _cacheManager.Get(GetCacheKey("ReadLocalization"), "LiquidThemeRegion", () =>
            {
                //Load first localization from global theme
                var retVal = InnerReadLocalization(_globalThemeBlobProvider, "", WorkContext.CurrentLanguage);

                //Next need merge current theme localization with default
                var currentThemeLocalization = InnerReadLocalization(_themeBlobProvider, CurrentThemePath, WorkContext.CurrentLanguage);
                if (currentThemeLocalization != null)
                {
                    retVal.Merge(currentThemeLocalization, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                }

                return retVal;
            });
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

        /// <summary>
        /// Get relative url for global assets
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public string GetGlobalAssetAbsoluteUrl(string assetName)
        {
            return UrlBuilder.ToAppAbsolute(_globalThemeAssetsRelativeUrl.TrimEnd('/') + "/" + assetName.TrimStart('/'), WorkContext.CurrentStore, WorkContext.CurrentLanguage);
        }
        #endregion


        private static JObject InnerReadLocalization(IContentBlobProvider themeBlobProvider, string themePath, Language language)
        {
            JObject retVal = null;
            var localeFolderPath = Path.Combine(themePath, "locales");
            if (themeBlobProvider.PathExists(localeFolderPath))
            {
                var currentLocalePath = Path.Combine(localeFolderPath, string.Concat(language.TwoLetterLanguageName, ".json"));
                var localeDefaultPath = themeBlobProvider.Search(localeFolderPath, "*.default.json", false).FirstOrDefault();

                JObject localeJson = null;
                JObject defaultJson = null;

                if (themeBlobProvider.PathExists(currentLocalePath))
                {
                    using (var stream = themeBlobProvider.OpenRead(currentLocalePath))
                    {
                        localeJson = JsonConvert.DeserializeObject<dynamic>(stream.ReadToString());
                    }
                }

                if (localeDefaultPath != null && themeBlobProvider.PathExists(localeDefaultPath))
                {
                    using (var stream = themeBlobProvider.OpenRead(localeDefaultPath))
                    {
                        defaultJson = JsonConvert.DeserializeObject<dynamic>(stream.ReadToString());
                    }
                }

                //Need merge default and requested localization json to resulting object
                retVal = defaultJson ?? localeJson;
                if (defaultJson != null && localeJson != null)
                {
                    retVal.Merge(localeJson, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                }
            }
            return retVal;
        }


        private static JObject InnerGetSettings(IContentBlobProvider themeBlobProvider, string themePath)
        {
            JObject retVal = null;
            var settingsPath = Path.Combine(themePath, "config\\settings_data.json");
            if (themeBlobProvider.PathExists(settingsPath))
            {
                using (var stream = themeBlobProvider.OpenRead(settingsPath))
                {
                    var settings = JsonConvert.DeserializeObject<JObject>(stream.ReadToString());
                    // now get settings for current theme and add it as a settings parameter
                    retVal = settings["current"] as JObject;
                    if (retVal == null)
                    {
                        //is setting preset name need return it as active
                        retVal = settings["presets"][settings["current"].ToString()] as JObject;
                    }
                }
            }
            return retVal;
        }

        private string ReadTemplateByPath(string templatePath)
        {
            var retVal = _cacheManager.Get(GetCacheKey("ReadTemplateByName", templatePath), "LiquidThemeRegion", () =>
            {
                if (!String.IsNullOrEmpty(templatePath))
                {
                    //First try find content in current store themer
                    IContentBlobProvider blobProvider = _themeBlobProvider;
                    if (!blobProvider.PathExists(templatePath))
                    {
                        //Else search in global theme
                        blobProvider = _globalThemeBlobProvider;
                    }
                    using (var stream = blobProvider.OpenRead(templatePath))
                    {
                        return stream.ReadToString();
                    }

                }
                throw new FileSystemException("Error - No such template {0}.", templatePath);
            });
            return retVal;
        }

        private string GetCacheKey(params string[] parts)
        {
            var retVal = new string[] { CurrentThemePath, WorkContext.CurrentLanguage.CultureName, WorkContext.CurrentCurrency.Code };
            if (parts != null)
            {
                retVal = retVal.Concat(parts.Select(x => x == null ? String.Empty : x)).ToArray();
            }
            return String.Join(":", retVal).GetHashCode().ToString();
        }




    }
}
