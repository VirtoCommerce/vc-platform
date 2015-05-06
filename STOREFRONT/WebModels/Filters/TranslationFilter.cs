#region
using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Models.Services;
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
            var service = new CommerceService();
            var locs = service.GetLocale();
            var defaultLocs = service.GetLocale(true);

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