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

       
            #region Catalog
            modelBuilder.Entity<dataModel.Catalog>().ToTable("Catalog").HasKey(x => x.Id).Property(x=>x.Id);
            #endregion

            #region Category
            modelBuilder.Entity<dataModel.Category>().ToTable("Category").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Category>().HasOptional(x => x.ParentCategory).WithMany().HasForeignKey(x => x.ParentCategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Category>().HasRequired(x => x.Catalog).WithMany().HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(true);
            #endregion

            #region Item
            modelBuilder.Entity<dataModel.Item>().ToTable("Item").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Item>().HasRequired(m => m.Catalog).WithMany().HasForeignKey(x=>x.CatalogId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Item>().HasOptional(m => m.Category).WithMany().HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Item>().HasOptional(m => m.Parent).WithMany(x=>x.Childrens).HasForeignKey(x => x.ParentId).WillCascadeOnDelete(false);
            #endregion

            #region Property
            modelBuilder.Entity<dataModel.Property>().ToTable("Property").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Property>().HasOptional(m => m.Catalog).WithMany(x => x.Properties).HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Property>().HasOptional(m => m.Category).WithMany(x => x.Properties).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);

            #endregion

            #region PropertyDictionaryValue
            modelBuilder.Entity<dataModel.PropertyDictionaryValue>().ToTable("PropertyDictionaryValue").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.PropertyDictionaryValue>().HasRequired(m => m.Property).WithMany(x=>x.DictionaryValues).HasForeignKey(x => x.PropertyId).WillCascadeOnDelete(true);
            #endregion

            #region PropertyAttribute
            modelBuilder.Entity<dataModel.PropertyAttribute>().ToTable("PropertyAttribute").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.PropertyAttribute>().HasRequired(m => m.Property).WithMany(x => x.PropertyAttributes).HasForeignKey(x => x.PropertyId).WillCascadeOnDelete(true);
            #endregion

            #region PropertyValue
            modelBuilder.Entity<dataModel.PropertyValue>().ToTable("PropertyValue").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.PropertyValue>().HasOptional(m => m.CatalogItem).WithMany(x => x.ItemPropertyValues).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.PropertyValue>().HasOptional(m => m.Category).WithMany(x => x.CategoryPropertyValues).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.PropertyValue>().HasOptional(m => m.Catalog).WithMany(x => x.CatalogPropertyValues).HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(false);
            #endregion

            #region ItemRelation
            modelBuilder.Entity<dataModel.ItemRelation>().ToTable("ItemRelation").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.ItemRelation>().HasRequired(m => m.ChildItem).WithMany().HasForeignKey(x => x.ChildItemId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.ItemRelation>().HasOptional(m => m.ParentItem).WithMany().HasForeignKey(x => x.ParentItemId).WillCascadeOnDelete(false);

            #endregion

            #region CatalogImage
            modelBuilder.Entity<dataModel.Image>().ToTable("CatalogImage").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Image>().HasOptional(m => m.Category).WithMany(x=>x.Images).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Image>().HasOptional(m => m.CatalogItem).WithMany(x => x.Images).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(false);
            #endregion

            #region EditorialReview
            modelBuilder.Entity<dataModel.EditorialReview>().ToTable("EditorialReview").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.EditorialReview>().HasRequired(x => x.CatalogItem).WithMany(x => x.EditorialReviews).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(true);
            #endregion

            #region AssociationGroup
            modelBuilder.Entity<dataModel.AssociationGroup>().ToTable("AssociationGroup").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.AssociationGroup>().HasRequired(x => x.CatalogItem).WithMany(x => x.AssociationGroups).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(true);
            #endregion

            #region Association
            modelBuilder.Entity<dataModel.Association>().ToTable("Association").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Association>().HasRequired(m => m.CatalogItem).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Association>().HasRequired(m => m.AssociationGroup).WithMany(x => x.Associations).HasForeignKey(x => x.AssociationGroupId).WillCascadeOnDelete(true);
            #endregion

            #region Asset
            modelBuilder.Entity<dataModel.Asset>().ToTable("CatalogAsset").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Asset>().HasRequired(m => m.CatalogItem).WithMany(x=>x.Assets).HasForeignKey(x=>x.ItemId).WillCascadeOnDelete(true);
            #endregion

            #region CatalogLanguage
            modelBuilder.Entity<dataModel.CatalogLanguage>().ToTable("CatalogLanguage").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.CatalogLanguage>().HasRequired(m => m.Catalog).WithMany(x => x.CatalogLanguages).HasForeignKey(x=>x.CatalogId).WillCascadeOnDelete(true);
            #endregion

       

            #region CategoryItemRelation
            modelBuilder.Entity<dataModel.CategoryItemRelation>().ToTable("CategoryItemRelation").HasKey(x => x.Id).Property(x => x.Id);

            modelBuilder.Entity<dataModel.CategoryItemRelation>().HasRequired(p => p.Category).WithMany().HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.CategoryItemRelation>().HasRequired(p => p.CatalogItem).WithMany(x => x.CategoryLinks).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.CategoryItemRelation>().HasRequired(p => p.Catalog).WithMany().HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(false);
            #endregion


            #region CategoryRelation
            modelBuilder.Entity<dataModel.CategoryRelation>().ToTable("CategoryRelation").HasKey(x => x.Id).Property(x => x.Id);

            modelBuilder.Entity<dataModel.CategoryRelation>().HasOptional(x => x.TargetCategory)
                                       .WithMany(x => x.IncommingLinks)
                                       .HasForeignKey(x => x.TargetCategoryId).WillCascadeOnDelete(false);

            modelBuilder.Entity<dataModel.CategoryRelation>().HasRequired(x => x.SourceCategory)
                                       .WithMany(x => x.OutgoingLinks)
                                       .HasForeignKey(x => x.SourceCategoryId).WillCascadeOnDelete(false);

            modelBuilder.Entity<dataModel.CategoryRelation>().HasOptional(x => x.TargetCatalog)
                                       .WithMany(x => x.IncommingLinks)
                                       .HasForeignKey(x => x.TargetCatalogId).WillCascadeOnDelete(false);
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        #region ICatalogRepository Members
        public IQueryable<dataModel.Category> Categories
        {
            get { return GetAsQueryable<dataModel.Category>(); }
        }

        public IQueryable<dataModel.Catalog> Catalogs
        {
            get { return GetAsQueryable<dataModel.Catalog>(); }
        }

        public IQueryable<dataModel.PropertyValue> PropertyValues
        {
            get { return GetAsQueryable<dataModel.PropertyValue>(); }
        }

		public IQueryable<dataModel.Image> Images 
		{
			get { return GetAsQueryable<dataModel.Image>(); }
		}
		public IQueryable<dataModel.Asset> Assets
		{
			get { return GetAsQueryable<dataModel.Asset>(); }
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

        public IQueryable<dataModel.ItemRelation> ItemRelations
        {
            get { return GetAsQueryable<dataModel.ItemRelation>(); }
        }

        public IQueryable<dataModel.PropertyDictionaryValue> PropertyDictionaryValues
        {
            get { return GetAsQueryable<dataModel.PropertyDictionaryValue>(); }
        }
        public IQueryable<dataModel.CategoryItemRelation> CategoryItemRelations
        {
            get { return GetAsQueryable<dataModel.CategoryItemRelation>(); }
        }

        public IQueryable<dataModel.AssociationGroup> AssociationGroups
        {
            get { return GetAsQueryable<dataModel.AssociationGroup>(); }
        }

        public IQueryable<dataModel.Association> Associations
        {
            get { return GetAsQueryable<dataModel.Association>(); }
        }

        public IQueryable<dataModel.CategoryRelation> CategoryLinks
        {
            get { return GetAsQueryable<dataModel.CategoryRelation>(); }
        }

        public dataModel.Catalog GetCatalogById(string catalogId)
        {
            dataModel.Catalog retVal = Catalogs.Include(x => x.CatalogLanguages)
                                                         .Include(x => x.CatalogPropertyValues)
                                                         .Include(x => x.IncommingLinks)
                                                         .Include(x => x.Properties)
                                                         .FirstOrDefault(x => x.Id == catalogId);
            return retVal;
        }

        public dataModel.Category[] GetAllCategoryParents(dataModel.Category category)
        {
            var retVal = new List<dataModel.Category>();

            if (category.ParentCategoryId != null)
            {
                var parentCategory = Categories.FirstOrDefault(x => x.Id == category.ParentCategoryId);
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
            var result = Categories.Include(x => x.CategoryPropertyValues)
                .Include(x => x.OutgoingLinks)
                .Include(x => x.IncommingLinks)
				.Include(x=> x.Images)
                .Include(x => x.Properties)
                .FirstOrDefault(x => x.Id == categoryId);

            return result;
        }

        public dataModel.Category[] GetCategoriesByIds(string[] categoriesIds)
        {
            var result = categoriesIds.Select(GetCategoryById).ToArray();
            return result;
        }

        public dataModel.Item[] GetItemByIds(string[] itemIds, coreModel.ItemResponseGroup respGroup = coreModel.ItemResponseGroup.ItemLarge)
        {
            if (!itemIds.Any())
            {
                return new dataModel.Item[] { };
            }

            //Used breaking query EF performance concept https://msdn.microsoft.com/en-us/data/hh949853.aspx#8
            var retVal = Items.Include(x => x.Catalog).Include(x => x.Category).Include(x=>x.Images).Where(x => itemIds.Contains(x.Id)).ToArray();


            if ((respGroup & coreModel.ItemResponseGroup.Links) == coreModel.ItemResponseGroup.Links)
            {
                var relations = CategoryItemRelations.Where(x => itemIds.Contains(x.ItemId)).ToArray();
            }
            if ((respGroup & coreModel.ItemResponseGroup.ItemProperties) == coreModel.ItemResponseGroup.ItemProperties)
            {
                var propertyValues = PropertyValues.Where(x => itemIds.Contains(x.ItemId)).ToArray();
            }
            if ((respGroup & coreModel.ItemResponseGroup.ItemAssets) == coreModel.ItemResponseGroup.ItemAssets)
            {
                var assets = Assets.Where(x => itemIds.Contains(x.ItemId)).ToArray();
            }
            if ((respGroup & coreModel.ItemResponseGroup.ItemEditorialReviews) == coreModel.ItemResponseGroup.ItemEditorialReviews)
            {
                var editorialReviews = EditorialReviews.Where(x => itemIds.Contains(x.ItemId)).ToArray();
            }
            if ((respGroup & coreModel.ItemResponseGroup.ItemAssociations) == coreModel.ItemResponseGroup.ItemAssociations)
            {
                var associations = AssociationGroups.Include(x=>x.Associations).Where(x => itemIds.Contains(x.ItemId)).ToArray();
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
                                        .Include(x => x.DictionaryValues)
                                        .Include(x => x.PropertyAttributes)
                                        .Where(x => propIds.Contains(x.Id))
                                        .ToArray();
            return retVal;
        }

        public dataModel.Catalog GetPropertyCatalog(string propId)
        {
            var catalogId = Properties.Where(x=>x.Id == propId).Select(x=> x.CatalogId).FirstOrDefault();
            if (catalogId != null)
            {
                return GetCatalogById(catalogId);
            }
            return null;
        }

        public dataModel.Category GetPropertyCategory(string propId)
        {
            var categoryId = Properties.Where(x => x.Id == propId).Select(x => x.CategoryId).FirstOrDefault();
            if (categoryId != null)
            {
                return GetCategoryById(categoryId);
            }
            return null;
        }

      
        public dataModel.Property[] GetAllCategoryProperties(dataModel.Category category)
        {
            var retVal = new List<dataModel.Property>();
            retVal.AddRange(category.Properties);

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
            retVal.AddRange(category.Catalog.Properties);
            return retVal.Distinct().ToArray();
        }


          
        public void RemoveItems(string[] itemIds)
        {
            //TODO: Replace to SQL command
        
            var items = GetItemByIds(itemIds);
            foreach (var item in items)
            {
                //Delete all variations
                RemoveItems(Items.Where(x => x.ParentId == item.Id).Select(x => x.Id).ToArray());

                //Delete all item links
                var allItemLinks = CategoryItemRelations.Where(x => x.ItemId == item.Id).ToArray();
                foreach (var itemLink in allItemLinks)
                {
                    base.Remove(itemLink);
                }
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
            //TODO: Replace to SQL command
            foreach (var id in ids)
            {
                //Recursive delete all child categories
                var allChildrenIds = Categories.Where(x => x.ParentCategoryId == id).Select(x => x.Id).ToArray();
                RemoveCategories(allChildrenIds);

                //Remove all products from category
                var productsIds = Items.Where(x => x.CategoryId == id).Select(x => x.Id).ToArray();
                RemoveItems(productsIds);

                //Remove all categoryRelations
                foreach (var categoryRelation in CategoryLinks.Where(x => x.SourceCategoryId == id || x.TargetCategoryId == id))
                {
                    base.Remove(categoryRelation);
                }

                var category = GetCategoryById(id);
          
                base.Remove(category);

            }
        }

        public void RemoveCatalogs(string[] ids)
        {
            //TODO: Replace to SQL command
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
            
                base.Remove(catalogBase);

            }
        }
        #endregion


    }
}
