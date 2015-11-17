using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class TierPrice : ValueObject<TierPrice>
    {
        public Money Price { get; set; }

        public int Quantity { get; set; }
    }
}
