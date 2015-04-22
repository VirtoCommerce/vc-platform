using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.PagesModule.Web
{
	#region

	using Microsoft.Practices.Unity;
	using System;
	using System.IO;
	using System.Web.Hosting;
	using VirtoCommerce.Content.Pages.Data;
	using VirtoCommerce.Content.Pages.Data.Repositories;
	using VirtoCommerce.Content.Pages.Data.Services;
	using VirtoCommerce.Foundation.Data.Infrastructure;
	using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
	using VirtoCommerce.PagesModule.Web.Controllers.Api;

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
			var settingsManager = this._container.Resolve<ISettingsManager>();

			var githubLogin = settingsManager.GetValue("VirtoCommerce.PagesModule.GitHub.Login", string.Empty);
			var githubPassword = settingsManager.GetValue("VirtoCommerce.PagesModule.GitHub.Password", string.Empty);
			var githubProductHeaderValue =
				settingsManager.GetValue("VirtoCommerce.PagesModule.GitHub.ProductHeaderValue", string.Empty);
			var githubOwnerName = settingsManager.GetValue("VirtoCommerce.PagesModule.GitHub.OwnerName", string.Empty);
			var githubRepositoryName = settingsManager.GetValue(
				"VirtoCommerce.PagesModule.GitHub.RepositoryName",
				string.Empty);

			var githubMainPath = "/Pages/";
			var fileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Pages/");


			Func<string, IPagesService> factory = (x) =>
			{
				switch (x)
				{
					case "GitHub":
						return new PagesServiceImpl(new GitHubPagesRepositoryImpl(
							githubLogin,
							githubPassword,
							githubProductHeaderValue,
							githubOwnerName,
							githubRepositoryName,
							githubMainPath));

					case "Database":
						return new PagesServiceImpl(new DatabasePagesRepositoryImpl("VirtoCommerce",
							new AuditableInterceptor(),
							new EntityPrimaryKeyGeneratorInterceptor()));

					case "File System":
						return new PagesServiceImpl(new FileSystemPagesRepositoryImpl(fileSystemMainPath));

					default:
						return new PagesServiceImpl(new FileSystemPagesRepositoryImpl(fileSystemMainPath));
				}
			};

			if (!Directory.Exists(fileSystemMainPath))
			{
				Directory.CreateDirectory(fileSystemMainPath);
			}

			this._container.RegisterType<PagesController>(new InjectionConstructor(factory, settingsManager));

		}

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
			using (var context = new DatabasePagesRepositoryImpl())
			{
				SqlPagesDatabaseInitializer initializer = new SqlPagesDatabaseInitializer();
				initializer.InitializeDatabase(context);
			}
		}

		#endregion
	}
}