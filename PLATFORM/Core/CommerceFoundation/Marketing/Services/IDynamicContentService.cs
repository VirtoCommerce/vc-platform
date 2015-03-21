using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.Foundation.Marketing.Services
{
	[ServiceContract]
	public interface IDynamicContentService
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        DynamicContentItem[] GetItems(string placeId, DateTime now, TagSet tags);
	}
}
