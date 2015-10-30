using CyberSource.Clients;
using CyberSource.Clients.SoapServiceReference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Commerce.Model;
using System.Text;
using System.Globalization;

namespace Cyber.Source.Managers
{
    public class CyberSourceMethod : PaymentMethod
    {
        private const string _cyberSourceMerchantIdStoreSetting = "CyberSource.MerchantId";
        private const string _cyberSourceMerchantReferenceCodeStoreSetting = "CyberSource.MerchantReferenceCode";
        private const string _cyberSourcePaymentMethodStoreSetting = "CyberSource.PaymentMethod";
        private const string _cyberSourceWorkModeStoreSetting = "CyberSource.WorkMode";
        private const string _cyberSourceThankYouPageRelativeUrl = "CyberSource.ThankYouPageRelativeUrl";
        public CyberSourceMethod() : base("CyberSource") { }

        public override PaymentMethodGroupType PaymentMethodGroupType
        {
            get
            {
                return PaymentMethodGroupType.BankCard;
            }
        }

        public override PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        public string MerchantId
        {
            get
            {
                return GetSetting(_cyberSourceMerchantIdStoreSetting);
            }
        }

        public string MerchantReferenceCode
        {
            get
            {
                return GetSetting(_cyberSourceMerchantReferenceCodeStoreSetting);
            }
        }

        public string PaymentMethod
        {
            get
            {
                return GetSetting(_cyberSourcePaymentMethodStoreSetting);
            }
        }

        public string WorkMode
        {
            get
            {
                return GetSetting(_cyberSourceWorkModeStoreSetting);
            }
        }

        public string ThankYouPageRelativeUrl
        {
            get
            {
                return GetSetting(_cyberSourceThankYouPageRelativeUrl);
            }
        }

        public override CaptureProcessPaymentResult CaptureProcessPayment(CaptureProcessPaymentEvaluationContext context)
        {
            var retVal = new CaptureProcessPaymentResult();

            var request = PrepareCaptureProcessPaymentRequest(context);

            var reply = NVPClient.RunTransaction(request);
            if (reply != null && reply.ContainsKey("decision") && reply.ContainsKey("reasonCode"))
            {
                var decision = (string)reply["decision"];
                var reasonCode = int.Parse((string)reply["reasonCode"]);
                var isAccept = decision.Equals("ACCEPT", StringComparison.InvariantCultureIgnoreCase);
                var isSuccessReasonCode = reasonCode == 100;
                if (isAccept && isSuccessReasonCode)
                {
                    context.Payment.OuterId = (string)reply["requestID"];
                    retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
                    context.Payment.CapturedDate = DateTime.UtcNow;
                    context.Payment.IsApproved = true;
                    retVal.IsSuccess = true;
                }
                else
                {
                    if (reasonCode == 101)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is {2}", decision, reasonCode, EnumerateValues(reply, "missingField")));
                    if (reasonCode == 102)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is {2}", decision, reasonCode, EnumerateValues(reply, "invalidField")));
                    if (reasonCode == 204)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is not enough funds", decision, reasonCode));

                    throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}", decision, reasonCode));
                }
            }
            else
            {
                throw new NullReferenceException("no reply from cyber source");
            }

            return retVal;
        }

        public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
        {
            var retVal = new ProcessPaymentResult();

            var request = PrepareProcessPaymentRequest(context);

            var reply = NVPClient.RunTransaction(request);
            if (reply != null && reply.ContainsKey("decision") && reply.ContainsKey("reasonCode"))
            {
                var decision = (string)reply["decision"];
                var reasonCode = int.Parse((string)reply["reasonCode"]);
                var isAccept = decision.Equals("ACCEPT", StringComparison.InvariantCultureIgnoreCase);
                var isSuccessReasonCode = reasonCode == 100;
                if (isAccept && isSuccessReasonCode)
                {
                    context.Payment.OuterId = (string)reply["requestID"];
                    if (IsSeparatePaymentAction())
                    {
                        retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Authorized;
                        context.Payment.AuthorizedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
                        context.Payment.AuthorizedDate = context.Payment.CapturedDate = DateTime.UtcNow;
                        context.Payment.IsApproved = true;
                    }
                    retVal.IsSuccess = true;
                    retVal.RedirectUrl = string.Format("{0}/{1}?id={2}", context.Store.Url, ThankYouPageRelativeUrl, context.Order.Id);
                }
                else
                {
                    if (reasonCode == 101)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is {2}", decision, reasonCode, EnumerateValues(reply, "missingField")));
                    if (reasonCode == 102)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is {2}", decision, reasonCode, EnumerateValues(reply, "invalidField")));
                    if (reasonCode == 204)
                        throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}, full info of reason is not enough funds", decision, reasonCode));

                    throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}", decision, reasonCode));
                }
            }
            else
            {
                throw new NullReferenceException("no reply from cyber source");
            }

            return retVal;
        }

        public override RefundProcessPaymentResult RefundProcessPayment(RefundProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
        {
            return new ValidatePostProcessRequestResult { IsSuccess = false };
        }

        public override VoidProcessPaymentResult VoidProcessPayment(VoidProcessPaymentEvaluationContext context)
        {
            var retVal = new VoidProcessPaymentResult();

            Hashtable request = new Hashtable();

            request.Add("ccAuthReversalService_run", "true");
            request.Add("ccAuthReversalService_authRequestID", context.Payment.OuterId);
            request.Add("merchantID", MerchantId);
            request.Add("merchantReferenceCode", context.Payment.Number);
            request.Add("purchaseTotals_currency", context.Payment.Currency.ToString());
            request.Add("item_0_unitPrice", context.Payment.Sum.ToString());
            //request.Add("item_0_quantity", "1");

            var reply = NVPClient.RunTransaction(request);
            if (reply != null && reply.ContainsKey("decision") && reply.ContainsKey("reasonCode"))
            {
                var decision = (string)reply["decision"];
                var reasonCode = int.Parse((string)reply["reasonCode"]); ;
                var isAccept = decision.Equals("ACCEPT", StringComparison.InvariantCultureIgnoreCase);
                var isSuccessReasonCode = reasonCode == 100;
                if (isAccept && isSuccessReasonCode)
                {
                    retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Voided;
                    context.Payment.VoidedDate = context.Payment.CancelledDate = DateTime.UtcNow;
                    retVal.IsSuccess = true;
                }
                else
                {
                    throw new NullReferenceException(string.Format("result from cyber source, not success, decision is {0}, reasonCode is {1}", decision, reasonCode));
                }
            }
            else
            {
                throw new NullReferenceException("no reply from cyber source");
            }

            return retVal;
        }

        private Hashtable PrepareProcessPaymentRequest(ProcessPaymentEvaluationContext context)
        {
            Hashtable request = new Hashtable();
            request.Add("merchantID", MerchantId);
            request.Add("merchantReferenceCode", context.Payment.Number);

            if (IsSeparatePaymentAction())
            {
                request.Add("ccAuthService_run", "true");
            }
            else
            {
                request.Add("ccAuthService_run", "true");
                request.Add("ccCaptureService_run", "true");
            }

            if(IsTest())
                request.Add("sendToProduction", "false");
            else
                request.Add("sendToProduction", "true");

            // Set billing address of payment as address for request
            var address = context.Payment.BillingAddress;
            
            // Set first billing address in order
            if (address == null && context.Order.Addresses != null)
                address = context.Order.Addresses.FirstOrDefault(a => a.AddressType == AddressType.Billing || a.AddressType == AddressType.BillingAndShipping);
            
            // Set any first address in order
            if (address == null)
                address = context.Order.Addresses.FirstOrDefault();

            // Throw exception, is no address
            if (address == null)
                throw new NullReferenceException("No address, payment can't be processed");

            SetBillingAddress(address, request);
            SetCreditCardInfo(context.BankCardInfo, request);

            request.Add("purchaseTotals_currency", context.Payment.Currency.ToString());
            request.Add("item_0_unitPrice", context.Payment.Sum.ToString(CultureInfo.InvariantCulture));
            //request.Add("item_0_quantity", "1");

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Hashtable PrepareCaptureProcessPaymentRequest(CaptureProcessPaymentEvaluationContext context)
        {
            Hashtable request = new Hashtable();

            if (IsTest())
                request.Add("sendToProduction", "false");
            else
                request.Add("sendToProduction", "true");

            request.Add("ccCaptureService_authRequestID", context.Payment.OuterId);
            request.Add("merchantID", MerchantId);
            request.Add("merchantReferenceCode", context.Payment.Number);
            request.Add("ccCaptureService_run", "true");
            request.Add("purchaseTotals_currency", context.Payment.Currency.ToString());
            request.Add("item_0_unitPrice", context.Payment.Sum.ToString());
            //request.Add("item_0_quantity", "1");

            return request;
        }

        private void SetBillingAddress(VirtoCommerce.Domain.Commerce.Model.Address address, Hashtable request)
        {
            request.Add("billTo_firstName", address.FirstName);
            request.Add("billTo_lastName", address.LastName);
            request.Add("billTo_street1", address.Line1);

            if (!string.IsNullOrEmpty(address.Line2))
                request.Add("billTo_street2", address.Line2);

            request.Add("billTo_city", address.City);
            request.Add("billTo_state", address.RegionName);
            request.Add("billTo_postalCode", address.PostalCode);
            request.Add("billTo_country", address.CountryName);

            if (!string.IsNullOrEmpty(address.Email))
                request.Add("billTo_email", address.Email);

            if (!string.IsNullOrEmpty(address.Phone))
                request.Add("billTo_phone", address.Phone);
        }

        private void SetCreditCardInfo(BankCardInfo bankCardInfo, Hashtable request)
        {
            request.Add("card_accountNumber", bankCardInfo.BankCardNumber);
            request.Add("card_expirationMonth", bankCardInfo.BankCardMonth);
            request.Add("card_expirationYear", bankCardInfo.BankCardYear);
        }

        private bool IsSeparatePaymentAction()
        {
            var retVal = false;
            if(PaymentMethod.Equals("Authorization/Capture"))
            {
                retVal = false;
            }
            return retVal;
        }

        private bool IsTest()
        {
            var retVal = true;
            if (WorkMode.ToLower().Equals("live"))
            {
                retVal = false;
            }
            return retVal;
        }

        private static string EnumerateValues(Hashtable reply, string fieldName)
        {
            StringBuilder builder = new StringBuilder();
            string val = "";
            for (int i = 0; val != null; ++i)
            {
                val = (string)reply[fieldName + "_" + i];
                if (val != null)
                {
                    builder.Append(val + "\n");
                }
            }
            return (builder.ToString());
        }
    }
}