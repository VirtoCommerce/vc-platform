using System;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [HandleJsonError]
    public class ApiAccountController : StorefrontControllerBase
    {
        public ApiAccountController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder)
            : base(workContext, urlBuilder)
        {
        }

        // GET: storefrontapi/account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetCurrentCustomer()
        {
            foreach (var address in WorkContext.CurrentCustomer.Addresses)
            {
                address.Id = Guid.NewGuid().ToString();
            }

            return Json(WorkContext.CurrentCustomer);
        }
    }
}