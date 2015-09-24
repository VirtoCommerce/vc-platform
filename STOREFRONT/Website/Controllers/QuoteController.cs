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
        [Route("recalculate")]
        public async Task<ActionResult> Recalculate(QuoteRequest model)
        {
            Context.QuoteRequest = await QuoteService.GetByNumberAsync(Context.StoreId, Context.CustomerId, model.Number);

            foreach (var modelQuoteItem in model.Items)
            {
                var quoteItem = Context.QuoteRequest.Items.FirstOrDefault(i => i.Id == modelQuoteItem.Id);
                quoteItem.SelectedTierPrice = modelQuoteItem.SelectedTierPrice;
            }

            Context.QuoteRequest = await QuoteService.RecalculateAsync(Context.QuoteRequest);

            return Json(Context.QuoteRequest);
        }

        [HttpPost]
        [Route("submit")]
        public async Task<ActionResult> Submit(QuoteRequest model)
        {
            if (!ProposalPricesUnique(model.Items))
            {
                return Json(new { errorMessage = "Proposal prices quantities must be unique." });
            }

            Context.ActualQuoteRequest.Comment = model.Comment;
            Context.ActualQuoteRequest.FirstName = model.FirstName;
            Context.ActualQuoteRequest.Language = model.LastName;

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

            if (User.Identity.IsAuthenticated)
            {
                Context.ActualQuoteRequest.Tag = null;
                Context.ActualQuoteRequest.CustomerName = Context.Customer.Name;
            }

            await QuoteService.UpdateQuoteRequestAsync(Context.ActualQuoteRequest);

            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = VirtualPathUtility.ToAbsolute("~/quote");
                return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/account/login?returnUrl=" + returnUrl) });
            }

            return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/account/quote/" + Context.ActualQuoteRequest.Number) });
        }

        private bool ProposalPricesUnique(ICollection<QuoteItem> quoteItems)
        {
            bool isUnique = true;

            foreach (var quoteItem in quoteItems)
            {
                var uniqueProposalPrices = quoteItem.ProposalPrices.GroupBy(pp => pp.Quantity).Select(pp => pp.First());
                if (quoteItem.ProposalPrices.Count != uniqueProposalPrices.Count())
                {
                    isUnique = false;
                    break;
                }
            }

            return isUnique;
        }
    }
}