using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Storefront.Common
{
    public static class NameValueCollectionExtensions
    {
        [CLSCompliant(false)]
        public static T GetValue<T>(this NameValueCollection nameValuePairs, string configKey, T defaultValue) where T : IConvertible
        {
            T result = default(T);

            if (nameValuePairs.AllKeys.Contains(configKey))
            {
                var tmpValue = nameValuePairs[configKey];

                result = (T)Convert.ChangeType(tmpValue, typeof(T));
            }
            else
            {
                return defaultValue;
            }

            return result;
        }
    }
}
