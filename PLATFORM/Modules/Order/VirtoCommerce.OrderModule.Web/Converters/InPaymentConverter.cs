using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class InPaymentConverter
	{
		public static webModel.PaymentIn ToWebModel(this coreModel.PaymentIn payment)
		{
			var retVal = new webModel.PaymentIn();
			retVal.InjectFrom(payment);
			retVal.Currency = payment.Currency;


			retVal.ChildrenOperations = payment.GetFlatObjectsListWithInterface<coreModel.IOperation>().Select(x => x.ToWebModel()).ToList();

			if (payment.DynamicProperties != null)
				retVal.DynamicProperties = payment.DynamicProperties;

			return retVal;
		}

		public static coreModel.PaymentIn ToCoreModel(this webModel.PaymentIn payment)
		{
			var retVal = new coreModel.PaymentIn();
			retVal.InjectFrom(payment);
			retVal.PaymentStatus = EnumUtility.SafeParse<PaymentStatus>(payment.Status, PaymentStatus.Custom);

		
			retVal.Currency = payment.Currency;


			if (payment.DynamicProperties != null)
				retVal.DynamicProperties = payment.DynamicProperties;


			return retVal;
		}


	}
}
