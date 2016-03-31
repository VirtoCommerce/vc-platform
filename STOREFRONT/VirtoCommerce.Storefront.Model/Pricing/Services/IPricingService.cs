using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model.Pricing.Services
{
    public interface IPricingService
    {
        Task EvaluateProductPricesAsync(IEnumerable<Product> products);
        void EvaluateProductPrices(IEnumerable<Product> products);
    }
}
