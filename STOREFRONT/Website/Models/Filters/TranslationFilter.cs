#region
using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

#endregion

namespace VirtoCommerce.Web.Models.Filters
{
    public class TranslationFilter
    {
        #region Public Methods and Operators
        public static string t(string input, params object[] variables)
        {
            var locs = LoadLocales();
            var defaultLocs = LoadLocales(true);

            if (locs == null && defaultLocs == null)
            {
                return input;
            }

            string retVal;

            // first get a template string
            if (variables != null && variables.Length > 0)
            {
                var dictionary =
                    variables.Where(x => (x is Tuple<string, object>))
                        .Select(x => x as Tuple<string, object>)
                        .ToDictionary(x => x.Item1, x => x.Item2);

                string template;
                if (dictionary.ContainsKey("count") && dictionary["count"] != null) // execute special count routing
                {
                    var count = dictionary["count"].ToInt();
                    JToken templateToken;
                    switch (count)
                    {
                        case 1:
                            templateToken = locs.GetValue(defaultLocs, input + ".one");
                            break;
                        case 0:
                            templateToken = locs.GetValue(defaultLocs, input + ".zero");
                            break;
                        case 2:
                            templateToken = locs.GetValue(defaultLocs, input + ".two");
                            break;
                        default:
                            templateToken = locs.GetValue(defaultLocs, input + ".other");
                            break;
                    }

                    if (templateToken == null)
                    {
                        templateToken = locs.GetValue(defaultLocs, input + ".other");
                        template = templateToken != null ? templateToken.ToString() : String.Empty;
                    }
                    else
                    {
                        template = templateToken.ToString();
                    }
                }
                else
                {
                    template = locs.GetValue(defaultLocs, input);
                }

                var templateEngine = Template.Parse(template);
                retVal = templateEngine.Render(Hash.FromDictionary(dictionary));
            }
            else
            {
                retVal = locs.GetValue(defaultLocs, input);
            }

            return retVal;
        }
        #endregion

        #region Methods
        private static string GetCurrentLanguageLocaleFile(string theme, string language)
        {
            var locDir = String.Format("~/App_Data/Themes/{0}/locales/", theme);
            var files = VirtualPathProviderHelper.ListFiles(locDir);
            if (files == null || !files.Any())
            {
                return null;
            }

            var culture = language.TryGetCultureInfo();

            // check specific culture file existance
            var foundFiles = files.Where(f => f.Contains(String.Format("{0}.json", culture.Name)));

            if (foundFiles.Any())
            {
                return foundFiles.First();
            }

            // check general culture file existance
            foundFiles = files.Where(f => f.Contains(String.Format("{0}.json", culture.TwoLetterISOLanguageName)));

            if (foundFiles.Any())
            {
                return foundFiles.First();
            }

            // didn't find any language
            return null;
        }

        private static string GetDefaultLanguageLocaleFile(string theme)
        {
            var locDir = String.Format("~/App_Data/Themes/{0}/locales/", theme);
            var files = VirtualPathProviderHelper.ListFiles(locDir);
            if (files == null || !files.Any())
            {
                return null;
            }

            var foundFiles = files.Where(f => f.Contains(String.Format("default.json")));

            if (foundFiles.Any())
            {
                return foundFiles.First();
            }

            // didn't find any language
            return null;
        }

        private static JObject LoadLocales(bool loadDefault = false)
        {
            var contextKey = String.Format(
                "vc-liquid-localizations-{0}-{1}-{2}",
                SiteContext.Current.Theme,
                SiteContext.Current.Language,
                loadDefault);
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                if (value is JObject)
                {
                    return value as JObject;
                }

                return null;
            }

            var fileName = GetCurrentLanguageLocaleFile(
                SiteContext.Current.Theme.ToString(),
                SiteContext.Current.Language);

            if (loadDefault)
            {
                fileName = GetDefaultLanguageLocaleFile(SiteContext.Current.Theme.ToString());
            }
            else
            {
                fileName = GetCurrentLanguageLocaleFile(
                    SiteContext.Current.Theme.ToString(),
                    SiteContext.Current.Language);
            }

            var filePath = String.Format("~/App_Data/Themes/{0}/locales/{1}", SiteContext.Current.Theme, fileName);
            if (String.IsNullOrEmpty(filePath))
            {
                return null;
            }

            var path = HttpContext.Current.Server.MapPath(filePath);
            var fileContents = VirtualPathProviderHelper.Load(filePath);

            if (fileContents != null)
            {
                var contents = JsonConvert.DeserializeObject<dynamic>(fileContents);
                HttpRuntime.Cache.Insert(contextKey, contents, new CacheDependency(new[] { path }));
                return contents;
            }
            HttpRuntime.Cache.Insert(contextKey, String.Empty, new CacheDependency(new[] { path }));
            return null;
        }
        #endregion
    }

    public static class LocaleExtensions
    {
        #region Public Methods and Operators
        public static string GetValue(this JObject source, JObject defaultSource, string key)
        {
            JToken token = null;

            if (source != null)
            {
                token = source.SelectToken(key);
            }

            if (token != null)
            {
                return token.ToString();
            }

            token = defaultSource.SelectToken(key);
            if (token != null)
            {
                return token.ToString();
            }

            return key;
        }
        #endregion
    }
}