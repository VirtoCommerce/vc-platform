using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.Foundation.Marketing.Services
{
	[ServiceContract]
	
	public interface IMarketingService
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
		Promotion[] EvaluatePromotions(IPromotionEvaluationContext context);

		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
		void RegisterToUsePromotion(IPromotionEvaluationContext context, Promotion promotion);

	}
}
