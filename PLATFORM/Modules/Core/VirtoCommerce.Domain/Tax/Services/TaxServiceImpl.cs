using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Tax.Model;

namespace VirtoCommerce.Domain.Tax.Services
{
	public class TaxServiceImpl : ITaxService
	{
		private List<Func<TaxProvider>> _taxProviderFactories = new List<Func<TaxProvider>>();

        #region ITaxService Members

        public TaxProvider[] GetAllTaxProviders()
		{
			return _taxProviderFactories.Select(x => x()).ToArray();
		}

		public void RegisterTaxProvider(Func<TaxProvider> providerFactory)
		{
			if (providerFactory == null)
			{
				throw new ArgumentNullException("providerFactory");
			}
            _taxProviderFactories.Add(providerFactory);
		}

		#endregion
	}
}
