using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.Foundation.Marketing.Factories
{
	public class DynamicContentEntityFactory : FactoryBase, IDynamicContentEntityFactory
	{
		public DynamicContentEntityFactory()
		{
			RegisterStorageType(typeof(DynamicContentItem), "DynamicContentItem");
			RegisterStorageType(typeof(DynamicContentItemProperty), "DynamicContentItemProperty");
			RegisterStorageType(typeof(DynamicContentPlace), "DynamicContentPlace");
			RegisterStorageType(typeof(DynamicContentPublishingGroup), "DynamicContentPublishingGroup");
			RegisterStorageType(typeof(PublishingGroupContentItem), "PublishingGroupContentItem");
			RegisterStorageType(typeof(PublishingGroupContentPlace), "PublishingGroupContentPlace");
		}
	}
}
