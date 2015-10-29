using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class WorkContextConverter
    {
        public static Dictionary<string, object> ToLiquidThemeContext(this WorkContext workContext)
        {
            var retVal = new Dictionary<string, object>();
            foreach(var propertyInfo in workContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                retVal.Add(propertyInfo.Name.Decamelize(), propertyInfo.GetValue(workContext));
            }

            return retVal;
        }
    }
}
