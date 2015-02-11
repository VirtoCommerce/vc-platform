using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerceCMS.Data.Repositories;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerceCMS.ThemeModule.Web.Controllers.Api;
using System.IO;
using System.Web.Hosting;

namespace VirtoCommerceCMS.ThemeModule.Web
{
	public class Module : IModule//, IDatabaseModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			var settingsManager = _container.Resolve<ISettingsManager>();

			var githubLogin = settingsManager.GetValue("VirtoCommerceCMS.ThemeModule.GitHub.Login", string.Empty);
			var githubPassword = settingsManager.GetValue("VirtoCommerceCMS.ThemeModule.GitHub.Password", string.Empty);
			var githubProductHeaderValue = settingsManager.GetValue("VirtoCommerceCMS.ThemeModule.GitHub.ProductHeaderValue", string.Empty);
			var githubOwnerName = settingsManager.GetValue("VirtoCommerceCMS.ThemeModule.GitHub.OwnerName", string.Empty);
			var githubRepositoryName = settingsManager.GetValue("VirtoCommerceCMS.ThemeModule.GitHub.RepositoryName", string.Empty);

			Func<string, IFileRepository> factory = (x) =>
				{
					switch (x)
					{
						case "GitHub":
							return new GitHubFileRepositoryImpl(githubLogin, githubPassword, githubProductHeaderValue, githubOwnerName, githubRepositoryName);

						case "Database":
							return new DatabaseFileRepositoryImpl();

						case "File System":
							return new FileSystemFileRepositoryImpl();

						default:
							return new FileSystemFileRepositoryImpl();
					}
				};

			//if(!Directory.Exists(HostingEnvironment.MapPath("~/Themes/")))
			//{
			//	Directory.CreateDirectory(HostingEnvironment.MapPath("~/Themes/"));
			//}

			_container.RegisterType<ThemeController>(new InjectionConstructor(factory, settingsManager));
		}

		#endregion



		#region IDatabaseModule Members

		public void SetupDatabase(SampleDataLevel sampleDataLevel)
		{

		}

		#endregion
	}
}
