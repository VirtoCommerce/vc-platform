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
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;

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
                //Load product seo informations for each items and item category
                SeoInfo[] seoInfos = null;
                if ((respGroup & coreModel.ItemResponseGroup.Seo) == coreModel.ItemResponseGroup.Seo)
                {
                    var categoryIds = dbItems.Select(x => x.CategoryId).Distinct().ToArray();
                    seoInfos = _commerceService.GetObjectsSeo(itemIds.Concat(categoryIds).ToArray()).ToArray();
                }

                foreach (var dbItem in dbItems)
                {
                    var product = dbItem.ToCoreModel();
                    retVal.Add(product);
                    //Populate product seo
                    if (seoInfos != null)
                    {
                        product.SeoInfos = seoInfos.Where(x => x.ObjectId == product.Id).ToList();
                        if(product.Category != null)
                        {
                            product.Category.SeoInfos = seoInfos.Where(x => x.ObjectId == product.Category.Id).ToList();
                        }
                    }
                }
            }

			return retVal.ToArray();
		}

		public coreModel.CatalogProduct Create(coreModel.CatalogProduct item)
		{
			var dbItem = item.ToDataModel();

			using (var repository = _catalogRepositoryFactory())
			{
				repository.Add(dbItem);
				item.Id = dbItem.Id;

				if (item.Variations != null)
				{
					foreach (var variation in item.Variations)
					{
						variation.MainProductId = dbItem.Id;
						variation.CatalogId = dbItem.CatalogId;
						var dbVariation = variation.ToDataModel();
						repository.Add(dbVariation);
						variation.Id = dbVariation.Id;
					}
				}

				CommitChanges(repository);
			}

			//Need add seo separately
			if (item.SeoInfos != null)
			{
				foreach (var seoInfo in item.SeoInfos)
				{
					seoInfo.ObjectId = dbItem.Id;
					seoInfo.ObjectType = typeof(coreModel.CatalogProduct).Name;
					_commerceService.UpsertSeo(seoInfo);
				}
			}

			if (item.Variations != null)
			{
				foreach (var variation in item.Variations)
				{
					if (variation.SeoInfos != null)
					{
						foreach (var seoInfo in variation.SeoInfos)
						{
							seoInfo.ObjectId = variation.Id;
							seoInfo.ObjectType = typeof(coreModel.CatalogProduct).Name;
							_commerceService.UpsertSeo(seoInfo);
						}
					}
				}
			}

			var retVal = GetById(dbItem.Id, coreModel.ItemResponseGroup.ItemLarge);
			return retVal;
		}

		public void Update(coreModel.CatalogProduct[] items)
		{
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

						item.Patch(dbItem);
					}

					//Patch seoInfo
					if (item.SeoInfos != null)
					{
						foreach (var seoInfo in item.SeoInfos)
						{
							seoInfo.ObjectId = item.Id;
							seoInfo.ObjectType = typeof(coreModel.CatalogProduct).Name;
						}
						var seoInfos = new ObservableCollection<SeoInfo>(_commerceService.GetObjectsSeo(new[] { item.Id }));
						seoInfos.ObserveCollection(x => _commerceService.UpsertSeo(x), x => _commerceService.DeleteSeo(new[] { x.Id }));
						item.SeoInfos.Patch(seoInfos, (source, target) => _commerceService.UpsertSeo(source));
					}
				}
				CommitChanges(repository);
			}

		}

		public void Delete(string[] itemIds)
		{
			using (var repository = _catalogRepositoryFactory())
			{
                var seoInfos = _commerceService.GetObjectsSeo(itemIds);
                _commerceService.DeleteSeo(seoInfos.Select(x => x.Id).ToArray());

                repository.RemoveItems(itemIds);
				CommitChanges(repository);
			}
		}
		#endregion
	}
}
