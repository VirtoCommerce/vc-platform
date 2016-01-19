using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Data.Tax
{
    public class FixedTaxRateProvider : TaxProvider
    {
        public FixedTaxRateProvider()
            : base("FixedRate")
        {
        }

        public FixedTaxRateProvider(params SettingEntry[] settings)
            : this()
        {
            Settings = settings;
        }

        private decimal Rate
        {
            get
            {
                decimal retVal = 0;
                var settingRate = Settings.Where(x => x.Name == "VirtoCommerce.Core.FixedTaxRateProvider.Rate").FirstOrDefault();
                if (settingRate != null)
                {
                    retVal = Decimal.Parse(settingRate.Value, CultureInfo.InvariantCulture);
                }
                return retVal;
            }
        }

        public override IEnumerable<TaxRate> CalculateRates(Domain.Common.IEvaluationContext context)
        {
            var taxEvalContext = context as TaxEvaluationContext;
            if (taxEvalContext == null)
            {
                throw new NullReferenceException("taxEvalContext");
            }

            var retVal = new List<TaxRate>();

            foreach (var line in taxEvalContext.Lines)
            {
                var rate = new TaxRate
                {
                    Rate = line.Amount * Rate * 0.01m,
                    Currency = taxEvalContext.Currency,
                    TaxProvider = this,
                    Line = line
                };
                retVal.Add(rate);
            }

            return retVal;
        }
    }
}
