using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Tax.Model
{
    /// <summary>
    /// Represent one abstract position for tax calculation
    /// </summary>
    public class TaxLine : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public string TaxType { get; set; }
    }
}
