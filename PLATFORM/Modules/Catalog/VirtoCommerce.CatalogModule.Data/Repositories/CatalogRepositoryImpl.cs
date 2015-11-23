using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.Text;

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
            modelBuilder.Entity<dataModel.Catalog>().ToTable("Catalog").HasKey(x => x.Id).Property(x => x.Id);
            #endregion

            #region Category
            modelBuilder.Entity<dataModel.Category>().ToTable("Category").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Category>().HasOptional(x => x.ParentCategory).WithMany().HasForeignKey(x => x.ParentCategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Category>().HasRequired(x => x.Catalog).WithMany().HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(true);
            #endregion

            #region Item
            modelBuilder.Entity<dataModel.Item>().ToTable("Item").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Item>().HasRequired(m => m.Catalog).WithMany().HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Item>().HasOptional(m => m.Category).WithMany().HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Item>().HasOptional(m => m.Parent).WithMany(x => x.Childrens).HasForeignKey(x => x.ParentId).WillCascadeOnDelete(false);
            #endregion

            #region Property
            modelBuilder.Entity<dataModel.Property>().ToTable("Property").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.Property>().HasOptional(m => m.Catalog).WithMany(x => x.Properties).HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(false);
            modelBuilder.Entity<dataModel.Property>().HasOptional(m => m.Category).WithMany(x => x.Properties).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);

            #endregion

            #region PropertyDictionaryValue
            modelBuilder.Entity<dataModel.PropertyDictionaryValue>().ToTable("PropertyDictionaryValue").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.PropertyDictionaryValue>().HasRequired(m => m.Property).WithMany(x => x.DictionaryValues).HasForeignKey(x => x.PropertyId).WillCascadeOnDelete(true);
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
            modelBuilder.Entity<dataModel.Image>().HasOptional(m => m.Category).WithMany(x => x.Images).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
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
            modelBuilder.Entity<dataModel.Asset>().HasRequired(m => m.CatalogItem).WithMany(x => x.Assets).HasForeignKey(x => x.ItemId).WillCascadeOnDelete(true);
            #endregion

            #region CatalogLanguage
            modelBuilder.Entity<dataModel.CatalogLanguage>().ToTable("CatalogLanguage").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<dataModel.CatalogLanguage>().HasRequired(m => m.Catalog).WithMany(x => x.CatalogLanguages).HasForeignKey(x => x.CatalogId).WillCascadeOnDelete(true);
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
                .Include(x => x.Images)
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
            var retVal = Items.Include(x => x.Catalog).Include(x => x.Category).Include(x => x.Images).Where(x => itemIds.Contains(x.Id)).ToArray();


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
                var associations = AssociationGroups.Include(x => x.Associations).Where(x => itemIds.Contains(x.ItemId)).ToArray();
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
            var catalogId = Properties.Where(x => x.Id == propId).Select(x => x.CatalogId).FirstOrDefault();
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

        //Load all category properties with inherited from parents categories and catalog
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
            retVal.AddRange(category.Catalog.Properties.Where(x=>x.CategoryId == null));
            return retVal.Distinct().ToArray();
        }



        public void RemoveItems(string[] itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return;
  
            const string queryPattern =
            @"DELETE CR FROM CategoryItemRelation  CR INNER JOIN Item I ON I.Id = CR.ItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE IR FROM ItemRelation  IR INNER JOIN Item I ON I.Id = IR.ChildItemId  OR I.Id = IR.ParentItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE CI FROM CatalogImage CI INNER JOIN Item I ON I.Id = CI.ItemId
            WHERE I.Id IN ({0})  OR I.ParentId IN ({0})

            DELETE CA FROM CatalogAsset CA INNER JOIN Item I ON I.Id = CA.ItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE PV FROM PropertyValue PV INNER JOIN Item I ON I.Id = PV.ItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE ER FROM EditorialReview ER INNER JOIN Item I ON I.Id = ER.ItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE AG FROM AssociationGroup AG INNER JOIN Item I ON I.Id = AG.ItemId
            WHERE I.Id IN ({0}) OR I.ParentId IN ({0})

            DELETE  FROM Item  WHERE ParentId IN ({0})

            DELETE  FROM Item  WHERE Id IN ({0})";

            var query = String.Format(queryPattern, String.Join(", ", itemIds.Select(x => String.Format("'{0}'", x))));

            ObjectContext.ExecuteStoreCommand(query);
        }

        public void RemoveCategories(string[] ids)
        {
            if (ids == null || !ids.Any())
                return;

            var allCategoriesIds = GetAllCategoriesChildrenIdsRecursive(ids).Concat(ids);
            const string queryPattern =
            @"DELETE CI FROM CatalogImage CI INNER JOIN Category C ON C.Id = CI.CategoryId WHERE C.Id IN ({0}) 
            DELETE PV FROM PropertyValue PV INNER JOIN Category C ON C.Id = PV.CategoryId WHERE C.Id IN ({0}) 
            DELETE CR FROM CategoryRelation CR INNER JOIN Category C ON C.Id = CR.SourceCategoryId OR C.Id = CR.TargetCategoryId  WHERE C.Id IN ({0}) 
            DELETE P FROM Property P INNER JOIN Category C ON C.Id = P.CategoryId  WHERE C.Id IN ({0})";
                
            var itemsIds = Items.Where(x => allCategoriesIds.Contains(x.CategoryId)).Select(x => x.Id).ToArray();

            RemoveItems(itemsIds);

            var query = String.Format(queryPattern, String.Join(", ", allCategoriesIds.Select(x => String.Format("'{0}'", x))));
            var queryBuilder = new StringBuilder(query);
            //Need remove categories in prior hierarchy order from  child to parent
            foreach (var categoryId in allCategoriesIds)
            {
                queryBuilder.AppendLine(String.Format("DELETE FROM Category WHERE Id = '{0}'", categoryId));
            }
            ObjectContext.ExecuteStoreCommand(queryBuilder.ToString());
        }

        public void RemoveCatalogs(string[] ids)
        {
            if (ids == null || !ids.Any())
                return;

            var catalogCategoriesIds = Categories.Where(x => ids.Contains(x.CatalogId)).Select(x => x.Id).ToArray();
            var catalogItemsIds = Items.Where(x => x.CategoryId == null && ids.Contains(x.CatalogId)).Select(x => x.Id).ToArray();

            RemoveItems(catalogItemsIds);
            RemoveCategories(catalogCategoriesIds);

            const string queryPattern =
            @"DELETE CL FROM CatalogLanguage CL INNER JOIN Catalog C ON C.Id = CL.CatalogId WHERE C.Id IN ({0})
            DELETE CR FROM CategoryRelation CR INNER JOIN Catalog C ON C.Id = CR.TargetCatalogId WHERE C.Id IN ({0}) 
            DELETE P FROM Property P INNER JOIN Catalog C ON C.Id = P.CatalogId  WHERE C.Id IN ({0})
            DELETE FROM Catalog WHERE Id IN ({0})";
            var query = String.Format(queryPattern, String.Join(", ", ids.Select(x => String.Format("'{0}'", x))));
            ObjectContext.ExecuteStoreCommand(query);


        }
        #endregion


        private string[] GetAllCategoriesChildrenIdsRecursive(string[] categoryIds)
        {
            var retVal = new List<string>();
            var childs = Categories.Where(x => categoryIds.Contains(x.ParentCategoryId)).Select(x => x.Id).Distinct().ToList();
            if (childs.Any())
            {
                retVal.AddRange(GetAllCategoriesChildrenIdsRecursive(retVal.ToArray()));
            }
            retVal.AddRange(childs);
            return retVal.ToArray();
        }


    }
}
