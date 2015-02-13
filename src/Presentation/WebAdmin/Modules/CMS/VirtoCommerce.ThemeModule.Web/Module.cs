namespace VirtoCommerce.ThemeModule.Web
{
	#region

	using System;

	using Microsoft.Practices.Unity;

	using VirtoCommerce.Content.Data.Repositories;
	using VirtoCommerce.Framework.Web.Modularity;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.ThemeModule.Web.Controllers.Api;
	using VirtoCommerce.Content.Data.Services;
	using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

	#endregion

	public class Module : IModule//, IDatabaseModule
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

			var githubLogin = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Login", string.Empty);
			var githubPassword = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Password", string.Empty);
			var githubProductHeaderValue =
				settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.ProductHeaderValue", string.Empty);
			var githubOwnerName = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.OwnerName", string.Empty);
			var githubRepositoryName = settingsManager.GetValue(
				"VirtoCommerce.ThemeModule.GitHub.RepositoryName",
				string.Empty);

			var githubMainPath = "/Themes/";
			var fileSystemMainPath = "~/Themes/";
			var databaseMainPath = "Themes/";

			Func<IFileRepository> databaseFileRepository = () =>
			{
				return new DatabaseFileRepositoryImpl("VirtoCommerce", githubMainPath, new AuditableInterceptor(),
															   new EntityPrimaryKeyGeneratorInterceptor());
			};

			Func<string, IThemeService> factory = (x) =>
			{
				switch (x)
				{
					case "GitHub":
						return new ThemeServiceImpl(new GitHubFileRepositoryImpl(
							githubLogin,
							githubPassword,
							githubProductHeaderValue,
							githubOwnerName,
							githubRepositoryName,
							githubMainPath));

					case "Database":
						return new ThemeServiceImpl(new DatabaseFileRepositoryImpl(databaseMainPath));

					case "File System":
						return new ThemeServiceImpl(new FileSystemFileRepositoryImpl(fileSystemMainPath));

					default:
						return new ThemeServiceImpl(new FileSystemFileRepositoryImpl(fileSystemMainPath));
				}
			};

			//if(!Directory.Exists(HostingEnvironment.MapPath("~/Themes/")))
			//{
			//	Directory.CreateDirectory(HostingEnvironment.MapPath("~/Themes/"));
			//}

			this._container.RegisterType<ThemeController>(new InjectionConstructor(factory, settingsManager));
		}

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{
		}

		#endregion
	}
}