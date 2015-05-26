using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PaypalPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private static string PaypalAPIModeStoreSetting = "Paypal.Mode";
		private static string PaypalAPIUserNameStoreSetting = "Paypal.APIUsername";
		private static string PaypalAPIPasswordStoreSetting = "Paypal.APIPassword";
		private static string PaypalAPISignatureStoreSetting = "Paypal.APISignature";

		private static string PaypalModeConfigSettingName = "mode";
		private static string PaypalUsernameConfigSettingName = "account1.apiUsername";
		private static string PaypalPasswordConfigSettingName = "account1.apiPassword";
		private static string PaypalSignatureConfigSettingName = "account1.apiSignature";

		private static string SandboxPaypalBaseUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";
		private static string LivePaypalBaseUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";

		public PaypalPaymentMethod()
			: base("Paypal")
		{
		}

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

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			if (context == null && context.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			if (context.Order == null)
				throw new NullReferenceException("no order with this id");

			if (!(context.Store != null && !string.IsNullOrEmpty(context.Store.Url)))
				throw new NullReferenceException("no store with this id");

			var config = GetConfigMap(context.Store);

			var url = context.Store.Url;

			var request = CreatePaypalRequest(context.Order, context.Store, context.Payment);

			var service = new PayPalAPIInterfaceServiceService(config);

			SetExpressCheckoutResponseType setEcResponse = null;

			try
			{
				setEcResponse = service.SetExpressCheckout(request);

				CheckResponse(setEcResponse);

				retVal.IsSuccess = true;
				retVal.NewPaymentStatus = PaymentStatus.Pending;
				retVal.OuterId = setEcResponse.Token;
				var redirectBaseUrl = GetBaseUrl(Mode);
				retVal.RedirectUrl = string.Format(redirectBaseUrl, retVal.OuterId);
			}
			catch (System.Exception ex)
			{
				retVal.Error = ex.Message;
			}

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			if (context == null && context.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			if (context.Order == null)
				throw new NullReferenceException("no order with this id");

			if (!(context.Store != null && !string.IsNullOrEmpty(context.Store.Url)))
				throw new NullReferenceException("no store with this id");

			var config = GetConfigMap(context.Store);

			var service = new PayPalAPIInterfaceServiceService(config);

			GetExpressCheckoutDetailsResponseType response = null;
			DoExpressCheckoutPaymentResponseType doResponse = null;

			var getExpressCheckoutDetailsRequest = GetGetExpressCheckoutDetailsRequest(context.OuterId);
			try
			{
				response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);

				CheckResponse(response);

				var doExpressCheckoutPaymentRequest = GetDoExpressCheckoutPaymentRequest(response, context.OuterId);
				doResponse = service.DoExpressCheckoutPayment(doExpressCheckoutPaymentRequest);

				CheckResponse(doResponse);

				response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);

				var status = response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus;

				if (response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus.Equals("PaymentActionCompleted"))
				{
					retVal.IsSuccess = true;
					retVal.NewPaymentStatus = PaymentStatus.Paid;
					retVal.ReturnUrl = string.Format("{0}/checkout/thanks?orderId={1}&isSuccess=true", context.Store.Url, context.Order.Id);
				}
			}
			catch (System.Exception ex)
			{
				retVal.Error = ex.Message;
				retVal.ReturnUrl = string.Format("{0}/checkout/thanks?orderId={1}&isSuccess=false&errorMessage={2}", context.Store.Url, context.Order.Id, ex.Message);
			}

			return retVal;
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Redirection; }
		}

		private string GetBaseUrl(string mode)
		{
			var retVal = string.Empty;

			if(mode.ToLower().Equals("sandbox"))
			{
				retVal = SandboxPaypalBaseUrlFormat;
			}
			else if(mode.ToLower().Equals("live"))
			{
				retVal = LivePaypalBaseUrlFormat;
			}

			return retVal;
		}

		private string FormatMoney(decimal amount)
		{
			return amount.ToString("F2", new CultureInfo("en-US"));
		}

		private PaymentDetailsType GetPaypalPaymentDetail(CurrencyCodeType currency, PaymentActionCodeType paymentAction, PaymentIn payment)
		{
			var paymentDetails = new PaymentDetailsType { PaymentAction = paymentAction };
			paymentDetails.OrderTotal = new BasicAmountType(currency, FormatMoney(payment.Sum));

			return paymentDetails;
		}

		private Dictionary<string, string> GetConfigMap(Store store)
		{
			var retVal = new Dictionary<string, string>();

			retVal.Add(PaypalModeConfigSettingName, Mode);
			retVal.Add(PaypalUsernameConfigSettingName, APIUsername);
			retVal.Add(PaypalPasswordConfigSettingName, APIPassword);
			retVal.Add(PaypalSignatureConfigSettingName, APISignature);

			return retVal;
		}

		private SetExpressCheckoutReq CreatePaypalRequest(CustomerOrder order, Store store, PaymentIn payment)
		{
			var retVal = new SetExpressCheckoutReq();

			var request = new SetExpressCheckoutRequestType();

			var ecDetails = new SetExpressCheckoutRequestDetailsType
			{
				CallbackTimeout = "3",
				ReturnURL = string.Format("{0}/admin/api/paymentcallback?cancel=false&orderId={1}", store.Url, order.Id),
				CancelURL = string.Format("{0}/admin/api/paymentcallback?cancel=true&orderId={1}", store.Url, order.Id),
				SolutionType = SolutionTypeType.MARK
			};

			var currency = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), order.Currency.ToString());

			var billingAddress = order.Addresses.FirstOrDefault(s => s.AddressType == VirtoCommerce.Domain.Order.Model.AddressType.Billing);

			if (billingAddress != null)
				ecDetails.BuyerEmail = billingAddress.Email;
			else
				billingAddress = order.Addresses.FirstOrDefault();

			if (billingAddress != null && !string.IsNullOrEmpty(billingAddress.Email))
				ecDetails.BuyerEmail = billingAddress.Email;

			ecDetails.PaymentDetails.Add(GetPaypalPaymentDetail(currency, PaymentActionCodeType.SALE, payment));

			request.SetExpressCheckoutRequestDetails = ecDetails;

			retVal.SetExpressCheckoutRequest = request;

			return retVal;
		}

		private GetExpressCheckoutDetailsReq GetGetExpressCheckoutDetailsRequest(string paymentId)
		{
			return new GetExpressCheckoutDetailsReq
			{
				GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType
				{
					Token = paymentId
				}
			};
		}

		private DoExpressCheckoutPaymentReq GetDoExpressCheckoutPaymentRequest(GetExpressCheckoutDetailsResponseType response, string paymentId)
		{
			return new DoExpressCheckoutPaymentReq
			{
				DoExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType
				{
					DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType
					{
						Token = paymentId,
						PayerID = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID,
						PaymentDetails = response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails
					}
				}
			};
		}

		private bool CheckResponse(AbstractResponseType response)
		{
			if (response != null)
			{
				if (response.Ack.Equals(AckCodeType.FAILURE) || (response.Errors != null && response.Errors.Count > 0))
				{
					StringBuilder sb = new StringBuilder();
					foreach (var error in response.Errors)
					{
						sb.AppendLine(error.LongMessage);
					}

					throw new NullReferenceException(sb.ToString());
				}
			}
			else
			{
				throw new NullReferenceException("response in null");
			}

			return true;
		}

	
	}
}