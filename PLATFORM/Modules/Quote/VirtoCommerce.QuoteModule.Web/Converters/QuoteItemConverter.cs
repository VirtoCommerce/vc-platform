using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.QuoteModule.Web.Converters
{
    public static class QuoteItemConverter
    {
        public static webModel.QuoteItem ToWebModel(this coreModel.QuoteItem quoteItem)
        {
            var retVal = new webModel.QuoteItem();
            retVal.InjectFrom(quoteItem);
            retVal.Currency = quoteItem.Currency;

            if (quoteItem.ProposalPrices != null)
            {
                retVal.ProposalPrices = quoteItem.ProposalPrices.Select(x=>x.ToWebModel()).OrderBy(x=>x.Quantity).ToList();
            }
            if(quoteItem.SelectedTierPrice != null)
            {
                retVal.SelectedTierPrice = quoteItem.SelectedTierPrice.ToWebModel();
            }
            return retVal;
        }

        public static coreModel.QuoteItem ToCoreModel(this webModel.QuoteItem quoteItem)
        {
            var retVal = new coreModel.QuoteItem();
            retVal.InjectFrom(quoteItem);
            retVal.Currency = quoteItem.Currency;

            if (quoteItem.ProposalPrices != null)
            {
                retVal.ProposalPrices = quoteItem.ProposalPrices.Select(x => x.ToCoreModel()).ToList();
            }
            if (quoteItem.SelectedTierPrice != null)
            {
                retVal.SelectedTierPrice = quoteItem.SelectedTierPrice.ToCoreModel();
            }
            return retVal;
        }


    }
}