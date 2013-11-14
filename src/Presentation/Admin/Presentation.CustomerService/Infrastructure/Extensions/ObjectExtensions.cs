using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }

        public static bool HasProperty(this object objectToCheck, string propertyName)
        {
            var type = objectToCheck.GetType();
            return type.GetProperty(propertyName)!=null;
        }


        public static PropertyValueType GetPropertyValueType(this object objectToCheck,string propertyName)
        {
            PropertyValueType result=PropertyValueType.DateTime;


            var type = objectToCheck.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var propType = propertyInfo.PropertyType;
                if (propType.FullName.Contains("DateTime"))
                {
                    result = PropertyValueType.DateTime;
                }
                else if (propType.FullName.Contains("String"))
                {
                    result=PropertyValueType.ShortString;
                }
                else if (propType.FullName.Contains("Int32"))
                {
                    result=PropertyValueType.Integer;
                }
                else if (propType.FullName.Contains("Decimal"))
                {
                    result=PropertyValueType.Decimal;
                }
                else if (propType.FullName.Contains("Boolean"))
                {
                    result=PropertyValueType.Boolean;
                }


            }


            return result;
        }

    }
}
