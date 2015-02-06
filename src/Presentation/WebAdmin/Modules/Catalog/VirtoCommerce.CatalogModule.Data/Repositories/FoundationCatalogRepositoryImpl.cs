using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundationConfig = VirtoCommerce.Foundation.AppConfig.Model;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public class FoundationCatalogRepositoryImpl : EFCatalogRepository, IFoundationCatalogRepository
	{
        public FoundationCatalogRepositoryImpl(string nameOrConnectionString)
            : this(nameOrConnectionString, null)
        {
        }
        public FoundationCatalogRepositoryImpl(string nameOrConnectionString, IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
		{
			
		}

		#region IModuleCatalogRepository Members

		public foundation.CatalogBase GetCatalogById(string catalogId)
		{
			foundation.CatalogBase retVal = Catalogs.OfType<foundation.Catalog>()
														 .Include(x => x.CatalogLanguages)
                .FirstOrDefault(x => x.CatalogId == catalogId) ??
				(foundation.CatalogBase)Catalogs.OfType<foundation.VirtualCatalog>()
									  .FirstOrDefault(x => x.CatalogId == catalogId);
			return retVal;
		}

		public foundation.LinkedCategory[] GetCategoryLinks(string categoryId)
		{
			var retVal = new List<foundation.LinkedCategory>();
			//Load links for both categories (source and target)
			var allLinks = Categories.OfType<foundation.LinkedCategory>()
										.AsNoTracking()
										.Where(x => x.ParentCategoryId == categoryId || x.LinkedCategoryId == categoryId)
										.ToArray();
			foreach (var link in allLinks)
			{

				//Need to swap link role for both source and target categories
				if (categoryId != link.ParentCategoryId)
				{
					link.LinkedCategoryId = link.ParentCategoryId;
					link.LinkedCatalogId = link.CatalogId;
				}
				retVal.Add(allLinks);
			
			}
			return retVal.ToArray();
		}


		public foundation.LinkedCategory[] GetCatalogLinks(string catalogId)
		{
			var retVal = Categories.OfType<foundation.LinkedCategory>()
										.AsNoTracking()
										.Where(x => x.LinkedCatalogId == catalogId && x.LinkedCategoryId == null)
										.ToArray();

			return retVal;
		}

		public foundation.Category[] GetAllCategoryParents(foundation.Category category)
		{
			var retVal = new List<foundation.Category>();

			if (category.ParentCategoryId != null)
			{
				var parentCategory = Categories.OfType<foundation.Category>()
									.FirstOrDefault(x => x.CategoryId == category.ParentCategoryId);
				if (parentCategory != null)
				{
					retVal.Add(parentCategory);
					retVal.AddRange(GetAllCategoryParents(parentCategory));
				}
			}
			return retVal.ToArray();
		}

		public foundation.Category GetCategoryById(string categoryId)
		{
			var retVal = Categories.OfType<foundation.Category>()
										.Include(x => x.CategoryPropertyValues)
										.Include(x => x.PropertySet.PropertySetProperties.Select(y => y.Property))
										.FirstOrDefault(x => x.CategoryId == categoryId);

					
			return retVal;
		}

		public foundation.Item[] GetItemByIds(string[] itemIds, moduleModel.ItemResponseGroup respGroup = moduleModel.ItemResponseGroup.ItemLarge)
		{
			var query = base.Items.Include(x => x.Catalog)
								   .Include(x => x.CategoryItemRelations)
								  .Where(x => itemIds.Contains(x.ItemId));
			if ((respGroup & moduleModel.ItemResponseGroup.ItemProperties) == moduleModel.ItemResponseGroup.ItemProperties)
			{
				query = query.Include(x => x.ItemPropertyValues);
			}
			if ((respGroup & moduleModel.ItemResponseGroup.ItemAssets) == moduleModel.ItemResponseGroup.ItemAssets)
			{
				query = query.Include(x => x.ItemAssets);
			}
			if ((respGroup & moduleModel.ItemResponseGroup.ItemEditorialReviews) == moduleModel.ItemResponseGroup.ItemEditorialReviews)
			{
				query = query.Include(x => x.EditorialReviews);
			}
			if ((respGroup & moduleModel.ItemResponseGroup.ItemAssociations) == moduleModel.ItemResponseGroup.ItemAssociations)
			{
				query = query.Include(x => x.AssociationGroups.Select(y=>y.Associations));
			}
			
			var retVal = query.ToArray();
			return retVal;
		}

		public foundation.Property[] GetPropertiesByIds(string[] propIds)
		{
			var retVal = Properties.Include(x => x.Catalog)
										.Include(x => x.PropertyValues)
										.Include(x => x.PropertyAttributes)
										.Where(x => propIds.Contains(x.PropertyId))
										.ToArray();
			return retVal;
		}
		public foundation.Category GetPropertyCategory(string propId)
		{
			foundation.Category retVal = null;
			var propSet = PropertySets.FirstOrDefault(x => x.PropertySetProperties.Any(y => y.PropertyId == propId));
			if (propSet != null)
			{
				var categoryId = Categories.OfType<foundation.Category>()
								   .Where(x => x.PropertySetId == propSet.PropertySetId)
								   .Select(x => x.CategoryId).FirstOrDefault();
				if (categoryId != null)
				{
					retVal = GetCategoryById(categoryId);
				}
			}
			return retVal;
		}

		public foundation.Property[] GetAllCategoryProperties(foundation.Category category)
		{
			var retVal = new List<foundation.Property>();
			if (category.PropertySet != null)
			{
				retVal.AddRange(category.PropertySet.PropertySetProperties.Select(x => x.Property));
			}
			if (category.ParentCategoryId != null)
			{
				var parentCategory = GetCategoryById(category.ParentCategoryId);
				if (parentCategory != null)
				{
					retVal.AddRange(GetAllCategoryProperties(parentCategory));
				}
			}
			return retVal.ToArray();
		}

		public foundation.Item[] GetAllItemVariations(string itemId)
		{
			//Load Variations
			var itemIds = ItemRelations.Where(x => x.ParentItemId == itemId).Select(x => x.ChildItemId).ToArray();
			return GetItemByIds(itemIds.ToArray());
			
		}

		public void SetCategoryProperty(foundation.Category category, foundation.Property property)
		{
			if (category.PropertySet == null)
			{
				var propertySet = new foundation.PropertySet
				{
					CatalogId = category.CatalogId,
					Name = category.Name + " property set",
					TargetType = foundation.PropertyTargetType.Category.ToString()
				};
				Add(propertySet);
				category.PropertySetId = propertySet.PropertySetId;
			}

			var propertySetProperty = new foundation.PropertySetProperty
			{
				PropertySetId = category.PropertySetId,
				PropertyId = property.PropertyId
			};
			Add(propertySetProperty);

		}

		public void SetVariationRelation(foundation.Item item, string mainProductId)
		{
			var itemRelation = ItemRelations.FirstOrDefault(x => x.ChildItemId == item.ItemId);
			if (itemRelation == null)
			{
				 itemRelation = new foundation.ItemRelation
				{
					RelationTypeId = foundation.ItemRelationType.Sku,
					GroupName = "variation",
					Quantity = 1
				};
				Add(itemRelation);
			}
			if (itemRelation.ChildItemId != item.ItemId)
				itemRelation.ChildItemId = item.ItemId;
			if (itemRelation.ParentItemId != mainProductId)
				itemRelation.ParentItemId = mainProductId;

			//Need update all previous relations if main product changes
			var allPrevRelations = ItemRelations.Where(x => x.ParentItemId == item.ItemId).ToArray();
			foreach (var prevRelation in allPrevRelations)
			{
				prevRelation.ParentItemId = mainProductId;
			}
		}

		public void SwitchProductToMain(foundation.Item item)
		{
			var itemRelation = ItemRelations.FirstOrDefault(x => x.ChildItemId == item.ItemId);
			if (itemRelation != null)
			{
				//Make a old parent relation to new
				itemRelation.ChildItemId = itemRelation.ParentItemId;
				itemRelation.ParentItemId = item.ItemId;

				//Update all relations to new parent
				var allVariationRelations = ItemRelations.Include(x=>x.ChildItem).Where(x => x.ParentItemId == itemRelation.ParentItemId);
				foreach (var variationRelation in allVariationRelations)
				{
					variationRelation.ParentItemId = item.ItemId;
					//hide all variation in search
					variationRelation.ChildItem.IsActive = false;
				}
				

			}
		}

		public void SetItemCategoryRelation(foundation.Item item, foundation.Category category)
		{
			item.CategoryItemRelations.Add(new foundation.CategoryItemRelation
			{
				CatalogId = category.CatalogId,
				CategoryId = category.CategoryId,
				ItemId = item.ItemId
			});
		}

		public void RemoveItems(string[] itemIds)
		{
			var items = GetItemByIds(itemIds);
			foreach (var item in items)
			{
				base.Remove(item);
				//delete relations
				foreach (var relation in base.ItemRelations.Where(x => x.ChildItemId == item.ItemId || x.ParentItemId == item.ItemId))
				{
					base.Remove(relation);
				}
			}
		}
		#endregion


	}
}
