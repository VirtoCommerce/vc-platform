using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryItemConverter
    {
        public static DynamicPropertyDictionaryItemEntity ToEntity(this DynamicPropertyDictionaryItem model)
        {
            var result = new DynamicPropertyDictionaryItemEntity();

            if (!string.IsNullOrEmpty(model.Id))
                result.Id = model.Id;

            if (!string.IsNullOrEmpty(model.Name))
                result.Name = model.Name;

            if (model.DictionaryValues != null)
                result.DictionaryValues = new ObservableCollection<DynamicPropertyDictionaryValueEntity>(model.DictionaryValues.Select(v => v.ToEntity()));

            return result;
        }

        public static DynamicPropertyDictionaryItem ToModel(this DynamicPropertyDictionaryItemEntity entity)
        {
            var result = new DynamicPropertyDictionaryItem
            {
                Id = entity.Id,
                Name = entity.Name,
                DictionaryValues = entity.DictionaryValues.Select(v => v.ToModel()).ToArray(),
            };

            return result;
        }
    }
}
