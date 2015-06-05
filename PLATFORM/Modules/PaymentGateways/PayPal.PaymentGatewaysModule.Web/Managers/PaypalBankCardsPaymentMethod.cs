using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Model;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PaypalBankCardsPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		public PaypalBankCardsPaymentMethod() : base ("PayPalBankCards")
		{

		}

		private static string PaypalAPIModeStoreSetting = "Paypal.Mode";
		private static string PaypalAPIUserNameStoreSetting = "Paypal.APIUsername";
		private static string PaypalAPIPasswordStoreSetting = "Paypal.APIPassword";
		private static string PaypalAPISignatureStoreSetting = "Paypal.APISignature";
		private static string PaypalPaymentRedirectRelativePathStoreSetting = "Paypal.PaymentRedirectRelativePath";

		private static string PaypalModeConfigSettingName = "mode";
		private static string PaypalUsernameConfigSettingName = "account1.apiUsername";
		private static string PaypalPasswordConfigSettingName = "account1.apiPassword";
		private static string PaypalSignatureConfigSettingName = "account1.apiSignature";

		private static string SandboxPaypalBaseUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_ap-payment&paykey={0}";
		private static string LivePaypalBaseUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_ap-payment&paykey={0}";

		private string Mode
		{
			get
			{
				var retVal = GetSetting(PaypalAPIModeStoreSetting);
				return retVal;
			}
		}

		private string APIUsername
		{
			get
			{
				var retVal = GetSetting(PaypalAPIUserNameStoreSetting);
				return retVal;
			}
		}

		private string APIPassword
		{
			get
			{
				var retVal = GetSetting(PaypalAPIPasswordStoreSetting);
				return retVal;
			}
		}

		private string APISignature
		{
			get
			{
				var retVal = GetSetting(PaypalAPISignatureStoreSetting);
				return retVal;
			}
		}

		private string PaypalPaymentRedirectRelativePath
		{
			get
			{
				var retVal = GetSetting(PaypalPaymentRedirectRelativePathStoreSetting);
				return retVal;
			}
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Redirection; }
		}

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			if (!(context.Store != null && (!string.IsNullOrEmpty(context.Store.SecureUrl) || !string.IsNullOrEmpty(context.Store.Url))))
				throw new NullReferenceException("no store with this id");

			var url = string.Empty;
			if(!string.IsNullOrEmpty(context.Store.SecureUrl))
			{
				url = context.Store.SecureUrl;
			}
			else
			{
				url = context.Store.Url;
			}

			var config = GetConfigMap();

			var service = new AdaptivePaymentsService(config);

			var request = CreatePaypalRequest(context.Order, context.Payment, url);

			var response = service.Pay(request);

			retVal.OuterId = response.payKey;
			retVal.IsSuccess = true;
			var redirectBaseUrl = GetBaseUrl(Mode);
			retVal.RedirectUrl = string.Format(redirectBaseUrl, retVal.OuterId);

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			if (!(context.Store != null && (!string.IsNullOrEmpty(context.Store.SecureUrl) || !string.IsNullOrEmpty(context.Store.Url))))
				throw new NullReferenceException("no store with this id");

			var url = string.Empty;
			if (!string.IsNullOrEmpty(context.Store.SecureUrl))
			{
				url = context.Store.SecureUrl;
			}
			else
			{
				url = context.Store.Url;
			}

			var config = GetConfigMap();

			var service = new AdaptivePaymentsService(config);



			return retVal;
		}

		public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
		{
			var retVal = new ValidatePostProcessRequestResult();

			var cancel = queryString["cancel"];
			var paykey = queryString["paykey"];

			if (!string.IsNullOrEmpty(cancel) && !string.IsNullOrEmpty(paykey))
			{
				bool cancelValue;
				if (bool.TryParse(cancel, out cancelValue))
				{
					retVal.IsSuccess = !cancelValue;
					retVal.OuterId = paykey;
				}
			}

			return retVal;
		}

		private PayRequest CreatePaypalRequest(CustomerOrder order, PaymentIn payment, string url)
		{
			var receivers = new List<Receiver>();
			receivers.Add(new Receiver { amount = payment.Sum, email = "evgokhrimenko@gmail.com", invoiceId = payment.Id});

			PayRequest retVal = new PayRequest
			{
				requestEnvelope = new RequestEnvelope { errorLanguage = "en_US" },
				currencyCode = order.Currency.ToString(),
				receiverList = new ReceiverList(receivers),
				actionType = "PAY",
				cancelUrl = string.Format("{0}/{1}?cancel=true&orderId={2}", url, PaypalPaymentRedirectRelativePath, order.Id) + "&paykey=${paykey}",
				returnUrl = string.Format("{0}/{1}?cancel=false&orderId={2}", url, PaypalPaymentRedirectRelativePath, order.Id) + "&paykey=${paykey}"
			};

			return retVal;
		}

		private Dictionary<string, string> GetConfigMap()
		{
			var retVal = new Dictionary<string, string>();

			retVal.Add(PaypalModeConfigSettingName, Mode);
			retVal.Add(PaypalUsernameConfigSettingName, APIUsername);
			retVal.Add(PaypalPasswordConfigSettingName, APIPassword);
			retVal.Add(PaypalSignatureConfigSettingName, APISignature);
			retVal.Add("account1.applicationId", "APP-80W284485P519543T");

			return retVal;
		}

		private string GetBaseUrl(string mode)
		{
			var retVal = string.Empty;

			if (mode.ToLower().Equals("sandbox"))
			{
				retVal = SandboxPaypalBaseUrlFormat;
			}
			else if (mode.ToLower().Equals("live"))
			{
				retVal = LivePaypalBaseUrlFormat;
			}

			return retVal;
		}
	}
}