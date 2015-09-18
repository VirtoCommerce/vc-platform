using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web.Providers
{
    public class AvaTaxRateProvider : TaxProvider
    {
        public AvaTaxRateProvider()
            : base("AvaTaxRate")
        {
        }

        public AvaTaxRateProvider(params SettingEntry[] settings)
            : this()
        {
            Settings = settings;
        }

        public override IEnumerable<TaxRate> CalculateRates(IEvaluationContext context)
        {
            var taxEvalContext = context as TaxEvaluationContext;
            if (taxEvalContext == null)
            {
                throw new NullReferenceException("taxEvalContext");
            }

            var retVal = new List<TaxRate>();
            if (taxEvalContext.TaxRequest != null)
            {
                foreach (var line in taxEvalContext.TaxRequest.Lines)
                {
                    var rate = new TaxRate
                    {
                        Rate = line.Amount * 10,
                        Currency = taxEvalContext.TaxRequest.Currency,
                        TaxProvider = this,
                        Line = line
                    };
                    retVal.Add(rate);
                }
            }
            return retVal;
        }
    }
}