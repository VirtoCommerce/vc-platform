using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Tax.Model;

namespace VirtoCommerce.Domain.Tax.Services
{
    public interface ITaxService
    {
        TaxProvider[] GetAllTaxProviders();
        void RegisterTaxProvider(Func<TaxProvider> providerFactory);
    }
}
