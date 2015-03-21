using System.Collections.Generic;
using System.Activities;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Frameworks.Workflow;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.OrderWorkflow
{
	public abstract class OrderActivityBase : CodeActivity
	{
		public InArgument<OrderGroup> OrderGroupArgument { get; set; }
        public InArgument<WorkflowResult> ResultArgument { get; set; }

		protected OrderGroup CurrentOrderGroup { get; set; }
		protected WorkflowResult CurrentResult { get; set; }
		protected IServiceLocator ServiceLocator { get; set; }
		
		protected override void Execute(CodeActivityContext context)
		{
            ServiceLocator = context.GetExtension<IServiceLocator>();
			CurrentOrderGroup = OrderGroupArgument.Get(context);
			CurrentResult = ResultArgument.Get(context);
		}

        protected void RegisterWarning(string code, string message)
        {
            var parameters = new Dictionary<string, string> {{"Message", message}};

	        RegisterWarning(code, parameters);
        }

        protected void RegisterWarning(string code, IDictionary<string, string> parameters)
        {
            var message = new WorkflowMessage { Code = code, Parameters = new Dictionary<string, string>() };

            foreach (var param in parameters)
            {
                message.Parameters.Add(param.Key, param.Value);
            }

            CurrentResult.Warnings.Add(message);
        }

        protected void RegisterWarning(string code, LineItem lineItem, string message)
        {
            var parameters = new Dictionary<string, string>
	            {
		            {"ItemId", lineItem.CatalogItemId},
		            {"LineItemId", lineItem.LineItemId},
		            {"DisplayName", lineItem.DisplayName},
		            {"Message", message}
	            };

	        RegisterWarning(code, parameters);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.RequireExtension<IServiceLocator>();
            base.CacheMetadata(metadata);
        }
	}
}