using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Providers
{
    public class ResourceWorkflowActivityProvider : WFActivityProvider
    {
        protected Dictionary<string, string> WorkflowTypes = new Dictionary<string, string>();

        public override void Initialize(string name, NameValueCollection config)
        {
            foreach (var wfCfg in WorkflowConfiguration.Instance.AvailableWorkflows.Cast<WorkflowConfigurationElement>())
            {
                WorkflowTypes.Add(wfCfg.Name, wfCfg.Type);
            }
            base.Initialize(name, config);
        }

        public override Activity GetWorkflowActivity(string activityName)
        {
            if (WorkflowTypes.ContainsKey(activityName))
            {
                var type = Type.GetType(WorkflowTypes[activityName]);
                if (type == null)
                    throw new TypeLoadException(String.Format("Specified workflow activity class \"{0}\" can not be created.", WorkflowTypes[activityName]));
                return (Activity)Activator.CreateInstance(type);
            }

            return null;
        }

    }
}
