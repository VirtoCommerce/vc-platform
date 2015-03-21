using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Payment.Services;
using webModel = VirtoCommerce.CoreModule.Web.Model;

namespace VirtoCommerce.CoreModule.Web.Converters
{
	public static class PaymentGatewayConverter
	{
		public static webModel.PaymentGateway ToWebModel(this coreModel.IPaymentGateway gateway)
		{
			var retVal = new webModel.PaymentGateway();
			retVal.InjectFrom(gateway);

			return retVal;
		}
	}
}