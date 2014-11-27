using Microsoft.Practices.Unity;
using Owin;
using System;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.SecurityModule.Web;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.CoreModule.Web.Notification;

namespace VirtoCommerce.CoreModule.Web
{
	[Module(ModuleName = "CoreModule", OnDemand = true)]
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IAppBuilder _appBuilder;

		public Module(IUnityContainer container, IAppBuilder appBuilder)
		{
			_container = container;
			_appBuilder = appBuilder;
		}

		#region IModule Members

		public void Initialize()
		{
			OwinConfig.Configure(_appBuilder);

			#region Security

			_container.RegisterType<Func<IFoundationSecurityRepository>>(
				new InjectionFactory(x => new Func<IFoundationSecurityRepository>(() =>
					new FoundationSecurityRepositoryImpl("VirtoCommerce"))));

			#endregion

			#region Customer

			_container.RegisterType<Func<IFoundationCustomerRepository>>(
				new InjectionFactory(x => new Func<IFoundationCustomerRepository>(() =>
					new FoundationCustomerRepositoryImpl("VirtoCommerce"))));

			#endregion

			#region Notification
			_container.RegisterInstance<INotifier>(new InMemoryNotifierImpl());
			#endregion
		}

		#endregion
	}
}
