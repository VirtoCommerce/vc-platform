using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static module.Property ToModuleModel(this foundation.Property dbProperty, module.Catalog catalog, module.Category category)
        {
            if (dbProperty == null)
                throw new ArgumentNullException("dbProperty");

            var retVal = new module.Property
            {
                Id = dbProperty.PropertyId,
                Required = dbProperty.IsRequired,
                Name = dbProperty.Name,
                Multivalue = dbProperty.IsMultiValue,
                Dictionary = dbProperty.IsEnum,
                Multilanguage = dbProperty.IsLocaleDependant,
                ValueType = ((foundation.PropertyValueType)dbProperty.PropertyValueType).ToModuleModel(),
                CatalogId = catalog.Id,
                Catalog = catalog,
                CategoryId = category == null ? null : category.Id,
                Category = category,
            };
            module.PropertyType propertyType;
            if (!string.IsNullOrEmpty(dbProperty.TargetType) && Enum.TryParse(dbProperty.TargetType, out propertyType))
            {
                retVal.Type = propertyType;
            }

            if (dbProperty.PropertyAttributes != null)
            {
                retVal.Attributes = new List<module.PropertyAttribute>();
                retVal.Attributes.AddRange(dbProperty.PropertyAttributes.Select(x => x.ToModuleModel(retVal)));
            }
            if (dbProperty.PropertyValues != null)
            {
                retVal.DictionaryValues = new List<module.PropertyDictionaryValue>();
                retVal.DictionaryValues.AddRange(dbProperty.PropertyValues.Select(x => x.ToModuleModel(retVal)));
            }

            return retVal;
        }


        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static foundation.Property ToFoundation(this module.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var retVal = new foundation.Property
            {
                PropertyValueType = (int)property.ValueType.ToFoundation(),
                CatalogId = property.CatalogId,
                IsMultiValue = property.Multivalue,
                IsLocaleDependant = property.Multilanguage,
                IsEnum = property.Dictionary,
                IsRequired = property.Required,
                TargetType = property.Type.ToString(),
                Name = property.Name,
            };

            if (property.Id != null)
                retVal.PropertyId = property.Id;

			retVal.PropertyAttributes = new NullCollection<foundation.PropertyAttribute>();
            if (property.Attributes != null)
            {
				retVal.PropertyAttributes = new ObservableCollection<foundation.PropertyAttribute>();
                foreach (var attribute in property.Attributes)
                {
                    var dbAttribute = attribute.ToFoundation();
                    dbAttribute.PropertyId = retVal.PropertyId;
                    retVal.PropertyAttributes.Add(dbAttribute);
                }
            }

			retVal.PropertyValues = new NullCollection<foundation.PropertyValue>();
            if (property.DictionaryValues != null)
            {
				retVal.PropertyValues = new ObservableCollection<foundation.PropertyValue>();
                foreach (var dictValue in property.DictionaryValues)
                {
                    var dbDictValue = dictValue.ToFoundation(property);
                    retVal.PropertyValues.Add(dbDictValue);
                }
            }

            return retVal;

        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundation.Property source, foundation.Property target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            target.PropertyValueType = source.PropertyValueType;
            target.IsEnum = source.IsEnum;
            target.IsMultiValue = source.IsMultiValue;
            target.IsLocaleDependant = source.IsLocaleDependant;
            target.IsEnum = source.IsEnum;
            target.IsRequired = source.IsRequired;
            target.TargetType = source.TargetType;
            if (source.Name != null)
                target.Name = source.Name;

            //Attributes patch
            if (!source.PropertyAttributes.IsNullCollection())
            {
                source.PropertyAttributes.Patch(target.PropertyAttributes, new PropertyAttributeComparer(),
                                         (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
            }
            //Property dict values
            if (!source.PropertyValues.IsNullCollection())
            {
                source.PropertyValues.Patch(target.PropertyValues, new PropertyDictionaryValueComparer(),
                                         (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
            }
        }

    }

    public class ItemPropertyComparer : IEqualityComparer<foundation.Property>
    {
        #region IEqualityComparer<Item> Members

        public bool Equals(foundation.Property x, foundation.Property y)
        {
            return x.PropertyId == y.PropertyId;
        }

        public int GetHashCode(foundation.Property obj)
        {
			return obj.PropertyId.GetHashCode();
        }

        #endregion
    }
}
