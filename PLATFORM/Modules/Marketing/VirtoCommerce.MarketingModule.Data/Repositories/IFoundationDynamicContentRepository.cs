using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public interface IFoundationDynamicContentRepository : IDynamicContentRepository
	{
		DynamicContentFolder GetContentFolderById(string id);
		DynamicContentItem GetContentItemById(string id);
		DynamicContentPlace GetContentPlaceById(string id);
		DynamicContentPublishingGroup GetContentPublicationById(string id);
	}
}
