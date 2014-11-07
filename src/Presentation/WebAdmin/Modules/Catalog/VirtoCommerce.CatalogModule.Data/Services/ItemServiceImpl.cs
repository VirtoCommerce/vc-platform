using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Framework.Core.Caching;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class ItemServiceImpl : ModuleServiceBase, IItemService
	{
		private readonly Func<IFoundationCatalogRepository> _catalogRepositoryFactory;
		private readonly CacheManager _cacheManager;
		public ItemServiceImpl(Func<IFoundationCatalogRepository> catalogRepositoryFactory, CacheManager cacheManager)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_cacheManager = cacheManager;
		}

		#region IItemService Members

		public Model.CatalogProduct GetById(string itemId)
		{
			Model.CatalogProduct retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbItem = repository.GetItemByIds(new string[] { itemId }).FirstOrDefault();
				var dbCatalog = repository.GetCatalogById(dbItem.CatalogId);
				var dbVariations = repository.GetAllItemVariations(dbItem);
				var parentItemRelation = repository.ItemRelations.FirstOrDefault(x => x.ChildItemId == itemId);
				var parentItemId = parentItemRelation == null ? null : parentItemRelation.ParentItemId;
				var catalog = dbCatalog.ToModuleModel();
				if (dbItem.CategoryItemRelations.Any())
				{
					var dbCategory = repository.GetCategoryById(dbItem.CategoryItemRelations[0].CategoryId);
					var dpProperties = repository.GetAllCategoryProperties(dbCategory);
					var properties = dpProperties.Select(x => x.ToModuleModel(catalog, dbCategory.ToModuleModel(catalog))).ToArray();
					var category = dbCategory.ToModuleModel(catalog, properties);

					retVal = dbItem.ToModuleModel(catalog, category, properties, dbVariations, parentItemId);
				}
				else
				{
					retVal = dbItem.ToModuleModel(catalog, null, null, dbVariations, parentItemId);
				}
			}

			return retVal;
		}

		public Model.CatalogProduct Create(Model.CatalogProduct item)
		{
			var dbItem = item.ToFoundation();

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

			var retVal = GetById(dbItem.ItemId);
			return retVal;
		}

		public void Update(Model.CatalogProduct[] items)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				var dbItems = repository.GetItemByIds(items.Select(x => x.Id).ToArray());
				using (var changeTracker = base.GetChangeTracker(repository))
				{
					foreach (var dbItem in dbItems)
					{
						var item = items.FirstOrDefault(x => x.Id == dbItem.ItemId);
						if (item != null)
						{
							changeTracker.Attach(dbItem);

							var dbItemChanged = item.ToFoundation();
							dbItemChanged.Patch(dbItem);

							//Variation relation update
							if (item.MainProductId != null)
							{
								repository.SetVariationRelation(dbItem, item.MainProductId);
							}
						}
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
