using System;
using System.Collections.Generic;
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
        public static coreModel.PropertyValue ToCoreModel(this dataModel.PropertyValueBase dbPropValue, coreModel.Property[] properties)
        {
            if (dbPropValue == null)
                throw new ArgumentNullException("dbPropValue");

			var retVal = new coreModel.PropertyValue();
			retVal.InjectFrom(dbPropValue);
			retVal.LanguageCode = dbPropValue.Locale;
			retVal.PropertyName = dbPropValue.Name;
			retVal.Value = dbPropValue.ToObjectValue();
			retVal.ValueId = dbPropValue.KeyValue;
			retVal.ValueType = (coreModel.PropertyValueType)dbPropValue.ValueType;

            if (properties != null)
            {
                var property = properties.FirstOrDefault(x => String.Equals(x.Name, dbPropValue.Name, StringComparison.InvariantCultureIgnoreCase));
                if (property != null)
                {
                    retVal.PropertyId = property.Id;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propValue">The property value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">propValue</exception>
        public static dataModel.PropertyValueBase ToDataModel<T>(this coreModel.PropertyValue propValue) where T : dataModel.PropertyValueBase, new()
        {
            if (propValue == null)
                throw new ArgumentNullException("propValue");

            var retVal = new T();
			var id = retVal.Id;
			retVal.InjectFrom(propValue);
			if(propValue.Id == null)
			{
				retVal.Id = id;
			}

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
        public static void Patch(this dataModel.PropertyValueBase source, dataModel.PropertyValueBase target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }


			var patchInjectionPolicy = new PatchInjection<dataModel.PropertyValueBase>(x => x.BooleanValue, x => x.DateTimeValue,
																				  x => x.DecimalValue, x => x.IntegerValue,
																				  x => x.KeyValue, x => x.LongTextValue, x => x.ShortTextValue);
			target.InjectFrom(patchInjectionPolicy, source);
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
