using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyConverter
    {
        public static DynamicProperty ToModel(this DynamicPropertyEntity entity, string objectId)
        {
            var result = new DynamicProperty();

            result.InjectFrom(entity);
            result.ValueType = EnumUtility.SafeParse(entity.ValueType, DynamicPropertyValueType.Undefined);
            result.ObjectId = objectId;

            if (entity.IsLocaleDependent)
            {
                result.LocalizedNames = entity.Names.Select(n => n.ToModel()).ToArray();

                var groups = entity.Values
                    .Where(v => v.ObjectId == objectId)
                    .Where(v => !string.IsNullOrEmpty(v.Locale))
                    .GroupBy(v => v.Locale)
                    ;

                var localizedValues = new List<DynamicPropertyValue>();

                foreach (var group in groups)
                {
                    var values = group.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray();
                    var localizedValue = new DynamicPropertyValue { Locale = group.Key };

                    if (entity.IsArray)
                    {
                        localizedValue.ArrayValues = values;
                    }
                    else
                    {
                        localizedValue.Value = values.FirstOrDefault();
                    }

                    localizedValues.Add(localizedValue);
                }

                result.LocalizedValues = localizedValues.ToArray();
            }
            else
            {
                var values = entity.Values
                    .Where(v => v.ObjectId == objectId)
                    .Select(v => v.ToString(CultureInfo.InvariantCulture))
                    .ToArray();

                if (entity.IsArray)
                {
                    result.ArrayValues = values;
                }
                else
                {
                    result.Value = values.FirstOrDefault();
                }
            }

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicProperty property, bool saveLocalizedNames, bool saveObjectValues)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var result = new DynamicPropertyEntity();
            result.InjectFrom(property);
            result.SearchKey = string.Join("-", property.ObjectType, property.Name);
            result.ValueType = property.ValueType.ToString();

            if (saveLocalizedNames && property.IsLocaleDependent && property.LocalizedNames != null)
            {
                result.Names = new ObservableCollection<DynamicPropertyNameEntity>(property.LocalizedNames.Select(n => n.ToEntity()));
            }

            if (saveObjectValues && !string.IsNullOrEmpty(property.ObjectId)
                || !saveObjectValues && string.IsNullOrEmpty(property.ObjectId))
            {
                var values = new List<DynamicPropertyValue>();

                if (property.IsLocaleDependent)
                {
                    if (property.LocalizedValues != null)
                    {
                        values.AddRange(property.LocalizedValues);
                    }
                }
                else
                {
                    if (property.ArrayValues != null)
                    {
                        values.Add(new DynamicPropertyValue { ArrayValues = property.ArrayValues });
                    }
                    else if (property.Value != null)
                    {
                        values.Add(new DynamicPropertyValue { Value = property.Value });
                    }
                }

                result.Values = new ObservableCollection<DynamicPropertyValueEntity>(values.SelectMany(v => v.ToEntity(property)));
            }

            return result;
        }

        public static void Patch(this DynamicPropertyEntity source, DynamicPropertyEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (!source.Values.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyValueEntity v) => string.Join("-", v.Locale, v.ToString(CultureInfo.InvariantCulture)));
                source.Values.Patch(target.Values, comparer, (sourceItem, targetItem) => { });
            }

            if (!source.Names.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity n) => string.Join("-", n.Locale, n.Name));
                source.Names.Patch(target.Names, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
