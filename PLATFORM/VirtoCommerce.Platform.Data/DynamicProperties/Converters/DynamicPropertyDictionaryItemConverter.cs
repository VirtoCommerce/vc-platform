using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryItemConverter
    {
        public static DynamicPropertyDictionaryItemEntity ToEntity(this DynamicPropertyDictionaryItem model, DynamicPropertyEntity property)
        {
            var result = new DynamicPropertyDictionaryItemEntity();

            if (!string.IsNullOrEmpty(model.Id))
                result.Id = model.Id;

            if (!string.IsNullOrEmpty(model.Name))
                result.Name = model.Name;

            if (!string.IsNullOrEmpty(property.Id))
                result.PropertyId = property.Id;

            if (property.IsMultilingual && model.DisplayNames != null)
                result.DisplayNames = new ObservableCollection<DynamicPropertyDictionaryItemNameEntity>(model.DisplayNames.Select(v => v.ToEntity()));

            return result;
        }

        public static DynamicPropertyDictionaryItem ToModel(this DynamicPropertyDictionaryItemEntity entity)
        {
            var result = new DynamicPropertyDictionaryItem
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayNames = entity.DisplayNames.Select(v => v.ToModel()).ToArray(),
            };

            return result;
        }

        public static void Patch(this DynamicPropertyDictionaryItemEntity source, DynamicPropertyDictionaryItemEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (!string.IsNullOrEmpty(source.Name))
            {
                target.Name = source.Name;
            }

            if (target.Property.IsMultilingual && !source.DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyDictionaryItemNameEntity v) => string.Join("-", v.Locale, v.Name));
                source.DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
