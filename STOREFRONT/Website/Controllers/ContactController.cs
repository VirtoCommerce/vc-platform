using System;
using System.Collections.Generic;
using System.Linq;
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

            return new EmptyResult();
        }
    }
}