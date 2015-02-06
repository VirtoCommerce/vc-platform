using Klarna.Checkout;
using Klarna.PaymentGatewaysModule.Web.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;

namespace Klarna.PaymentGatewaysModule.Web.Managers
{
	public class KlarnaPaymentGatewayImpl : IPaymentGateway
	{
		private int _appKey;
		private string _secretKey;
		private const string ContentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

		public KlarnaPaymentGatewayImpl(int appKey, string secretKey)
		{
			if (appKey == 0)
				throw new KlarnaCredentialsException("appKey is wrong");

			if (string.IsNullOrEmpty(secretKey))
				throw new KlarnaCredentialsException("secretKey is wrong");

			_appKey = appKey;
			_secretKey = secretKey;
		}

		private const string _gatewayCode = "Klarna";
		private const string _description = "Klarna description";
		private const string _logoUrl = "https://cdn.klarna.com/1.0/shared/image/generic/logo/global/tagline/vertical-blue.png";

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

			Uri resourceUri = new Uri(id);

			var connector = Connector.Create(_secretKey);

			Order order = new Order(connector, resourceUri)
			{
				ContentType = ContentType
			};

			order.Fetch();

			var klarnaCart = order.GetValue("cart") as JObject;

			var amount = (decimal)klarnaCart["total_price_including_tax"];
			retVal.Amount = amount;

			var status = order.GetValue("status") as string;
			retVal.IsApproved = status == "checkout_complete";

			var createdDate = order.GetValue("started_at") as string;
			DateTime dateTime;
			retVal.CreatedDate = DateTime.TryParse(createdDate, out dateTime) ? dateTime : new Nullable<DateTime>();

			var currency = order.GetValue("purchase_currency") as string;
			retVal.Currency = (VirtoCommerce.Foundation.Money.CurrencyCodes)Enum.Parse(typeof(VirtoCommerce.Foundation.Money.CurrencyCodes), currency);

			var gatewayCode = order.GetValue("id") as string;
			retVal.GatewayCode = gatewayCode;

			return retVal;
		}

		public PaymentInfo CreatePayment(PaymentInfo paymentInfo)
		{
			throw new NotImplementedException();
		}
	}
}