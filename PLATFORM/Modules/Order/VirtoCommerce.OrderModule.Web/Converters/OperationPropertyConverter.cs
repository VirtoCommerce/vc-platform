using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class OperationPropertyConverter
	{
		public static webModel.OperationProperty ToWebModel(this coreModel.OperationProperty property)
		{
			var retVal = new webModel.OperationProperty();
			retVal.InjectFrom(property);

			retVal.ValueType = property.ValueType;
			retVal.Value = property.Value != null ? property.Value.ToString() : null;

			return retVal;
		}

		public static coreModel.OperationProperty ToCoreModel(this webModel.OperationProperty property)
		{
			var retVal = new coreModel.OperationProperty();
			retVal.InjectFrom(property);

			retVal.ValueType = property.ValueType;
			retVal.Value = property.Value;

			return retVal;
		}


	}
}