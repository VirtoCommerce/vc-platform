using System.ServiceModel;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.Foundation.Customers.Services
{
	[ServiceContract]
	public interface ICaseAlertsService
	{ 
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
		CaseAlert[] GetCaseAlerts(ICaseAlertEvaluationContext customerCase);
	}
}
