using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ProductSearchResult : ResponseCollection<Product>
    {
        public Facet[] Facets { get; set; }
    }
}
