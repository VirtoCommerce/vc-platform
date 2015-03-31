#region
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;

#endregion

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
                return this._securityService ?? (this._securityService = new SecurityService(this.HttpContext));
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
        #endregion
    }
}