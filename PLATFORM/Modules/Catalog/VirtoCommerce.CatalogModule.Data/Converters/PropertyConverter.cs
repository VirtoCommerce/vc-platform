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
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class PropertyConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Property ToCoreModel(this dataModel.Property dbProperty)
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
			retVal.Catalog = dbProperty.Catalog.ToCoreModel(convertProps: false);
			retVal.Category = dbProperty.Category != null ? dbProperty.Category.ToCoreModel(convertProps: false) : null;

			coreModel.PropertyType propertyType;
			if (!string.IsNullOrEmpty(dbProperty.TargetType) && Enum.TryParse(dbProperty.TargetType, out propertyType))
			{
				retVal.Type = propertyType;
			}

			retVal.DisplayNames = retVal.Catalog.Languages.Select(x => new PropertyDisplayName { LanguageCode = x.LanguageCode }).ToList();
			if (dbProperty.PropertyAttributes != null)
			{
				retVal.Attributes = new List<coreModel.PropertyAttribute>();
				retVal.Attributes.AddRange(dbProperty.PropertyAttributes.Select(x => x.ToCoreModel(retVal)));

				//Load display names from attributes
				foreach (var displayNameAttribute in retVal.Attributes.Where(x => x.Name.StartsWith("DisplayName")))
				{
					var languageCode = displayNameAttribute.Name.Substring("DisplayName".Length);
					var displayName = retVal.DisplayNames.FirstOrDefault(x => String.Equals(x.LanguageCode, languageCode, StringComparison.InvariantCultureIgnoreCase));
					if(displayName != null)
					{
						displayName.Name = displayNameAttribute.Value;
					}
				}
			
			}

			if (dbProperty.DictionaryValues != null)
			{
				retVal.DictionaryValues = new List<coreModel.PropertyDictionaryValue>();
				retVal.DictionaryValues.AddRange(dbProperty.DictionaryValues.Select(x => x.ToCoreModel()));
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
		
			retVal.InjectFrom(property);

            retVal.PropertyValueType = (int)property.ValueType;
			retVal.IsMultiValue = property.Multivalue;
			retVal.IsLocaleDependant = property.Multilanguage;
			retVal.IsEnum = property.Dictionary;
			retVal.IsRequired = property.Required;
			retVal.TargetType = property.Type.ToString();

			if (property.Attributes != null)
			{
				retVal.PropertyAttributes = new ObservableCollection<dataModel.PropertyAttribute>();
				foreach (var attribute in property.Attributes)
				{
					var dbAttribute = attribute.ToDataModel();
					retVal.PropertyAttributes.Add(dbAttribute);
				}
			}

			if (property.DictionaryValues != null)
			{
				retVal.DictionaryValues = new ObservableCollection<dataModel.PropertyDictionaryValue>();
				foreach (var dictValue in property.DictionaryValues)
				{
					var dbDictValue = dictValue.ToDataModel();
					retVal.DictionaryValues.Add(dbDictValue);
				}
			}

			if (property.DisplayNames != null)
			{
				foreach (var displayName in property.DisplayNames.Where(x=> !String.IsNullOrEmpty(x.Name)))
				{
					var attributeName = "DisplayName" + displayName.LanguageCode;
					var existAttribute = retVal.PropertyAttributes.FirstOrDefault(x => x.PropertyAttributeName == attributeName);
					if(existAttribute == null)
					{
						existAttribute = new dataModel.PropertyAttribute
						{
                            PropertyId = property.Id,
							PropertyAttributeName = attributeName,
						};
						retVal.PropertyAttributes.Add(existAttribute);
					}
					existAttribute.PropertyAttributeValue = displayName.Name;
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
				var attributeComparer = AnonymousComparer.Create((dataModel.PropertyAttribute x) => x.IsTransient() ?  x.PropertyAttributeName : x.Id);
				source.PropertyAttributes.Patch(target.PropertyAttributes, attributeComparer, (sourceAsset, targetAsset) => sourceAsset.Patch(targetAsset));
			}
			//Property dict values
			if (!source.DictionaryValues.IsNullCollection())
			{
				source.DictionaryValues.Patch(target.DictionaryValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}
		}

	}

}
