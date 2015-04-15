using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.FormModels;

namespace VirtoCommerce.Web.Controllers
{
    public class CheckoutController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Step1");
        }

        [HttpGet]
        [Route("checkout/step-1")]
        public async Task<ActionResult> Step1()
        {
            if (this.Context.Cart.ItemCount == 0)
            {
                return View("cart");
            }

            this.Context.Checkout = await this.Service.GetCheckoutAsync();

            if (this.Context.Checkout != null)
            {
                this.Context.Checkout.Email = this.Context.Customer != null ? this.Context.Customer.Email : null;
                this.Context.Checkout.ShippingAddress = this.Context.Customer != null ? this.Context.Customer.DefaultAddress : new CustomerAddress();
                this.Context.Checkout.Currency = this.Context.Shop.Currency;
            }

            return View("checkout-step-1");
        }

        [HttpPost]
        public async Task<ActionResult> Step1(CheckoutFirstStepFormModel formModel)
        {
            this.Context.Checkout = await this.Service.GetCheckoutAsync();
            this.Context.Checkout.Currency = this.Context.Shop.Currency;

            var form = this.Service.GetForm(formModel.form_type);

            if (form != null)
            {
                if (!ModelState.IsValid)
                {
                    var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                    form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                    return this.View("checkout-step-1");
                }

                var customer = await this.CustomerService.GetCustomerAsync(formModel.Email, this.Context.Shop.StoreId);

                if (customer == null)
                {
                    var account = await this.SecurityService.GetUserByNameAsync(formModel.Email);

                    if (account == null)
                    {
                        await this.SecurityService.RegisterUser(
                            formModel.Email, formModel.FirstName, formModel.LastName, null, this.Context.StoreId);
                    }

                    var user = await this.SecurityService.GetUserByNameAsync(formModel.Email);

                    var customerAddresses = new List<CustomerAddress>();
                    customerAddresses.Add(new CustomerAddress
                    {
                        Address1 = formModel.Address1,
                        Address2 = formModel.Address2,
                        City = formModel.City,
                        Company = formModel.Company,
                        Country = formModel.Country,
                        CountryCode = "RUS",
                        FirstName = formModel.FirstName,
                        LastName = formModel.LastName,
                        Phone = formModel.Phone,
                        Province = formModel.Province,
                        Zip = formModel.Zip
                    });

                    customer = await this.CustomerService.CreateCustomerAsync(
                        formModel.Email, formModel.FirstName, formModel.LastName, user.Id, customerAddresses);
                }

                this.Context.Customer = customer;
                this.Context.Checkout.Email = customer.Email;

                this.Context.Checkout.ShippingAddress = new CustomerAddress
                {
                    Address1 = formModel.Address1,
                    Address2 = formModel.Address2.Length > 0 ? formModel.Address2 : null,
                    City = formModel.City,
                    Company = formModel.Company.Length > 0 ? formModel.Company : null,
                    Country = formModel.Country,
                    CountryCode = "RUS",
                    FirstName = formModel.FirstName,
                    LastName = formModel.LastName,
                    Phone = formModel.Phone.Length > 0 ? formModel.Phone : null,
                    Province = formModel.Province,
                    Zip = formModel.Zip
                };

                await this.Service.UpdateCheckoutAsync(this.Context.Checkout);
            }

            return RedirectToAction("Step2");
        }

        [HttpGet]
        [Route("checkout/step-2")]
        public async Task<ActionResult> Step2()
        {
            this.Context.Checkout = await this.Service.GetCheckoutAsync();

            if (this.Context.Checkout.ShippingAddress == null || !this.Context.Checkout.ShippingAddress.IsFilledCorrectly)
            {
                return View("checkout-step-1");
            }

            return View("checkout-step-2");
        }

        [HttpPost]
        public async Task<ActionResult> Step2(CheckoutSecondStepFormModel formModel)
        {
            var form = this.Service.GetForm(formModel.form_type);

            if (form != null)
            {
                if (!ModelState.IsValid)
                {
                    var errors = this.ModelState.Values.SelectMany(v => v.Errors);
                    form.Errors = new[] { errors.Select(e => e.ErrorMessage).FirstOrDefault() };

                    return this.View("checkout-step-2");
                }

                this.Context.Checkout = await this.Service.GetCheckoutAsync();
                this.Context.Checkout.Currency = this.Context.Shop.Currency;

                this.Context.Checkout.BillingAddress = new CustomerAddress
                {
                    Address1 = formModel.Address1,
                    Address2 = formModel.Address2.Length > 0 ? formModel.Address2 : null,
                    City = formModel.City,
                    Company = formModel.Company.Length > 0 ? formModel.Company : null,
                    Country = formModel.Country,
                    CountryCode = "RUS",
                    FirstName = formModel.FirstName,
                    LastName = formModel.LastName,
                    Phone = formModel.Phone.Length > 0 ? formModel.Phone : null,
                    Province = formModel.Province,
                    Zip = formModel.Zip
                };

                this.Context.Checkout.ShippingMethod = this.Context.Checkout.ShippingMethods.FirstOrDefault(sm => sm.Handle == formModel.ShippingMethodId);
                this.Context.Checkout.PaymentMethod = this.Context.Checkout.PaymentMethods.FirstOrDefault(pm => pm.Handle == formModel.PaymentMethodId);

                this.Context.Customer = await this.CustomerService.GetCustomerAsync(this.Context.Checkout.Email, this.Context.Shop.StoreId);
                this.Context.Customer.Addresses.Add(this.Context.Checkout.ShippingAddress);

                await this.CustomerService.UpdateCustomerAsync(this.Context.Customer);

                this.Context.Checkout.CustomerId = this.Context.Customer.Id;
            }

            var order = await this.Service.CreateOrderAsync(this.Context.Checkout);

            return RedirectToAction("Order", "Account", new { Id = order.Id });
        }
    }
}