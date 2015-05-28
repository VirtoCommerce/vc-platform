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

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			return retVal;
		}

		public override ValidatePostProcessRequestResult ValidatePostProcessRequest(object context)
		{
			throw new NotImplementedException();
		}
	}
}