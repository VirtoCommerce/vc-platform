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
    public static class DynamicPropertyConverter
    {
        public static DynamicObjectProperty ToDynamicObjectProperty(this DynamicPropertyEntity entity, string objectId)
        {
            var retVal = new DynamicObjectProperty();
            retVal.InjectFrom(entity);
            retVal.ObjectId = objectId;
            retVal.ValueType = EnumUtility.SafeParse(entity.ValueType, DynamicPropertyValueType.Undefined);
            retVal.DisplayNames = entity.DisplayNames.Select(x => x.ToModel()).ToArray();
            retVal.Values = entity.ObjectValues.Where(x => x.ObjectId == objectId).Select(x => x.ToModel()).ToArray();
            return retVal;
        }

        public static DynamicProperty ToModel(this DynamicPropertyEntity entity)
        {
            var result = new DynamicProperty();
            result.InjectFrom(entity);

            result.ValueType = EnumUtility.SafeParse(entity.ValueType, DynamicPropertyValueType.Undefined);

            result.DisplayNames = entity.DisplayNames.Select(n => n.ToModel()).ToArray();

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicProperty model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var result = new DynamicPropertyEntity();
            result.InjectFrom(model);

            if (model.ValueType != DynamicPropertyValueType.Undefined)
                result.ValueType = model.ValueType.ToString();

            if (model.DisplayNames != null)
                result.DisplayNames = new ObservableCollection<DynamicPropertyNameEntity>(model.DisplayNames.Select(n => n.ToEntity()));

            return result;
        }

        public static DynamicPropertyEntity ToEntity(this DynamicObjectProperty model, string objectId, string objectType)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var result = model.ToEntity();
            result.DisplayNames = new NullCollection<DynamicPropertyNameEntity>();
            result.ObjectValues = new ObservableCollection<DynamicPropertyObjectValueEntity>(model.Values.Select(x => x.ToEntity(model, objectId, objectType)));

            return result;
        }

        public static void Patch(this DynamicPropertyEntity source, DynamicPropertyEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjectionPolicy = new PatchInjection<DynamicPropertyEntity>(x => x.Name, x => x.IsRequired, x => x.IsArray);
            target.InjectFrom(patchInjectionPolicy, source);

            if (!source.DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity x) => string.Join("-", x.Locale, x.Name));
                source.DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }

            if (!source.ObjectValues.IsNullCollection())
            {
                source.ObjectValues.Patch(target.ObjectValues, new DynamicPropertyObjectValueComparer(), (sourceValue, targetValue) => sourceValue.Patch(targetValue));
            }
        }
    }
}
