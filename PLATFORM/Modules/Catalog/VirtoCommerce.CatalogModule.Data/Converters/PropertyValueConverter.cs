using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyValueConverter
    {

        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbPropValue">The database property value.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dbPropValue</exception>
        public static coreModel.PropertyValue ToCoreModel(this dataModel.PropertyValue dbPropValue)
        {
            if (dbPropValue == null)
                throw new ArgumentNullException("dbPropValue");

            var retVal = new coreModel.PropertyValue();
            retVal.InjectFrom(dbPropValue);
            retVal.LanguageCode = dbPropValue.Locale;
            retVal.PropertyName = dbPropValue.Name;
            retVal.Value = GetPropertyValue(dbPropValue);
            retVal.ValueId = dbPropValue.KeyValue;
            retVal.ValueType = (coreModel.PropertyValueType)dbPropValue.ValueType;
            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propValue">The property value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">propValue</exception>
        public static dataModel.PropertyValue ToDataModel(this coreModel.PropertyValue propValue, PrimaryKeyResolvingMap pkMap) 
        {
            if (propValue == null)
                throw new ArgumentNullException("propValue");

            var retVal = new dataModel.PropertyValue();
            pkMap.AddPair(propValue, retVal);
            retVal.InjectFrom(propValue);
   
            retVal.Name = propValue.PropertyName;
            retVal.KeyValue = propValue.ValueId;
            retVal.Locale = propValue.LanguageCode;
            retVal.ValueType = (int)propValue.ValueType;
            SetPropertyValue(retVal, propValue.ValueType, propValue.Value.ToString());

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.PropertyValue source, dataModel.PropertyValue target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }


            var patchInjectionPolicy = new PatchInjection<dataModel.PropertyValue>(x => x.BooleanValue, x => x.DateTimeValue,
                                                                                  x => x.DecimalValue, x => x.IntegerValue,
                                                                                  x => x.KeyValue, x => x.LongTextValue, x => x.ShortTextValue);
            target.InjectFrom(patchInjectionPolicy, source);
        }

        private static object GetPropertyValue(dataModel.PropertyValue propertyValue)
        {
            
            switch (propertyValue.ValueType)
            {
                case (int)coreModel.PropertyValueType.Boolean:
                    return propertyValue.BooleanValue;
                case (int)coreModel.PropertyValueType.DateTime:
                    return propertyValue.DateTimeValue;
                case (int)coreModel.PropertyValueType.Number:
                    return propertyValue.DecimalValue;
                case (int)coreModel.PropertyValueType.LongText:
                    return propertyValue.LongTextValue;
                default:
                    return propertyValue.ShortTextValue;
            }

    }

        private static void SetPropertyValue(dataModel.PropertyValue retVal, coreModel.PropertyValueType type, string value)
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
                    Decimal.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out parsedDecimal);
                    retVal.DecimalValue = parsedDecimal;
                    break;
                case coreModel.PropertyValueType.DateTime:
                    retVal.DateTimeValue = DateTime.Parse(value);
                    break;
                case coreModel.PropertyValueType.Boolean:
                    retVal.BooleanValue = Boolean.Parse(value);
                    break;
            }
        }
    }




}
