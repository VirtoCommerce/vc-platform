using System;
using System.Collections.Generic;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
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
        public static module.PropertyDictionaryValue ToModuleModel(this foundation.PropertyValue dbPropValue, module.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

			var retVal = new module.PropertyDictionaryValue();
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
        public static foundation.PropertyValue ToFoundation(this module.PropertyDictionaryValue propDictValue, module.Property property)
        {
            var retVal = new foundation.PropertyValue
            {
                Locale = propDictValue.LanguageCode,
                PropertyId = property.Id,
            };
            SetPropertyValue(retVal, property.ValueType, propDictValue.Value);

            return retVal;
        }


        /// <summary>
        /// Patch CatalogLanguage type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundation.PropertyValue source, foundation.PropertyValue target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var newValue = target.ToString();
            if (newValue != null)
                SetPropertyValue(target, (module.PropertyValueType)target.ValueType, target.ToString());
            if (source.KeyValue != null)
                target.KeyValue = source.KeyValue;
        }

		private static void SetPropertyValue(foundation.PropertyValueBase retVal, module.PropertyValueType type, string value)
        {
            switch (type)
            {
				case module.PropertyValueType.LongText:
                    retVal.LongTextValue = value;
                    break;
				case module.PropertyValueType.ShortText:
                    retVal.ShortTextValue = value;
                    break;
				case module.PropertyValueType.Number:
                    decimal parsedDecimal;
                    Decimal.TryParse(value, out parsedDecimal);
                    retVal.DecimalValue = parsedDecimal;
                    break;
            }
        }

    }

   
}
