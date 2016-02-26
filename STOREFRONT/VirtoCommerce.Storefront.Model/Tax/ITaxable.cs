using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// It is an abstraction that represents a taxable entity
    /// </summary>
    public interface ITaxable
    {
        Money TaxTotal { get; }
        string TaxType { get; }
        ICollection<TaxDetail> TaxDetails { get; }

        void ApplyTaxRates(IEnumerable<TaxRate> taxRates);
    }
}
