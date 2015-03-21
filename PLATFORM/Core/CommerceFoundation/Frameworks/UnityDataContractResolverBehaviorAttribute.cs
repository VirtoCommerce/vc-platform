using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace VirtoCommerce.Foundation.Frameworks
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class UnityDataContractResolverBehaviorAttribute : Attribute, IContractBehavior
	{
		private ContractResolver _resolver;
		private DataContractSurrogate _surrogate;

		public UnityDataContractResolverBehaviorAttribute(Type entityFactoryType)
		{
			if (entityFactoryType == null)
			{
				throw new ArgumentNullException("entityFactoryType");
			}
			if (!entityFactoryType.GetInterfaces().Any(x => x == typeof(IFactory)))
			{
				throw new ArgumentException("entityFactoryType must implement IFactory");
			}

            var container = ServiceLocator.Current;
			//var container = new UnityContainer();
			//container.LoadConfiguration();

			var entityFactory = container.GetInstance(entityFactoryType) as IFactory;
			var knownTypes = entityFactory as IKnownSerializationTypes;

			_resolver = new ContractResolver(entityFactory, knownTypes);
			_surrogate = new DataContractSurrogate(entityFactory, knownTypes);
		}

		public string TypeNamespace { get; set; }

		#region IContractBehavior Members

		public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			CreateMyDataContractSerializerOperationBehaviors(contractDescription);
		}

		public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
		{
			CreateMyDataContractSerializerOperationBehaviors(contractDescription);
		}

		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}

		#endregion



		internal void CreateMyDataContractSerializerOperationBehaviors(ContractDescription contractDescription)
		{
			foreach (var operation in contractDescription.Operations)
			{
				CreateMyDataContractSerializerOperationBehavior(operation);
			}
		}

		internal void CreateMyDataContractSerializerOperationBehavior(OperationDescription operation)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationbehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			dataContractSerializerOperationbehavior.DataContractResolver = this._resolver;
			dataContractSerializerOperationbehavior.DataContractSurrogate = this._surrogate;
		}


	}
}
