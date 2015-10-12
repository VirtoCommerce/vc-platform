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

            Context.ActualQuoteRequest.Status = "New";
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
            if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(model.Email))
            {
                return Json(new { errorMessage = "Field \"Email\" is required" });
            }
            if (model.BillingAddress != null)
            {
                if (model.BillingAddressErrors.Count > 0)
                {
                    var firstError = model.BillingAddressErrors.First();
                    return Json(new { errorMessage = firstError });
                }
            }
            if (model.ShippingAddress != null)
            {
                if (model.ShippingAddressErrors.Count > 0)
                {
                    var firstError = model.ShippingAddressErrors.First();
                    return Json(new { errorMessage = firstError });
                }
            }
            Context.ActualQuoteRequest.CustomerId = Context.CustomerId;
            Context.ActualQuoteRequest.CustomerName = Context.Customer.Name;
            Context.ActualQuoteRequest.Comment = model.Comment;
            Context.ActualQuoteRequest.Email = model.Email;
            Context.ActualQuoteRequest.BillingAddress = model.BillingAddress;
            Context.ActualQuoteRequest.ShippingAddress = model.ShippingAddress;

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
                        Price = existingQuoteItem.SalePrice == 0 ? existingQuoteItem.ListPrice : existingQuoteItem.SalePrice
                    });
                }
            }

            if (!Context.ActualQuoteRequest.ProposalPricesUnique)
            {
                return Json(new { errorMessage = "Proposal prices quantities must be unique." });
            }

            if (User.Identity.IsAuthenticated)
            {
                Context.ActualQuoteRequest.Tag = null;
            }

            Context.ActualQuoteRequest.Status = "Processing";

            await QuoteService.UpdateQuoteRequestAsync(Context.ActualQuoteRequest);

            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = VirtualPathUtility.ToAbsolute("~/quote");
                return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/account/login?returnUrl=" + returnUrl) });
            }

            return Json(new { redirectUrl = VirtualPathUtility.ToAbsolute("~/account/quote/" + Context.ActualQuoteRequest.Number) });
        }
    }
}