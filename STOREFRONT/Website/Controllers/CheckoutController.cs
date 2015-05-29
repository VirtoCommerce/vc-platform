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
                        CountryCode = "RUS",
                        FirstName = formModel.FirstName,
                        LastName = formModel.LastName,
                        Phone = !string.IsNullOrEmpty(formModel.Phone) ? formModel.Phone : null,
                        Province = formModel.Province,
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
            var checkout = await Service.GetCheckoutAsync();

            if (checkout.ShippingAddress == null || !checkout.ShippingAddress.IsFilledCorrectly)
            {
                return RedirectToAction("Step1", "Checkout");
            }

            checkout.ShippingMethods = await Service.GetShippingMethodsAsync(checkout.Id);
            checkout.PaymentMethods = await Service.GetPaymentMethodsAsync(checkout.Id);

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
                        CountryCode = "RUS",
                        FirstName = formModel.FirstName,
                        LastName = formModel.LastName,
                        Phone = !string.IsNullOrEmpty(formModel.Phone) ? formModel.Phone : null,
                        Province = formModel.Province,
                        Zip = formModel.Zip
                    };

                    checkout.BillingAddress = billingAddress;

                    checkout.ShippingMethods = await Service.GetShippingMethodsAsync(checkout.Id);
                    checkout.PaymentMethods = await Service.GetPaymentMethodsAsync(checkout.Id);

                    if (checkout.RequiresShipping)
                    {
                        checkout.ShippingMethod = checkout.ShippingMethods.FirstOrDefault(sm => sm.Handle == formModel.ShippingMethodId);
                    }

                    checkout.PaymentMethod = checkout.PaymentMethods.FirstOrDefault(pm => pm.Handle == formModel.PaymentMethodId);

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
                        var paymentResult = await Service.ProcessPaymentAsync(dtoOrder.Id, inPayment.Id);

                        if (paymentResult != null)
                        {
                            if(paymentResult.IsSuccess)
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
                            }
                            else
                            {
                                Context.ErrorMessage = paymentResult.Error;

                                return View("error");
                            }
                        }
                    }
                }
                else
                {
                    form.Errors = formErrors;
                    form.PostedSuccessfully = false;

                    return View("checkout-step-2");
                }
            }

            return View("error");
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

                    Context.Order = await CustomerService.GetOrderAsync(Context.StoreId, Context.CustomerId, orderId);
                    return View("thanks_page");
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
    }
}