using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Model;

namespace Paypal.ExpressCheckout.Managers
{
	public class PaypalExpressCheckoutPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
	{
		private const string _paypalAPIModeStoreSetting = "Paypal.ExpressCheckout.Mode";
		private const string _paypalPaymentModeStoreSetting = "Paypal.ExpressCheckout.PaymentMode";
		private const string _paypalAPIUserNameStoreSetting = "Paypal.ExpressCheckout.APIUsername";
		private const string _paypalAPIPasswordStoreSetting = "Paypal.ExpressCheckout.APIPassword";
		private const string _paypalAPISignatureStoreSetting = "Paypal.ExpressCheckout.APISignature";
		private const string _paypalPaymentRedirectRelativePathStoreSetting = "Paypal.ExpressCheckout.PaymentRedirectRelativePath";
		private const string _paypalPaymentActionTypeStoreSetting = "Paypal.ExpressCheckout.PaymentActionType";

		private const string PaypalModeConfigSettingName = "mode";
		private const string PaypalUsernameConfigSettingName = "account1.apiUsername";
		private const string PaypalPasswordConfigSettingName = "account1.apiPassword";
		private const string PaypalSignatureConfigSettingName = "account1.apiSignature";

		private const string SandboxPaypalBaseUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";
		private const string LivePaypalBaseUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";


		public PaypalExpressCheckoutPaymentMethod()
			: base("Paypal.ExpressCheckout")
		{
		}

		private string Mode
		{
			get
			{
				var retVal = GetSetting(_paypalAPIModeStoreSetting);
				return retVal;
			}
		}

		private string APIUsername
		{
			get
			{
				var retVal = GetSetting(_paypalAPIUserNameStoreSetting);
				return retVal;
			}
		}

		private string APIPassword
		{
			get
			{
				var retVal = GetSetting(_paypalAPIPasswordStoreSetting);
				return retVal;
			}
		}

		private string APISignature
		{
			get
			{
				var retVal = GetSetting(_paypalAPISignatureStoreSetting);
				return retVal;
			}
		}

		private string PaypalPaymentRedirectRelativePath
		{
			get
			{
				var retVal = GetSetting(_paypalPaymentRedirectRelativePathStoreSetting);
				return retVal;
			}
		}

		private PaymentActionCodeType PaypalPaymentActionType
		{
			get
			{
				var settingValue = GetSetting(_paypalPaymentActionTypeStoreSetting);
				return GetPaymentActionType(settingValue);
			}
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Redirection; }
		}

		public override PaymentMethodGroupType PaymentMethodGroupType
		{
			get { return PaymentMethodGroupType.Paypal; }
		}

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			if (context.Order == null)
				throw new ArgumentNullException("context.Order is null");
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment is null");
			if (context.Store == null)
				throw new ArgumentNullException("context.Store is null");
			if (string.IsNullOrEmpty(context.Store.Url))
				throw new NullReferenceException("url of store not set");

			var retVal = new ProcessPaymentResult();

			var config = GetConfigMap();
			var url = context.Store.Url;
			var request = GetSetExpressCheckoutRequest(context.Order, context.Store, context.Payment);
			var service = new PayPalAPIInterfaceServiceService(config);

			try
			{
				var setEcResponse = service.SetExpressCheckout(request);

				CheckResponse(setEcResponse);

				retVal.IsSuccess = true;
				retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Pending;
				retVal.OuterId = context.Payment.OuterId = setEcResponse.Token;
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
			if (context.Order == null)
				throw new ArgumentNullException("context.Order is null");
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment is null");
			if (context.Store == null)
				throw new ArgumentNullException("context.Store is null");
			if (string.IsNullOrEmpty(context.Store.Url))
				throw new NullReferenceException("url of store not set");

			var retVal = new PostProcessPaymentResult();

			retVal.OrderId = context.Order.Id;

			var config = GetConfigMap();

			var service = new PayPalAPIInterfaceServiceService(config);

		    var getExpressCheckoutDetailsRequest = GetGetExpressCheckoutDetailsRequest(context.OuterId);
			try
			{
				var response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);

				CheckResponse(response);

				var status = response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus;

				if (!status.Equals("PaymentActionCompleted"))
				{
					var doExpressCheckoutPaymentRequest = GetDoExpressCheckoutPaymentRequest(response, context.OuterId);
					var doResponse = service.DoExpressCheckoutPayment(doExpressCheckoutPaymentRequest);

					CheckResponse(doResponse);

					response = service.GetExpressCheckoutDetails(getExpressCheckoutDetailsRequest);
					status = response.GetExpressCheckoutDetailsResponseDetails.CheckoutStatus;
				}
				if (status.Equals("PaymentActionCompleted"))
				{
					retVal.IsSuccess = true;
					retVal.OuterId = response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails[0].TransactionId;
					if(PaypalPaymentActionType == PaymentActionCodeType.AUTHORIZATION)
					{
						retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Authorized;
						context.Payment.OuterId = retVal.OuterId;
						context.Payment.AuthorizedDate = DateTime.UtcNow;
					}
					else if (PaypalPaymentActionType == PaymentActionCodeType.SALE)
					{
						retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
						context.Payment.OuterId = retVal.OuterId;
						context.Payment.IsApproved = true;
						context.Payment.CapturedDate = DateTime.UtcNow;
					}
				}
				else
				{
					retVal.ErrorMessage = "Payment process not successfully ends";
				}
			}
			catch (System.Exception ex)
			{
				retVal.ErrorMessage = ex.Message;
			}

			return retVal;
		}

		public override VoidProcessPaymentResult VoidProcessPayment(VoidProcessPaymentEvaluationContext context)
		{
			if (context.Payment == null)
				throw new ArgumentNullException("context.Payment is null");

			VoidProcessPaymentResult retVal = new VoidProcessPaymentResult();

			if(!context.Payment.IsApproved && context.Payment.PaymentStatus == PaymentStatus.Authorized)
			{
				try
				{
					var config = GetConfigMap();
					var service = new PayPalAPIInterfaceServiceService(config);
					var doVoidResponse = service.DoVoid(new DoVoidReq { DoVoidRequest = new DoVoidRequestType { AuthorizationID = context.Payment.OuterId, Note = "Cancel payment" } });

					CheckResponse(doVoidResponse);

					if(context.Payment.OuterId == doVoidResponse.AuthorizationID)
					{
						retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Voided;
						context.Payment.VoidedDate = context.Payment.CancelledDate = DateTime.UtcNow;
						context.Payment.IsCancelled = true;
						retVal.IsSuccess = true;
					}
				}
				catch(Exception ex)
				{
					retVal.ErrorMessage = ex.Message;
				}
			}

			return retVal;
		}

		public override CaptureProcessPaymentResult CaptureProcessPayment(CaptureProcessPaymentEvaluationContext context)
		{
			if (context == null || context.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			CaptureProcessPaymentResult retVal = new CaptureProcessPaymentResult();

			if (!context.Payment.IsApproved && (context.Payment.PaymentStatus == PaymentStatus.Authorized || context.Payment.PaymentStatus == PaymentStatus.Cancelled))
			{
				try
				{
					var config = GetConfigMap();
					var service = new PayPalAPIInterfaceServiceService(config);
					DoCaptureReq doCaptureRequest = GetDoCaptureRequest(context.Payment);

					var doCaptureResponse = service.DoCapture(doCaptureRequest);

					CheckResponse(doCaptureResponse);

					if(doCaptureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.COMPLETED)
					{
						retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
						context.Payment.CapturedDate = DateTime.UtcNow;
						context.Payment.IsApproved = true;
						retVal.IsSuccess = true;
					}
				}
				catch(Exception ex)
				{
					retVal.ErrorMessage = ex.Message;
				}
			}

			return retVal;
		}

		public override RefundProcessPaymentResult RefundProcessPayment(RefundProcessPaymentEvaluationContext context)
		{
			if (context == null || context.Payment == null)
				throw new ArgumentNullException("paymentEvaluationContext");

			RefundProcessPaymentResult retVal = new RefundProcessPaymentResult();

			if (context.Payment.IsApproved && (context.Payment.PaymentStatus == PaymentStatus.Paid || context.Payment.PaymentStatus == PaymentStatus.Cancelled))
			{
				try
				{
					var config = GetConfigMap();
					var service = new PayPalAPIInterfaceServiceService(config);
					RefundTransactionReq refundTransctionRequest = GetRefundTransactionRequest(context.Payment);
					service.RefundTransaction(refundTransctionRequest);
				}
				catch(Exception ex)
				{
					retVal.ErrorMessage = ex.Message;
				}
			}

			return new RefundProcessPaymentResult { ErrorMessage = "Not implemented yet" };
		}

		public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
		{
			var retVal = new ValidatePostProcessRequestResult();

			var cancel = queryString["cancel"];
			var token = queryString["token"];

			if (!string.IsNullOrEmpty(cancel) && !string.IsNullOrEmpty(token))
			{
				bool cancelValue;
				if (bool.TryParse(cancel, out cancelValue))
				{
					retVal.IsSuccess = !cancelValue;
					retVal.OuterId = token;
				}
			}

			return retVal;
		}

		#region Private methods

		private SetExpressCheckoutReq GetSetExpressCheckoutRequest(CustomerOrder order, Store store, PaymentIn payment)
		{
			var retVal = new SetExpressCheckoutReq();

			var request = new SetExpressCheckoutRequestType();

			var ecDetails = new SetExpressCheckoutRequestDetailsType
			{
				CallbackTimeout = "3",
				ReturnURL = string.Format("{0}/{1}?cancel=false&orderId={2}", store.Url, PaypalPaymentRedirectRelativePath, order.Id),
				CancelURL = string.Format("{0}/{1}?cancel=true&orderId={2}", store.Url, PaypalPaymentRedirectRelativePath, order.Id)
			};

			if (_paypalPaymentModeStoreSetting.Equals("BankCard"))
			{
				ecDetails.SolutionType = SolutionTypeType.SOLE;
				ecDetails.LandingPage = LandingPageType.BILLING;
			}
			else
			{
				ecDetails.SolutionType = SolutionTypeType.MARK;
				ecDetails.LandingPage = LandingPageType.LOGIN;
			}

			var billingAddress = order.Addresses.FirstOrDefault(s => s.AddressType == VirtoCommerce.Domain.Order.Model.AddressType.Billing);

			if (billingAddress != null)
				ecDetails.BuyerEmail = billingAddress.Email;
			else
				billingAddress = order.Addresses.FirstOrDefault();

			if (billingAddress != null && !string.IsNullOrEmpty(billingAddress.Email))
				ecDetails.BuyerEmail = billingAddress.Email;

			ecDetails.PaymentDetails.Add(GetPaypalPaymentDetail(order.Currency.ToString(), PaypalPaymentActionType, payment));

			request.SetExpressCheckoutRequestDetails = ecDetails;

			retVal.SetExpressCheckoutRequest = request;

			return retVal;
		}

		private DoCaptureReq GetDoCaptureRequest(PaymentIn payment)
		{
			var retVal = new DoCaptureReq();

			retVal.DoCaptureRequest = new DoCaptureRequestType();
			retVal.DoCaptureRequest.Amount = new BasicAmountType(GetPaypalCurrency(payment.Currency.ToString()), FormatMoney(payment.Sum));
			retVal.DoCaptureRequest.AuthorizationID = payment.OuterId;
			retVal.DoCaptureRequest.CompleteType = CompleteCodeType.COMPLETE;

			return retVal;
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
						PaymentDetails = response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails,
						PaymentAction = PaypalPaymentActionType
					}
				}
			};
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

		private RefundTransactionReq GetRefundTransactionRequest(PaymentIn payment)
		{
			var retVal = new RefundTransactionReq();

			retVal.RefundTransactionRequest = new RefundTransactionRequestType();
			retVal.RefundTransactionRequest.TransactionID = payment.OuterId;
			//retVal.RefundTransactionRequest.Amount

			return retVal;
		}

		private PaymentDetailsType GetPaypalPaymentDetail(string currency, PaymentActionCodeType paymentAction, PaymentIn payment)
		{
			var paymentDetails = new PaymentDetailsType { PaymentAction = paymentAction };
			paymentDetails.OrderTotal = new BasicAmountType(GetPaypalCurrency(currency), FormatMoney(payment.Sum));

			return paymentDetails;
		}

		private Dictionary<string, string> GetConfigMap()
		{
			var retVal = new Dictionary<string, string>();

			retVal.Add(PaypalModeConfigSettingName, Mode);
			retVal.Add(PaypalUsernameConfigSettingName, APIUsername);
			retVal.Add(PaypalPasswordConfigSettingName, APIPassword);
			retVal.Add(PaypalSignatureConfigSettingName, APISignature);

			return retVal;
		}

		private CurrencyCodeType GetPaypalCurrency(string currency)
		{
			return (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), currency);
		}

		private PaymentActionCodeType GetPaymentActionType(string actionType)
		{
			return (PaymentActionCodeType)Enum.Parse(typeof(PaymentActionCodeType), actionType);
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

		private string FormatMoney(decimal amount)
		{
			return amount.ToString("F2", new CultureInfo("en-US"));
		}

		private bool CheckResponse(AbstractResponseType response)
		{
			if (response != null)
			{
				if (response.Errors != null && response.Errors.Count > 0)
				{
					StringBuilder sb = new StringBuilder();
					foreach (var error in response.Errors)
					{
						sb.Append("LongMessage: ").Append(error.LongMessage).Append(Environment.NewLine);
						sb.Append("ShortMessage: ").Append(error.ShortMessage).Append(Environment.NewLine);
						sb.Append("ErrorCode: ").Append(error.ErrorCode).Append(Environment.NewLine);
					}

					throw new NullReferenceException(sb.ToString());
				}
				else if(!(response.Ack == AckCodeType.SUCCESS) && !(response.Ack == AckCodeType.SUCCESSWITHWARNING))
				{
					throw new NullReferenceException("Paypal error without detail info");
				}
			}
			else
			{
				throw new NullReferenceException("response in null");
			}

			return true;
		}

		#endregion
	}
}