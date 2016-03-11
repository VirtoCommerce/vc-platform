using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Quote.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    public class QuoteRequestController : StorefrontControllerBase
    {
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;

        public QuoteRequestController(WorkContext workContext, IStorefrontUrlBuilder urlBuilder, IQuoteRequestBuilder quoteRequestBuilder)
            : base(workContext, urlBuilder)
        {
            _quoteRequestBuilder = quoteRequestBuilder;
         }

        /// <summary>
        /// Display user current quote request
        /// </summary>
        /// <returns></returns>
        // GET: quoterequest
        [HttpGet]
        public ActionResult CurrentQuoteRequest()
        {
            return View("quote-request", WorkContext);
        }

        /// <summary>
        /// Display quote request by number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        // GET: /quoterequest/{number}
        [HttpGet]
        public async Task<ActionResult> QuoteRequestByNumber(string number)
        {
            var builder = await _quoteRequestBuilder.LoadQuoteRequestAsync(number, WorkContext.CurrentLanguage, WorkContext.AllCurrencies);
            WorkContext.CurrentQuoteRequest = builder.QuoteRequest;
            if (WorkContext.CurrentCustomer.Id != WorkContext.CurrentQuoteRequest.CustomerId)
            {
                throw new StorefrontException("Requested quote not belongs to current user");
            }
            return View("quote-request", WorkContext);
        }


        /// <summary>
        /// List of all quote requests for current user
        /// </summary>
        /// <returns></returns>
        // GET: /account/quoterequests
        [HttpGet]
        public ActionResult QuoteRequests()
        {
            //All customer quote requests filled in WorkContext.Customer.QuoteRequests
            return View("customers/quote-requests", WorkContext);
        }
    }
}