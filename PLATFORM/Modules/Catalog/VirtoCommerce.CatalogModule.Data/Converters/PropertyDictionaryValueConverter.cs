using System;
using System.Collections.Generic;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyDictionaryValueConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static coreModel.PropertyDictionaryValue ToCoreModel(this dataModel.PropertyValue dbPropValue, coreModel.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

			var retVal = new coreModel.PropertyDictionaryValue();
			retVal.InjectFrom(dbPropValue);

			retVal.LanguageCode = dbPropValue.Locale;
			retVal.Value = dbPropValue.ToString();
			retVal.PropertyId = property.Id;
			retVal.Property = property;
          
            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static dataModel.PropertyValue ToDataModel(this coreModel.PropertyDictionaryValue propDictValue, coreModel.Property property)
        {
            var retVal = new dataModel.PropertyValue
            {
	            Locale = propDictValue.LanguageCode,
                PropertyId = property.Id
            };
			retVal.InjectFrom(propDictValue);

			if(propDictValue.Id != null)
			{
				retVal.Id = propDictValue.Id;
			}
            SetPropertyValue(retVal, property.ValueType, propDictValue.Value);

            return retVal;
        }


        /// <summary>
        /// Patch CatalogLanguage type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.PropertyValue source, dataModel.PropertyValue target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var newValue = target.ToString();
            if (newValue != null)
                SetPropertyValue(target, (coreModel.PropertyValueType)target.ValueType, target.ToString());
            if (source.KeyValue != null)
                target.KeyValue = source.KeyValue;
        }

		private static void SetPropertyValue(dataModel.PropertyValueBase retVal, coreModel.PropertyValueType type, string value)
        {
            switch (type)
            {
				case coreModel.PropertyValueType.LongText:
                    retVal.LongTextValue = value;
                    break;
				case coreModel.PropertyValueType.ShortText:
                    retVal.ShortTextValue = value;
                    break;
				case coreModel.PropertyValueType.Number:
                    decimal parsedDecimal;
                    Decimal.TryParse(value, out parsedDecimal);
                    retVal.DecimalValue = parsedDecimal;
                    break;
            }
        }

    }

   
}
