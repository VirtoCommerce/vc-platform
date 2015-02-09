using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Services;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PayPalPaymentGatewayImpl : IPaymentGateway
	{
		private string _gatewayCode;
		private string _description;
		private string _logoUrl;

		private string _appKey;
		private string _secret;

		public PayPalPaymentGatewayImpl(string appKey, string secret, string gatewayCode, string description, string logoUrl)
		{
			_gatewayCode = gatewayCode;
			_description = description;
			_logoUrl = logoUrl;

			_appKey = appKey;
			_secret = secret;
		}

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