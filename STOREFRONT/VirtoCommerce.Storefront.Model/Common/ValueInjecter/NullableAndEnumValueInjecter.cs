using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;
using System;
using System.Reflection;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class NullableAndEnumValueInjecter : ValueInjection
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

                    if (targetProperty.PropertyType == typeof(string) && sourceProperty.PropertyType.IsEnum)
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source).ToString());
                    }
                    else
                    {
                        if (!targetProperty.PropertyType.IsEnum)
                        {
                            targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                        }
                    }
                }
            }
        }

        private static bool PropertyMatch(PropertyInfo source, PropertyInfo target)
        {
            var result = source.CanRead && source.GetGetMethod() != null;

            if (result)
            {
                result = target.CanWrite && target.GetSetMethod() != null;
            }

            if (result)
            {
                result = !source.PropertyType.IsArray && !target.PropertyType.IsArray;
            }

            if (result)
            {
                result = !target.PropertyType.IsArray && !target.PropertyType.IsArray;
            }

            if (result)
            {
                result = source.PropertyType == target.PropertyType || source.PropertyType == Nullable.GetUnderlyingType(target.PropertyType)
                        || Nullable.GetUnderlyingType(source.PropertyType) == target.PropertyType;

                if (!result)
                {
                    result = source.PropertyType.IsEnum || target.PropertyType.IsEnum;
                }
            }

            return result;
        }
    }
}