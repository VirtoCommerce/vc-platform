using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
    public static class PropertyConverter
    {
        public static coreModel.Property ToCoreModel(this foundationModel.ContactPropertyValue entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var retVal = new coreModel.Property();
            retVal.InjectFrom(entity);
            retVal.ValueType = (coreModel.PropertyValueType)entity.ValueType;
            retVal.Value = GetPropertyValue(entity);

            return retVal;
        }

        public static foundationModel.ContactPropertyValue ToFoundation(this coreModel.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var retVal = new foundationModel.ContactPropertyValue();
            retVal.InjectFrom(property);
            retVal.ValueType = (int)property.ValueType;
            SetPropertyValue(retVal, property);
            return retVal;
        }


        /// <summary>
        /// Patch 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this foundationModel.ContactPropertyValue source, foundationModel.ContactPropertyValue target)
        {
            var patchInjectionPolicy = new PatchInjection<foundationModel.ContactPropertyValue>(x => x.BooleanValue, x => x.DateTimeValue,
                                                                                          x => x.DecimalValue, x => x.IntegerValue,
                                                                                          x => x.ShortTextValue, x => x.LongTextValue, x => x.ValueType);
            target.InjectFrom(patchInjectionPolicy, source);
        }

        private static object GetPropertyValue(foundationModel.ContactPropertyValue propValue)
        {
            object retVal = null;
            switch ((coreModel.PropertyValueType)propValue.ValueType)
            {
                case coreModel.PropertyValueType.Boolean:
                    retVal = propValue.BooleanValue;
                    break;
                case coreModel.PropertyValueType.DateTime:
                    retVal = propValue.DateTimeValue;
                    break;
                case coreModel.PropertyValueType.Decimal:
                    retVal = propValue.DecimalValue;
                    break;
                case coreModel.PropertyValueType.Integer:
                    retVal = propValue.IntegerValue;
                    break;
                case coreModel.PropertyValueType.LongText:
                    retVal = propValue.LongTextValue;
                    break;
                case coreModel.PropertyValueType.ShortText:
                    retVal = propValue.ShortTextValue;
                    break;
            }
            return retVal;
        }

        private static void SetPropertyValue(foundationModel.ContactPropertyValue retVal, coreModel.Property property)
        {
            switch (property.ValueType)
            {
                case coreModel.PropertyValueType.Boolean:
                    retVal.BooleanValue = Convert.ToBoolean(property.Value);
                    break;
                case coreModel.PropertyValueType.DateTime:
                    retVal.DateTimeValue = Convert.ToDateTime(property.Value);
                    break;
                case coreModel.PropertyValueType.Decimal:
                    retVal.DecimalValue = Convert.ToDecimal(property.Value);
                    break;
                case coreModel.PropertyValueType.Integer:
                    retVal.IntegerValue = Convert.ToInt32(property.Value);
                    break;
                case coreModel.PropertyValueType.LongText:
                    retVal.LongTextValue = Convert.ToString(property.Value);
                    break;
                case coreModel.PropertyValueType.ShortText:
                    retVal.ShortTextValue = Convert.ToString(property.Value);
                    break;
            }
        }

    }
}
