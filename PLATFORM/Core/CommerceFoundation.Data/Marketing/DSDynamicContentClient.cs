using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.Foundation.Data.Marketing
{
	public class DSDynamicContentClient : DSClientBase, IDynamicContentRepository
	{
		[InjectionConstructor]
		public DSDynamicContentClient(IDynamicContentEntityFactory entityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(DynamicContentConfiguration.Instance.Connection.DataServiceUri), entityFactory, tokenInjector)
		{
		}

        public DSDynamicContentClient(Uri serviceUri, IDynamicContentEntityFactory entityFactory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, entityFactory, tokenInjector)
		{
		}

		#region IDynamicContentRepository Members
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

		#endregion
	}
}
