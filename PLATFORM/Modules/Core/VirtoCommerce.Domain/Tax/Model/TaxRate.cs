using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Tax.Model
{
    public class TaxRate : ValueObject<TaxRate>
    {
        public decimal Rate { get; set; }
        public CurrencyCodes Currency { get; set; }

        public TaxLine Line { get; set; }
        public TaxProvider TaxProvider { get; set; }
    }
}
