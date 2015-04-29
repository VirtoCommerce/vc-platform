using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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

			var retVal = new module.Property();
			retVal.InjectFrom(dbProperty);
			retVal.Required = dbProperty.IsRequired;
			retVal.Multivalue = dbProperty.IsMultiValue;
			retVal.Multilanguage = dbProperty.IsLocaleDependant;
			retVal.Dictionary = dbProperty.IsEnum;
			retVal.ValueType = (module.PropertyValueType)dbProperty.PropertyValueType;
			retVal.CatalogId = catalog.Id;
			retVal.Catalog = catalog;
			retVal.CategoryId = category == null ? null : category.Id;
			retVal.Category = category;

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

			var retVal = new foundation.Property();
			retVal.InjectFrom(property);
			retVal.PropertyValueType = (int)property.ValueType;
			retVal.IsMultiValue = property.Multivalue;
			retVal.IsLocaleDependant = property.Multilanguage;
			retVal.IsEnum = property.Dictionary;
			retVal.IsRequired = property.Required;
			retVal.TargetType = property.Type.ToString();

			retVal.PropertyAttributes = new NullCollection<foundation.PropertyAttribute>();
			if (property.Attributes != null)
			{
				retVal.PropertyAttributes = new ObservableCollection<foundation.PropertyAttribute>();
				foreach (var attribute in property.Attributes)
				{
					var dbAttribute = attribute.ToFoundation();
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

			var patchInjectionPolicy = new PatchInjection<foundation.Property>(x => x.PropertyValueType, x => x.IsEnum, x => x.IsMultiValue, x => x.IsLocaleDependant,
																			   x => x.IsRequired, x => x.TargetType, x => x.Name);
			target.InjectFrom(patchInjectionPolicy, source);


			//Attributes patch
			if (!source.PropertyAttributes.IsNullCollection())
			{
				source.PropertyAttributes.Patch(target.PropertyAttributes, (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
			}
			//Property dict values
			if (!source.PropertyValues.IsNullCollection())
			{
				source.PropertyValues.Patch(target.PropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
		}

	}

}
