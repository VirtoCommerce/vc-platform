using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.DynamicProperties.Converters
{
    public static class DynamicPropertyDictionaryItemConverter
    {
        public static DynamicPropertyDictionaryItem ToModel(this DynamicPropertyDictionaryItemEntity entity)
        {
            var result = new DynamicPropertyDictionaryItem();
            result.InjectFrom(entity);

            result.DisplayNames = entity.DisplayNames.Select(v => v.ToModel()).ToArray();

            return result;
        }

        public static DynamicPropertyDictionaryItemEntity ToEntity(this DynamicPropertyDictionaryItem model, DynamicPropertyEntity property)
        {
            var result = new DynamicPropertyDictionaryItemEntity();
            result.InjectFrom(model);

            result.PropertyId = property.Id;

            if (model.DisplayNames != null)
                result.DisplayNames = new ObservableCollection<DynamicPropertyDictionaryItemNameEntity>(model.DisplayNames.Select(v => v.ToEntity()));

            return result;
        }

        public static void Patch(this DynamicPropertyDictionaryItemEntity source, DynamicPropertyDictionaryItemEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjectionPolicy = new PatchInjection<DynamicPropertyDictionaryItemEntity>(x => x.Name);
            target.InjectFrom(patchInjectionPolicy, source);

            if (!source.DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyDictionaryItemNameEntity v) => string.Join("-", v.Locale, v.Name));
                source.DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
