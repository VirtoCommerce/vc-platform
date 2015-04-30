using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
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
        public CatalogRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
		{
			
		}
		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			InheritanceMapping(modelBuilder);

			MapEntity<dataModel.CategoryItemRelation>(modelBuilder, toTable: "CategoryItemRelation");
			MapEntity<dataModel.ItemAsset>(modelBuilder, toTable: "ItemAsset");
			MapEntity<dataModel.Association>(modelBuilder, toTable: "Association");
			MapEntity<dataModel.AssociationGroup>(modelBuilder, toTable: "AssociationGroup");
			MapEntity<dataModel.CatalogLanguage>(modelBuilder, toTable: "CatalogLanguage");
			MapEntity<dataModel.EditorialReview>(modelBuilder, toTable: "EditorialReview");
			MapEntity<dataModel.ItemRelation>(modelBuilder, toTable: "ItemRelation");
			MapEntity<dataModel.Property>(modelBuilder, toTable: "Property");
			MapEntity<dataModel.PropertyAttribute>(modelBuilder, toTable: "PropertyAttribute");
			MapEntity<dataModel.PropertySet>(modelBuilder, toTable: "PropertySet");
			MapEntity<dataModel.PropertySetProperty>(modelBuilder, toTable: "PropertySetProperty");
		
			// Introducing FOREIGN KEY constraint 'FK_dbo.Association_dbo.Item_ItemId' on table 'Association' may cause cycles or multiple cascade paths.
			modelBuilder.Entity<dataModel.Association>().HasRequired(m => m.CatalogItem).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<dataModel.CategoryItemRelation>().HasRequired(p => p.Category).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<dataModel.ItemRelation>().HasRequired(m => m.ChildItem).WithMany().WillCascadeOnDelete(false);
			// cascade delete Item and Category when PropertySet is deleted. This should happen ONLY when catalog is being deleted.
			modelBuilder.Entity<dataModel.Item>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<dataModel.Category>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Inheritances the mapping.
		/// </summary>
		/// <param name="modelBuilder">The model builder.</param>
		private void InheritanceMapping(DbModelBuilder modelBuilder)
		{
			#region Catalog TPT
			modelBuilder.Entity<dataModel.CatalogBase>().Map(entity =>
			{
				entity.ToTable("CatalogBase");
			});
			modelBuilder.Entity<dataModel.VirtualCatalog>().Map(entity =>
			{
				entity.ToTable("VirtualCatalog");
			});
			modelBuilder.Entity<dataModel.Catalog>().Map(entity =>
			{
				entity.ToTable("Catalog");
			});
			#endregion

			#region Category TPT
			modelBuilder.Entity<dataModel.CategoryBase>().Map(entity =>
			{
				entity.ToTable("CategoryBase");
			});
			modelBuilder.Entity<dataModel.Category>().Map(entity =>
			{
				entity.ToTable("Category");
			});
			modelBuilder.Entity<dataModel.LinkedCategory>().Map(entity =>
			{
				entity.ToTable("LinkedCategory");
			});
			#endregion

			#region Item TPH
			MapEntity<dataModel.Item>(modelBuilder, toTable: "Item");
			MapEntity<dataModel.Product>(modelBuilder, toTable: "Item", discriminatorValue: "Product");
		
			#endregion

			#region PropertyValueBase TPC
			modelBuilder.Entity<dataModel.CatalogPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CatalogPropertyValue");
			});
			modelBuilder.Entity<dataModel.CategoryPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CategoryPropertyValue");
			});
			modelBuilder.Entity<dataModel.PropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("PropertyValue");
			});
			modelBuilder.Entity<dataModel.ItemPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("ItemPropertyValue");
			});
			#endregion

			#region Navigation properties required

			modelBuilder.Entity<dataModel.Item>()
				.HasMany(c => c.AssociationGroups)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<dataModel.Item>()
				.HasMany(c => c.CategoryItemRelations)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<dataModel.Item>()
				.HasMany(c => c.ItemAssets)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<dataModel.AssociationGroup>()
						.HasMany(c => c.Associations)
						.WithRequired(a => a.AssociationGroup);

			modelBuilder.Entity<dataModel.Property>()
						.HasMany(x => x.PropertyValues)
						.WithRequired(x => x.Property);

			#endregion
		}



		#region ICatalogRepository Members
		public IQueryable<dataModel.CategoryBase> Categories
		{
			get { return GetAsQueryable<dataModel.CategoryBase>(); }
		}

		public IQueryable<dataModel.CatalogBase> Catalogs
		{
			get { return GetAsQueryable<dataModel.CatalogBase>(); }
		}

		public IQueryable<dataModel.Item> Items
		{
			get { return GetAsQueryable<dataModel.Item>(); }
		}

		public IQueryable<dataModel.Property> Properties
		{
			get { return GetAsQueryable<dataModel.Property>(); }
		}

		public IQueryable<dataModel.PropertySet> PropertySets
		{
			get { return GetAsQueryable<dataModel.PropertySet>(); }
		}

		public IQueryable<dataModel.ItemRelation> ItemRelations
		{
			get { return GetAsQueryable<dataModel.ItemRelation>(); }
		}

		public IQueryable<dataModel.CategoryItemRelation> CategoryItemRelations
		{
			get { return GetAsQueryable<dataModel.CategoryItemRelation>(); }
		}

		public IQueryable<dataModel.Association> Associations
		{
			get { return GetAsQueryable<dataModel.Association>(); }
		}

		public dataModel.CatalogBase GetCatalogById(string catalogId)
		{
			dataModel.CatalogBase retVal = Catalogs.OfType<dataModel.Catalog>()
														 .Include(x => x.CatalogLanguages)
														 .Include(x => x.CatalogPropertyValues)
														 .Include(x => x.PropertySet.PropertySetProperties.Select(y => y.Property))
														 .FirstOrDefault(x => x.Id == catalogId);
			if (retVal == null)
			{
				retVal = Catalogs.OfType<dataModel.VirtualCatalog>()
								 .FirstOrDefault(x => x.Id == catalogId);
			}
			return retVal;
		}

		public dataModel.LinkedCategory[] GetCategoryLinks(string categoryId)
		{
			var retVal = new List<dataModel.LinkedCategory>();
			//Load links for both categories (source and target)
			var allLinks = Categories.OfType<dataModel.LinkedCategory>()
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


		public dataModel.LinkedCategory[] GetCatalogLinks(string catalogId)
		{
			var retVal = Categories.OfType<dataModel.LinkedCategory>()
										.AsNoTracking()
										.Where(x => x.LinkedCatalogId == catalogId && x.LinkedCategoryId == null)
										.ToArray();

			return retVal;
		}

		public dataModel.Category[] GetAllCategoryParents(dataModel.Category category)
		{
			var retVal = new List<dataModel.Category>();

			if (category.ParentCategoryId != null)
			{
				var parentCategory = Categories.OfType<dataModel.Category>()
									.FirstOrDefault(x => x.Id == category.ParentCategoryId);
				if (parentCategory != null)
				{
					retVal.Add(parentCategory);
					retVal.AddRange(GetAllCategoryParents(parentCategory));
				}
			}
			return retVal.ToArray();
		}

		public dataModel.Category GetCategoryById(string categoryId)
		{
			var retVal = Categories.OfType<dataModel.Category>()
										.Include(x => x.CategoryPropertyValues)
										.Include(x => x.PropertySet.PropertySetProperties.Select(y => y.Property))
										.FirstOrDefault(x => x.Id == categoryId);

					
			return retVal;
		}

		public dataModel.Item[] GetItemByIds(string[] itemIds, coreModel.ItemResponseGroup respGroup = coreModel.ItemResponseGroup.ItemLarge)
		{
			var query = Items.Include(x => x.Catalog).Where(x => itemIds.Contains(x.Id));

            if ((respGroup & coreModel.ItemResponseGroup.Categories) == coreModel.ItemResponseGroup.Categories)
            {
                query = query.Include(x => x.CategoryItemRelations);
            }
			if ((respGroup & coreModel.ItemResponseGroup.ItemProperties) == coreModel.ItemResponseGroup.ItemProperties)
			{
				query = query.Include(x => x.ItemPropertyValues);
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemAssets) == coreModel.ItemResponseGroup.ItemAssets)
			{
				query = query.Include(x => x.ItemAssets);
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemEditorialReviews) == coreModel.ItemResponseGroup.ItemEditorialReviews)
			{
				query = query.Include(x => x.EditorialReviews);
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemAssociations) == coreModel.ItemResponseGroup.ItemAssociations)
			{
				query = query.Include(x => x.AssociationGroups.Select(y=>y.Associations));
			}
			
			var retVal = query.ToArray();
			return retVal;
		}

		public dataModel.Property[] GetPropertiesByIds(string[] propIds)
		{
			var retVal = Properties.Include(x => x.Catalog)
										.Include(x => x.PropertyValues)
										.Include(x => x.PropertyAttributes)
										.Where(x => propIds.Contains(x.Id))
										.ToArray();
			return retVal;
		}
		public dataModel.Catalog GetPropertyCatalog(string propId)
		{
			dataModel.Catalog retVal = null;
			var propSet = PropertySets.FirstOrDefault(x => x.PropertySetProperties.Any(y => y.PropertyId == propId));
			if (propSet != null)
			{
				var catalogId = Catalogs.OfType<dataModel.Catalog>()
								   .Where(x => x.PropertySetId == propSet.Id)
								   .Select(x => x.Id).FirstOrDefault();
				if (catalogId != null)
				{
					retVal = GetCatalogById(catalogId) as dataModel.Catalog;
				}
			}
			return retVal;
		}

		public dataModel.Category GetPropertyCategory(string propId)
		{
			dataModel.Category retVal = null;
			var propSet = PropertySets.FirstOrDefault(x => x.PropertySetProperties.Any(y => y.PropertyId == propId));
			if (propSet != null)
			{
				var categoryId = Categories.OfType<dataModel.Category>()
								   .Where(x => x.PropertySetId == propSet.Id)
								   .Select(x => x.Id).FirstOrDefault();
				if (categoryId != null)
				{
					retVal = GetCategoryById(categoryId);
				}
			}
			return retVal;
		}

		public dataModel.Property[] GetCatalogProperties(dataModel.CatalogBase catalogBase)
		{
			var retVal = new List<dataModel.Property>();
			var catalog = catalogBase as dataModel.Catalog;
			if (catalog != null)
			{
				if(catalog.PropertySet == null && catalog.PropertySetId != null)
				{
					catalog = GetCatalogById(catalogBase.Id) as dataModel.Catalog;
				}
				if (catalog.PropertySet != null)
				{
					retVal.AddRange(catalog.PropertySet.PropertySetProperties.Select(x => x.Property));
				}
			}
			return retVal.ToArray();
		}

		public dataModel.Property[] GetAllCategoryProperties(dataModel.Category category)
		{
			var retVal = new List<dataModel.Property>();
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

		public dataModel.Item[] GetAllItemVariations(string itemId)
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
        public Dictionary<string, IEnumerable<dataModel.Item>> GetAllItemsVariations(string[] itemIds)
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

		public void SetCatalogProperty(dataModel.Catalog catalog, dataModel.Property property)
		{
			if (catalog.PropertySet == null)
			{
				var propertySet = new dataModel.PropertySet
				{
					CatalogId = catalog.Id,
					Name = catalog.Name + " property set",
					TargetType = "Catalog"
				};
				Add(propertySet);
				catalog.PropertySetId = propertySet.Id;
			}

			var propertySetProperty = new dataModel.PropertySetProperty
			{
				PropertySetId = catalog.PropertySetId,
				PropertyId = property.Id
			};
			Add(propertySetProperty);

		}

		public void SetCategoryProperty(dataModel.Category category, dataModel.Property property)
		{
			if (category.PropertySet == null)
			{
				var propertySet = new dataModel.PropertySet
				{
					CatalogId = category.CatalogId,
					Name = category.Name + " property set",
					TargetType = "Category"
				};
				Add(propertySet);
				category.PropertySetId = propertySet.Id;
			}

			var propertySetProperty = new dataModel.PropertySetProperty
			{
				PropertySetId = category.PropertySetId,
				PropertyId = property.Id
			};
			Add(propertySetProperty);

		}

		public void SetVariationRelation(dataModel.Item item, string mainProductId)
		{
			var itemRelation = ItemRelations.FirstOrDefault(x => x.ChildItemId == item.Id);
			if (itemRelation == null)
			{
				 itemRelation = new dataModel.ItemRelation
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

		public void SwitchProductToMain(dataModel.Item item)
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

		public void SetItemCategoryRelation(dataModel.Item item, dataModel.Category category)
		{
			item.CategoryItemRelations.Add(new dataModel.CategoryItemRelation
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
