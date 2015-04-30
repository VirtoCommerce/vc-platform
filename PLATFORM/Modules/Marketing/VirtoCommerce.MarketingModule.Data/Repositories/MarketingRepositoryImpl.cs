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
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer<MarketingRepositoryImpl>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<Promotion>(modelBuilder, toTable: "Promotion");
			MapEntity<Coupon>(modelBuilder, toTable: "Coupon");
			MapEntity<PromotionUsage>(modelBuilder, toTable: "PromotionUsage");

			MapEntity<DynamicContentItem>(modelBuilder, toTable: "DynamicContentItem");
			MapEntity<DynamicContentPlace>(modelBuilder, toTable: "DynamicContentPlace");
			MapEntity<DynamicContentPublishingGroup>(modelBuilder, toTable: "DynamicContentPublishingGroup");
			MapEntity<PublishingGroupContentItem>(modelBuilder, toTable: "PublishingGroupContentItem");
			MapEntity<PublishingGroupContentPlace>(modelBuilder, toTable: "PublishingGroupContentPlace");
			MapEntity<DynamicContentFolder>(modelBuilder, toTable: "DynamicContentFolder");

			modelBuilder.Entity<PublishingGroupContentItem>().HasRequired(p => p.ContentItem).WithMany().WillCascadeOnDelete(false);
			modelBuilder.Entity<PublishingGroupContentPlace>().HasRequired(p => p.ContentPlace).WithMany().WillCascadeOnDelete(false);

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
