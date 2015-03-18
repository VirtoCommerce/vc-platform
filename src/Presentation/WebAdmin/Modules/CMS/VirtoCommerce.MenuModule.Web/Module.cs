namespace VirtoCommerce.MenuModule.Web
{
	#region

	using Microsoft.Practices.Unity;
	using System;
	using System.IO;
	using System.Web.Hosting;
	using VirtoCommerce.Content.Menu.Data;
	using VirtoCommerce.Content.Menu.Data.Repositories;
	using VirtoCommerce.Content.Menu.Data.Services;
	using VirtoCommerce.Foundation.Data.Infrastructure;
	using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
	using VirtoCommerce.Framework.Web.Modularity;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.MenuModule.Web.Controllers.Api;

	#endregion

	public class Module : IModule, IDatabaseModule
	{
		#region Fields

		private readonly IUnityContainer _container;

		#endregion

		#region Constructors and Destructors

		public Module(IUnityContainer container)
		{
			this._container = container;
		}

		#endregion

		#region Public Methods and Operators

		public void Initialize()
		{
			var repository = new DatabaseMenuRepositoryImpl("VirtoCommerce", new AuditableInterceptor(),
															   new EntityPrimaryKeyGeneratorInterceptor());

			var service = new MenuServiceImpl(repository);

			this._container.RegisterType<MenuController>(new InjectionConstructor(service));

		}

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			using (var context = new DatabaseMenuRepositoryImpl())
			{
				SqlMenuDatabaseInitializer initializer = new SqlMenuDatabaseInitializer();
				initializer.InitializeDatabase(context);
			}
		}

		#endregion
	}
}