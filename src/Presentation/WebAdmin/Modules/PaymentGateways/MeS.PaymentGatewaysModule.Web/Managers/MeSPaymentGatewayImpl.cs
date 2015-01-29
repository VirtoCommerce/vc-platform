using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Services;

namespace MeS.PaymentGatewaysModule.Web.Managers
{
	public class MeSPaymentGatewayImpl : IPaymentGateway
	{
		private const string _code = "MeS";
		private const string _description = "MeS Description";
		private const string _logoUrl = "https://www.merchante-solutions.com/wp-content/themes/Foundation-master/img/logo2.png";

		public string Code
		{
			get { return _code; }
		}

		public string Description
		{
			get { return _description; }
		}

		public string LogoUrl
		{
			get { return _logoUrl; }
		}

		public VirtoCommerce.Domain.Payment.Model.PaymentInfo GetPaymentById(string id)
		{
			throw new NotImplementedException();
		}

		public VirtoCommerce.Domain.Payment.Model.PaymentInfo CreatePayment(VirtoCommerce.Domain.Payment.Model.PaymentInfo paymentInfo)
		{
			throw new NotImplementedException();
		}
	}
}