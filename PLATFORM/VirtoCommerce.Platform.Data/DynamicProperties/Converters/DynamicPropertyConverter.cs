using System;
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
        public static DynamicPropertyObjectValue[] ToModel(this DynamicPropertyEntity entity, string objectId)
        {
            var property = entity.ToModel();

            var groups = entity.ObjectValues
                .Where(v => v.ObjectId == objectId)
                .GroupBy(v => v.Locale)
                .ToArray();

            var result = (from @group in groups
                          select new DynamicPropertyObjectValue
                          {
                              Property = property,
                              ObjectId = objectId,
                              Locale = @group.Key,
                              Values = @group.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray(),
                          }).ToArray();

            return result;
        }

        public static DynamicProperty ToModel(this DynamicPropertyEntity entity)
        {
            var result = new DynamicProperty();
            result.InjectFrom(entity);

            result.ValueType = EnumUtility.SafeParse(entity.ValueType, DynamicPropertyValueType.Undefined);

            if (entity.IsMultilingual)
                result.DisplayNames = entity.DisplayNames.Select(n => n.ToModel()).ToArray();

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicProperty model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var result = new DynamicPropertyEntity();

            if (!string.IsNullOrEmpty(model.Id))
                result.Id = model.Id;

            if (model.ObjectType != null)
                result.ObjectType = model.ObjectType;

            if (model.Name != null)
                result.Name = model.Name;

            result.IsArray = model.IsArray;
            result.IsDictionary = model.IsDictionary;
            result.IsMultilingual = model.IsMultilingual;
            result.IsRequired = model.IsRequired;

            if (model.ValueType != DynamicPropertyValueType.Undefined)
                result.ValueType = model.ValueType.ToString();

            if (model.IsMultilingual && model.DisplayNames != null)
                result.DisplayNames = new ObservableCollection<DynamicPropertyNameEntity>(model.DisplayNames.Select(n => n.ToEntity()));

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

            if (target.IsMultilingual && !source.DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity n) => string.Join("-", n.Locale, n.Name));
                source.DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
