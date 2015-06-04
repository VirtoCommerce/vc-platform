using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.OrderModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class OperationPropertyConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.OperationProperty ToCoreModel(this dataModel.OperationPropertyEntity dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.OperationProperty();
			retVal.InjectFrom(dbEntity);
			retVal.ValueType = (coreModel.PropertyValueType)Enum.Parse(typeof(coreModel.PropertyValueType), dbEntity.ValueType, true);
			retVal.Value = dbEntity.RawValue();
			return retVal;

		}


		public static dataModel.OperationPropertyEntity ToDataModel(this coreModel.OperationProperty property)
		{
			if (property == null)
				throw new ArgumentNullException("property");

			var retVal = new dataModel.OperationPropertyEntity();
			retVal.InjectFrom(property);
			retVal.ValueType = property.ValueType.ToString();
			SetPropertyValue(retVal, property);
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.OperationPropertyEntity source, dataModel.OperationPropertyEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.OperationPropertyEntity>(x => x.BooleanValue, x => x.DateTimeValue,
																					   x => x.DecimalValue, x => x.IntegerValue,
																					   x => x.Locale, x => x.LongTextValue, x => x.ShortTextValue,
																					   x => x.ValueType);
			target.InjectFrom(patchInjectionPolicy, source);

		}

		private static void SetPropertyValue(dataModel.OperationPropertyEntity retVal, coreModel.OperationProperty property)
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
