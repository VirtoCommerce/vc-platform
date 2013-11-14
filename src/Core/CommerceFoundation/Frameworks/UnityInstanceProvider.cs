using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace VirtoCommerce.Foundation.Frameworks
{
	/// <summary>
	/// The unity instance provider. This class provides
	/// an extensibility point for creating instances of wcf
	/// service.
	/// <remarks>
	/// The goal is to inject dependencies from the inception point
	/// </remarks>
	/// </summary>
	public class UnityInstanceProvider : IInstanceProvider
	{
		#region Members

		Type _serviceType;

		#endregion

		#region Constructor

		/// <summary>
		/// Create a new instance of unity instance provider
		/// </summary>
		/// <param name="serviceType">The service where we apply the instance provider</param>
		public UnityInstanceProvider(Type serviceType)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");

			this._serviceType = serviceType;
			//var container = new UnityContainer();
			//_container = container.LoadConfiguration();
		}

		#endregion

		#region IInstance Provider Members

		/// <summary>
		/// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
		/// </summary>
		/// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
		/// <param name="message"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
		/// <returns><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></returns>
		public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
		{
			//This is the only call to UNITY container in the whole solution
            //DependencyResolver.Current.GetService<ICustomerSessionService>()
            //UnityContainer
            var instance = ServiceLocator.Current.GetInstance(_serviceType);
            return instance;
			//return _container.Resolve(_serviceType);
		}
		/// <summary>
		/// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
		/// </summary>
		/// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
		/// <returns><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></returns>
		public object GetInstance(InstanceContext instanceContext)
		{
			return GetInstance(instanceContext, null);
		}

		/// <summary>
		/// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
		/// </summary>
		/// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
		/// <param name="instance"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			if (instance is IDisposable)
				((IDisposable)instance).Dispose();
		}

		#endregion

	}
}
