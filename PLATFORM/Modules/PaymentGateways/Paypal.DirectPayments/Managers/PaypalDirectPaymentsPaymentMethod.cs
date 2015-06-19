using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace Paypal.DirectPayments.Managers
{
    public class PaypalDirectPaymentsPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
    {
        private const string _paypalApiModeStoreSetting = "Paypal.DirectPayments.Mode";
        private const string _paypalApiUserNameStoreSetting = "Paypal.DirectPayments.APIUsername";
        private const string _paypalApiPasswordStoreSetting = "Paypal.DirectPayments.APIPassword";
        private const string _paypalApiSignatureStoreSetting = "Paypal.DirectPayments.APISignature";
        private const string _paypalPaymentRedirectRelativePathStoreSetting = "Paypal.DirectPayments.PaymentRedirectRelativePath";

        //private const string _paypalModeConfigSettingName = "mode";
        //private const string _paypalUsernameConfigSettingName = "account1.apiUsername";
        //private const string _paypalPasswordConfigSettingName = "account1.apiPassword";
        //private const string _paypalSignatureConfigSettingName = "account1.apiSignature";

        //private const string _sandboxPaypalBaseUrlFormat = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";
        //private const string _livePaypalBaseUrlFormat = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&useraction=commit&token={0}";


        public PaypalDirectPaymentsPaymentMethod()
            : base("Paypal.DirectPayments")
        {
        }

        private string Mode
        {
            get
            {
                var retVal = GetSetting(_paypalApiModeStoreSetting);
                return retVal;
            }
        }

        private string APIUsername
        {
            get
            {
                var retVal = GetSetting(_paypalApiUserNameStoreSetting);
                return retVal;
            }
        }

        private string APIPassword
        {
            get
            {
                var retVal = GetSetting(_paypalApiPasswordStoreSetting);
                return retVal;
            }
        }

        private string APISignature
        {
            get
            {
                var retVal = GetSetting(_paypalApiSignatureStoreSetting);
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

        public override PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.Standard; }
        }

        public override PaymentMethodGroupType PaymentMethodGroupType
        {
            get { return PaymentMethodGroupType.BankCard; }
        }

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
        {
            var retVal = new ProcessPaymentResult();

            if (!(context.Store != null && !string.IsNullOrEmpty(context.Store.Url)))
                throw new NullReferenceException("no store with this id");

            var doDirectPaymentRequest = GetDoDirectPaymentRequest(context);



            //send request
            var service = GetService();
            var response = service.DoDirectPayment(doDirectPaymentRequest);

            string error;
            bool success = CheckResponse(response, out error);

            if (success)
            {
                retVal.OuterId = response.TransactionID;
                retVal.IsSuccess = (response.Ack.Value == AckCodeType.SUCCESS || response.Ack.Value == AckCodeType.SUCCESSWITHWARNING);
                retVal.NewPaymentStatus = retVal.IsSuccess ? PaymentStatus.Paid : PaymentStatus.Voided;
            }
            else
            {
                retVal.Error = error;
            }

            return retVal;
        }

        public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
        {
            return new ValidatePostProcessRequestResult();
        }

        #region Private methods

        private PayPalAPIInterfaceServiceService GetService()
        {
            var config = new Dictionary<string, string>();

            var isSandbox = Mode.ToLower().Equals("sandbox");

            var url = isSandbox ? "https://api-3t.sandbox.paypal.com/2.0" : "https://api-3t.paypal.com/2.0";

            config.Add("PayPalAPI", url);
            config.Add("mode", Mode);
            config.Add("account0.apiUsername", APIUsername);
            config.Add("account0.apiPassword", APIPassword);
            config.Add("account0.apiSignature", APISignature);

            var service = new PayPalAPIInterfaceServiceService(config);
            return service;
        }

        private DoDirectPaymentReq GetDoDirectPaymentRequest(ProcessPaymentEvaluationContext context)
        {
            var retVal = new DoDirectPaymentReq();

            retVal.DoDirectPaymentRequest = new DoDirectPaymentRequestType();
            retVal.DoDirectPaymentRequest.Version = GetApiVersion();
            var details = new DoDirectPaymentRequestDetailsType();
            retVal.DoDirectPaymentRequest.DoDirectPaymentRequestDetails = details;
            details.PaymentAction = PaymentActionCodeType.SALE;

            //credit card
            details.CreditCard = GetCreditCardDetails(context);


            //order totals
            details.PaymentDetails = new PaymentDetailsType();
            details.PaymentDetails.OrderTotal = new BasicAmountType();
            details.PaymentDetails.OrderTotal.value = FormatMoney(context.Payment.Sum);
            details.PaymentDetails.OrderTotal.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), context.Order.Currency.ToString());
            details.PaymentDetails.Custom = context.Order.Id;
            details.PaymentDetails.ButtonSource = "virtoCommerceManagerCart";

            return retVal;
        }

        private CreditCardDetailsType GetCreditCardDetails(ProcessPaymentEvaluationContext context)
        {
            var retVal = new CreditCardDetailsType();

            retVal.CreditCardNumber = context.BankCardInfo.BankCardNumber;
			retVal.CreditCardType = GetPaypalCreditCardType(context.BankCardInfo.BankCardType);
			retVal.ExpMonth = context.BankCardInfo.BankCardMonth;
			retVal.ExpYear = context.BankCardInfo.BankCardYear;
			retVal.CVV2 = context.BankCardInfo.BankCardCVV2;
            retVal.CardOwner = new PayerInfoType();

            if (context.Order.Addresses.Any(x => x.AddressType == VirtoCommerce.Domain.Order.Model.AddressType.Billing))
            {
                var billingAddress = context.Order.Addresses.FirstOrDefault(x => x.AddressType == VirtoCommerce.Domain.Order.Model.AddressType.Billing);
                retVal.CardOwner.PayerCountry = GetPaypalCountryCodeType(billingAddress.CountryCode);

                retVal.CardOwner.Address = new PayPal.PayPalAPIInterfaceService.Model.AddressType();
                retVal.CardOwner.Address.Street1 = billingAddress.Line1;
                retVal.CardOwner.Address.Street2 = billingAddress.Line2;
                retVal.CardOwner.Address.CityName = billingAddress.City;
                retVal.CardOwner.Address.StateOrProvince = billingAddress.RegionName;

                retVal.CardOwner.Address.Country = GetPaypalCountryCodeType(billingAddress.CountryCode);
                retVal.CardOwner.Address.PostalCode = billingAddress.Zip;
                retVal.CardOwner.Payer = billingAddress.Email;
                retVal.CardOwner.PayerName = new PersonNameType();
                retVal.CardOwner.PayerName.FirstName = billingAddress.FirstName;
                retVal.CardOwner.PayerName.LastName = billingAddress.LastName;
            }
            else
            {
                throw new NullReferenceException("no billing address");
            }

            return retVal;
        }

        private CountryCodeType GetPaypalCountryCodeType(string threeLetterCountryCode)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var region = regions.FirstOrDefault(x => x.ThreeLetterISORegionName.Contains(threeLetterCountryCode));

            var payerCountry = CountryCodeType.US;
            if (region != null)
            {
                try
                {
                    payerCountry = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), region.TwoLetterISORegionName.ToUpperInvariant());
                }
                catch
                {
                }
            }
            return payerCountry;
        }

        private CreditCardTypeType GetPaypalCreditCardType(string creditCardType)
        {
            if (String.IsNullOrEmpty(creditCardType))
                return CreditCardTypeType.VISA;

            if (creditCardType.Equals("VISA", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.VISA;
            if (creditCardType.Equals("MASTERCARD", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.MASTERCARD;
            if (creditCardType.Equals("DISCOVER", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.DISCOVER;
            if (creditCardType.Equals("AMEX", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.AMEX;
            if (creditCardType.Equals("MAESTRO", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.MAESTRO;
            if (creditCardType.Equals("SOLO", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.SOLO;
            if (creditCardType.Equals("SWITCH", StringComparison.InvariantCultureIgnoreCase))
                return CreditCardTypeType.SWITCH;

            return (CreditCardTypeType)Enum.Parse(typeof(CreditCardTypeType), creditCardType);
        }

        private string GetApiVersion()
        {
            return "117";
        }

        private string FormatMoney(decimal amount)
        {
            return amount.ToString("F2", new CultureInfo("en-US"));
        }

        private bool CheckResponse(AbstractResponseType response, out string error)
        {
            var retVal = false;

            if (response != null)
            {
                if ((response.Errors != null && response.Errors.Count > 0) || !(response.Ack.Equals(AckCodeType.SUCCESS) || response.Ack.Equals(AckCodeType.SUCCESSWITHWARNING)))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var err in response.Errors)
                    {
                        sb.AppendLine(err.LongMessage);
                    }

                    error = sb.ToString();
                }
                else
                {
                    error = string.Empty;
                }

                retVal = true;
            }
            else
            {
                error = "response in null";
            }

            return retVal;
        }

        #endregion
    }
}