using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Tax.Model
{
    public class TaxEvaluationContext : IEvaluationContext
    {
        public TaxEvaluationContext(TaxRequest taxRequest)
        {
            TaxRequest = taxRequest;
        }
        public TaxRequest TaxRequest { get; set; }

    }
}
