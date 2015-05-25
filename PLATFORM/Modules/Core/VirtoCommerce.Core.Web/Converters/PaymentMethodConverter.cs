using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Payment.Model;
using webModel = VirtoCommerce.CoreModule.Web.Model;

namespace VirtoCommerce.CoreModule.Web.Converters
{
	public static class PaymentMethodConverter
	{
		public static webModel.PaymentMethod ToWebModel(this coreModel.PaymentMethod method)
		{
			var retVal = new webModel.PaymentMethod();
			retVal.InjectFrom(method);

			return retVal;
		}
	}
}