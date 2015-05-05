using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public interface IMarketingRepository : IRepository
	{
		IQueryable<Promotion> Promotions { get; }
		IQueryable<Coupon> Coupons { get; }
		IQueryable<PromotionUsage> PromotionUsages { get; }
		IQueryable<DynamicContentFolder> Folders { get; }
		IQueryable<DynamicContentItem> Items { get; }
		IQueryable<DynamicContentPlace> Places { get; }
		IQueryable<DynamicContentPublishingGroup> PublishingGroups { get; }
		IQueryable<PublishingGroupContentItem> PublishingGroupContentItems { get; }
		IQueryable<PublishingGroupContentPlace> PublishingGroupContentPlaces { get; }

		Promotion GetPromotionById(string id);
		Promotion[] GetActivePromotions();
		DynamicContentFolder GetContentFolderById(string id);
		DynamicContentItem GetContentItemById(string id);
		DynamicContentPlace GetContentPlaceById(string id);
		DynamicContentPublishingGroup GetContentPublicationById(string id);
	}
}
