using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Routing;
using VirtoCommerce.Web.Models.Services;

namespace VirtoCommerce.Web.Controllers
{
    [Canonicalized(typeof(AccountController)/*, typeof(CheckoutController)*/, Order = 1)]
    public abstract class StoreControllerBase : Controller
    {
        private CustomerService _customerService;
        private SecurityService _securityService;
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
                return this._service ?? (this._service = new CommerceService());
            }
        }
        #endregion

        #region Methods
        protected override void Initialize(RequestContext requestContext)
        {
            requestContext.HttpContext.Items["theme"] = this.Context.Theme.ToString();
            base.Initialize(requestContext);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            this.Context.Template = viewName;
            return base.View(viewName, masterName, model ?? this.Context);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            this.Context.Template = viewName;
            return base.PartialView(viewName, model ?? this.Context);
        }

        protected SubmitForm GetForm(string formId)
        {
            SubmitForm form = null;

            form = SiteContext.Current.Forms.FirstOrDefault(f => f.Id == formId);

            return form;
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