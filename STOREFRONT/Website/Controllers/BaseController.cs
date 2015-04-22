using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;

namespace VirtoCommerce.Web.Controllers
{
    public class BaseController : Controller
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

        protected void UpdateForms(IEnumerable<SubmitForm> forms, bool shouldRemoveExisting = false)
        {
            if (shouldRemoveExisting)
            {
                Session.Remove("Forms");
            }

            var sessionForms = Session["Forms"] as ICollection<SubmitForm>;

            if (sessionForms == null)
            {
                Session.Add("Forms", forms);
                Context.Forms = forms.ToArray();
            }
            else
            {
                var updatedForms = new List<SubmitForm>();

                foreach (var sessionForm in sessionForms)
                {
                    var updatingForm = forms.FirstOrDefault(f => f.Id == sessionForm.Id);

                    if (updatingForm != null)
                    {
                        updatedForms.Add(updatingForm);
                    }
                    else
                    {
                        sessionForm.Errors = null;
                        updatedForms.Add(sessionForm);
                    }
                }

                Session["Forms"] = updatedForms;
                Context.Forms = updatedForms.ToArray();
            }
        }

        protected SubmitForm GetForm(string formId)
        {
            SubmitForm form = null;

            if (Session["Forms"] != null)
            {
                var forms = Session["Forms"] as ICollection<SubmitForm>;

                if (forms != null)
                {
                    form = forms.FirstOrDefault(f => f.Id == formId);
                }
            }

            return form;
        }

        protected SubmitFormErrors GetFormErrors(ModelStateDictionary modelState)
        {
            SubmitFormErrors formErrors = null;

            if (!modelState.IsValid)
            {
                var errorsDisctionary = new Dictionary<string, string>();

                foreach (var error in modelState.Where(f => f.Value.Errors.Count > 0))
                {
                    var errorMessage = error.Value.Errors.First();
                    errorsDisctionary.Add(error.Key, errorMessage.ErrorMessage);
                }

                formErrors = new SubmitFormErrors(errorsDisctionary);
            }

            return formErrors;
        }
        #endregion
    }
}