using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Payment.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.QuoteModule.Web.Converters;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.QuoteModule.Data.Converters;

namespace VirtoCommerce.QuoteModule.Web.Controllers.Api
{
    [RoutePrefix("api/quote/requests")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class QuoteModuleController : ApiController
    {
        private readonly IQuoteRequestService _quoteRequestService;
        private readonly IQuoteTotalsCalculator _totalsCalculator;
        private readonly IStoreService _storeService;
        public QuoteModuleController(IQuoteRequestService quoteRequestService, IQuoteTotalsCalculator totalsCalculator, IStoreService storeService)
        {
            _quoteRequestService = quoteRequestService;
            _totalsCalculator = totalsCalculator;
            _storeService = storeService;
        }

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpGet]
        [ResponseType(typeof(coreModel.QuoteRequestSearchResult))]
        [Route("")]
        public IHttpActionResult Search([ModelBinder(typeof(SearchCriteriaBinder))] coreModel.QuoteRequestSearchCriteria criteria)
        {
            var retVal = _quoteRequestService.Search(criteria);
            return Ok(retVal);
        }


        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>Return a single RFQ</remarks>
        /// <param name="id">RFQ id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.QuoteRequest))]
        [Route("{id}")]
        public IHttpActionResult GetById(string id)
        {
            var quote = _quoteRequestService.GetByIds(new[] { id }).FirstOrDefault();
            if (quote == null)
            {
                return NotFound();
            }
            quote.Totals = _totalsCalculator.CalculateTotals(quote);
            var retVal = quote.ToWebModel();
            return Ok(retVal);
        }

        /// <summary>
        ///  Create new RFQ
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        [HttpPost]
        [ResponseType(typeof(webModel.QuoteRequest))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult Create(webModel.QuoteRequest quoteRequest)
        {
            var coreQuote = quoteRequest.ToCoreModel();
            var retVal = _quoteRequestService.SaveChanges(new coreModel.QuoteRequest[] { coreQuote }).First();
            return Ok(retVal);
        }

        /// <summary>
        ///  Update a existing RFQ
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
        public IHttpActionResult Update(webModel.QuoteRequest quoteRequest)
        {
            var coreQuote = quoteRequest.ToCoreModel();
            _quoteRequestService.SaveChanges(new coreModel.QuoteRequest[] { coreQuote });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
		/// Calculate totals
		/// </summary>
		/// <remarks>Return totals for selected tier prices</remarks>
		/// <param name="quoteRequest">RFQ</param>
        [HttpPut]
        [ResponseType(typeof(webModel.QuoteRequest))]
        [Route("recalculate")]
        public IHttpActionResult CalculateTotals(webModel.QuoteRequest quoteRequest)
        {
            var coreQuote = quoteRequest.ToCoreModel();
            coreQuote.Totals = _totalsCalculator.CalculateTotals(coreQuote);
            return Ok(coreQuote.ToWebModel());

        }

        /// <summary>
		/// Get available shipping methods with prices for quote requests
		/// </summary>
		/// <param name="quoteRequest">RFQ</param>
        [HttpGet]
        [ResponseType(typeof(webModel.ShipmentMethod[]))]
        [Route("{id}/shipmentmethods")]
        public IHttpActionResult GetShipmentMethods(string id)
        {
            var quote = _quoteRequestService.GetByIds(id).FirstOrDefault();
            if (quote != null)
            {
                var store = _storeService.GetById(quote.StoreId);

                if (store != null)
                {
                    var cartFromQuote = quote.ToCartModel();
                    var evalContext = new ShippingEvaluationContext(cartFromQuote);
                    var rates = store.ShippingMethods.Where(x => x.IsActive).SelectMany(x => x.CalculateRates(evalContext)).ToArray();
                    var retVal = rates.Select(x => new webModel.ShipmentMethod
                    {
                        Currency = x.Currency,
                        Name = x.ShippingMethod.Name,
                        Price = x.Rate,
                        OptionName = x.OptionName,
                        ShipmentMethodCode = x.ShippingMethod.Code,
                        LogoUrl = x.ShippingMethod.LogoUrl
                    }).ToArray();

                    return Ok(retVal);
                }
            }
            return NotFound();
        }

    }
}
