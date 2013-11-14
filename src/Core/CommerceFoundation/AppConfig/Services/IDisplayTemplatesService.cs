using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Foundation.AppConfig.Services
{
	[ServiceContract]
	public interface IDisplayTemplatesService
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetTemplate(TargetTypes targetType, TagSet tags);
	}
}
