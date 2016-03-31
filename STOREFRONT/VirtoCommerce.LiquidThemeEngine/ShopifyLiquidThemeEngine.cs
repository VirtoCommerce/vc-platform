using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CacheManager.Core;
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
        private const string _angularInterpolateTagStart = "{(";
        private const string _angularInterpolateTagStop = ")}";
        private const string _globalThemeName = "default";
        private const string _defaultMasterView = "theme";
        private const string _liquidTemplateFormat = "{0}.liquid";
        private static readonly string[] _templatesDiscoveryFolders = { "templates", "snippets", "layout", "assets" };
        private static readonly Regex _templateRegex = new Regex(@"[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private readonly string _themesLocalPath;
        private readonly string _themesAssetsRelativeUrl;
        private readonly string _globalThemeAssetsRelativeUrl;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly Func<IStorefrontUrlBuilder> _storeFrontUrlBuilderFactory;
        private readonly ILocalCacheManager _cacheManager;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly SassCompilerProxy _saasCompiler = new SassCompilerProxy();

        public ShopifyLiquidThemeEngine(ILocalCacheManager cacheManager, Func<WorkContext> workContextFactory, Func<IStorefrontUrlBuilder> storeFrontUrlBuilderFactory, string themesLocalPath, string themesAssetsRelativeUrl, string globalThemeAssetsRelativeUrl)
        {
            _workContextFactory = workContextFactory;
            _storeFrontUrlBuilderFactory = storeFrontUrlBuilderFactory;
            _themesLocalPath = themesLocalPath;
            _themesAssetsRelativeUrl = themesAssetsRelativeUrl;
            _globalThemeAssetsRelativeUrl = globalThemeAssetsRelativeUrl;
            _cacheManager = cacheManager;

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

            //Observe themes file system changes to invalidate cache if changes occur
            _fileSystemWatcher = MonitorThemeFileSystemChanges();
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
        /// Current theme local path
        /// </summary>
        private string CurrentThemeLocalPath
        {
            get
            {
                return Path.Combine(_themesLocalPath, WorkContext.CurrentStore.Id, CurrentThemeName);
            }
        }
        /// <summary>
        /// Global theme local path
        /// </summary>
        private string GlobalThemeLocalPath
        {
            get
            {
                return Path.Combine(_themesLocalPath, _globalThemeName);
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
            var assetPath = (searchInGlobalThemeOnly ? GlobalThemeLocalPath : CurrentThemeLocalPath) + "\\assets";
            string[] files = null;
            if (Directory.Exists(assetPath))
            {
                files = Directory.GetFiles(assetPath, fileName, SearchOption.AllDirectories);
            }

            if (!searchInGlobalThemeOnly && (files == null || !files.Any()))
            {
                //Try to find asset in default theme
                assetPath = GlobalThemeLocalPath + "\\assets";
                if (Directory.Exists(assetPath))
                {
                    files = Directory.GetFiles(assetPath, fileName, SearchOption.AllDirectories);
                }
            }
            //We find requested asset need return resulting stream
            if (files != null && files.Any())
            {
                retVal = File.OpenRead(files.FirstOrDefault());
            }
            else
            {
                //Otherwise it may be liquid template 
                fileName = fileName.Replace(".scss.css", ".scss");
                var settings = GetSettings("''");
                //Try to parse liquid asset resource
                var themeAssetPath = ResolveTemplatePath(fileName, searchInGlobalThemeOnly);
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
            var curentThemediscoveryPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(CurrentThemeLocalPath, x, liquidTemplateFileName));
            //First try to find template in current theme folder
            var retVal = curentThemediscoveryPaths.FirstOrDefault(x => File.Exists(x));
            if (searchInGlobalThemeOnly || (retVal == null && GlobalThemeLocalPath != CurrentThemeLocalPath))
            {
                //Then try to find in global theme
                var globalThemeDiscoveyPaths = _templatesDiscoveryFolders.Select(x => Path.Combine(GlobalThemeLocalPath, x, liquidTemplateFileName));
                retVal = globalThemeDiscoveyPaths.FirstOrDefault(x => File.Exists(x));
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
            //Replace escaped angular interpolated tag symbols to standard
            if(retVal != null)
            {
                //TODO: Need make it in more by liquid processor compatible way (may be using exist tokenizer or something like that)
                retVal = retVal.Replace(_angularInterpolateTagStart, "{{");
                retVal = retVal.Replace(_angularInterpolateTagStop, "}}");
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

                var resultSettings = InnerGetSettings(GlobalThemeLocalPath);
                if (GlobalThemeLocalPath != CurrentThemeLocalPath)
                {
                    var currentThemeSettings = InnerGetSettings(CurrentThemeLocalPath);
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
                //Load first localization from default theme
                var retVal = InnerReadLocalization(GlobalThemeLocalPath, WorkContext.CurrentLanguage);
                if (GlobalThemeLocalPath != CurrentThemeLocalPath)
                {
                    //Next need merge current theme localization with default
                    var currentThemeLocalization = InnerReadLocalization(CurrentThemeLocalPath, WorkContext.CurrentLanguage);
                    if (currentThemeLocalization != null)
                    {
                        retVal.Merge(currentThemeLocalization, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                    }
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


        private JObject InnerReadLocalization(string themePath, Language language)
        {
            JObject retVal = null;
            var localeDirectory = new DirectoryInfo(Path.Combine(themePath, "locales"));
            if (localeDirectory.Exists)
            {
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
                retVal = defaultJson ?? localeJson;
                if (defaultJson != null && localeJson != null)
                {
                    retVal.Merge(localeJson, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                }
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

        private string ReadTemplateByPath(string templatePath)
        {
            var retVal = _cacheManager.Get(GetCacheKey("ReadTemplateByName", templatePath), "LiquidThemeRegion", () =>
            {
                if (!String.IsNullOrEmpty(templatePath) && File.Exists(templatePath))
                {
                    return File.ReadAllText(templatePath);
                }
                throw new FileSystemException("Error - No such template {0}.", templatePath);
            });
            return retVal;
        }

        private string GetCacheKey(params string[] parts)
        {
            var retVal = new string[] { CurrentThemeLocalPath, GlobalThemeLocalPath, WorkContext.CurrentLanguage.CultureName, WorkContext.CurrentCurrency.Code };
            if (parts != null)
            {
                retVal = retVal.Concat(parts.Select(x => x == null ? String.Empty : x)).ToArray();
            }
            return String.Join(":", retVal).GetHashCode().ToString();
        }

        private FileSystemWatcher MonitorThemeFileSystemChanges()
        {
            var fileSystemWatcher = new FileSystemWatcher();

            if (Directory.Exists(_themesLocalPath))
            {
                fileSystemWatcher.Path = _themesLocalPath;
                fileSystemWatcher.IncludeSubdirectories = true;

                FileSystemEventHandler handler = (sender, args) =>
                {
                    _cacheManager.Clear();
                };
                RenamedEventHandler renamedHandler = (sender, args) =>
                {
                    _cacheManager.Clear();
                };
                var throttledHandler = handler.Throttle(TimeSpan.FromSeconds(5));
                // Add event handlers.
                fileSystemWatcher.Changed += throttledHandler;
                fileSystemWatcher.Created += throttledHandler;
                fileSystemWatcher.Deleted += throttledHandler;
                fileSystemWatcher.Renamed += renamedHandler;

                // Begin watching.
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            return fileSystemWatcher;
        }


    }
}
