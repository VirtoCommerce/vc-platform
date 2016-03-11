using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class ProxyCatalogSearchResult : CatalogSearchResult
    {
        bool _resultsLoaded = false;
        Func<Task<CatalogSearchResult>> _resultsGetter;
        public ProxyCatalogSearchResult(Func<Task<CatalogSearchResult>> resultsGetter)
        {
            _resultsGetter = resultsGetter;
        }

        IStorefrontPagedList<Product> _products = null;
        public override IStorefrontPagedList<Product> Products
        {
            get
            {
                LoadResults();
                return _products;
            }

            set
            {
                _products = value;
            }
        }

        protected virtual async void LoadResults()
        {
            if (_resultsLoaded)
                return;

            var results = await _resultsGetter();
            _products = results.Products;
            _resultsLoaded = true;
        }
    }
}
