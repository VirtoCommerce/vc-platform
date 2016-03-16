using System;
using System.Reflection;
using Omu.ValueInjecter;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class NullableAndEnumValueInjecter: ConventionInjection
    {
   
        protected override bool Match(ConventionInfo c)
        {
            return PropertyMatch(c.SourceProp, c.TargetProp);
        }

        protected override object SetValue(ConventionInfo c)
        {
            if (c.TargetProp.Type.IsEnum)
            {
                if (c.SourceProp.Value != null)
                {
                    var value = Enum.Parse(c.TargetProp.Type, c.SourceProp.Value.ToString(), true);
                    c.TargetProp.Value = value;
                }
            }

            if (c.TargetProp.Type == typeof(string) && c.SourceProp.Type.IsEnum)
            {
                c.TargetProp.Value = c.SourceProp.Value.ToString(); 
            }
            else
            {
                if (!c.TargetProp.Type.IsEnum)
                {
                    c.TargetProp.Value = c.SourceProp.Value;
                }
            }
            return c.TargetProp.Value;
        }
    

    //public class NullableAndEnumValueInjecter : ValueInjection
    //{
    //    protected override void Inject(object source, object target)
    //    {
    //        var sourceProps = source.GetProps();
    //        var targetType = target.GetType();

    //        foreach (var sourceProperty in sourceProps)
    //        {
    //            var targetProperty = targetType.GetProperty(sourceProperty.Name);
    //            if (targetProperty != null && PropertyMatch(sourceProperty, targetProperty))
    //            {

    //                if (targetProperty.PropertyType.IsEnum)
    //                {
    //                    var value = Enum.Parse(targetProperty.PropertyType, sourceProperty.GetValue(source).ToString(), true);
    //                    targetProperty.SetValue(target, value);
    //                }

    //                if (targetProperty.PropertyType == typeof(string) && sourceProperty.PropertyType.IsEnum)
    //                {
    //                    targetProperty.SetValue(target, sourceProperty.GetValue(source).ToString());
    //                }
    //                else
    //                {
    //                    if (!targetProperty.PropertyType.IsEnum)
    //                    {
    //                        targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
    //                    }
    //                }
    //            }
    //        }
    //    }

        private static bool PropertyMatch(ConventionInfo.PropInfo source, ConventionInfo.PropInfo target)
        {
            var result = string.Equals(source.Name, target.Name);
            if (result)
            {
                result = !source.Type.IsArray && !target.Type.IsArray;
            }
            if (result)
            {
                result = !target.Type.IsArray && !target.Type.IsArray;
            }

            if (result)
            {
                result = source.Type == target.Type || source.Type == Nullable.GetUnderlyingType(target.Type)
                        || Nullable.GetUnderlyingType(source.Type) == target.Type;

                if (!result)
                {
                    result = source.Type.IsEnum || target.Type.IsEnum;
                }
            }

            return result;
        }
    }
}