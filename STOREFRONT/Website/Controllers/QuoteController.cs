using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            Context.QuoteRequest.AddItem(quoteItem);

            Context.QuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.QuoteRequest);

            return Json(Context.QuoteRequest);
        }

        [HttpGet]
        [Route("remove")]
        public async Task<ActionResult> RemoveItem(string id)
        {
            Context.QuoteRequest.RemoveItem(id);

            Context.QuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.QuoteRequest);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("submit")]
        public async Task<ActionResult> Submit(QuoteRequest model)
        {
            Context.QuoteRequest.Comment = model.Comment;
            Context.QuoteRequest.FirstName = model.FirstName;
            Context.QuoteRequest.Language = model.LastName;
            Context.QuoteRequest.Tag = null;
            Context.QuoteRequest.IsSubmitted = true;

            foreach (var quoteItem in model.Items)
            {
                var existingQuoteItem = Context.QuoteRequest.Items.FirstOrDefault(i => i.Id == quoteItem.Id);

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

            Context.QuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.QuoteRequest);

            return Json(Context.QuoteRequest);
        }
    }
}