using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
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
		public static coreModel.Property ToCoreModel(this dataModel.Property dbProperty, coreModel.Catalog catalog, coreModel.Category category)
		{
			if (dbProperty == null)
				throw new ArgumentNullException("dbProperty");

			var retVal = new coreModel.Property();
			retVal.InjectFrom(dbProperty);
			retVal.Required = dbProperty.IsRequired;
			retVal.Multivalue = dbProperty.IsMultiValue;
			retVal.Multilanguage = dbProperty.IsLocaleDependant;
			retVal.Dictionary = dbProperty.IsEnum;
			retVal.ValueType = (coreModel.PropertyValueType)dbProperty.PropertyValueType;
			retVal.CatalogId = catalog.Id;
			retVal.Catalog = catalog;
			retVal.CategoryId = category == null ? null : category.Id;
			retVal.Category = category;

			coreModel.PropertyType propertyType;
			if (!string.IsNullOrEmpty(dbProperty.TargetType) && Enum.TryParse(dbProperty.TargetType, out propertyType))
			{
				retVal.Type = propertyType;
			}

			if (dbProperty.PropertyAttributes != null)
			{
				retVal.Attributes = new List<coreModel.PropertyAttribute>();
				retVal.Attributes.AddRange(dbProperty.PropertyAttributes.Select(x => x.ToCoreModel(retVal)));
			}
			if (dbProperty.PropertyValues != null)
			{
				retVal.DictionaryValues = new List<coreModel.PropertyDictionaryValue>();
				retVal.DictionaryValues.AddRange(dbProperty.PropertyValues.Select(x => x.ToCoreModel(retVal)));
			}

			return retVal;
		}


		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static dataModel.Property ToDataModel(this coreModel.Property property)
		{
			if (property == null)
				throw new ArgumentNullException("property");

			var retVal = new dataModel.Property();
			var id = retVal.Id;
			retVal.InjectFrom(property);

			if(property.Id == null)
			{
				retVal.Id = id;
			}
			retVal.PropertyValueType = (int)property.ValueType;
			retVal.IsMultiValue = property.Multivalue;
			retVal.IsLocaleDependant = property.Multilanguage;
			retVal.IsEnum = property.Dictionary;
			retVal.IsRequired = property.Required;
			retVal.TargetType = property.Type.ToString();

			retVal.PropertyAttributes = new NullCollection<dataModel.PropertyAttribute>();
			if (property.Attributes != null)
			{
				retVal.PropertyAttributes = new ObservableCollection<dataModel.PropertyAttribute>();
				foreach (var attribute in property.Attributes)
				{
					var dbAttribute = attribute.ToDataModel();
					retVal.PropertyAttributes.Add(dbAttribute);
				}
			}

			retVal.PropertyValues = new NullCollection<dataModel.PropertyValue>();
			if (property.DictionaryValues != null)
			{
				retVal.PropertyValues = new ObservableCollection<dataModel.PropertyValue>();
				foreach (var dictValue in property.DictionaryValues)
				{
					var dbDictValue = dictValue.ToDataModel(property);
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
		public static void Patch(this dataModel.Property source, dataModel.Property target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.Property>(x => x.PropertyValueType, x => x.IsEnum, x => x.IsMultiValue, x => x.IsLocaleDependant,
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
