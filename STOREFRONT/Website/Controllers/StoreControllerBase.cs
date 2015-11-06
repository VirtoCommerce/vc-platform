using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Routing;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Services;

namespace VirtoCommerce.Web.Controllers
{
    [Canonicalized(typeof(AccountController)/*, typeof(CheckoutController)*/, Order = 1)]
    public abstract class StoreControllerBase : Controller
    {
        private CustomerService _customerService;
        private SecurityService _securityService;
        private QuotesService _quoteService;
        private CommerceService _service;

        #region Properties
        protected SiteContext Context
        {
            get
            {
                return SiteContext.Current;
            }
        }

        protected CustomerService CustomerService
        {
            get
            {
                return this._customerService ?? (this._customerService = new CustomerService());
            }
        }

        protected QuotesService QuoteService
        {
            get
            {
                return _quoteService ?? (_quoteService = new QuotesService());
            }
        }

        protected SecurityService SecurityService
        {
            get
            {
                return this._securityService ?? (this._securityService = new SecurityService());
            }
        }

        protected CommerceService Service
        {
            get
            {
                return this._service ?? (this._service = CommerceService.Create());
            }
        }
        #endregion

        #region Methods
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if(viewName != null)
                this.Context.Template = viewName;
            return base.View(viewName, masterName, model ?? this.Context);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            if (viewName != null)
                this.Context.Template = viewName;
            return base.PartialView(viewName, model ?? this.Context);
        }

        protected SubmitForm GetForm(string formId)
        {
            SubmitForm form = null;

            form = SiteContext.Current.Forms.FirstOrDefault(f => f.Id == formId);

            return form;
        }

        protected async Task<ICollection<DynamicProperty>> GetDynamicPropertiesAsync()
        {
            var propertyClient = ClientContext.Clients.CreateDynamicPropertyClient();
            var properties = await propertyClient.GetDynamicPropertiesForTypeAsync("VirtoCommerce.Domain.Customer.Model.Contact");

            foreach (var property in properties.Where(x => x.IsDictionary))
            {
                property.DictionaryItems = await propertyClient.GetDynamicPropertyDictionaryItemsAsync("VirtoCommerce.Domain.Customer.Model.Contact", property.Id);
            }

            return properties.Select(x => x.ToViewModel()).ToList();
        }

        protected async Task<ICollection<DynamicProperty>> PopulateDynamicPropertiesAsync(IDictionary<string, string> formProperties)
        {
            var dynamicProperties = await GetDynamicPropertiesAsync();

            foreach (var dynamicProperty in dynamicProperties)
            {
                dynamicProperty.Values = new List<string>();
                string value = null;
                if (formProperties.TryGetValue(dynamicProperty.Name, out value))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (dynamicProperty.ValueType.Equals("Boolean"))
                        {
                            value = "true";
                        }

                        if (dynamicProperty.IsDictionary)
                        {
                            var dictionaryItem = dynamicProperty.DictionaryItems.FirstOrDefault(di => di.Name == value);
                            if (dictionaryItem != null)
                            {
                                dynamicProperty.Values.Add(dictionaryItem.Name);
                            }
                        }
                        else
                        {
                            dynamicProperty.Values.Add(value);
                        }
                    }
                }
                else
                {
                    if (dynamicProperty.ValueType.Equals("Boolean"))
                    {
                        dynamicProperty.Values.Add("false");
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return dynamicProperties;
        }

        protected void ValidateDynamicProperties(ICollection<DynamicProperty> dynamicProperties)
        {
            foreach (var dynamicProperty in dynamicProperties)
            {
                if (dynamicProperty.IsRequired)
                {
                    if (dynamicProperty.Values == null || dynamicProperty.Values != null && !dynamicProperty.Values.Any())
                    {
                        ModelState.AddModelError(dynamicProperty.Name, string.Format("Field \"{0}\" is required", dynamicProperty.Name));
                    }
                }
            }
        }

        protected SubmitFormErrors GetFormErrors(ModelStateDictionary modelState)
        {
            SubmitFormErrors formErrors = null;

            if (!modelState.IsValid)
            {
                var errorsDictionary = new Dictionary<string, string>();

                foreach (var error in modelState.Where(f => f.Value.Errors.Count > 0))
                {
                    var errorMessage = error.Value.Errors.First();
                    errorsDictionary.Add(error.Key, errorMessage.ErrorMessage);
                }

                formErrors = new SubmitFormErrors(errorsDictionary);
            }

            return formErrors;
        }

        protected virtual void SetPageMeta(SeoKeyword keyword)
        {
            if (keyword == null)
            {
                return;
            }

            var ctx = SiteContext.Current;
            ctx.PageDescription = keyword.MetaDescription;
            ctx.PageTitle = keyword.Title;
            ctx.PageKeywords = keyword.MetaKeywords;
        }
        #endregion
    }
}