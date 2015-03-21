using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class InPaymentConverter
	{
		public static webModel.PaymentIn ToWebModel(this coreModel.PaymentIn payment)
		{
			var retVal = new webModel.PaymentIn();
			retVal.InjectFrom(payment);
			retVal.Currency = payment.Currency;

			retVal.Organization = retVal.OrganizationId;
			retVal.Customer = retVal.CustomerId;

			retVal.ChildrenOperations = payment.ChildrenOperations.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.PaymentIn ToCoreModel(this webModel.PaymentIn payment)
		{
			var retVal = new coreModel.PaymentIn();
			retVal.InjectFrom(payment);
		
			retVal.Currency = payment.Currency;
			return retVal;
		}


	}
}
