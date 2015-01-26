using System;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.CoreModule.Web.Controllers.Api;
using VirtoCommerce.CoreModule.Web.Notification;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Settings;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web
{
	[Module(ModuleName = "CoreModule", OnDemand = true)]
	public class Module : IModule
	{
		private const string _connectionStringName = "VirtoCommerce";
		private readonly IUnityContainer _container;
		private readonly IAppBuilder _appBuilder;

		public Module(IUnityContainer container, IAppBuilder appBuilder)
		{
			_container = container;
			_appBuilder = appBuilder;
		}

		#region IModule Members

		public void SetupDatabase(bool insertSampleData, bool reducedSampleData)
		{
			using (var db = new EFSecurityRepository(_connectionStringName))
			{
				SqlSecurityDatabaseInitializer initializer;

				if (insertSampleData)
				{
					initializer = new SqlSecuritySampleDatabaseInitializer();
				}
				else
				{
					initializer = new SqlSecurityDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}

			using (var db = new EFCustomerRepository(_connectionStringName))
			{
				SqlCustomerDatabaseInitializer initializer;

				if (insertSampleData)
				{
					initializer = new SqlCustomerSampleDatabaseInitializer();
				}
				else
				{
					initializer = new SqlCustomerDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}

			using (var db = new EFAppConfigRepository(_connectionStringName))
			{
				SqlAppConfigDatabaseInitializer initializer;

				if (insertSampleData)
				{
					if (reducedSampleData)
					{
						initializer = new SqlAppConfigReducedSampleDatabaseInitializer();
					}
					else
					{
						initializer = new SqlAppConfigSampleDatabaseInitializer();
					}
				}
				else
				{
					initializer = new SqlAppConfigDatabaseInitializer();
				}

				initializer.InitializeDatabase(db);
			}
		}

		public void Initialize()
		{
			OwinConfig.Configure(_appBuilder);

			#region Security

			_container.RegisterType<Func<IFoundationSecurityRepository>>(
				new InjectionFactory(x => new Func<IFoundationSecurityRepository>(() =>
					new FoundationSecurityRepositoryImpl(_connectionStringName))));

			#endregion

			#region Customer

			_container.RegisterType<Func<IFoundationCustomerRepository>>(
				new InjectionFactory(x => new Func<IFoundationCustomerRepository>(() =>
					new FoundationCustomerRepositoryImpl(_connectionStringName))));

			#endregion

			#region Notification
			_container.RegisterInstance<INotifier>(new InMemoryNotifierImpl());
			#endregion

			#region Settings
			var modulesPath = HostingEnvironment.MapPath("~/Modules");
			Func<IAppConfigRepository> appConfigRepFactory = () => new EFAppConfigRepository(_connectionStringName);

			var settingsManager = new SettingsManager(modulesPath, appConfigRepFactory);
			_container.RegisterInstance<ISettingsManager>(settingsManager);

			_container.RegisterType<SettingController>(new InjectionConstructor(settingsManager));
			#endregion
		}

		#endregion
	}
}
