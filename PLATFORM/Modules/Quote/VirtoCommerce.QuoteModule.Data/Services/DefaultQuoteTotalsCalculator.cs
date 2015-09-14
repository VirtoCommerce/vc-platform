using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Services;
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

            if (quote.StoreId != null)
            {
                //Calculate shipment total
                var evalContext = new ShippingEvaluationContext(cartFromQuote);
                var store = _storeService.GetById(quote.StoreId);
                if (store != null && quote.ShipmentMethod != null)
                {
                    var rate = store.ShippingMethods.Where(x => x.IsActive && x.Code == quote.ShipmentMethod.ShipmentMethodCode)
                                                    .SelectMany(x => x.CalculateRates(evalContext))
                                                    .Where(x => quote.ShipmentMethod.OptionName != null ? quote.ShipmentMethod.OptionName == x.OptionName : true)
                                                    .FirstOrDefault();
                    retVal.ShippingTotal = rate != null ? rate.Rate : 0m;
                }
            }

            //Calculate tax total

            //Calculate subtotal
            var items = quote.Items.Where(x => x.SelectedTierPrice != null);
            if (quote.Items != null)
            {
                retVal.OriginalSubTotalExlTax = items.Sum(x => x.SalePrice * x.SelectedTierPrice.Quantity);
                retVal.SubTotalExlTax = items.Sum(x => x.SelectedTierPrice.Price * x.SelectedTierPrice.Quantity);
                if (quote.ManualRelDiscountAmount > 0)
                {
                    retVal.SubTotalExlTax = quote.ManualRelDiscountAmount;
                }
                else if (quote.ManualRelDiscountAmount > 0)
                {
                    retVal.SubTotalExlTax = retVal.SubTotalExlTax - retVal.SubTotalExlTax * quote.ManualRelDiscountAmount;
                }
            }

            return retVal;
        }
        #endregion
    }
}
