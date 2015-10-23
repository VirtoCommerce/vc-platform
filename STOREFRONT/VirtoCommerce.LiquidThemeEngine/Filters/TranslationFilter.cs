using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.ViewEngine.FileSystems;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// Filter used for localization 
    /// </summary>
    public class TranslationFilter
    {
        private static string[] _countSuffixes = new []{ ".zero", ".one", ".two" };

        public TranslationFilter()
        {
        }

        #region Public Methods and Operators
        public static string T(string key, params object[] variables)
        {
            var retVal = key;
            var shopifyFileSystem = Template.FileSystem as ShopifyThemeLiquidFileSystem;
            if (shopifyFileSystem != null)
            {
                var localization = shopifyFileSystem.ReadLocalization();
                if (localization != null)
                {
                    //try to transform localization key
                    key = TryTransformKey(key, variables);
                    retVal = (localization[key] ?? String.Empty).ToString();
                }
            }

            return retVal;
        }
        #endregion

        private static string TryTransformKey(string input, params object[] variables)
        {
            var retVal = input;
            if(variables != null)
            {
                var dictionary = variables.OfType<Tuple<string, object>>().ToDictionary(x => x.Item1, x => x.Item2);
                object countValue;
                if (dictionary.TryGetValue("count", out countValue) && countValue != null)
                {
                    var count = Convert.ToUInt16(countValue);
                    retVal += count < 2 ? _countSuffixes[count] : ".other";
                }
            }

            return retVal;
        }


    }




}
