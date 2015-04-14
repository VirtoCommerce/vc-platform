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
		public DynamicContentFolder GetContentFolderById(string id)
		{
			var retVal = Folders.FirstOrDefault(x => x.DynamicContentFolderId == id);
			if(retVal != null)
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
			var retVal =  Items.Include(x => x.PropertyValues).FirstOrDefault(x => x.DynamicContentItemId == id);
			if(retVal != null)
			{
				retVal.Folder = GetContentFolderById(retVal.FolderId);
			}
			return retVal;
		}

		public DynamicContentPlace GetContentPlaceById(string id)
		{
			var retVal =  Places.FirstOrDefault(x => x.DynamicContentPlaceId == id);
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
									.FirstOrDefault(x => x.DynamicContentPublishingGroupId == id);
		}

		#endregion
	}

}
