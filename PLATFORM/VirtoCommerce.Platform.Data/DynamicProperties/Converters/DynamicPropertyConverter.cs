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

            result.LocalizedNames = entity.Names.Select(n => n.ToModel()).ToArray();

            var groups = entity.Values
                .Where(v => v.ObjectId == objectId)
                .GroupBy(v => v.Locale)
                .ToArray();

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

            result.Values = localizedValues.ToArray();

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicProperty property, bool updateProperty)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var result = new DynamicPropertyEntity();
            result.InjectFrom(property);

            if (!string.IsNullOrEmpty(property.Id))
                result.Id = property.Id;

            if (property.ValueType != DynamicPropertyValueType.Undefined)
                result.ValueType = property.ValueType.ToString();

            if (updateProperty && property.LocalizedNames != null)
            {
                result.Names = new ObservableCollection<DynamicPropertyNameEntity>(property.LocalizedNames.Select(n => n.ToEntity()));
            }

            if (property.Values != null
                && (updateProperty && string.IsNullOrEmpty(property.ObjectId) || !updateProperty && !string.IsNullOrEmpty(property.ObjectId)))
            {
                result.Values = new ObservableCollection<DynamicPropertyValueEntity>(property.Values.SelectMany(v => v.ToEntity(property)));
            }

            return result;
        }

        public static void Patch(this DynamicPropertyEntity source, DynamicPropertyEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (!string.IsNullOrEmpty(source.Name))
            {
                target.Name = source.Name;
            }

            if (!source.Names.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity n) => string.Join("-", n.Locale, n.Name));
                source.Names.Patch(target.Names, comparer, (sourceItem, targetItem) => { });
            }

            if (!source.Values.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyValueEntity v) => string.Join("-", v.Locale, v.ToString(CultureInfo.InvariantCulture)));
                source.Values.Patch(target.Values, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
