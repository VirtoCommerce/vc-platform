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
		public IStoreService _storeService;
		public ICustomerOrderService _customerOrderService;

		private static string PaypalAPIModeStoreSetting = "Paypal.Mode";
		private static string PaypalAPIUserNameStoreSetting = "Paypal.APIUsername";
		private static string PaypalAPIPasswordStoreSetting = "Paypal.APIPassword";
		private static string PaypalAPISignatureStoreSetting = "Paypal.APISignature";

		private static string PaypalModeConfigSettingName = "mode";
		private static string PaypalUsernameConfigSettingName = "account1.apiUsername";
		private static string PaypalPasswordConfigSettingName = "account1.apiPassword";
		private static string PaypalSignatureConfigSettingName = "account1.apiSignature";

		public PaypalPaymentMethod(IStoreService storeService, ICustomerOrderService customerOrderService)
			: base("Paypal")
		{
			if (storeService == null)
				throw new ArgumentNullException("storeService");

			if (customerOrderService == null)
				throw new ArgumentNullException("customerOrderService");

			_storeService = storeService;
			_customerOrderService = customerOrderService;
		}

		public override ProcessPaymentResult ProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			var paymentEvaluationContext = context as PaymentEvaluationContext;
			if (paymentEvaluationContext == null && paymentEvaluationContext.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			var order = _customerOrderService.GetById(paymentEvaluationContext.OrderId, VirtoCommerce.Domain.Order.Model.CustomerOrderResponseGroup.Full);
			if (order == null)
				throw new NullReferenceException("no order with this id");

			var store = _storeService.GetById(order.StoreId);
			if (!(store != null && !string.IsNullOrEmpty(store.Url)))
				throw new NullReferenceException("no store with this id");

			var config = GetConfigMap(store);

			var url = store.Url;

			var request = CreatePaypalRequest(order, store, paymentEvaluationContext.Payment);

			var service = new PayPalAPIInterfaceServiceService(config);

			SetExpressCheckoutResponseType setEcResponse = null;

			try
			{
				setEcResponse = service.SetExpressCheckout(request);

				CheckResponse(setEcResponse);

				retVal.IsSuccess = true;
				retVal.NewPaymentStatus = PaymentStatus.Pending;

				var existedPayment = order.InPayments.FirstOrDefault(p => p.Id == paymentEvaluationContext.Payment.Id);
				if(existedPayment != null)
				{
					existedPayment.OuterId = setEcResponse.Token;
					_customerOrderService.Update(new CustomerOrder[] { order });
				}
			}
			catch (System.Exception ex)
			{
				retVal.Error = ex.Message;

				//throw new NullReferenceException("paypal not work", ex);
			}

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(VirtoCommerce.Domain.Common.IEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			var paymentEvaluationContext = context as PaymentEvaluationContext;
			if (paymentEvaluationContext == null && paymentEvaluationContext.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			var order = _customerOrderService.GetById(paymentEvaluationContext.OrderId, VirtoCommerce.Domain.Order.Model.CustomerOrderResponseGroup.Full);
			if (order == null)
				throw new NullReferenceException("no order with this id");

			var store = _storeService.GetById(order.StoreId);
			if (!(store != null && !string.IsNullOrEmpty(store.Url)))
				throw new NullReferenceException("no store with this id");

			var config = GetConfigMap(store);

			var service = new PayPalAPIInterfaceServiceService(config);

			GetExpressCheckoutDetailsResponseType response = null;
			DoExpressCheckoutPaymentResponseType doResponse = null;

			var getExpressCheckoutDetailsRequest = GetGetExpressCheckoutDetailsRequest(paymentEvaluationContext.OuterId);
			try
			{
				response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);

				CheckResponse(response);

				var doExpressCheckoutPaymentRequest = GetDoExpressCheckoutPaymentRequest(response, paymentEvaluationContext.OuterId);
				doResponse = service.DoExpressCheckoutPayment(doExpressCheckoutPaymentRequest);

				CheckResponse(doResponse);

				response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);

				var status = response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus;

				if (response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus.Equals("PaymentActionCompleted"))
				{
					var existedPayment = order.InPayments.FirstOrDefault(p => p.OuterId == paymentEvaluationContext.OuterId);
					existedPayment.IsApproved = true;
					_customerOrderService.Update(new CustomerOrder[] { order });

					retVal.IsSuccess = true;
					retVal.NewPaymentStatus = PaymentStatus.Paid;
					retVal.ReturnUrl = string.Format("{0}/checkout/thanks?orderId={1}&isSuccess=true", store.Url, paymentEvaluationContext.OrderId);
				}
			}
			catch (System.Exception ex)
			{
				retVal.Error = ex.Message;
				retVal.ReturnUrl = string.Format("{0}/checkout/thanks?orderId={1}&isSuccess=false", store.Url, paymentEvaluationContext.OrderId);

				//throw new NullReferenceException("paypal not work", ex);
			}

			return retVal;
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Redirection; }
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

			var mode = GetSetting(store, PaypalAPIModeStoreSetting);
			var username = GetSetting(store, PaypalAPIUserNameStoreSetting);
			var password = GetSetting(store, PaypalAPIPasswordStoreSetting);
			var signature = GetSetting(store, PaypalAPISignatureStoreSetting);

			retVal.Add(PaypalModeConfigSettingName, mode);
			retVal.Add(PaypalUsernameConfigSettingName, username);
			retVal.Add(PaypalPasswordConfigSettingName, password);
			retVal.Add(PaypalSignatureConfigSettingName, signature);

			return retVal;
		}

		private string GetSetting(Store store, string settingName)
		{
			var setting = store.Settings.FirstOrDefault(s => s.Name == settingName);

			if (setting == null && setting.Value is string && string.IsNullOrEmpty((string)setting.Value))
				throw new NullReferenceException(string.Format("{0} setting is not exist or null"));

			return (string)setting.Value;
		}

		private SetExpressCheckoutReq CreatePaypalRequest(CustomerOrder order, Store store, PaymentIn payment)
		{
			var retVal = new SetExpressCheckoutReq();

			var request = new SetExpressCheckoutRequestType();

			var ecDetails = new SetExpressCheckoutRequestDetailsType
			{
				CallbackTimeout = "3",
				ReturnURL = string.Format("{0}/admin/api/paymentgateway/paypal/push?cancel=false&orderId={1}&redirectUrl={2}", store.Url, order.Id, HttpUtility.UrlEncode(store.Url)),
				CancelURL = string.Format("{0}/admin/api/paymentgateway/paypal/push?cancel=true&orderId={1}&redirectUrl={2}", store.Url, order.Id, HttpUtility.UrlEncode(store.Url)),
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