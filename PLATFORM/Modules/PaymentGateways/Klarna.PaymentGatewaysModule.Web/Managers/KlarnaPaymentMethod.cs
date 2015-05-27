using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Payment.Model;

namespace Klarna.PaymentGatewaysModule.Web.Managers
{
    public class KlarnaPaymentMethod : VirtoCommerce.Domain.Payment.Model.PaymentMethod
    {
        //private const string _testBaseUrl = "https://checkout.testdrive.klarna.com/checkout/orders";
        //private const string _contentType = "application/vnd.klarna.checkout.aggregated-order-v2+json";

        private const string _klarnaModeStoreSetting = "Klarna.Mode";
        private const string _klarnaAppKeyStoreSetting = "Klarna.AppKey";
        private const string _klarnaAppSecretStoreSetting = "Klarna.AppSecret";

        public KlarnaPaymentMethod()
            : base("Klarna")
        {
        }

        private string Mode
        {
            get
            {
                var retVal = GetSetting(_klarnaModeStoreSetting);
                return retVal;
            }
        }

        private string AppKey
        {
            get
            {
                var retVal = GetSetting(_klarnaAppKeyStoreSetting);
                return retVal;
            }
        }

        private string AppSecret
        {
            get
            {
                var retVal = GetSetting(_klarnaAppSecretStoreSetting);
                return retVal;
            }
        }


        public override PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.PreparedForm; }
        }

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
        {
            var retVal = new ProcessPaymentResult();

            //if (context.Order != null && context.Store != null && context.Payment != null)
            //{
            //	KlarnaLocalization localization;
            //	if(context.Order.Shipments != null && context.Order.Shipments.Count > 0)
            //	{
            //		localization = GetLocalization(context.Order.Currency.ToString(), context.Order.Shipments.FirstOrDefault().DeliveryAddress.CountryName);
            //	}
            //	else
            //	{
            //		localization = GetLocalization(context.Order.Currency.ToString(), null);
            //	}

            //	if (localization != null)
            //	{
            //		var cartItems = CreateKlarnaCartItems(context.Order);

            //		//Create cart
            //		var cart = new Dictionary<string, object> { { "items", cartItems } };
            //		var data = new Dictionary<string, object>
            //		{
            //			{ "cart", cart }
            //		};

            //		//Create klarna order
            //		var connector = Connector.Create(AppSecret);
            //		Order order = null;
            //		var merchant = new Dictionary<string, object>
            //		{
            //			{ "id", AppKey },
            //			{ "terms_uri", klarnaPaymentInfo.TermsUrl },
            //			{ "checkout_uri", klarnaPaymentInfo.CheckoutUrl },
            //			{ "confirmation_uri", klarnaPaymentInfo.ConfirmationUrl },
            //			{ "push_uri", klarnaPaymentInfo.PushUrl }
            //		};

            //		data.Add("purchase_country", localization.CountryName);
            //		data.Add("purchase_currency", localization.Currency);
            //		data.Add("locale", localization.Locale);
            //		data.Add("merchant", merchant);
            //		order =
            //			new Order(connector)
            //			{
            //				BaseUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders"),
            //				ContentType = ContentType
            //			};
            //		order.Create(data);
            //		order.Fetch();

            //		//Gets snippet
            //		var gui = order.GetValue("gui") as JObject;
            //		var html = gui["snippet"].Value<string>();

            //		retVal.IsSuccess = true;
            //		retVal.NewPaymentStatus = PaymentStatus.Pending;
            //		retVal.HtmlForm = html;
            //		//retVal.OuterId = order.GetValue
            //	}
            //	else
            //	{
            //		retVal.Error = "Klarna is not available for this order";
            //	}
            //}

            return retVal;
        }

        public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
        {
            var retVal = new PostProcessPaymentResult();

            var paymentEvaluationContext = context as PostProcessPaymentEvaluationContext;

            return retVal;
        }

        private List<Dictionary<string, object>> CreateKlarnaCartItems(CustomerOrder order)
        {
            var cartItems = new List<Dictionary<string, object>>();
            foreach (var lineItem in order.Items)
            {
                var addedItem = new Dictionary<string, object>();

                addedItem.Add("type", "physical");

                if (!string.IsNullOrEmpty(lineItem.Name))
                {
                    addedItem.Add("name", lineItem.Name);
                }
                if (lineItem.Quantity > 0)
                {
                    addedItem.Add("quantity", lineItem.Quantity);
                }
                if (lineItem.BasePrice > 0)
                {
                    addedItem.Add("unit_price", lineItem.BasePrice);
                }
                cartItems.Add(addedItem);
            }

            return cartItems;
        }

        private class KlarnaLocalization
        {
            public string CountryName { get; set; }
            public string Locale { get; set; }
            public string Currency { get; set; }
        }

        private List<KlarnaLocalization> GetLocalizations()
        {
            var retVal = new List<KlarnaLocalization>();

            retVal.Add(new KlarnaLocalization { CountryName = "Sweden", Currency = "SEK", Locale = "sv-se" });
            retVal.Add(new KlarnaLocalization { CountryName = "Finland", Currency = "EUR", Locale = "fi-fi" });
            retVal.Add(new KlarnaLocalization { CountryName = "Norway", Currency = "NOK", Locale = "nb-no" });
            retVal.Add(new KlarnaLocalization { CountryName = "Germany", Currency = "EUR", Locale = "de-de" });
            retVal.Add(new KlarnaLocalization { CountryName = "Austria", Currency = "EUR", Locale = "de-at" });

            return retVal;
        }

        private KlarnaLocalization GetLocalization(string currency, string country)
        {
            var localizations = GetLocalizations();
            if (string.IsNullOrEmpty(country))
            {
                return localizations.FirstOrDefault(l => l.Currency == currency && l.CountryName == country);
            }
            else
            {
                return localizations.FirstOrDefault(l => l.Currency == currency);
            }
        }
    }
}