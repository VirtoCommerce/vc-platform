using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.FormModels;
using VirtoCommerce.Web.Models.Routing;
using VirtoCommerce.Web.Models.Storage;

namespace VirtoCommerce.Web.Controllers
{
    [Canonicalized(typeof(CheckoutController))]
    public class CheckoutController : StoreControllerBase
    {
        //
        // GET: /Checkout
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Step1", "Checkout");
        }

        //
        // GET: /Checkout/Step1
        [HttpGet]
        [Route("checkout/step-1")]
        public async Task<ActionResult> Step1()
        {
            if (Context.Cart.ItemCount == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var checkout = await Service.GetCheckoutAsync();

            if (User.Identity.IsAuthenticated)
            {
                if (checkout.ShippingAddress == null)
                {
                    checkout.ShippingAddress = Context.Customer.Addresses.LastOrDefault();
                }
            }

            Context.Checkout = checkout;

            if (checkout.RequiresShipping)
            {
                return View("checkout-step-1");
            }
            else
            {
                return RedirectToAction("Step2", "Checkout");
            }
        }

        //
        // POST: /Checkout/Step1
        [HttpPost]
        public async Task<ActionResult> Step1(CheckoutFirstStepFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            var checkout = await Service.GetCheckoutAsync();

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    form.PostedSuccessfully = true;

                    var shippingAddress = new CustomerAddress
                    {
                        Address1 = formModel.Address1,
                        Address2 = !string.IsNullOrEmpty(formModel.Address2) ? formModel.Address2 : null,
                        City = formModel.City,
                        Company = !string.IsNullOrEmpty(formModel.Company) ? formModel.Company : null,
                        Country = formModel.Country,
                        CountryCode = "US", //TODO Set country code by selected country name
                        FirstName = formModel.FirstName,
                        LastName = formModel.LastName,
                        Phone = !string.IsNullOrEmpty(formModel.Phone) ? formModel.Phone : null,
                        Province = formModel.Province,
                        ProvinceCode = "CA", //TODO Set province code by selected province name
                        Zip = formModel.Zip
                    };

                    checkout.Currency = Context.Shop.Currency;
                    checkout.Email = formModel.Email;
                    checkout.ShippingAddress = shippingAddress;

                    if (User.Identity.IsAuthenticated)
                    {
                        var customer = await this.CustomerService.GetCustomerAsync(formModel.Email, Context.Shop.StoreId);
                        if (customer != null)
                        {
                            customer.Addresses.Add(shippingAddress);
                            await CustomerService.UpdateCustomerAsync(customer);
                        }
                    }

                    await Service.UpdateCheckoutAsync(checkout);

                    return RedirectToAction("Step2", "Checkout");
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    return RedirectToAction("Step1", "Checkout");
                }
            }

            Context.ErrorMessage = "Liquid error: Form context was not found.";

            return View("error");
        }

        //
        // GET: /Checkout/Step2
        [HttpGet]
        [Route("checkout/step-2")]
        public async Task<ActionResult> Step2()
        {
            if (Context.Cart.ItemCount == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var checkout = await Service.GetCheckoutAsync();

            if (checkout.RequiresShipping && checkout.ShippingAddress == null ||
                checkout.ShippingAddress != null && !checkout.ShippingAddress.IsFilledCorrectly)
            {
                return RedirectToAction("Step1", "Checkout");
            }

            checkout.ShippingMethods = await Service.GetShippingMethodsAsync(checkout.Id);
            checkout.PaymentMethods = await Service.GetCartPaymentMethodsAsync(checkout.Id);

            if (User.Identity.IsAuthenticated)
            {
                if (checkout.BillingAddress == null)
                {
                    checkout.BillingAddress = Context.Customer.Addresses.LastOrDefault();
                }
            }

            Context.Checkout = checkout;

            return View("checkout-step-2");
        }

        //
        // POST: /Checkout/Step2
        [HttpPost]
        public async Task<ActionResult> Step2(CheckoutSecondStepFormModel formModel)
        {
            var form = GetForm(formModel.form_type);

            if (form != null)
            {
                var formErrors = GetFormErrors(ModelState);

                if (formErrors == null)
                {
                    var checkout = await Service.GetCheckoutAsync();

                    if (!checkout.RequiresShipping)
                    {
                        checkout.Email = formModel.Email;
                    }

                    var billingAddress = new CustomerAddress
                    {
                        Address1 = formModel.Address1,
                        Address2 = !string.IsNullOrEmpty(formModel.Address2) ? formModel.Address2 : null,
                        City = formModel.City,
                        Company = !string.IsNullOrEmpty(formModel.Company) ? formModel.Company : null,
                        Country = formModel.Country,
                        CountryCode = "US", //TODO Set country code by selected country name
                        FirstName = formModel.FirstName,
                        LastName = formModel.LastName,
                        Phone = !string.IsNullOrEmpty(formModel.Phone) ? formModel.Phone : null,
                        Province = formModel.Province,
                        ProvinceCode = "CA", //TODO Set province code by selected province name
                        Zip = formModel.Zip
                    };

                    checkout.BillingAddress = billingAddress;

                    checkout.ShippingMethods = await Service.GetShippingMethodsAsync(checkout.Id);
                    checkout.PaymentMethods = await Service.GetCartPaymentMethodsAsync(checkout.Id);

                    if (checkout.RequiresShipping)
                    {
                        checkout.ShippingMethod = checkout.ShippingMethods.FirstOrDefault(sm => sm.Handle == formModel.ShippingMethodId);
                    }

                    checkout.PaymentMethod = checkout.PaymentMethods.FirstOrDefault(pm => pm.Code == formModel.PaymentMethodId);
                    if (checkout.PaymentMethod.Type.Equals("Standard", StringComparison.OrdinalIgnoreCase))
                    {
                        checkout.PaymentMethod.CardNumber = formModel.CardNumber;
                        checkout.PaymentMethod.CardExpirationMonth = formModel.CardExpirationMonth;
                        checkout.PaymentMethod.CardExpirationYear = formModel.CardExpirationYear;
                        checkout.PaymentMethod.CardCvv = formModel.CardCvv;
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        var customer = await CustomerService.GetCustomerAsync(checkout.Email, Context.Shop.StoreId);
                        if (customer != null)
                        {
                            customer.Addresses.Add(billingAddress);
                            await this.CustomerService.UpdateCustomerAsync(customer);
                        }
                    }

                    await Service.UpdateCheckoutAsync(checkout);

                    var dtoOrder = await Service.CreateOrderAsync();

                    checkout.Order = dtoOrder.AsWebModel();
                    Context.Checkout = checkout;

                    var inPayment = dtoOrder.InPayments.FirstOrDefault(); // For test

                    if (inPayment != null)
                    {
                        string cardType = GetCreditCardType(formModel.CardNumber);

                        if (!String.IsNullOrEmpty(cardType))
                        {
                            var cardInfo = new ApiClient.DataContracts.BankCardInfo
                            {
                                BankCardCVV2 = formModel.CardCvv,
                                BankCardMonth = int.Parse(formModel.CardExpirationMonth),
                                BankCardNumber = formModel.CardNumber,
                                BankCardType = cardType,
                                BankCardYear = int.Parse(formModel.CardExpirationYear)
                            };

                            var paymentResult = await Service.ProcessPaymentAsync(dtoOrder.Id, inPayment.Id, cardInfo);

                            if (paymentResult != null)
                            {
                                if (paymentResult.IsSuccess)
                                {
                                    if (paymentResult.PaymentMethodType == ApiClient.DataContracts.PaymentMethodType.Redirection)
                                    {
                                        if (!string.IsNullOrEmpty(paymentResult.RedirectUrl))
                                        {
                                            return Redirect(paymentResult.RedirectUrl);
                                        }
                                    }
                                    if (paymentResult.PaymentMethodType == ApiClient.DataContracts.PaymentMethodType.PreparedForm)
                                    {
                                        if (!string.IsNullOrEmpty(paymentResult.HtmlForm))
                                        {
                                            SiteContext.Current.Set("payment_html_form", paymentResult.HtmlForm);
                                            return View("payment");
                                        }
                                    }
                                    if (paymentResult.PaymentMethodType == ApiClient.DataContracts.PaymentMethodType.Standard)
                                    {
                                        var productsIds = dtoOrder.Items.Select(i => i.ProductId);
                                        var catalogItems = await Service.GetCatalogItemsByIdsAsync(productsIds, "ItemAssets");

                                        var nonShippingProducts = catalogItems.Where(ci => ci.ProductType == "Digital");
                                        if (nonShippingProducts.Count() > 0)
                                        {
                                            var downloadLinks = new List<ProductDownloadLinks>();

                                            foreach (var nonShippingProduct in nonShippingProducts)
                                            {
                                                var productLinks = new ProductDownloadLinks
                                                {
                                                    ProductName = nonShippingProduct.Name
                                                };

                                                int linkCount = 1;
                                                foreach (var asset in nonShippingProduct.Assets)
                                                {
                                                    var url = Url.Action("Index", "Download", new { @file = asset.Name, @oid = dtoOrder.Id, @pid = nonShippingProduct.Id }, Request.Url.Scheme);
                                                    productLinks.Links.Add(new DownloadLink
                                                    {
                                                        Filename = asset.Name,
                                                        Text = nonShippingProduct.Assets.Count() > 1 ? String.Format("Download link {0}", linkCount) : "Download link",
                                                        Url = url
                                                    });

                                                    linkCount++;
                                                }

                                                downloadLinks.Add(productLinks);
                                            }

                                            Context.Set("download_links", downloadLinks);
                                        }

                                        Context.Order = dtoOrder.AsWebModel();

                                        return View("thanks_page");
                                    }
                                }
                                else
                                {
                                    Context.ErrorMessage = paymentResult.Error;

                                    return View("error");
                                }
                            }
                        }
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    return RedirectToAction("Step2");
                }
            }

            return View("error");
        }

        //
        // GET: /checkout/bankcardform
        [HttpGet]
        public ActionResult BankCardForm()
        {
            return PartialView("bank_card_form");
        }

        //
        // GET: /checkout/externalpaymentcallback
        [HttpGet]
        public async Task<ActionResult> ExternalPaymentCallback()
        {
            var parameters = new List<KeyValuePair<string, string>>();

            foreach (var key in HttpContext.Request.QueryString.AllKeys)
            {
                parameters.Add(new KeyValuePair<string, string>(key, HttpContext.Request.QueryString[key]));
            }

            var postPaymentResult = await Service.PostPaymentProcessAsync(parameters);

            if (postPaymentResult != null)
            {
                if (postPaymentResult.IsSuccess)
                {
                    string orderId = HttpContext.Request.QueryString["orderId"];

                    var order = await CustomerService.GetOrderAsync(Context.StoreId, Context.CustomerId, orderId);

                    if (order != null)
                    {
                        var productsIds = order.Items.Select(i => i.ProductId);
                        var catalogItems = await Service.GetCatalogItemsByIdsAsync(productsIds, "ItemAssets");

                        var nonShippingProducts = catalogItems.Where(ci => ci.ProductType == "Digital");
                        if (nonShippingProducts.Count() > 0)
                        {
                            var downloadLinks = new List<ProductDownloadLinks>();

                            foreach (var nonShippingProduct in nonShippingProducts)
                            {
                                var productLinks = new ProductDownloadLinks
                                {
                                    ProductName = nonShippingProduct.Name
                                };

                                int linkCount = 1;
                                foreach (var asset in nonShippingProduct.Assets)
                                {
                                    var url = Url.Action("Index", "Download", new { @file = asset.Name, @oid = order.Id, @pid = nonShippingProduct.Id }, Request.Url.Scheme);
                                    productLinks.Links.Add(new DownloadLink
                                    {
                                        Filename = asset.Name,
                                        Text = nonShippingProduct.Assets.Count() > 1 ? String.Format("Download link {0}", linkCount) : "Download link",
                                        Url = url
                                    });

                                    linkCount++;
                                }

                                downloadLinks.Add(productLinks);
                            }

                            Context.Set("download_links", downloadLinks);
                        }

                        Context.Order = order.AsWebModel();
                        return View("thanks_page");
                    }
                }
                else
                {
                    Context.ErrorMessage = postPaymentResult.Error;
                    return View("error");
                }
            }

            return View("error");
        }

        //
        // GET: /checkout/recalculateshippingmethod
        [HttpGet]
        public async Task<ActionResult> RecalculateShippingMethod(string id)
        {
            var checkout = await Service.GetCheckoutAsync();

            var shippingMethods = await Service.GetShippingMethodsAsync(SiteContext.Current.Cart.Key);

            if (shippingMethods != null)
            {
                checkout.ShippingMethod = shippingMethods.FirstOrDefault(sm => sm.Handle == id);

                var culture = GetCultureInfoByCurrencyCode(SiteContext.Current.Shop.Currency);

                checkout.StringifiedShippingPrice = checkout.ShippingPrice.ToString("C", culture);
                checkout.StringifiedTotalPrice = checkout.TotalPrice.ToString("C", culture);
            }

            return Json(checkout);
        }

        //
        // GET: /checkout/validatecardnumber
        [HttpGet]
        public JsonResult ValidateCardNumber(string cardNumber)
        {
            string type = GetCreditCardType(cardNumber);

            if (type != null)
            {
                return Json(type, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private CultureInfo GetCultureInfoByCurrencyCode(string currencyCode)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var culture = CultureInfo.GetCultureInfo(SiteContext.Current.Shop.DefaultLanguage);

            foreach (var ci in cultures)
            {
                var ri = new RegionInfo(ci.LCID);

                if (ri.ISOCurrencySymbol == currencyCode)
                {
                    break;
                }
            }

            return culture;
        }

        private string GetCreditCardType(string cardNumber)
        {
            string cardType = null;

            if (cardNumber.All(c => c >= '0' && c <= '9'))
            {
                if (cardNumber.Length == 15)
                {
                    if (cardNumber[0] == '3' && (cardNumber[1] == '4' || cardNumber[1] == '7'))
                    {
                        cardType = "AMERICAN EXPRESS";
                    }
                }
                if (cardNumber.Length == 14)
                {
                    int first8Digits = int.Parse(cardNumber.Substring(0, 8));
                    if (first8Digits >= 60110000 && first8Digits <= 60119999 ||
                        first8Digits >= 65000000 && first8Digits <= 65999999 ||
                        first8Digits >= 62212600 && first8Digits <= 62292599)
                    {
                        cardType = "DISCOVER";
                    }
                }
                if (cardNumber.Length == 16)
                {
                    if (cardNumber[0] == '5' && cardNumber[1] >= '1' && cardNumber[1] <= '5')
                    {
                        cardType = "MASTERCARD";
                    }
                }
                if (cardNumber.Length == 13 || cardNumber.Length == 16)
                {
                    if (cardNumber[0] == '4')
                    {
                        cardType = "VISA";
                    }
                }
            }

            return cardType;
        }
    }
}