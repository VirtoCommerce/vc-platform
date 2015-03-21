using Microsoft.Practices.Unity;
using System;
using System.Activities;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks.Workflow.Providers;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Services
{
    public class WFWorkflowService : IWorkflowService
    {
        private IWFWorkflowActivityProvider _activityProvider;

        [InjectionConstructor]
        public WFWorkflowService(IWFWorkflowActivityProvider activityProvider)
        {
            _activityProvider = activityProvider;
        }

        #region IWorkflowService Members
        public virtual WorkflowResult RunWorkflow(string workflowName, Dictionary<string, object> parameters, object[] extensions = null)
        {
            var retVal = new WorkflowResult();
            parameters["ResultArgument"] = retVal;

            var activity = _activityProvider.GetWorkflowActivity(workflowName);
            if (activity == null)
            {
                throw new ArgumentException("Activity (workflow) not found by name: " + workflowName);
            }

            //var validationResults = ActivityValidationServices.Validate(activity);
            //if (validationResults.Errors.Count() == 0)
            //{
            var invoker = new WorkflowInvoker(activity);
            if (extensions != null)
            {
                foreach (var ext in extensions)
                {
                    invoker.Extensions.Add(ext);
                }
            }

            invoker.Invoke(parameters);

            //}
            //else
            //{
            //    throw new ValidationException();
            //}

            //ActivityInvoker.Invoke(activity, parameters, extensions);
            return retVal;
        }
        #endregion
    }
}
