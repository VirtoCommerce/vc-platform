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
            this.Context.Checkout = await this.Service.GetCheckoutAsync();

            if (this.Context.Checkout != null)
            {
                this.Context.Checkout.Email = this.Context.Customer != null ? this.Context.Customer.Email : null;
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

                this.Context.Checkout.Email = formModel.Email;
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

                var customer = await this.CustomerService.GetCustomerAsync(this.Context.Checkout.Email, this.Context.Shop.StoreId);

                if (customer == null)
                {
                    await this.SecurityService.RegisterUser(this.Context.Checkout.Email, null, null, "12345", this.Context.Shop.StoreId);
                }

                this.Context.Customer = await this.CustomerService.GetCustomerAsync(this.Context.Checkout.Email, this.Context.Shop.StoreId);

                this.Context.Checkout.CustomerId = this.Context.Customer.Id;

                await this.Service.CreateOrderAsync(this.Context.Checkout);
            }

            return View("thanks-page");
        }
    }
}