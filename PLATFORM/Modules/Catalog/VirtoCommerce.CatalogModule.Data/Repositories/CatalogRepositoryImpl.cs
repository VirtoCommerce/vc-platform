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
			Database.SetInitializer<CatalogRepositoryImpl>(null);
		}

		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			InheritanceMapping(modelBuilder);

			#region CategoryLink

			modelBuilder.Entity<dataModel.CategoryRelation>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<dataModel.CategoryRelation>().HasOptional(x => x.TargetCategory)
									   .WithMany(x => x.IncommingLinks)
									   .HasForeignKey(x => x.TargetCategoryId).WillCascadeOnDelete(false);

			modelBuilder.Entity<dataModel.CategoryRelation>().HasRequired(x => x.SourceCategory)
									   .WithMany(x => x.OutgoingLinks)
									   .HasForeignKey(x => x.SourceCategoryId).WillCascadeOnDelete(false);

			modelBuilder.Entity<dataModel.CategoryRelation>().HasOptional(x => x.TargetCatalog)
									   .WithMany(x => x.IncommingLinks)
									   .HasForeignKey(x => x.TargetCatalogId).WillCascadeOnDelete(false);

			modelBuilder.Entity<dataModel.CategoryRelation>().ToTable("CategoryRelation");
			#endregion

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
			modelBuilder.Entity<dataModel.Category>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(true);
			modelBuilder.Entity<dataModel.Catalog>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(true);

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
			//modelBuilder.Entity<dataModel.LinkedCategory>().Map(entity =>
			//{
			//	entity.ToTable("LinkedCategory");
			//});
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

			modelBuilder.Entity<dataModel.Item>()
				.HasMany(c => c.Childrens)
				.WithOptional(p => p.Parent).HasForeignKey(x => x.ParentId);

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

		public IQueryable<dataModel.ItemPropertyValue> ItemPropertyValues
		{
			get { return GetAsQueryable<dataModel.ItemPropertyValue>(); }
		}

		public IQueryable<dataModel.ItemAsset> ItemAssets
		{
			get { return GetAsQueryable<dataModel.ItemAsset>(); }
		}

		public IQueryable<dataModel.Item> Items
		{
			get { return GetAsQueryable<dataModel.Item>(); }
		}

		public IQueryable<dataModel.EditorialReview> EditorialReviews
		{
			get { return GetAsQueryable<dataModel.EditorialReview>(); }
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

		public IQueryable<dataModel.CategoryRelation> CategoryLinks
		{
			get { return GetAsQueryable<dataModel.CategoryRelation>(); }
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
								.Include(x => x.IncommingLinks)
								.FirstOrDefault(x => x.Id == catalogId);
			}
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
			return GetCategoriesByIds(new string[] { categoryId }).FirstOrDefault();
		}
		public dataModel.Category[] GetCategoriesByIds(string[] categoriesIds)
		{
			var retVal = Categories.Where(x => categoriesIds.Contains(x.Id)).OfType<dataModel.Category>()
										.Include(x => x.CategoryPropertyValues)
										.Include(x => x.OutgoingLinks)
										.Include(x => x.IncommingLinks)
										.Include(x => x.PropertySet.PropertySetProperties.Select(y => y.Property));
			return retVal.ToArray();
		}

		public dataModel.Item[] GetItemByIds(string[] itemIds, coreModel.ItemResponseGroup respGroup = coreModel.ItemResponseGroup.ItemLarge)
		{
			if (!itemIds.Any())
				return new dataModel.Item[] { };
			//Used breaking query EF performance concept https://msdn.microsoft.com/en-us/data/hh949853.aspx#8
			var retVal = Items.Include(x => x.Catalog).Where(x => itemIds.Contains(x.Id)).ToArray();


			if ((respGroup & coreModel.ItemResponseGroup.Categories) == coreModel.ItemResponseGroup.Categories)
			{
				var relations = CategoryItemRelations.Where(x => itemIds.Contains(x.ItemId)).ToArray();
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemProperties) == coreModel.ItemResponseGroup.ItemProperties)
			{
				var propertyValues = ItemPropertyValues.Where(x => itemIds.Contains(x.ItemId)).ToArray();
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemAssets) == coreModel.ItemResponseGroup.ItemAssets)
			{
				var assets = ItemAssets.Where(x => itemIds.Contains(x.ItemId)).ToArray();
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemEditorialReviews) == coreModel.ItemResponseGroup.ItemEditorialReviews)
			{
				var editorialReviews = EditorialReviews.Where(x => itemIds.Contains(x.ItemId)).ToArray();
			}
			if ((respGroup & coreModel.ItemResponseGroup.ItemAssociations) == coreModel.ItemResponseGroup.ItemAssociations)
			{
				var associations = Associations.Where(x => itemIds.Contains(x.ItemId)).ToArray();
			}
			if ((respGroup & coreModel.ItemResponseGroup.Variations) == coreModel.ItemResponseGroup.Variations)
			{
				var variationIds = Items.Where(x => itemIds.Contains(x.ParentId)).Select(x => x.Id).ToArray();
				var variations = GetItemByIds(variationIds, respGroup);
			}
			//Load parents
			var parentIds = retVal.Where(x => x.Parent == null && x.ParentId != null).Select(x => x.ParentId).ToArray();
			var parents = GetItemByIds(parentIds, respGroup);
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
				if (catalog.PropertySet == null && catalog.PropertySetId != null)
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
			if (category.Catalog == null)
			{
				category.Catalog = GetCatalogById(category.CatalogId);
			}
			retVal.AddRange(GetCatalogProperties(category.Catalog));
			return retVal.Distinct().ToArray();
		}


		public void SetCatalogProperty(dataModel.Catalog catalog, dataModel.Property property)
		{
			if (catalog.PropertySet == null)
			{
				var propertySet = new dataModel.PropertySet
				{
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
				//Delete all variations
				RemoveItems(Items.Where(x => x.ParentId == item.Id).Select(x => x.Id).ToArray());

				//delete all relations
				var allItemRelations = ItemRelations.Where(x => x.ChildItemId == item.Id || x.ParentItemId == item.Id);
				foreach (var relation in allItemRelations)
				{
					base.Remove(relation);
				}

				base.Remove(item);

			}
		}

		public void RemoveCategories(string[] ids)
		{
			foreach (var id in ids)
			{
				//Recursive delete all child categories
				var allChildrenIds = Categories.Where(x => x.ParentCategoryId == id).Select(x => x.Id).ToArray();
				RemoveCategories(allChildrenIds);

				//Remove all products from category
				var productsIds = CategoryItemRelations.Where(x => x.CategoryId == id).Select(x => x.ItemId).ToArray();
				RemoveItems(productsIds);

				//Remove all categoryRelations
				foreach (var categoryRelation in CategoryLinks.Where(x => x.SourceCategoryId == id || x.TargetCategoryId == id))
				{
					base.Remove(categoryRelation);
				}

				var category = GetCategoryById(id);
				if (category.PropertySet != null)
				{
					//Remove properties
					foreach (var property in category.PropertySet.PropertySetProperties.Select(x => x.Property).ToArray())
					{
						base.Remove(property);
					}

				}

				base.Remove(category);

			}
		}

		public void RemoveCatalogs(string[] ids)
		{
			foreach (var id in ids)
			{
				//Recursive remove all categories and products
				var categoriesIds = Categories.Where(x => x.CatalogId == id && x.ParentCategoryId == null).Select(x => x.Id).ToArray();
				RemoveCategories(categoriesIds);

				//Remove items direct belong to catalog
				var itemIds = Items.Where(x => x.CatalogId == id).Select(x => x.Id).ToArray();
				RemoveItems(itemIds);

				//Remove catalog itself
				var catalogBase = GetCatalogById(id);
				var catalog = catalogBase as dataModel.Catalog;
				if (catalog != null && catalog.PropertySet != null)
				{
					foreach (var property in catalog.PropertySet.PropertySetProperties.Select(x => x.Property).ToArray())
					{
						base.Remove(property);
					}
				}

				base.Remove(catalogBase);

			}
		}
		#endregion


	}
}
