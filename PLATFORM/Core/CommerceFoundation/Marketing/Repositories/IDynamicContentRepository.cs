using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.Foundation.Marketing.Repositories
{
	public interface IDynamicContentRepository : IRepository
	{
		IQueryable<DynamicContentItem> Items { get; }
		IQueryable<DynamicContentPlace> Places { get; }
		IQueryable<DynamicContentPublishingGroup> PublishingGroups { get; }
		IQueryable<PublishingGroupContentItem> PublishingGroupContentItems { get; }
		IQueryable<PublishingGroupContentPlace> PublishingGroupContentPlaces { get; }
	}
}
