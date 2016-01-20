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
    public class TaxEvaluationContext : Entity, IEvaluationContext
    {
        public TaxEvaluationContext()
        {
            Lines = new List<TaxLine>();
        }

        public string Code { get; set; }
        public string Type { get; set; }

        public Customer.Model.Contact Customer { get; set; }
        public Customer.Model.Organization Organization { get; set; }
        public Address Address { get; set; }
        public string Currency { get; set; }
        public ICollection<TaxLine> Lines { get; set; }

    }
}
