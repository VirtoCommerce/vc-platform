using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class PaymentConverter
	{
		public static webModel.Payment ToWebModel(this coreModel.Payment payment)
		{
			var retVal = new webModel.Payment();
			retVal.InjectFrom(payment);
			retVal.Currency = payment.Currency;

			if (payment.BillingAddress != null)
				retVal.BillingAddress = payment.BillingAddress.ToWebModel();

			return retVal;
		}

		public static coreModel.Payment ToCoreModel(this webModel.Payment payment)
		{
			var retVal = new coreModel.Payment();
			retVal.InjectFrom(payment);

			retVal.Currency = payment.Currency;

			if (payment.BillingAddress != null)
				retVal.BillingAddress = payment.BillingAddress.ToCoreModel();

			return retVal;
		}


	}
}
