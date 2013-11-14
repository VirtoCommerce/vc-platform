using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.ManagementClient.Security.Tokens
{
	public class SecurityTokenEndpointBehavior : BehaviorExtensionElement, IEndpointBehavior, IClientMessageInspector
	{
		#region BehaviorExtensionElement members

		public override Type BehaviorType
		{
			get
			{
				return typeof(SecurityTokenEndpointBehavior);
			}
		}

		protected override object CreateBehavior()
		{
			return this;
		}

		#endregion

		#region IEndpointBehavior members

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.MessageInspectors.Add(this);
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}

		#endregion

		#region IClientMessageInspector members

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			var tokenInjector = ServiceLocator.Current.GetInstance<ISecurityTokenInjector>();

			if (tokenInjector != null)
			{
				object httpRequestMessageObject;

				if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
				{
					var httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;

					if (httpRequestMessage != null)
						tokenInjector.InjectToken(httpRequestMessage.Headers);
				}
				else
				{
					var httpRequestMessage = new HttpRequestMessageProperty();
					tokenInjector.InjectToken(httpRequestMessage.Headers);
					request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
				}
			}

			return null;
		}

		#endregion
	}
}
