using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using System.Diagnostics;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class ItemServiceImpl : ServiceBase, IItemService
    {
        private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
        private readonly ICommerceService _commerceService;

        public ItemServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICommerceService commerceService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _commerceService = commerceService;
        }

        #region IItemService Members

        public coreModel.CatalogProduct GetById(string itemId, coreModel.ItemResponseGroup respGroup)
        {
            var results = this.GetByIds(new[] { itemId }, respGroup);
            return results.Any() ? results.First() : null;
        }

        public coreModel.CatalogProduct[] GetByIds(string[] itemIds, coreModel.ItemResponseGroup respGroup)
        {
            var retVal = new List<coreModel.CatalogProduct>();
            using (var repository = _catalogRepositoryFactory())
            {
                var dbItems = repository.GetItemByIds(itemIds, respGroup);

                retVal.AddRange(dbItems.Select(x => x.ToCoreModel()));
                //Populate product seo
                if ((respGroup & coreModel.ItemResponseGroup.Seo) == coreModel.ItemResponseGroup.Seo)
                {
                    var expandedProducts = retVal.Concat(retVal.Where(x => x.Variations != null).SelectMany(x => x.Variations)).ToArray();
                    var allCategories = expandedProducts.Select(x => x.Category).ToArray();
                    var allSeoObjects = expandedProducts.OfType<ISeoSupport>().Concat(allCategories.OfType<ISeoSupport>()).ToArray();
                    _commerceService.LoadSeoForObjects(allSeoObjects);

                }
            }
            return retVal.ToArray();
        }

        public void Create(coreModel.CatalogProduct[] items)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _catalogRepositoryFactory())
            {
                foreach (var item in items)
                {
                    var dbItem = item.ToDataModel(pkMap);
                    if (item.Variations != null)
                    {
                        foreach (var variation in item.Variations)
                        {
                            variation.MainProductId = dbItem.Id;
                            variation.CatalogId = dbItem.CatalogId;
                            var dbVariation = variation.ToDataModel(pkMap);
                            dbItem.Childrens.Add(dbVariation);
                        }
                    }
                    repository.Add(dbItem);
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
       
            //Update SEO 
            var itemsWithVariations = items.Concat(items.Where(x => x.Variations != null).SelectMany(x => x.Variations)).ToArray();
            _commerceService.UpsertSeoForObjects(itemsWithVariations);
        }

        public coreModel.CatalogProduct Create(coreModel.CatalogProduct item)
        {
            Create(new[] { item });

            var retVal = GetById(item.Id, coreModel.ItemResponseGroup.ItemLarge);
            return retVal;
        }

        public void Update(coreModel.CatalogProduct[] items)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var now = DateTime.UtcNow;
            using (var repository = _catalogRepositoryFactory())
            using (var changeTracker = base.GetChangeTracker(repository))
            {
                var dbItems = repository.GetItemByIds(items.Select(x => x.Id).ToArray(), coreModel.ItemResponseGroup.ItemLarge);
                foreach (var dbItem in dbItems)
                {
                    var item = items.FirstOrDefault(x => x.Id == dbItem.Id);
                    if (item != null)
                    {
                        changeTracker.Attach(dbItem);

                        item.Patch(dbItem, pkMap);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            //Update seo for products
            _commerceService.UpsertSeoForObjects(items);

        }

        public void Delete(string[] itemIds)
        {
            var items = GetByIds(itemIds, coreModel.ItemResponseGroup.Seo | coreModel.ItemResponseGroup.Variations);
            using (var repository = _catalogRepositoryFactory())
            {
                repository.RemoveItems(itemIds);
                CommitChanges(repository);
            }
            var expandedItemsWithVariations = items.Concat(items.SelectMany(x => x.Variations));
            foreach (var item in expandedItemsWithVariations)
            {
                _commerceService.DeleteSeoForObject(item);
            }
        }
        #endregion
    }
}
