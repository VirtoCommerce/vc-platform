using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CatalogSeoDublicatesDetector : ISeoDuplicatesDetector
    {
        private readonly IItemService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public CatalogSeoDublicatesDetector(IItemService productService, ICategoryService categoryService, IStoreService storeService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _storeService = storeService;
        }
        #region ISeoConflictDetector Members
        public IEnumerable<SeoInfo> DetectSeoDuplicates(string objectType, string objectId, IEnumerable<SeoInfo> allSeoDuplicates)
        {
            var retVal = new List<SeoInfo>();
            string catalogId = null;
            if(string.Equals(objectType, typeof(Store).Name, StringComparison.OrdinalIgnoreCase))
            {
                var store = _storeService.GetById(objectId);
                if (store != null)
                {
                    catalogId = store.Catalog;
                }
            }
            if (string.Equals(objectType, typeof(Category).Name, StringComparison.OrdinalIgnoreCase))
            {
                var category = _categoryService.GetById(objectId, CategoryResponseGroup.Info);
                if (category != null)
                {
                    catalogId = category.CatalogId;
                }
            }
            if (string.Equals(objectType, typeof(CatalogProduct).Name, StringComparison.OrdinalIgnoreCase))
            {
                var product = _productService.GetById(objectId, ItemResponseGroup.ItemInfo);
                if (product != null)
                {
                    catalogId = product.CatalogId;
                }
            }

            if(!string.IsNullOrEmpty(catalogId))
            {
                retVal.AddRange(DetectSeoDublicatesForCatalog(catalogId, allSeoDuplicates));            
            }
            return retVal;
        }
        #endregion

        private IEnumerable<SeoInfo> DetectSeoDublicatesForCatalog(string catalogId, IEnumerable<SeoInfo> allDublicatedSeos)
        {
            var productsSeo = allDublicatedSeos.Where(x => string.Equals(x.ObjectType, typeof(CatalogProduct).Name, StringComparison.OrdinalIgnoreCase));
            var categoriesSeo = allDublicatedSeos.Where(x => string.Equals(x.ObjectType, typeof(Category).Name, StringComparison.OrdinalIgnoreCase));

            var products = _productService.GetByIds(productsSeo.Select(x => x.ObjectId).Distinct().ToArray(), ItemResponseGroup.ItemInfo | ItemResponseGroup.Links);
            var categories = _categoryService.GetByIds(categoriesSeo.Select(x => x.ObjectId).Distinct().ToArray(), CategoryResponseGroup.WithLinks);

            var retVal = new List<SeoInfo>();
            foreach (var product in products.Where(x => x.CatalogId == catalogId || x.Links.Any(y => y.CatalogId == catalogId)))
            {
                var productSeo = productsSeo.First(x => x.ObjectId == product.Id);
                productSeo.Name = product.Name;
                retVal.Add(productSeo);

            }
            foreach (var category in categories.Where(x => x.CatalogId == catalogId || x.Links.Any(y => y.CatalogId == catalogId)))
            {
                var categorySeo = categoriesSeo.First(x => x.ObjectId == category.Id);
                categorySeo.Name = category.Name;
                retVal.Add(categorySeo);

            }
            return retVal;
        }
    }
}
