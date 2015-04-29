using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public class CatalogRepositoryImpl : EFRepositoryBase, ICatalogRepository
	{
		public CatalogRepositoryImpl()
		{
		}

        public CatalogRepositoryImpl(string nameOrConnectionString)
            : this(nameOrConnectionString, null)
        {
        }
        public CatalogRepositoryImpl(string nameOrConnectionString, IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
		{
			
		}
		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<foundation.CategoryItemRelation>().HasKey(x => x.Id).Property(x => x.Id)
								.HasColumnName("CategoryItemRelationId");
			modelBuilder.Entity<foundation.ItemAsset>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("ItemAssetId");
			modelBuilder.Entity<foundation.Association>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("AssociationId");
			modelBuilder.Entity<foundation.AssociationGroup>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("AssociationGroupId");
			modelBuilder.Entity<foundation.CatalogBase>().HasKey(x => x.Id).Property(x => x.Id)
									.HasColumnName("CatalogId");
			modelBuilder.Entity<foundation.CategoryBase>().HasKey(x => x.Id).Property(x => x.Id)
								.HasColumnName("CategoryId");
			modelBuilder.Entity<foundation.CatalogLanguage>().HasKey(x => x.Id).Property(x => x.Id)
										.HasColumnName("CatalogLanguageId");
			modelBuilder.Entity<foundation.EditorialReview>().HasKey(x => x.Id).Property(x => x.Id)
									.HasColumnName("EditorialReviewId");
			modelBuilder.Entity<foundation.ItemRelation>().HasKey(x => x.Id).Property(x => x.Id)
									.HasColumnName("ItemRelationId");
			modelBuilder.Entity<foundation.Property>().HasKey(x => x.Id).Property(x => x.Id)
							.HasColumnName("PropertyId");
			modelBuilder.Entity<foundation.PropertyValueBase>().HasKey(x => x.Id).Property(x => x.Id)
							.HasColumnName("PropertyValueId");
			modelBuilder.Entity<foundation.Item>().HasKey(x => x.Id).Property(x => x.Id)
						.HasColumnName("ItemId");
			modelBuilder.Entity<foundation.PropertyAttribute>().HasKey(x => x.Id).Property(x => x.Id)
						.HasColumnName("PropertyAttributeId");
			modelBuilder.Entity<foundation.PropertySet>().HasKey(x => x.Id).Property(x => x.Id)
						.HasColumnName("PropertySetId");
			modelBuilder.Entity<foundation.PropertySetProperty>().HasKey(x => x.Id).Property(x => x.Id)
						.HasColumnName("PropertySetPropertyId");

			InheritanceMapping(modelBuilder);

			MapEntity<foundation.CategoryItemRelation>(modelBuilder, toTable: "CategoryItemRelation");
			MapEntity<foundation.ItemAsset>(modelBuilder, toTable: "ItemAsset");
			MapEntity<foundation.Association>(modelBuilder, toTable: "Association");
			MapEntity<foundation.AssociationGroup>(modelBuilder, toTable: "AssociationGroup");
			MapEntity<foundation.CatalogLanguage>(modelBuilder, toTable: "CatalogLanguage");
			MapEntity<foundation.EditorialReview>(modelBuilder, toTable: "EditorialReview");
			MapEntity<foundation.ItemRelation>(modelBuilder, toTable: "ItemRelation");
			MapEntity<foundation.Property>(modelBuilder, toTable: "Property");
			MapEntity<foundation.PropertyAttribute>(modelBuilder, toTable: "PropertyAttribute");
			MapEntity<foundation.PropertySet>(modelBuilder, toTable: "PropertySet");
			MapEntity<foundation.PropertySetProperty>(modelBuilder, toTable: "PropertySetProperty");
		
			// Introducing FOREIGN KEY constraint 'FK_dbo.Association_dbo.Item_ItemId' on table 'Association' may cause cycles or multiple cascade paths.
			modelBuilder.Entity<foundation.Association>().HasRequired(m => m.CatalogItem).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<foundation.CategoryItemRelation>().HasRequired(p => p.Category).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<foundation.ItemRelation>().HasRequired(m => m.ChildItem).WithMany().WillCascadeOnDelete(false);
			// cascade delete Item and Category when PropertySet is deleted. This should happen ONLY when catalog is being deleted.
			modelBuilder.Entity<foundation.Item>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<foundation.Category>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Inheritances the mapping.
		/// </summary>
		/// <param name="modelBuilder">The model builder.</param>
		private void InheritanceMapping(DbModelBuilder modelBuilder)
		{
			#region Catalog TPT
			modelBuilder.Entity<foundation.CatalogBase>().Map(entity =>
			{
				entity.ToTable("CatalogBase");
			});
			modelBuilder.Entity<foundation.VirtualCatalog>().Map(entity =>
			{
				entity.ToTable("VirtualCatalog");
			});
			modelBuilder.Entity<foundation.Catalog>().Map(entity =>
			{
				entity.ToTable("Catalog");
			});
			#endregion

			#region Category TPT
			modelBuilder.Entity<foundation.CategoryBase>().Map(entity =>
			{
				entity.ToTable("CategoryBase");
			});
			modelBuilder.Entity<foundation.Category>().Map(entity =>
			{
				entity.ToTable("Category");
			});
			modelBuilder.Entity<foundation.LinkedCategory>().Map(entity =>
			{
				entity.ToTable("LinkedCategory");
			});
			#endregion

			#region Item TPH
			MapEntity<foundation.Item>(modelBuilder, toTable: "Item");
			MapEntity<foundation.Product>(modelBuilder, toTable: "Item", discriminatorValue: "Product");
		
			#endregion

			#region PropertyValueBase TPC
			modelBuilder.Entity<foundation.CatalogPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CatalogPropertyValue");
			});
			modelBuilder.Entity<foundation.CategoryPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CategoryPropertyValue");
			});
			modelBuilder.Entity<foundation.PropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("PropertyValue");
			});
			modelBuilder.Entity<foundation.ItemPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("ItemPropertyValue");
			});
			#endregion

			#region Navigation properties required

			modelBuilder.Entity<foundation.Item>()
				.HasMany(c => c.AssociationGroups)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<foundation.Item>()
				.HasMany(c => c.CategoryItemRelations)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<foundation.Item>()
				.HasMany(c => c.ItemAssets)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<foundation.AssociationGroup>()
						.HasMany(c => c.Associations)
						.WithRequired(a => a.AssociationGroup);

			modelBuilder.Entity<foundation.Property>()
						.HasMany(x => x.PropertyValues)
						.WithRequired(x => x.Property);

			#endregion
		}



		#region ICatalogRepository Members

		public IQueryable<foundation.SeoUrlKeyword> SeoUrlKeywords
		{
			get { return GetAsQueryable<foundation.SeoUrlKeyword>(); }
		}

		public IQueryable<foundation.CategoryBase> Categories
		{
			get { return GetAsQueryable<foundation.CategoryBase>(); }
		}

		public IQueryable<foundation.CatalogBase> Catalogs
		{
			get { return GetAsQueryable<foundation.CatalogBase>(); }
		}

		public IQueryable<foundation.Item> Items
		{
			get { return GetAsQueryable<foundation.Item>(); }
		}

		public IQueryable<foundation.Property> Properties
		{
			get { return GetAsQueryable<foundation.Property>(); }
		}

		public IQueryable<foundation.PropertySet> PropertySets
		{
			get { return GetAsQueryable<foundation.PropertySet>(); }
		}

		public IQueryable<foundation.ItemRelation> ItemRelations
		{
			get { return GetAsQueryable<foundation.ItemRelation>(); }
		}

		public IQueryable<foundation.CategoryItemRelation> CategoryItemRelations
		{
			get { return GetAsQueryable<foundation.CategoryItemRelation>(); }
		}

		public IQueryable<foundation.Association> Associations
		{
			get { return GetAsQueryable<foundation.Association>(); }
		}

		public foundation.SeoUrlKeyword[] GetAllSeoInformation(string id)
		{
			return SeoUrlKeywords.Where(x => x.IsActive && (string.IsNullOrEmpty(id) || x.KeywordValue == id)).ToArray();
		}

		public foundation.CatalogBase GetCatalogById(string catalogId)
		{
			foundation.CatalogBase retVal = Catalogs.OfType<foundation.Catalog>()
														 .Include(x => x.CatalogLanguages)
														 .Include(x => x.CatalogPropertyValues)
														 .Include(x => x.PropertySet.PropertySetProperties.Select(y => y.Property))
														 .FirstOrDefault(x => x.Id == catalogId);
			if (retVal == null)
			{
				retVal = Catalogs.OfType<foundation.VirtualCatalog>()
								 .FirstOrDefault(x => x.Id == catalogId);
			}
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
				retVal.Add(link);
			
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
									.FirstOrDefault(x => x.Id == category.ParentCategoryId);
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
										.FirstOrDefault(x => x.Id == categoryId);

					
			return retVal;
		}

		public foundation.Item[] GetItemByIds(string[] itemIds, moduleModel.ItemResponseGroup respGroup = moduleModel.ItemResponseGroup.ItemLarge)
		{
			var query = Items.Include(x => x.Catalog).Where(x => itemIds.Contains(x.Id));

            if ((respGroup & moduleModel.ItemResponseGroup.Categories) == moduleModel.ItemResponseGroup.Categories)
            {
                query = query.Include(x => x.CategoryItemRelations);
            }
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
										.Where(x => propIds.Contains(x.Id))
										.ToArray();
			return retVal;
		}
		public foundation.Catalog GetPropertyCatalog(string propId)
		{
			foundation.Catalog retVal = null;
			var propSet = PropertySets.FirstOrDefault(x => x.PropertySetProperties.Any(y => y.PropertyId == propId));
			if (propSet != null)
			{
				var catalogId = Catalogs.OfType<foundation.Catalog>()
								   .Where(x => x.PropertySetId == propSet.Id)
								   .Select(x => x.Id).FirstOrDefault();
				if (catalogId != null)
				{
					retVal = GetCatalogById(catalogId) as foundation.Catalog;
				}
			}
			return retVal;
		}

		public foundation.Category GetPropertyCategory(string propId)
		{
			foundation.Category retVal = null;
			var propSet = PropertySets.FirstOrDefault(x => x.PropertySetProperties.Any(y => y.PropertyId == propId));
			if (propSet != null)
			{
				var categoryId = Categories.OfType<foundation.Category>()
								   .Where(x => x.PropertySetId == propSet.Id)
								   .Select(x => x.Id).FirstOrDefault();
				if (categoryId != null)
				{
					retVal = GetCategoryById(categoryId);
				}
			}
			return retVal;
		}

		public foundation.Property[] GetCatalogProperties(foundation.CatalogBase catalogBase)
		{
			var retVal = new List<foundation.Property>();
			var catalog = catalogBase as foundation.Catalog;
			if (catalog != null)
			{
				if(catalog.PropertySet == null && catalog.PropertySetId != null)
				{
					catalog = GetCatalogById(catalogBase.Id) as foundation.Catalog;
				}
				if (catalog.PropertySet != null)
				{
					retVal.AddRange(catalog.PropertySet.PropertySetProperties.Select(x => x.Property));
				}
			}
			return retVal.ToArray();
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

			//Add catalog properties
			if(category.Catalog == null)
			{
				category.Catalog = GetCatalogById(category.CatalogId);
			}
			retVal.AddRange(GetCatalogProperties(category.Catalog));
			return retVal.Distinct().ToArray();
		}

		public foundation.Item[] GetAllItemVariations(string itemId)
		{
			//Load Variations
			var itemIds = ItemRelations.Where(x => x.ParentItemId == itemId).Select(x => x.ChildItemId).ToArray();
			return GetItemByIds(itemIds.ToArray());
			
		}

        /// <summary>
        /// Method loads all items variations using single database request and returns them grouped by the item id.
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public Dictionary<string, IEnumerable<foundation.Item>> GetAllItemsVariations(string[] itemIds)
        {
            var singleRelations =
                ItemRelations.Where(x => itemIds.Contains(x.ParentItemId))
                    .Select(x => new { Parent = x.ParentItemId, Child = x.ChildItemId })
                    .ToArray();

            // group items
            var groupedRelations = singleRelations.GroupBy(x => x.Parent, x => x.Child, (key, g) => new { Parent = key, Children = g.ToList()});

            // now get items from database all at once
            var relationItemIds = singleRelations.Select(x => x.Child).Distinct().ToArray();

            var items = GetItemByIds(relationItemIds);

            var groupedItems =
                groupedRelations.Select(
                    x => new { Parent = x.Parent, Children = items.Where(y => x.Children.Contains(y.Id)) }).ToDictionary(x=>x.Parent, y=>y.Children);

            return groupedItems;
        }

		public void SetCatalogProperty(foundation.Catalog catalog, foundation.Property property)
		{
			if (catalog.PropertySet == null)
			{
				var propertySet = new foundation.PropertySet
				{
					CatalogId = catalog.Id,
					Name = catalog.Name + " property set",
					TargetType = "Catalog"
				};
				Add(propertySet);
				catalog.PropertySetId = propertySet.Id;
			}

			var propertySetProperty = new foundation.PropertySetProperty
			{
				PropertySetId = catalog.PropertySetId,
				PropertyId = property.Id
			};
			Add(propertySetProperty);

		}

		public void SetCategoryProperty(foundation.Category category, foundation.Property property)
		{
			if (category.PropertySet == null)
			{
				var propertySet = new foundation.PropertySet
				{
					CatalogId = category.CatalogId,
					Name = category.Name + " property set",
					TargetType = "Category"
				};
				Add(propertySet);
				category.PropertySetId = propertySet.Id;
			}

			var propertySetProperty = new foundation.PropertySetProperty
			{
				PropertySetId = category.PropertySetId,
				PropertyId = property.Id
			};
			Add(propertySetProperty);

		}

		public void SetVariationRelation(foundation.Item item, string mainProductId)
		{
			var itemRelation = ItemRelations.FirstOrDefault(x => x.ChildItemId == item.Id);
			if (itemRelation == null)
			{
				 itemRelation = new foundation.ItemRelation
				{
					RelationTypeId = "Sku",
					GroupName = "variation",
					Quantity = 1
				};
				Add(itemRelation);
			}
			if (itemRelation.ChildItemId != item.Id)
				itemRelation.ChildItemId = item.Id;
			if (itemRelation.ParentItemId != mainProductId)
				itemRelation.ParentItemId = mainProductId;

			//Need update all previous relations if main product changes
			var allPrevRelations = ItemRelations.Where(x => x.ParentItemId == item.Id).ToArray();
			foreach (var prevRelation in allPrevRelations)
			{
				prevRelation.ParentItemId = mainProductId;
			}
		}

		public void SwitchProductToMain(foundation.Item item)
		{
			var itemRelation = ItemRelations.FirstOrDefault(x => x.ChildItemId == item.Id);
			if (itemRelation != null)
			{
				//Make a old parent relation to new
				itemRelation.ChildItemId = itemRelation.ParentItemId;
				itemRelation.ParentItemId = item.Id;

				//Update all relations to new parent
				var allVariationRelations = ItemRelations.Include(x=>x.ChildItem).Where(x => x.ParentItemId == itemRelation.ParentItemId);
				foreach (var variationRelation in allVariationRelations)
				{
					variationRelation.ParentItemId = item.Id;
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
				CategoryId = category.Id,
				ItemId = item.Id
			});
		}

		public void RemoveItems(string[] itemIds)
		{
			var items = GetItemByIds(itemIds);
			foreach (var item in items)
			{
				base.Remove(item);
				//delete relations
				foreach (var relation in ItemRelations.Where(x => x.ChildItemId == item.Id || x.ParentItemId == item.Id))
				{
					base.Remove(relation);
				}
			}
		}
		#endregion


	}
}
