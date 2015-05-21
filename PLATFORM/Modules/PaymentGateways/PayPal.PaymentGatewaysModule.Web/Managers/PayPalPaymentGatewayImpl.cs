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
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PayPalPaymentGatewayImpl : IPaymentGateway
	{
		private string _gatewayCode;
		private string _description;
		private string _logoUrl;
		private PaymentGatewayType _gatewayType;

		private ICustomerOrderService _customerOrderService;
		private IStoreService _storeService;
		private ISettingsManager _settingsManager;

		private static string PaypalAPIModeStoreSetting = "Paypal.Mode";
		private static string PaypalAPIUserNameStoreSetting = "Paypal.APIUsername";
		private static string PaypalAPIPasswordStoreSetting = "Paypal.APIPassword";
		private static string PaypalAPISignatureStoreSetting = "Paypal.APISignature";

		private static string PaypalModeConfigSettingName = "mode";
		private static string PaypalUsernameConfigSettingName = "account1.apiUsername";
		private static string PaypalPasswordConfigSettingName = "account1.apiPassword";
		private static string PaypalSignatureConfigSettingName = "account1.apiSignature";

		//private static string 

		public PayPalPaymentGatewayImpl(
			string gatewayCode,
			string description,
			string logoUrl,
			PaymentGatewayType gatewayType,
			ICustomerOrderService customerOrderService,
			IStoreService storeService,
			ISettingsManager settingsManager)
		{
			if (string.IsNullOrEmpty(gatewayCode))
				throw new ArgumentNullException("gatewayCode");

			if (string.IsNullOrEmpty(description))
				throw new ArgumentNullException("description");

			if (string.IsNullOrEmpty(logoUrl))
				throw new ArgumentNullException("logoUrl");

			if (customerOrderService == null)
				throw new ArgumentNullException("customerOrderService");

			if (storeService == null)
				throw new ArgumentNullException("storeService");

			if (settingsManager == null)
				throw new ArgumentNullException("settingsManager");

			_gatewayCode = gatewayCode;
			_description = description;
			_logoUrl = logoUrl;
			_gatewayType = gatewayType;

			_customerOrderService = customerOrderService;
			_storeService = storeService;
			_settingsManager = settingsManager;
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

		public PaymentGatewayType GatewayType
		{
			get { return _gatewayType; }
		}

		public PaymentInfo GetPayment(string paymentId, string orderId)
		{
			var retVal = new DirectRedirectUrlPaymentInfo();

			var order = _customerOrderService.GetById(orderId, VirtoCommerce.Domain.Order.Model.CustomerOrderResponseGroup.Full);
			if (order == null)
				throw new NullReferenceException("no order with this id");

			var store = _storeService.GetById(order.StoreId);
			if (!(store != null && !string.IsNullOrEmpty(store.Url)))
				throw new NullReferenceException("no store with this id");

			var payment = order.InPayments.FirstOrDefault(p => p.OuterId == paymentId);
			if (payment == null)
				throw new NullReferenceException("no payment paypal in this order");

			var config = GetConfigMap(store);

			var service = new PayPalAPIInterfaceServiceService(config);

			GetExpressCheckoutDetailsResponseType response = null;
			DoExpressCheckoutPaymentResponseType doResponse = null;

			var getExpressCheckoutDetailsRequest = GetGetExpressCheckoutDetailsRequest(paymentId);
			try
			{
				response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);
			}
			catch (System.Exception ex)
			{
				throw new NullReferenceException("paypal not work", ex);
			}

			CheckResponse(response);

			var doExpressCheckoutPaymentRequest = GetDoExpressCheckoutPaymentRequest(response, paymentId);
			doResponse = service.DoExpressCheckoutPayment(doExpressCheckoutPaymentRequest);

			response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);
			var status = response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus;

			if (response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus.Equals("PaymentActionCompleted"))
			{
				payment.IsApproved = true;
				retVal = GetPaymentInfo(order, paymentId, true);
			}
			else
			{
				payment.IsApproved = false;
				retVal = GetPaymentInfo(order, paymentId, false);
			}

			_customerOrderService.Update(new CustomerOrder[] { order });

			return retVal;
		}

		public PaymentInfo CreatePayment(PaymentInfo paymentInfo)
		{
			var retVal = paymentInfo as DirectRedirectUrlPaymentInfo;

			var order = _customerOrderService.GetById(paymentInfo.OrderId, VirtoCommerce.Domain.Order.Model.CustomerOrderResponseGroup.Full);
			if (order == null)
				throw new NullReferenceException("no order with this id");

			var store = _storeService.GetById(order.StoreId);
			if (!(store != null && !string.IsNullOrEmpty(store.Url)))
				throw new NullReferenceException("no store with this id");

			var payment = order.InPayments.FirstOrDefault(p => p.GatewayCode == GatewayCode);
			if (payment == null)
				throw new NullReferenceException("no payment paypal in this order");

			var config = GetConfigMap(store);

			var url = store.Url;

			var request = CreatePaypalRequest(order, store, payment);

			var service = new PayPalAPIInterfaceServiceService(config);

			SetExpressCheckoutResponseType setEcResponse = null;

			try
			{
				setEcResponse = service.SetExpressCheckout(request);
			}
			catch (System.Exception ex)
			{
				throw new NullReferenceException("paypal not work", ex);
			}

			CheckResponse(setEcResponse);

			payment.OuterId = setEcResponse.Token;

			retVal.RedirectUrl = string.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd={0}", "_express-checkout&useraction=commit&token=" + setEcResponse.Token);
			retVal.Id = payment.OuterId;
			retVal.GatewayCode = GatewayCode;
			retVal.CreatedDate = DateTime.UtcNow;
			retVal.Currency = order.Currency;
			retVal.Status = payment.Status;

			_customerOrderService.Update(new CustomerOrder[] { order });

			return retVal;
		}

		private string FormatMoney(decimal amount)
		{
			return amount.ToString("F2", new CultureInfo("en-US"));
		}

		private PaymentDetailsType GetPaypalPaymentDetail(CurrencyCodeType currency, PaymentActionCodeType paymentAction, CustomerOrder order, Store store)
		{
			var paymentDetails = new PaymentDetailsType { PaymentAction = paymentAction };
			paymentDetails.OrderTotal = new BasicAmountType(currency, FormatMoney(order.Sum));

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
				ReturnURL = string.Format("{0}?cancel=false&orderId={}", store.Url),
				CancelURL = string.Format("{0}?cancel=true", store.Url),
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

			ecDetails.PaymentDetails.Add(GetPaypalPaymentDetail(currency, PaymentActionCodeType.SALE, order, store));

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

		private DirectRedirectUrlPaymentInfo GetPaymentInfo(CustomerOrder order, string paymentId, bool isApproved)
		{
			var payment = order.InPayments.FirstOrDefault(p => p.GatewayCode == GatewayCode);

			var retVal = new DirectRedirectUrlPaymentInfo
			{
				Amount = order.Sum,
				CreatedDate = payment.CreatedDate,
				Currency = order.Currency,
				GatewayCode = GatewayCode,
				Id = paymentId,
				IsApproved = isApproved,
				OrderId = order.Id,
				Status = payment.Status
			};

			return retVal;
		}

		private void CheckResponse(AbstractResponseType response)
		{
			if(response != null)
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
		}
	}
}