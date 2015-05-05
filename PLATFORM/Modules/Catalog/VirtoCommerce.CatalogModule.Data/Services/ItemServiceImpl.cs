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
			// TODO: Optimize performance (Sasha)
			// 1. Catalog should be cached and not retrieved every time from the db
			// 2. SEO info can be retrieved for all items at once instead of one by one
			// 3. Optimize how main variation is loaded
			// 4. Associations shouldn't be loaded always and must be optimized as well
			// 5. No need to get properties meta data to just retrieve property ID
			var retVal = new List<coreModel.CatalogProduct>();
			using (var repository = _catalogRepositoryFactory())
			{
				var dbItems = repository.GetItemByIds(itemIds, respGroup);
				Dictionary<string, IEnumerable<dataModel.Item>> dbAllVariations = null;
				if ((respGroup & coreModel.ItemResponseGroup.Variations) == coreModel.ItemResponseGroup.Variations)
				{
					dbAllVariations = repository.GetAllItemsVariations(itemIds, respGroup);
				}

				foreach (var dbItem in dbItems)
				{
					// Sasha: we don't need to return variations for the variation even if it is a product
					//var parentItemRelation = repository.ItemRelations.FirstOrDefault(x => x.ChildItemId == dbItem.ItemId);
					//var parentItemId = parentItemRelation == null ? null : parentItemRelation.ParentItemId;
					string parentItemId = null;

					/*
					foundation.Item[] dbVariations = null;
					if ((respGroup & module.ItemResponseGroup.Variations) == module.ItemResponseGroup.Variations)
					{
						//dbVariations = repository.GetAllItemVariations(parentItemId ?? dbItem.ItemId);
						dbVariations = repository.GetAllItemVariations(dbItem.ItemId);
						// Sasha: we don't need to return variations for a variation
						//When user load not main product need to include main product in variation list and exclude current

						if (parentItemId != null)
						{
							var dbMainItem = repository.GetItemByIds(new[] { parentItemId }, respGroup).FirstOrDefault();
							dbVariations = dbVariations.Concat(new[] { dbMainItem }).Where(x => x.ItemId != dbItem.ItemId).ToArray();
						}

						//Need this for add main product to variations list except current  
						dbVariations = dbVariations.Concat(new[] { dbItem }).Where(x => x.ItemId != dbItem.ItemId).ToArray();
					}
					*/

					dataModel.Item[] dbVariations = null;

					if (dbAllVariations != null && dbAllVariations.ContainsKey(dbItem.Id))
					{
						dbVariations = dbAllVariations[dbItem.Id].ToArray();
					}

					SeoUrlKeyword[] seoInfos = null;
					if ((respGroup & coreModel.ItemResponseGroup.Seo) == coreModel.ItemResponseGroup.Seo)
					{
						seoInfos = _commerceService.GetSeoKeywordsForEntity(dbItem.Id).ToArray();
					}

					var associatedProducts = new List<coreModel.CatalogProduct>();
					if ((respGroup & coreModel.ItemResponseGroup.ItemAssociations) == coreModel.ItemResponseGroup.ItemAssociations)
					{
						if (dbItem.AssociationGroups.Any())
						{
							foreach (var association in dbItem.AssociationGroups.SelectMany(x => x.Associations))
							{
								var associatedProduct = GetById(association.ItemId, coreModel.ItemResponseGroup.ItemAssets);
								associatedProducts.Add(associatedProduct);
							}
						}
					}

					var dbCatalog = repository.GetCatalogById(dbItem.CatalogId);

					var catalog = dbCatalog.ToCoreModel();
					coreModel.Category category = null;
					if (dbItem.CategoryItemRelations.Any())
					{
						var dbCategory = repository.GetCategoryById(dbItem.CategoryItemRelations.OrderBy(x => x.Priority).First().CategoryId);
						category = dbCategory.ToCoreModel(catalog);
					}
					retVal.Add(dbItem.ToCoreModel(catalog, category, null, dbVariations, seoInfos, parentItemId, associatedProducts.ToArray()));
				}
			}

			return retVal.ToArray();
        }

		public coreModel.CatalogProduct Create(coreModel.CatalogProduct item)
		{
			var dbItem = item.ToDataModel();

			using (var repository = _catalogRepositoryFactory())
			{
				if (item.CategoryId != null)
				{
					//Category relation
					var dbCategory = repository.GetCategoryById(item.CategoryId);
					if (dbCategory == null)
					{
						throw new NullReferenceException("dbCategory");
					}
					repository.SetItemCategoryRelation(dbItem, dbCategory);
				}

				//Variation relation
				if (item.MainProductId != null)
				{
					repository.SetVariationRelation(dbItem, item.MainProductId);
				}

				repository.Add(dbItem);

				CommitChanges(repository);
			}

			//Need add seo separately
			if (item.SeoInfos != null)
			{
				foreach (var seoInfo in item.SeoInfos)
				{
					var dbSeoInfo = seoInfo.ToCoreModel(dbItem);
					_commerceService.UpsertSeoKeyword(dbSeoInfo);
				}
			}

			var retVal = GetById(dbItem.Id, coreModel.ItemResponseGroup.ItemLarge);
			return retVal;
		}

		public void Update(coreModel.CatalogProduct[] items)
		{
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

						var dbItemChanged = item.ToDataModel();
						dbItemChanged.Patch(dbItem);

						//Variation relation update
						if (item.MainProductId != null)
						{
							repository.SetVariationRelation(dbItem, item.MainProductId);
						}
						//else
						//{
						//	//Switch item like a  main product
						//	repository.SwitchProductToMain(dbItem);
						//}
					}

					//Patch seoInfo
					if (item.SeoInfos != null)
					{
						var dbSeoInfos = new ObservableCollection<SeoUrlKeyword>(_commerceService.GetSeoKeywordsForEntity(item.Id));
						var changedSeoInfos = item.SeoInfos.Select(x => x.ToCoreModel(dbItem)).ToList();
						dbSeoInfos.ObserveCollection(x => _commerceService.UpsertSeoKeyword(x), x => _commerceService.DeleteSeoKeywords(new string[] { x.Id }));

						changedSeoInfos.Patch(dbSeoInfos, (source, target) => _commerceService.UpsertSeoKeyword(source));
					}
				}
				CommitChanges(repository);
			}

		}

		public void Delete(string[] itemIds)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				repository.RemoveItems(itemIds);
				CommitChanges(repository);
			}
		}

		#endregion
	
    }
}
