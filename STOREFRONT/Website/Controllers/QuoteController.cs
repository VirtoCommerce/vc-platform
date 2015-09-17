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
        public async Task AddItem(string variantId)
        {
            var product = await Service.GetProductAsync(Context, variantId);

            var quoteItem = product.ToQuoteItem();

            Context.QuoteRequest.AddItem(quoteItem);

            Context.QuoteRequest = await QuoteService.UpdateQuoteRequestAsync(Context.QuoteRequest);
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
        public async Task<ActionResult> CreateOrder(QuoteRequest quoteRequest)
        {
            Context.QuoteRequest.Comment = quoteRequest.Comment;
            Context.QuoteRequest.FirstName = quoteRequest.FirstName;
            Context.QuoteRequest.Language = quoteRequest.LastName;

            foreach (var quoteItem in quoteRequest.Items)
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

            return null;
        }
    }
}