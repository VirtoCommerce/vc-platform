using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Catalogs
{
	/// <summary>
	/// Implements the Catalog and PriceList repositories.
	/// </summary>
	public class EFCatalogRepository : EFRepositoryBase, ICatalogRepository, IPricelistRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EFCatalogRepository"/> class.
		/// </summary>
		public EFCatalogRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFCatalogRepository"/> class.
		/// </summary>
		/// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
		public EFCatalogRepository(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer<EFCatalogRepository>(null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFCatalogRepository"/> class.
		/// </summary>
		/// <param name="entityFactory">The entity factory.</param>
		/// <param name="interceptors">The interceptors.</param>
		[InjectionConstructor]
		public EFCatalogRepository(ICatalogEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: this(CatalogConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
		{
		}

		public EFCatalogRepository(string connectionStringName, ICatalogEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(connectionStringName, entityFactory, interceptors: interceptors)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<EFCatalogRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		/// before the model has been locked down and used to initialize the context.  The default
		/// implementation of this method does nothing, but it can be overridden in a derived class
		/// such that the model can be further configured before it is locked down.
		/// </summary>
		/// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
		/// <remarks>
		/// Typically, this method is called only once when the first instance of a derived context
		/// is created.  The model for that context is then cached and is for all further instances of
		/// the context in the app domain.  This caching can be disabled by setting the ModelCaching
		/// property on the given ModelBuidler, but note that this can seriously degrade performance.
		/// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
		/// classes directly.
		/// </remarks>
		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			InheritanceMapping(modelBuilder);

			MapEntity<CategoryItemRelation>(modelBuilder, toTable: "CategoryItemRelation");
			MapEntity<ItemAsset>(modelBuilder, toTable: "ItemAsset");
			MapEntity<Association>(modelBuilder, toTable: "Association");
			MapEntity<AssociationGroup>(modelBuilder, toTable: "AssociationGroup");
			MapEntity<CatalogLanguage>(modelBuilder, toTable: "CatalogLanguage");
			MapEntity<EditorialReview>(modelBuilder, toTable: "EditorialReview");
			MapEntity<ItemRelation>(modelBuilder, toTable: "ItemRelation");
			MapEntity<Price>(modelBuilder, toTable: "Price");
			MapEntity<Pricelist>(modelBuilder, toTable: "Pricelist");
			MapEntity<PricelistAssignment>(modelBuilder, toTable: "PricelistAssignment");
			MapEntity<Property>(modelBuilder, toTable: "Property");
			MapEntity<PropertyAttribute>(modelBuilder, toTable: "PropertyAttribute");
			MapEntity<PropertySet>(modelBuilder, toTable: "PropertySet");
			MapEntity<PropertySetProperty>(modelBuilder, toTable: "PropertySetProperty");
			MapEntity<Packaging>(modelBuilder, toTable: "Packaging");
			MapEntity<TaxCategory>(modelBuilder, toTable: "TaxCategory");

			// Introducing FOREIGN KEY constraint 'FK_dbo.Association_dbo.Item_ItemId' on table 'Association' may cause cycles or multiple cascade paths.
			modelBuilder.Entity<Association>().HasRequired(m => m.CatalogItem).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<CategoryItemRelation>().HasRequired(p => p.Category).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<ItemRelation>().HasRequired(m => m.ChildItem).WithMany().WillCascadeOnDelete(false);
			// cascade delete Item and Category when PropertySet is deleted. This should happen ONLY when catalog is being deleted.
            modelBuilder.Entity<Item>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<Category>().HasOptional(m => m.PropertySet).WithMany().WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Inheritances the mapping.
		/// </summary>
		/// <param name="modelBuilder">The model builder.</param>
		private void InheritanceMapping(DbModelBuilder modelBuilder)
		{
			#region Catalog TPT
			modelBuilder.Entity<CatalogBase>().Map(entity =>
			{
				entity.ToTable("CatalogBase");
			});
			modelBuilder.Entity<VirtualCatalog>().Map(entity =>
			{
				entity.ToTable("VirtualCatalog");
			});
			modelBuilder.Entity<Catalog>().Map(entity =>
			{
				entity.ToTable("Catalog");
			});
			#endregion

			#region Category TPT
			modelBuilder.Entity<CategoryBase>().Map(entity =>
			{
				entity.ToTable("CategoryBase");
			});
			modelBuilder.Entity<Category>().Map(entity =>
			{
				entity.ToTable("Category");
			});
			modelBuilder.Entity<LinkedCategory>().Map(entity =>
			{
				entity.ToTable("LinkedCategory");
			});
			#endregion

			#region Item TPH
			MapEntity<Item>(modelBuilder, toTable: "Item");
			MapEntity<Bundle>(modelBuilder, toTable: "Item", discriminatorValue: "Bundle");
			MapEntity<DynamicKit>(modelBuilder, toTable: "Item", discriminatorValue: "DynamicKit");
			MapEntity<Package>(modelBuilder, toTable: "Item", discriminatorValue: "Package");
			MapEntity<Product>(modelBuilder, toTable: "Item", discriminatorValue: "Product");
			MapEntity<Sku>(modelBuilder, toTable: "Item", discriminatorValue: "Sku");
			#endregion

			#region PropertyValueBase TPC
			modelBuilder.Entity<CatalogPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CatalogPropertyValue");
			});
			modelBuilder.Entity<CategoryPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("CategoryPropertyValue");
			});
			modelBuilder.Entity<PropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("PropertyValue");
			});
			modelBuilder.Entity<ItemPropertyValue>().Map(entity =>
			{
				entity.MapInheritedProperties();
				entity.ToTable("ItemPropertyValue");
			});
			#endregion

			#region Navigation properties required

			modelBuilder.Entity<Item>()
				.HasMany(c => c.AssociationGroups)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<Item>()
				.HasMany(c => c.CategoryItemRelations)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<Item>()
				.HasMany(c => c.ItemAssets)
				.WithRequired(p => p.CatalogItem);

			modelBuilder.Entity<AssociationGroup>()
						.HasMany(c => c.Associations)
						.WithRequired(a => a.AssociationGroup);

			modelBuilder.Entity<Property>()
						.HasMany(x => x.PropertyValues)
						.WithRequired(x => x.Property);

			#endregion
		}

		#region ICatalogRepository Members

		/// <summary>
		/// Gets the categories.
		/// </summary>
		/// <value>
		/// The categories.
		/// </value>
		public IQueryable<CategoryBase> Categories
		{
			get { return GetAsQueryable<CategoryBase>(); }
		}

		/// <summary>
		/// Gets the catalogs.
		/// </summary>
		/// <value>
		/// The catalogs.
		/// </value>
		public IQueryable<CatalogBase> Catalogs
		{
			get { return GetAsQueryable<CatalogBase>(); }
		}

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>
		/// The items.
		/// </value>
		public IQueryable<Item> Items
		{
			get { return GetAsQueryable<Item>(); }
		}

		/// <summary>
		/// Gets the properties.
		/// </summary>
		/// <value>
		/// The properties.
		/// </value>
		public IQueryable<Property> Properties
		{
			get { return GetAsQueryable<Property>(); }
		}

		/// <summary>
		/// Gets the property sets.
		/// </summary>
		/// <value>
		/// The property sets.
		/// </value>
		public IQueryable<PropertySet> PropertySets
		{
			get { return GetAsQueryable<PropertySet>(); }
		}

		/// <summary>
		/// Gets the item relations.
		/// </summary>
		/// <value>
		/// The item relations.
		/// </value>
		public IQueryable<ItemRelation> ItemRelations
		{
			get { return GetAsQueryable<ItemRelation>(); }
		}

		/// <summary>
		/// Gets the category item relations.
		/// </summary>
		/// <value>
		/// The category item relations.
		/// </value>
		public IQueryable<CategoryItemRelation> CategoryItemRelations
		{
			get { return GetAsQueryable<CategoryItemRelation>(); }
		}

		/// <summary>
		/// Gets the packagings.
		/// </summary>
		/// <value>
		/// The packagings.
		/// </value>
		public IQueryable<Packaging> Packagings
		{
			get { return GetAsQueryable<Packaging>(); }
		}

		/// <summary>
		/// Gets the tax categories.
		/// </summary>
		/// <value>
		/// The tax categories.
		/// </value>
		public IQueryable<TaxCategory> TaxCategories
		{
			get { return GetAsQueryable<TaxCategory>(); }
		}

		/// <summary>
		/// Gets the associations.
		/// </summary>
		/// <value>
		/// The associations.
		/// </value>
		public IQueryable<Association> Associations
		{
			get { return GetAsQueryable<Association>(); }
		}
		#endregion

		#region IPriceListRepository Members

		/// <summary>
		/// Gets the pricelists.
		/// </summary>
		/// <value>
		/// The pricelists.
		/// </value>
		public IQueryable<Pricelist> Pricelists
		{
			get { return GetAsQueryable<Pricelist>(); }
		}

		/// <summary>
		/// Gets the prices.
		/// </summary>
		/// <value>
		/// The prices.
		/// </value>
		public IQueryable<Price> Prices
		{
			get { return GetAsQueryable<Price>(); }
		}

		/// <summary>
		/// Gets the pricelist assignments.
		/// </summary>
		/// <value>
		/// The pricelist assignments.
		/// </value>
		public IQueryable<PricelistAssignment> PricelistAssignments
		{
			get { return GetAsQueryable<PricelistAssignment>(); }
		}

		#endregion
	}
}
