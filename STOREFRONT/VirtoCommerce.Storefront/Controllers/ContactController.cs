using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    [RoutePrefix("contact")]
    public class ContactController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly IVirtoCommercePlatformApi _platformApi;

        public ContactController(WorkContext context, IVirtoCommercePlatformApi platformApi)
        {
            _platformApi = platformApi;
            _workContext = context;
        }


        [Route("")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendContact(ContactForm model)
        {
            /*
            var form = Service.GetForm(SiteContext.Current, model.form_type);
            if (form != null)
            {
                form.PostedSuccessfully = true;
                // TODO: add sending email to a store owner
                //_platformApi.NotificationsSendNotificationAsync
            }
            */

            string url = Request.UrlReferrer.GetLeftPart(UriPartial.Path);
            url += (Request.QueryString.ToString() == "") ? "?contact_posted=true" : "?" + Request.QueryString.ToString() + "&contact_posted=true";
            var result = new RedirectResult(url);
            return await Task.FromResult(result);
        }
    }
}