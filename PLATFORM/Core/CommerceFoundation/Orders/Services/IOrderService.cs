using System.ServiceModel;
using System.ServiceModel.Web;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Search;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Foundation.Orders.Services
{
    [ServiceContract(Name = "OrderService", Namespace = "http://schemas.virtocommerce.com/1.0/order/")]
    [UnityDataContractResolverBehaviorAttribute(typeof(IOrderEntityFactory))]
    public interface IOrderService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        OrderSearchResults SearchOrders(string scope, ISearchCriteria criteria);
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        OrderWorkflowResult ExecuteWorkflow(string workflowName, OrderGroup orderGroup);
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
		CreatePaymentResult CreatePayment(Payment payment);
    }
}
