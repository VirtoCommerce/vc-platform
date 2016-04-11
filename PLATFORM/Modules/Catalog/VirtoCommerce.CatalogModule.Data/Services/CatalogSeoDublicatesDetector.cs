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
    /// <summary>
    /// Detect SEO duplicates based on  store, catalog, categories relationships and structure knowledge
    /// </summary>
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
            if(objectType.EqualsInvariant(typeof(Store).Name))
            {
                var store = _storeService.GetById(objectId);
                if (store != null)
                {
                    catalogId = store.Catalog;
                }
            }
            else if (objectType.EqualsInvariant(typeof(Category).Name))
            {
                var category = _categoryService.GetById(objectId, CategoryResponseGroup.Info);
                if (category != null)
                {
                    catalogId = category.CatalogId;
                }
            }
            else if (objectType.EqualsInvariant(typeof(CatalogProduct).Name))
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
        /// <summary>
        /// Detect SEO duplicates for object belongs to catalog  (physical or virtual) based on links information
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="allDublicatedSeos"></param>
        /// <returns></returns>
        private IEnumerable<SeoInfo> DetectSeoDublicatesForCatalog(string catalogId, IEnumerable<SeoInfo> allDublicatedSeos)
        {
            var productsSeo = allDublicatedSeos.Where(x => string.Equals(x.ObjectType, typeof(CatalogProduct).Name, StringComparison.OrdinalIgnoreCase));
            var categoriesSeo = allDublicatedSeos.Where(x => string.Equals(x.ObjectType, typeof(Category).Name, StringComparison.OrdinalIgnoreCase));

            var products = _productService.GetByIds(productsSeo.Select(x => x.ObjectId).Distinct().ToArray(),  ItemResponseGroup.Outlines);
            var categories = _categoryService.GetByIds(categoriesSeo.Select(x => x.ObjectId).Distinct().ToArray(), CategoryResponseGroup.WithOutlines);

            var retVal = new List<SeoInfo>();
            //Here we try to find between SEO duplicates records for products with directly or indirectly related to requested catalog
            foreach (var product in products)
            {              
                if (product.CatalogId == catalogId || product.Outlines.SelectMany(x=>x.Items).Any(x=>x.Id == catalogId))
                {
                    var productSeo = productsSeo.First(x => x.ObjectId == product.Id);
                    productSeo.Name = product.Name;
                    retVal.Add(productSeo);
                }
            }
            //Here we try to find between SEO duplicates records for categories with directly or indirectly related to requested catalog
            foreach (var category in categories)
            {
                if (category.CatalogId == catalogId || category.Outlines.SelectMany(x=>x.Items).Any(x => x.Id == catalogId))
                {
                    var categorySeo = categoriesSeo.First(x => x.ObjectId == category.Id);
                    categorySeo.Name = category.Name;
                    retVal.Add(categorySeo);
                }
            }
            //Return only duplicated SEO records
            return retVal.GroupBy(x => x.SemanticUrl).Where(x => x.Count() > 1).SelectMany(x => x);
        }
    }
}
