using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using VirtoCommerce.Domain.Payment.Model;

namespace DiBs.Managers
{
    public class DibsPaymentMethod : PaymentMethod
    {
        private const string md5ParameterString = "merchant={0}&orderid={1}&currency={2}&amount={3}";
        private const string md5ResponseString = "transact={0}&amount={1}&currency={2}";

        #region constants

        private const string redirectUrl = "DiBs.RedirectUrl";
        private const string acceptUrl = "DiBs.AcceptUrl";
        private const string mode = "DiBs.Mode";
        private const string callbackUrl = "DiBs.CallbackUrl";
        private const string md5Key1 = "DiBs.MD5Key1";
        private const string md5Key2 = "DiBs.MD5Key2";
        private const string merchantId = "DiBs.MerchantId";
        private const string formDesign = "DiBs.FormDesign";

        private const string md5KeyFormDataName = "md5key";
        private const string acceptUrlFormDataName = "accepturl";
        private const string callbackUrlFormDataName = "callbackurl";
        private const string merchantIdFormDataName = "merchant";
        private const string amountFormDataName = "amount";
        private const string orderIdFormDataName = "orderid";
        private const string orderInternalIdFormDataName = "orderinternalid";
        private const string currencyFormDataName = "currency";
        private const string testModeFormDataName = "test";
        private const string languageFormDataName = "lang";
        private const string cancelUrlFormDataName = "cancelurl";
        private const string decoratorFormDataName = "decorator";

        #endregion

        public DibsPaymentMethod(string code) : base(code) { }

        #region settings

        public string RedirectUrl
        {
            get { return GetSetting(redirectUrl); }
        }

        public string AcceptUrl
        {
            get { return GetSetting(acceptUrl); }
        }

        public string Mode
        {
            get { return GetSetting(mode); }
        }

        public string CallbackUrl
        {
            get { return GetSetting(callbackUrl); }
        }

        public string MD5Key1
        {
            get { return GetSetting(md5Key1); }
        }

        public string MD5Key2
        {
            get { return GetSetting(md5Key2); }
        }

        public string MerchantId
        {
            get { return GetSetting(merchantId); }
        }

        public string FormDecoarator
        {
            get { return GetSetting(formDesign); }
        }

        #endregion

        public override PaymentMethodGroupType PaymentMethodGroupType
        {
            get
            {
                return PaymentMethodGroupType.Alternative;
            }
        }

        public override PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.PreparedForm;
            }
        }

        public override CaptureProcessPaymentResult CaptureProcessPayment(CaptureProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
        {
            var transactionId = context.Parameters["transact"];

            var retVal = new PostProcessPaymentResult();
            retVal.NewPaymentStatus = context.Payment.PaymentStatus = PaymentStatus.Paid;
            context.Payment.CapturedDate = DateTime.UtcNow;
            context.Payment.IsApproved = true;
            retVal.OuterId = context.Payment.OuterId = transactionId;
            context.Payment.AuthorizedDate = DateTime.UtcNow;
            retVal.OrderId = context.Order.Id;
            retVal.IsSuccess = ValidatePostProcessRequest(context.Parameters).IsSuccess;

            return retVal;
        }

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
        {
            var retVal = new ProcessPaymentResult();

            if (context.Order != null && context.Store != null && context.Payment != null)
            {
                if (!(!string.IsNullOrEmpty(context.Store.SecureUrl) || !string.IsNullOrEmpty(context.Store.Url)))
                    throw new NullReferenceException("store must specify Url or SecureUrl property");

                var baseStoreUrl = string.Empty;
                if (!string.IsNullOrEmpty(context.Store.SecureUrl))
                {
                    baseStoreUrl = context.Store.SecureUrl;
                }
                else
                {
                    baseStoreUrl = context.Store.Url;
                }

                var orderId = context.Order.Number;

                //get md5 hash passing the order number, currency ISO code and order total
                var md5Hash = CalculateMD5Hash(orderId, (int)context.Order.Currency, (int)(context.Order.Sum * 100));

                var reqparm = new NameValueCollection();
                reqparm.Add(acceptUrlFormDataName, AcceptUrl);
                reqparm.Add(callbackUrlFormDataName, CallbackUrl);
                reqparm.Add(cancelUrlFormDataName, AcceptUrl);
                reqparm.Add(merchantIdFormDataName, MerchantId);
                reqparm.Add(orderIdFormDataName, orderId);
                reqparm.Add(orderInternalIdFormDataName, context.Order.Id);
                reqparm.Add(amountFormDataName, ((int)(context.Order.Sum * 100)).ToString());
                reqparm.Add(currencyFormDataName, ((int)context.Order.Currency).ToString());
                reqparm.Add(languageFormDataName, context.Store.DefaultLanguage.Substring(0, 2));
                reqparm.Add(md5KeyFormDataName, md5Hash);
                reqparm.Add(decoratorFormDataName, FormDecoarator);

                if (Mode == "test")
                {
                    reqparm.Add(testModeFormDataName, "1");
                }

                //build form to post to FlexWin
                var checkoutform = string.Empty;
                                
                checkoutform += string.Format("<form name='dibs' action='{0}' method='POST' charset='UTF-8'>", RedirectUrl);
                checkoutform += "<p>You'll be redirected to DIBS payment in a moment. If not, click the 'Procced' button...</p>";

                const string paramTemplateString = "<INPUT TYPE='hidden' name='{0}' value='{1}'>";
                foreach (string key in reqparm)
                    checkoutform += string.Format(paramTemplateString, key, reqparm[key]);
                checkoutform += "<button type='submit'>Proceed</button>";
                checkoutform += "</form>";

                checkoutform += "<script language='javascript'>document.dibs.submit();</script>";
                                
                retVal.HtmlForm = checkoutform;
                retVal.IsSuccess = true;
                retVal.NewPaymentStatus = PaymentStatus.Pending;
            }
            return retVal;
        }

        public override RefundProcessPaymentResult RefundProcessPayment(RefundProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
        {
            var retVal = new ValidatePostProcessRequestResult();
            retVal.OuterId = queryString["transact"];

            //calculate hash by transaction id, currency code and amount
            var md5Hash = CalculateResponseMD5Hash(queryString["transact"], queryString["currency"], queryString["amount"]);

            //compare calculated hash with the passed in response authkey field
            retVal.IsSuccess = md5Hash.Equals(queryString["authkey"]);

            return retVal;
        }

        public override VoidProcessPaymentResult VoidProcessPayment(VoidProcessPaymentEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        private string GetMD5Hash(string datastr)
        {
            HashAlgorithm mhash = new MD5CryptoServiceProvider();
            string res = string.Empty;

            byte[] bytValue = Encoding.UTF8.GetBytes(datastr);

            byte[] bytHash = mhash.ComputeHash(bytValue);

            mhash.Clear();

            for (int i = 0; i < bytHash.Length; i++)
            {
                if (bytHash[i] < 16)
                {
                    res += "0" + bytHash[i].ToString("x");
                }
                else
                {
                    res += bytHash[i].ToString("x");
                }
            }

            return res;
        }

        private string CalculateMD5Hash(string orderId, int currency, int amount)
        {
            var md5 = string.Format(md5ParameterString, MerchantId, orderId, currency, amount);
            md5 = GetMD5Hash(MD5Key2 + GetMD5Hash(MD5Key1 + md5));

            return md5;
        }

        private string CalculateResponseMD5Hash(string transact, string currency, string amount)
        {
            var md5 = string.Format(md5ResponseString, transact, amount, currency);
            md5 = GetMD5Hash(MD5Key2 + GetMD5Hash(MD5Key1 + md5));

            return md5;
        }
    }
}