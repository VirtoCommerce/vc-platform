using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models.Forms;

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("contact")]
    public class ContactController : StoreControllerBase
    {
        [Route("")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendContact(ContactFormModel model)
        {
            var form = Service.GetForm(SiteContext.Current, model.form_type);
            if (form != null)
            {
                form.PostedSuccessfully = true;
                // TODO: add sending email to a store owner
            }

            string url = Request.UrlReferrer.GetLeftPart(UriPartial.Path);
            url += (Request.QueryString.ToString() == "") ? "?contact_posted=true" : "?" + Request.QueryString.ToString() + "&contact_posted=true";
            var result = new RedirectResult(url);
            return await Task.FromResult(result);
        }
    }
}
