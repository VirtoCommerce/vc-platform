using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/questions/484085/an-algorithm-to-spacify-camelcased-strings
        /// </summary>
        /// <param name="str"></param>
        /// <param name="spacer"></param>
        /// <returns></returns>
        public static string Decamelize(this string str, char spacer = '_')
        {
            if (String.IsNullOrEmpty(str))
                return str;

            /* Note that the .ToString() is required, otherwise the char is implicitly
             * converted to an integer and the wrong overloaded ctor is used */
            StringBuilder sb = new StringBuilder(str[0].ToString());
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str, i))
                    sb.Append(spacer);
                sb.Append(str[i]);
            }
            return sb.ToString().ToLowerInvariant();
        }
    }
}
