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

            result.DisplayNames = entity.DisplayNames.Select(n => n.ToModel()).ToArray();

            result.DictionaryItems = entity.DictionaryItems.Select(i => i.ToModel()).ToArray();

            var objectValues = new List<DynamicPropertyObjectValue>();

            var groups = entity.ObjectValues
                .Where(v => v.ObjectId == objectId)
                .GroupBy(v => v.Locale)
                .ToArray();

            foreach (var group in groups)
            {
                var stringValues = group.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray();
                var objectValue = new DynamicPropertyObjectValue { Locale = group.Key };

                if (entity.IsDictionary)
                {
                    objectValue.DictionaryItemId = stringValues.FirstOrDefault();
                }
                else if (entity.IsArray)
                {
                    objectValue.ArrayValues = stringValues;
                }
                else
                {
                    objectValue.Value = stringValues.FirstOrDefault();
                }

                objectValues.Add(objectValue);
            }

            result.ObjectValues = objectValues.ToArray();

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicProperty model, bool updateProperty)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var result = new DynamicPropertyEntity();

            if (!string.IsNullOrEmpty(model.Id))
                result.Id = model.Id;

            if (updateProperty)
            {
                if (model.ObjectType != null)
                    result.ObjectType = model.ObjectType;

                if (model.Name != null)
                    result.Name = model.Name;

                result.IsArray = model.IsArray;
                result.IsDictionary = model.IsDictionary;

                if (model.ValueType != DynamicPropertyValueType.Undefined)
                    result.ValueType = model.ValueType.ToString();

                if (model.DisplayNames != null)
                    result.DisplayNames = new ObservableCollection<DynamicPropertyNameEntity>(model.DisplayNames.Select(n => n.ToEntity()));

                if (model.DictionaryItems != null)
                {
                    result.DictionaryItems = new ObservableCollection<DynamicPropertyDictionaryItemEntity>(model.DictionaryItems.Select(i => i.ToEntity()));
                }
            }

            if (model.ObjectValues != null
                && (updateProperty && string.IsNullOrEmpty(model.ObjectId) || !updateProperty && !string.IsNullOrEmpty(model.ObjectId)))
            {
                result.ObjectValues = new ObservableCollection<DynamicPropertyObjectValueEntity>(model.ObjectValues.SelectMany(v => v.ToEntity(model)));
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

            if (!source.DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity n) => string.Join("-", n.Locale, n.Name));
                source.DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }

            if (!source.ObjectValues.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyObjectValueEntity v) => string.Join("-", v.Locale, v.ToString(CultureInfo.InvariantCulture)));
                source.ObjectValues.Patch(target.ObjectValues, comparer, (sourceItem, targetItem) => { });

                foreach (var objectValue in target.ObjectValues)
                {
                    objectValue.ObjectType = target.ObjectType;
                    objectValue.ValueType = target.ValueType;
                }
            }
        }
    }
}
