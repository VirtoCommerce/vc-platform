using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Web.Administration;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
	public class SiteHelper
	{
		private readonly IRepositoryFactory<IProjectRepository> _repository;
		private readonly string ConfigurationPath;
		private readonly string ProjectFormat = "VirtoCommerce-{0}";

		public SiteHelper(IRepositoryFactory<IProjectRepository> repository)
		{
			ConfigurationPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\iisexpress\config\applicationhost.config";
			_repository = repository;
		}

		public void StartSite(Project project)
		{
			EnsureConfiguration();

			// get existing or create new site
			var site = GetOrCreateSite(project);

			// start site on iisexpress
			IisExpress.Start(site.Name);

			var binding = site.Bindings[0];
			var url = String.Format("http://{0}:{1}", binding.Host, binding.EndPoint.Port);

			// open the browser
			Process.Start(url);

			// save the project IP and Port
			//if (createdNew)
			if (project.BrowseUrl != url)
			{
				var repo = _repository.GetRepositoryInstance();
				project.BrowseUrl = url;
				repo.Update(project);
				repo.UnitOfWork.Commit();
			}
		}

		private void EnsureConfiguration()
		{
			if (!File.Exists(ConfigurationPath))
			{
				IisExpress.Start(String.Empty);
			}
		}

		private Site GetOrCreateSite(Project project)
		{
			var name = GetIISName(project);

			using (var mgr = new ServerManager())
			{
				// check if site already exists
				var site = mgr.Sites.SingleOrDefault(x => x.Name == name);

				if (site == null)
				{
					// create new site
					int port;
					bool exists;

					do
					{
						// generate port number
						port = new Random().Next(2000, 3000);
						exists = mgr.Sites.Any(x => x.Bindings.Any(y => y.EndPoint.Port == port));
					} while (exists);

					var binding = String.Format("*:{0}:localhost", port);
					site = mgr.Sites.Add(name, "http", binding, project.Location.LocalPath);
					mgr.CommitChanges();
				}
				else
				{
					var pathOld = site.Applications["/"].VirtualDirectories["/"].PhysicalPath.ToLower();
					if (pathOld != project.Location.LocalPath.ToLower())
					{
						// update PhysicalPath
						site.Applications["/"].VirtualDirectories["/"].PhysicalPath = project.Location.LocalPath;
						mgr.CommitChanges();
					}
				}

				return site;
			}
		}

		private string GetIISName(Project project)
		{
			return String.Format(ProjectFormat, project.Name);
		}
	}
}
