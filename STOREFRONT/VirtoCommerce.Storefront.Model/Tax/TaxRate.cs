using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TaxRate
    {
        public TaxRate(Currency currency)
        {
            Rate = new Money(currency);
        }
        public Money Rate { get; set; }
        public TaxLine Line { get; set; }
    }
}
