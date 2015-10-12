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
    public static class QuoteRequestTotalsConverter
    {
        public static webModel.QuoteRequestTotals ToWebModel(this coreModel.QuoteRequestTotals totals)
        {
            var retVal = new webModel.QuoteRequestTotals();
            retVal.InjectFrom(totals);
            return retVal;
        }

        public static coreModel.QuoteRequestTotals ToCoreModel(this webModel.QuoteRequestTotals totals)
        {
            var retVal = new coreModel.QuoteRequestTotals();
            retVal.InjectFrom(totals);
            return retVal;
        }


    }
}