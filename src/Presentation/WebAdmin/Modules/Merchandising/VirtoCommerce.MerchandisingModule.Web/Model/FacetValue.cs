using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class FacetValue
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public object Value { get; set; }
        public bool IsApplied { get; set; }
    }
}
