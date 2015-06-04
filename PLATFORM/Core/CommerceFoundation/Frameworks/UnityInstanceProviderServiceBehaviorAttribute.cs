using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace VirtoCommerce.Foundation.Frameworks
{
	public class UnityInstanceProviderServiceBehaviorAttribute : Attribute, IServiceBehavior
	{
		#region IServiceBehavior Members

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (var item in serviceHostBase.ChannelDispatchers)
			{
				var dispatcher = item as ChannelDispatcher;
				if (dispatcher != null) // add new instance provider for each end point dispatcher
				{
					dispatcher.Endpoints.ToList().ForEach(endpoint =>
					{
						endpoint.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(serviceDescription.ServiceType);
					});
				}
			}

		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		#endregion
	}
}
