using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Common
{
    public class NullableAndEnumValueInjection : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetProps();
            var targetType = target.GetType();

            foreach (var sourceProperty in sourceProps)
            {
                var targetProperty = targetType.GetProperty(sourceProperty.Name);
                if (targetProperty != null && PropertyMatch(sourceProperty, targetProperty))
                {
               
                    if(targetProperty.PropertyType.IsEnum)
                    {
                        var value = Enum.Parse(targetProperty.PropertyType, sourceProperty.GetValue(source).ToString(), true);
                        targetProperty.SetValue(target, value);
                    }
                    else
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                    }
                }
            }
        }

        private static bool PropertyMatch(PropertyInfo source, PropertyInfo target)
        {
            var retVal = source.CanRead && source.GetGetMethod() != null;

            if (retVal)
            {
                retVal = target.CanWrite && target.GetSetMethod() != null;
            }

            if (retVal)
            {
                retVal = !source.PropertyType.IsArray && !target.PropertyType.IsArray;
            }

            if(retVal)
            {
                retVal = source.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) == null && target.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) == null;
            }
            if (retVal)
            {
                retVal = !target.PropertyType.IsArray && !target.PropertyType.IsArray;
            }

            if (retVal)
            {
                retVal = source.PropertyType == target.PropertyType || source.PropertyType == Nullable.GetUnderlyingType(target.PropertyType)
                        || Nullable.GetUnderlyingType(source.PropertyType) == target.PropertyType;

                if(!retVal)
                {
                    retVal = source.PropertyType.IsEnum || target.PropertyType.IsEnum;
                }
            }
            return retVal;
        }
    }
}