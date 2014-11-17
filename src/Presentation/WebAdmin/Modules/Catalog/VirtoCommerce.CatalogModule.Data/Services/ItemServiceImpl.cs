using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.CatalogModule.Model;

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

		public module.CatalogProduct GetById(string itemId, module.ItemResponseGroup respGroup)
		{
			module.CatalogProduct retVal = null;
			using (var repository = _catalogRepositoryFactory())
			{
				var dbItem = repository.GetItemByIds(new string[] { itemId }, respGroup).FirstOrDefault();
				var dbCatalog = repository.GetCatalogById(dbItem.CatalogId);
				
				var parentItemRelation = repository.ItemRelations.FirstOrDefault(x => x.ChildItemId == itemId);
				var parentItemId = parentItemRelation == null ? null : parentItemRelation.ParentItemId;
				foundation.Item[] dbVariations = null;
				if ((respGroup & module.ItemResponseGroup.Variations) == module.ItemResponseGroup.Variations)
				{
					dbVariations = repository.GetAllItemVariations(parentItemId ?? itemId);
					//When user load not main product need a inclue main product in variation list and exlude current 
					if (parentItemId != null)
					{
						var dbMainItem = repository.GetItemByIds(new string[] { parentItemId }, respGroup).FirstOrDefault();
						dbVariations = dbVariations.Concat(new foundation.Item[] { dbMainItem }).Where(x => x.ItemId != itemId).ToArray();
					}

					//Need this for add main product to variations list except current  
					dbVariations = dbVariations.Concat(new foundation.Item[] { dbItem }).Where(x => x.ItemId != itemId).ToArray();
				}

				var catalog = dbCatalog.ToModuleModel();
				if (dbItem.CategoryItemRelations.Any())
				{
					var dbCategory = repository.GetCategoryById(dbItem.CategoryItemRelations.OrderBy(x=>x.Priority).First().CategoryId);
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

		public module.CatalogProduct Create(module.CatalogProduct item)
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

			var retVal = GetById(dbItem.ItemId, module.ItemResponseGroup.ItemLarge);
			return retVal;
		}

		public void Update(Model.CatalogProduct[] items)
		{
			using (var repository = _catalogRepositoryFactory())
			{
				var dbItems = repository.GetItemByIds(items.Select(x => x.Id).ToArray(), module.ItemResponseGroup.ItemLarge);
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
							else
							{
								//Switch item like a  main product
								repository.SwitchProductToMain(dbItem);
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
