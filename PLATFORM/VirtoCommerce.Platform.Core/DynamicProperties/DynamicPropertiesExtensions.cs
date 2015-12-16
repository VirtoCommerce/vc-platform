using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public static class DynamicPropertiesExtensions
    {
        public static T GetDynamicPropertyValue<T>(this IHasDynamicProperties owner, string propertyName, T defaultValue)
        {
            var result = defaultValue;

            if (owner != null && owner.DynamicProperties != null)
            {
                var propValue = owner.DynamicProperties.Where(v => v.Name == propertyName && v.Values != null)
                                                       .SelectMany(v => v.Values)
                                                       .FirstOrDefault();

                if (propValue != null && propValue.Value != null)
                {
                    var jObject = propValue.Value as JObject;
                    var dictItem = propValue.Value as DynamicPropertyDictionaryItem;
                    if (jObject != null)
                    {
                        dictItem = jObject.ToObject<DynamicPropertyDictionaryItem>();
                    }
                    if (dictItem != null)
                    {
                        result = (T)(object)dictItem.Name;
                    }
                    else
                    {
                        result = (T)propValue.Value;
                    }
                }
            }

            return result;
        }

        public static void DeepCopyPropertyValues(this IHasDynamicProperties sourceOwner, IHasDynamicProperties targetOwner)
        {
            if(sourceOwner == null)
            {
                throw new ArgumentNullException("sourceOwner");
            }
            if (targetOwner == null)
            {
                throw new ArgumentNullException("targetOwner");
            }

            var comparer = AnonymousComparer.Create((DynamicProperty x) => x.Name.ToLowerInvariant() + ":" + x.ValueType.ToString());
            var allExpandedSourceProps = sourceOwner.GetFlatObjectsListWithInterface<IHasDynamicProperties>().SelectMany(x => x.DynamicProperties).ToList();
            var allExpandedTargetProps = targetOwner.GetFlatObjectsListWithInterface<IHasDynamicProperties>().SelectMany(x => x.DynamicProperties).ToList();
            //Copy  property values for same proeprties  from one object to other 
            allExpandedSourceProps.CompareTo(allExpandedTargetProps, comparer, (state, sourceProp, targetProp) =>
            {
                if (state == EntryState.Modified)
                {
                    targetProp.Values = sourceProp.Values;
                }
            });
        }
    }
}
