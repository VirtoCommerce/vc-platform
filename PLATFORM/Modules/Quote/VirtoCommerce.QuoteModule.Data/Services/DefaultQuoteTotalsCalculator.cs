using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.QuoteModule.Data.Converters;

namespace VirtoCommerce.QuoteModule.Data.Services
{
    public class DefaultQuoteTotalsCalculator : IQuoteTotalsCalculator
    {
        private readonly IStoreService _storeService;
        public DefaultQuoteTotalsCalculator(IStoreService storeService)
        {
            _storeService = storeService;
        }
        #region IQuoteTotalsCalculator Members
        public QuoteRequestTotals CalculateTotals(QuoteRequest quote)
        {
            var retVal = new QuoteRequestTotals();
            var cartFromQuote = quote.ToCartModel();
            var store = _storeService.GetById(quote.StoreId);

            if (store != null)
            {
                //Calculate shipment total
                //firts try to get manual amount
                retVal.ShippingTotal = quote.ManualShippingTotal;
                if (retVal.ShippingTotal == 0 && quote.ShipmentMethod != null)
                {
                    //calculate total by using shipment gateways
                    var evalContext = new ShippingEvaluationContext(cartFromQuote);

                    var rate = store.ShippingMethods.Where(x => x.IsActive && x.Code == quote.ShipmentMethod.ShipmentMethodCode)
                                                    .SelectMany(x => x.CalculateRates(evalContext))
                                                    .Where(x => quote.ShipmentMethod.OptionName != null ? quote.ShipmentMethod.OptionName == x.OptionName : true)
                                                    .FirstOrDefault();
                    retVal.ShippingTotal = rate != null ? rate.Rate : 0m;
                }
                //Calculate taxes
                var taxProvider = store.TaxProviders.Where(x => x.IsActive).OrderBy(x => x.Priority).FirstOrDefault();
                if (taxProvider != null)
                {
                    var taxEvalContext = quote.ToTaxEvalContext();
                    retVal.TaxTotal = taxProvider.CalculateRates(taxEvalContext).Select(x => x.Rate).DefaultIfEmpty(0).Sum(x => x);
                }
            }

            //Calculate subtotal
            var items = quote.Items.Where(x => x.SelectedTierPrice != null);
            if (quote.Items != null)
            {
                retVal.OriginalSubTotalExlTax = items.Sum(x => x.SalePrice * x.SelectedTierPrice.Quantity);
                retVal.SubTotalExlTax = items.Sum(x => x.SelectedTierPrice.Price * x.SelectedTierPrice.Quantity);
                if (quote.ManualSubTotal > 0)
                {
                    retVal.DiscountTotal = retVal.SubTotalExlTax - quote.ManualSubTotal;
                    retVal.SubTotalExlTax = quote.ManualSubTotal;
                }
                else if (quote.ManualRelDiscountAmount > 0)
                {
                    retVal.DiscountTotal = retVal.SubTotalExlTax * quote.ManualRelDiscountAmount * 0.01m;
                }
            }

            return retVal;
        }
        #endregion
    }
}
