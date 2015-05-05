using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public class MarketingRepositoryImpl : EFRepositoryBase, IMarketingRepository
	{
		public MarketingRepositoryImpl()
		{
		}

		public MarketingRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null, null)
		{
		}
		public MarketingRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
			Configuration.LazyLoadingEnabled = false;
			Database.SetInitializer<MarketingRepositoryImpl>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Promotion>().ToTable("Promotion");
			modelBuilder.Entity<Promotion>().HasKey(x => x.Id)
						.Property(x => x.Id);

			modelBuilder.Entity<PromotionUsage>().ToTable("PromotionUsage");
			modelBuilder.Entity<PromotionUsage>().HasKey(x => x.Id)
						.Property(x => x.Id);
			modelBuilder.Entity<PromotionUsage>().HasRequired(x => x.Promotion)
								   .WithMany(x => x.PromotionUsages)
								   .HasForeignKey(x => x.PromotionId);

			modelBuilder.Entity<DynamicContentItem>().ToTable("DynamicContentItem");
			modelBuilder.Entity<DynamicContentItem>().HasKey(x => x.Id)
						.Property(x => x.Id);
			modelBuilder.Entity<DynamicContentItem>().HasOptional(x => x.Folder)
								   .WithMany(x => x.ContentItems)
								   .HasForeignKey(x => x.FolderId);

			modelBuilder.Entity<DynamicContentItemProperty>().ToTable("DynamicContentItemProperty");
			modelBuilder.Entity<DynamicContentItemProperty>().HasKey(x => x.Id)
					.Property(x => x.Id);
			modelBuilder.Entity<DynamicContentItemProperty>().HasRequired(x => x.DynamicContentItem)
									   .WithMany(x => x.PropertyValues)
									   .HasForeignKey(x => x.DynamicContentItemId);

			modelBuilder.Entity<DynamicContentPlace>().ToTable("DynamicContentPlace");
			modelBuilder.Entity<DynamicContentPlace>().HasKey(x => x.Id)
					.Property(x => x.Id);
			modelBuilder.Entity<DynamicContentPlace>().HasOptional(x => x.Folder)
							   .WithMany(x => x.ContentPlaces)
							   .HasForeignKey(x => x.FolderId);

			modelBuilder.Entity<DynamicContentPublishingGroup>().ToTable("DynamicContentPublishingGroup");
			modelBuilder.Entity<DynamicContentPublishingGroup>().HasKey(x => x.Id)
					.Property(x => x.Id);

			modelBuilder.Entity<PublishingGroupContentItem>().ToTable("PublishingGroupContentItem");
			modelBuilder.Entity<PublishingGroupContentItem>().HasKey(x => x.Id)
					.Property(x => x.Id);
			modelBuilder.Entity<PublishingGroupContentItem>().HasRequired(p => p.ContentItem)
					.WithMany().HasForeignKey(x=>x.DynamicContentItemId)
					.WillCascadeOnDelete(false);
	
			modelBuilder.Entity<PublishingGroupContentPlace>().ToTable("PublishingGroupContentPlace");
			modelBuilder.Entity<PublishingGroupContentPlace>().HasKey(x => x.Id)
					.Property(x => x.Id);
			modelBuilder.Entity<PublishingGroupContentPlace>().HasRequired(p => p.ContentPlace).WithMany()
				.HasForeignKey(x=>x.DynamicContentPlaceId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<DynamicContentFolder>().ToTable("DynamicContentFolder");
			modelBuilder.Entity<DynamicContentFolder>().HasKey(x => x.Id)
				.Property(x => x.Id);

	
			base.OnModelCreating(modelBuilder);
		}

		#region IMarketingRepository Members

		public IQueryable<Promotion> Promotions
		{
			get { return GetAsQueryable<Promotion>(); }
		}
		public IQueryable<Coupon> Coupons
		{
			get { return GetAsQueryable<Coupon>(); }
		}
		public IQueryable<PromotionUsage> PromotionUsages
		{
			get { return GetAsQueryable<PromotionUsage>(); }
		}

		public IQueryable<DynamicContentFolder> Folders
		{
			get { return GetAsQueryable<DynamicContentFolder>(); }

		}
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

		public Promotion GetPromotionById(string id)
		{
			var retVal = Promotions.Include(x => x.Coupons).FirstOrDefault(x => x.Id == id);
			return retVal;
		}

		public Promotion[] GetActivePromotions()
		{
			var now = DateTime.UtcNow;
			var retVal = Promotions.Where(x => x.IsActive && (x.StartDate == null || now >= x.StartDate) && (x.EndDate == null || x.EndDate >= now))
											   .OrderByDescending(x => x.Priority).ToArray();
			return retVal;
		}


		public DynamicContentFolder GetContentFolderById(string id)
		{
			var retVal = Folders.FirstOrDefault(x => x.Id == id);
			if (retVal != null)
			{
				if (retVal.ParentFolderId != null)
				{
					retVal.ParentFolder = GetContentFolderById(retVal.ParentFolderId);
				}
			}
			return retVal;
		}

		public DynamicContentItem GetContentItemById(string id)
		{
			var retVal = Items.Include(x => x.PropertyValues).FirstOrDefault(x => x.Id == id);
			if (retVal != null)
			{
				retVal.Folder = GetContentFolderById(retVal.FolderId);
			}
			return retVal;
		}

		public DynamicContentPlace GetContentPlaceById(string id)
		{
			var retVal = Places.FirstOrDefault(x => x.Id == id);
			if (retVal != null)
			{
				retVal.Folder = GetContentFolderById(retVal.FolderId);
			}
			return retVal;
		}

		public DynamicContentPublishingGroup GetContentPublicationById(string id)
		{
			return PublishingGroups.Include(x => x.ContentItems.Select(y => y.ContentItem))
									.Include(x => x.ContentPlaces.Select(y => y.ContentPlace))
									.FirstOrDefault(x => x.Id == id);
		}
		#endregion
	}

}
