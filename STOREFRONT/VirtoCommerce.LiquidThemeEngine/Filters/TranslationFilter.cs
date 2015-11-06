using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// Filter used for localization 
    /// </summary>
    public class TranslationFilter
    {
        private static string[] _countSuffixes = new[] { ".zero", ".one", ".two" };

        #region Public Methods and Operators
        public static string T(string key, params object[] variables)
        {
            var retVal = key;
            var themeAdaptor = (ShopifyLiquidThemeEngine)Template.FileSystem;
            var localization = themeAdaptor.ReadLocalization();
            var dictionary = variables != null ? variables.OfType<Tuple<string, object>>().ToDictionary(x => x.Item1, x => x.Item2) : null;

            if (localization != null)
            {
                if (dictionary != null)
                {
                    //try to transform localization key
                    key = TryTransformKey(key, dictionary);
                }
                retVal = (localization.SelectToken(key) ?? key).ToString();
            }

            if(dictionary != null)
            {
                retVal = themeAdaptor.RenderTemplate(retVal, dictionary);
            }
            return retVal;
        }
        #endregion

        private static string TryTransformKey(string input, Dictionary<string, object> variables)
        {
            var retVal = input;

            object countValue;
            if (variables.TryGetValue("count", out countValue) && countValue != null)
            {
                var count = Convert.ToUInt16(countValue);
                retVal += count < 2 ? _countSuffixes[count] : ".other";
            }
            return retVal;
        }


    }




}
