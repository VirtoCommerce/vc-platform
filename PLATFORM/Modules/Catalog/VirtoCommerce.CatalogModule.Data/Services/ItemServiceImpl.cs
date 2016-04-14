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
        private readonly IOutlineService _outlineService;

        public ItemServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICommerceService commerceService, IOutlineService outlineService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _commerceService = commerceService;
            _outlineService = outlineService;
        }

        #region IItemService Members

        public coreModel.CatalogProduct GetById(string itemId, coreModel.ItemResponseGroup respGroup, string catalogId = null)
        {
            var results = this.GetByIds(new[] { itemId }, respGroup, catalogId);
            return results.Any() ? results.First() : null;
        }

        public coreModel.CatalogProduct[] GetByIds(string[] itemIds, coreModel.ItemResponseGroup respGroup, string catalogId = null)
        {
            if (respGroup.HasFlag(coreModel.ItemResponseGroup.Outlines))
            {
                respGroup |= coreModel.ItemResponseGroup.Links;
            }

            coreModel.CatalogProduct[] result;

            using (var repository = _catalogRepositoryFactory())
            {
                result = repository.GetItemByIds(itemIds, respGroup)
                    .Select(x => x.ToCoreModel())
                    .ToArray();
            }

            // Fill outlines for products
            if (respGroup.HasFlag(coreModel.ItemResponseGroup.Outlines))
            {
                _outlineService.FillOutlinesForObjects(result, catalogId);
            }

            // Fill SEO info for products, variations and outline items
            if ((respGroup & coreModel.ItemResponseGroup.Seo) == coreModel.ItemResponseGroup.Seo)
            {
                var objectsWithSeo = new List<ISeoSupport>(result);

                var variations = result.Where(p => p.Variations != null)
                                       .SelectMany(p => p.Variations);
                objectsWithSeo.AddRange(variations);

                var outlineItems = result.Where(p => p.Outlines != null)
                                         .SelectMany(p => p.Outlines.SelectMany(o => o.Items));
                objectsWithSeo.AddRange(outlineItems);

                _commerceService.LoadSeoForObjects(objectsWithSeo.ToArray());
            }

            return result;
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
