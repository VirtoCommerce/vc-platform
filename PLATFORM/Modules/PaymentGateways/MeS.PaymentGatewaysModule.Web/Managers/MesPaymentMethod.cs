using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;

namespace MeS.PaymentGatewaysModule.Web.Managers
{
	public class MesPaymentMethod : PaymentMethod
	{
		public MesPaymentMethod()
			: base("Mes")
		{
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Standard; }
		}

		public override ProcessPaymentResult ProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			var paymentEvaluationContext = context as PaymentEvaluationContext;

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			var paymentEvaluationContext = context as PaymentEvaluationContext;

			return retVal;
		}
	}
}