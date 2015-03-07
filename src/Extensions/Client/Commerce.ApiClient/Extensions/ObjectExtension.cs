using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class ObjectExtension
    {
        #region Public Methods and Operators

        public static string ToNullOrString(this object o)
        {
            return o == null ? null : o.ToString();
        }

        /// <summary>
        ///     Builds a dictionary from the object's properties
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyDictionary(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return o.GetType().GetProperties()
                .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(o, null)))
                .ToDictionary();
        }

        #endregion
    }
}
