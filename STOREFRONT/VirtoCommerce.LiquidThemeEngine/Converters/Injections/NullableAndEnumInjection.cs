using System;
using System.Reflection;
using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;

namespace VirtoCommerce.LiquidThemeEngine.Converters.Injections
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

                    if (targetProperty.PropertyType.IsEnum)
                    {
                        var value = Enum.Parse(targetProperty.PropertyType, sourceProperty.GetValue(source).ToString(), true);
                        targetProperty.SetValue(target, value);
                    }
                    if(targetProperty.PropertyType == typeof(string) && sourceProperty.PropertyType.IsEnum)
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source).ToString());
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

            if (retVal)
            {
                retVal = !target.PropertyType.IsArray && !target.PropertyType.IsArray;
            }

            if (retVal)
            {
                retVal = source.PropertyType == target.PropertyType || source.PropertyType == Nullable.GetUnderlyingType(target.PropertyType)
                        || Nullable.GetUnderlyingType(source.PropertyType) == target.PropertyType;

                if (!retVal)
                {
                    retVal = source.PropertyType.IsEnum || target.PropertyType.IsEnum;
                }
            }

            return retVal;
        }
    }
}
