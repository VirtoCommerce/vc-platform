using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using VirtoCommerce.Foundation.Frameworks.Workflow.Providers;
using System.Web.Configuration;
using System.Configuration.Provider;
using VirtoCommerce.Foundation.Security;

namespace VirtoCommerce.Foundation.Frameworks.Workflow
{
	public sealed class WorkflowConfiguration : ConfigurationSection
	{
		private static Lazy<WorkflowConfiguration> _instance = new Lazy<WorkflowConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static WorkflowConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static WorkflowConfiguration CreateInstance()
		{
			return (WorkflowConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Workflow");
		}

		[ConfigurationProperty("activityProvider")]
		public ActivityProviderElement ActivityProviderElement
		{
			get
			{
				return base["activityProvider"] as ActivityProviderElement;
			}
		}

        [ConfigurationProperty("availableWorkflows")]
        public WorkflowsCollection AvailableWorkflows
        {
            get
            {
                return (WorkflowsCollection)this["availableWorkflows"] ?? new WorkflowsCollection();
            }
        }

		private ProviderCollection _workflowActivityProviders;
		public ProviderCollection WorkflowActivityProviders
		{
			get
			{
				if (this._workflowActivityProviders == null)
				{
					lock (this)
					{
						if (this._workflowActivityProviders == null)
						{
							_workflowActivityProviders = new ProviderCollection();

							ProvidersHelper.InstantiateProviders(this.ActivityProviderElement.ProvidersSetting,
																 _workflowActivityProviders,
																 typeof(WFActivityProvider));
						}
					}
				}
				return _workflowActivityProviders;
			}
		}

		public IWFWorkflowActivityProvider DefaultActivityProvider
		{
			get
			{
				string defaultProviderName = ActivityProviderElement.DefaultProviderName;
				var retVal = WorkflowActivityProviders[defaultProviderName];
				if (retVal == null)
				{
					throw new ConfigurationErrorsException("Default ActivityProvider not found.");
				}
				return retVal as IWFWorkflowActivityProvider;
			}
		}

	}

    public class ActivityProviderElement : ConfigurationElement
	{

		[ConfigurationProperty("defaultProvider", IsRequired = true)]
		public string DefaultProviderName
		{
			get
			{
				return (string)base["defaultProvider"];
			}
			set
			{
				base["defaultProvider"] = value;
			}
		}

		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection ProvidersSetting
		{
			get
			{
				return (ProviderSettingsCollection)base["providers"];
			}
		}

	}

    public class WorkflowConfigurationElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }


    }

    public class WorkflowsCollection : ConfigurationElementCollection
    {
        public WorkflowConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as WorkflowConfigurationElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkflowConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkflowConfigurationElement)element).Name;
        }
    }

}
