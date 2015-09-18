using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("quote")]
    public class QuoteController : StoreControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View("quote");
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> AddItem(string variantId)
        {
            var product = await Service.GetProductAsync(Context, variantId);

            var quoteItem = product.ToQuoteItem();

            Context.ActualQuoteRequest.AddItem(quoteItem);

            Context.ActualQuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.ActualQuoteRequest);

            return Json(Context.ActualQuoteRequest);
        }

        [HttpGet]
        [Route("remove")]
        public async Task<ActionResult> RemoveItem(string id)
        {
            Context.ActualQuoteRequest.RemoveItem(id);

            Context.ActualQuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.ActualQuoteRequest);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("submit")]
        public async Task<ActionResult> Submit(QuoteRequest model)
        {
            Context.ActualQuoteRequest.Comment = model.Comment;
            Context.ActualQuoteRequest.FirstName = model.FirstName;
            Context.ActualQuoteRequest.Language = model.LastName;
            Context.ActualQuoteRequest.Tag = null;
            Context.ActualQuoteRequest.IsSubmitted = true;

            foreach (var quoteItem in model.Items)
            {
                var existingQuoteItem = Context.ActualQuoteRequest.Items.FirstOrDefault(i => i.Id == quoteItem.Id);

                existingQuoteItem.Comment = quoteItem.Comment;

                existingQuoteItem.ProposalPrices = new List<TierPrice>();
                foreach (var tearPrice in quoteItem.ProposalPrices)
                {
                    existingQuoteItem.ProposalPrices.Add(new TierPrice
                    {
                        Quantity = tearPrice.Quantity,
                        Price = existingQuoteItem.ListPrice
                    });
                }
            }

            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = VirtualPathUtility.ToAbsolute("~/quote");
                return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/account/login?returnUrl=" + returnUrl) });
            }

            Context.ActualQuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.ActualQuoteRequest);

            return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/") });
        }
    }
}