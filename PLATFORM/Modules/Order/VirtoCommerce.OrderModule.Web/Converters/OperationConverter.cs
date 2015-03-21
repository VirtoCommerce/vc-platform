using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class OperationConverter
	{
		public static webModel.Operation ToWebModel(this coreModel.Operation operation)
		{
			var retVal = new webModel.Operation();
			retVal.InjectFrom(operation);
			return retVal;
		}

	}
}