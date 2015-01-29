using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Facet
    {
        public string FacetType { get; set; }
        public string Field { get; set; }
        public string Label { get; set; }
        public FacetValue[] Values { get; set; }
    }
}
