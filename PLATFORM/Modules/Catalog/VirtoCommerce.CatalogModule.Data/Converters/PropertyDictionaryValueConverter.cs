using System;
using System.Collections.Generic;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

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

            var retVal = new module.PropertyDictionaryValue
            {
                LanguageCode = dbPropValue.Locale,
                Value = dbPropValue.ToString(),
                PropertyId = property.Id,
                Property = property
            };
            if (dbPropValue.PropertyValueId != null)
                retVal.Id = dbPropValue.PropertyValueId;

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
            SetPropertyValue(retVal, property.ValueType.ToFoundation(), propDictValue.Value);

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
                SetPropertyValue(target, (foundation.PropertyValueType)target.ValueType, target.ToString());
            if (source.KeyValue != null)
                target.KeyValue = source.KeyValue;
        }

        private static void SetPropertyValue(foundation.PropertyValueBase retVal, foundation.PropertyValueType type, string value)
        {
            switch (type)
            {
                case foundation.PropertyValueType.LongString:
                    retVal.LongTextValue = value;
                    break;
                case foundation.PropertyValueType.ShortString:
                    retVal.ShortTextValue = value;
                    break;
                case foundation.PropertyValueType.Decimal:
                    decimal parsedDecimal;
                    Decimal.TryParse(value, out parsedDecimal);
                    retVal.DecimalValue = parsedDecimal;
                    break;
            }
        }

    }

    public class PropertyDictionaryValueComparer : IEqualityComparer<foundation.PropertyValue>
    {
        #region IEqualityComparer<CatalogLanguage> Members

        public bool Equals(foundation.PropertyValue x, foundation.PropertyValue y)
        {
            return x.PropertyValueId == y.PropertyValueId;
        }

        public int GetHashCode(foundation.PropertyValue obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }

}
