using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;

namespace MeS.PaymentGatewaysModule.Web.Managers
{
	public class MeSPaymentGatewayImpl : IPaymentGateway
	{
		private const string _gatewayCode = "MeS";
		private const string _description = "MeS Description";
		private const string _logoUrl = "https://www.merchante-solutions.com/wp-content/themes/Foundation-master/img/logo2.png";

		public string GatewayCode
		{
			get { return _gatewayCode; }
		}

		public string Description
		{
			get { return _description; }
		}

		public string LogoUrl
		{
			get { return _logoUrl; }
		}

		public PaymentInfo GetPaymentById(string id)
		{
			var retVal = new PaymentInfo();
			{

			};

			return retVal;
		}

		public PaymentInfo CreatePayment(PaymentInfo paymentInfo)
		{
			throw new NotImplementedException();
		}
	}
}