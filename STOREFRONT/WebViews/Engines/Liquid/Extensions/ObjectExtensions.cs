#region
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Extensions
{

    #region
    #endregion

    public static class ObjectExtensions
    {
        #region Public Methods and Operators
        public static int ToInt(this object o, int defaultValue = 0)
        {
            if (o == null)
            {
                return defaultValue;
            }

            if (o is int)
            {
                return (int)o;
            }

            var retVal = 0;
            return Int32.TryParse(o.ToString(), out retVal) ? retVal : defaultValue;
        }

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
            return
                o.GetType()
                    .GetProperties()
                    .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(o, null)))
                    .ToDictionary(x => x.Key, y => y.Value);
        }
        #endregion
    }
}