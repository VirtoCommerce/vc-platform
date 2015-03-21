using System;
using System.Collections.Generic;
using System.Linq;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using Omu.ValueInjecter;

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
        public static module.PropertyValue ToModuleModel(this foundation.PropertyValueBase dbPropValue, module.Property[] properties)
        {
            if (dbPropValue == null)
                throw new ArgumentNullException("dbPropValue");

            var retVal = new module.PropertyValue
            {
                Id = dbPropValue.PropertyValueId,
                LanguageCode = dbPropValue.Locale,
                PropertyName = dbPropValue.Name,
                Value = dbPropValue.ToObjectValue(), // retain the correct object type value
                ValueId = dbPropValue.KeyValue,
                ValueType = ((foundation.PropertyValueType)dbPropValue.ValueType).ToModuleModel()
            };

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
        public static foundation.PropertyValueBase ToFoundation<T>(this module.PropertyValue propValue) where T : foundation.PropertyValueBase, new()
        {
            if (propValue == null)
                throw new ArgumentNullException("propValue");

            var retVal = new T();

            if (propValue.Id != null)
                retVal.PropertyValueId = propValue.Id;

            retVal.Name = propValue.PropertyName;
            retVal.KeyValue = propValue.ValueId;
            retVal.Locale = propValue.LanguageCode;
            retVal.ValueType = (int)propValue.ValueType.ToFoundation();
            SetPropertyValue(retVal, propValue.ValueType.ToFoundation(), propValue.Value.ToString());

            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundation.PropertyValueBase source, foundation.PropertyValueBase target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }


			var patchInjectionPolicy = new PatchInjection<foundation.PropertyValueBase>(x => x.BooleanValue, x => x.DateTimeValue,
																				  x => x.DecimalValue, x => x.IntegerValue,
																				  x => x.KeyValue, x => x.LongTextValue, x => x.ShortTextValue);
			target.InjectFrom(patchInjectionPolicy, source);
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
				case foundation.PropertyValueType.DateTime:
					retVal.DateTimeValue = DateTime.Parse(value);
					break;
				case foundation.PropertyValueType.Boolean:
					retVal.BooleanValue = Boolean.Parse(value);
					break;
			}
		}
    }



    public class PropertyValueComparer : IEqualityComparer<foundation.PropertyValueBase>
    {
        #region IEqualityComparer<Item> Members

        public bool Equals(foundation.PropertyValueBase x, foundation.PropertyValueBase y)
        {
            return x.PropertyValueId == y.PropertyValueId;
        }

        public int GetHashCode(foundation.PropertyValueBase obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
