using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Marketing;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.Foundation.Data.Marketing
{
	public class EFDynamicContentRepository : EFRepositoryBase, IDynamicContentRepository
	{
        public EFDynamicContentRepository() : this("VirtoCommerce")
        {
        }

        public EFDynamicContentRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFDynamicContentRepository>(null);
        }

        public EFDynamicContentRepository(string nameOrConnectionString, DynamicContentEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString, factory: entityFactory, interceptors: interceptors)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new ValidateDatabaseInitializer<EFDynamicContentRepository>());
        }


        [InjectionConstructor]
        public EFDynamicContentRepository(DynamicContentEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : this(MarketingConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors: interceptors)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
				throw new ArgumentNullException("modelBuilder");

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			MapEntity<DynamicContentItem>(modelBuilder, toTable: "DynamicContentItem");
			MapEntity<DynamicContentPlace>(modelBuilder, toTable: "DynamicContentPlace");
			MapEntity<DynamicContentPublishingGroup>(modelBuilder, toTable: "DynamicContentPublishingGroup");
			MapEntity<PublishingGroupContentItem>(modelBuilder, toTable: "PublishingGroupContentItem");
			MapEntity<PublishingGroupContentPlace>(modelBuilder, toTable: "PublishingGroupContentPlace");

			modelBuilder.Entity<PublishingGroupContentItem>().HasRequired(p => p.ContentItem).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<PublishingGroupContentPlace>().HasRequired(p => p.ContentPlace).WithMany().WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}

		#region IDynamicContentRepository Members
		
		public IQueryable<DynamicContentItem> Items
		{
			get { return GetAsQueryable<DynamicContentItem>(); }
		}

		public IQueryable<DynamicContentPlace> Places
		{
			get { return GetAsQueryable<DynamicContentPlace>(); }
		}

		public IQueryable<DynamicContentPublishingGroup> PublishingGroups
		{
			get { return GetAsQueryable<DynamicContentPublishingGroup>(); }
		}

		public IQueryable<PublishingGroupContentItem> PublishingGroupContentItems
		{
			get { return GetAsQueryable<PublishingGroupContentItem>(); }
		}

		public IQueryable<PublishingGroupContentPlace> PublishingGroupContentPlaces
		{
			get { return GetAsQueryable<PublishingGroupContentPlace>(); }
		}

		#endregion
	}
}
