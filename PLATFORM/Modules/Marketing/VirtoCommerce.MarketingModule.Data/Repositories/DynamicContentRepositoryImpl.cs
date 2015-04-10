using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public class DynamicContentRepositoryImpl : EFDynamicContentRepository, IFoundationDynamicContentRepository
	{
		public DynamicContentRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null, null)
		{
		}
		public DynamicContentRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
		}


		#region IFoundationDynamicContentRepository Members

		public DynamicContentItem GetContentItemById(string id)
		{
			return Items.Include(x => x.PropertyValues).FirstOrDefault(x => x.DynamicContentItemId == id);
		}

		public DynamicContentPlace GetContentPlaceById(string id)
		{
			return Places.FirstOrDefault(x => x.DynamicContentPlaceId == id);
		}

		public DynamicContentPublishingGroup GetContentPublicationById(string id)
		{
			return PublishingGroups.Include(x => x.ContentItems.Select(y => y.ContentItem))
									.Include(x => x.ContentPlaces.Select(y => y.ContentPlace))
									.FirstOrDefault(x => x.DynamicContentPublishingGroupId == id);
		}

		#endregion
	}

}
