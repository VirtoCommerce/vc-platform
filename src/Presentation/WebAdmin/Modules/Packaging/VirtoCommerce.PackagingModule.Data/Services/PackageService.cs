using System.IO;
using System.Linq;
using NuGet;
using VirtoCommerce.PackagingModule.Data.Repositories;
using VirtoCommerce.PackagingModule.Model;
using VirtoCommerce.PackagingModule.Services;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.PackagingModule.Data.Services
{
	public class PackageService : IPackageService
	{
		private readonly string _sourcePath;
		private readonly string _projectPath;
		private readonly string _packagesPath;
		private readonly string _binPath;

		private readonly ProjectManager _projectManager;

		public PackageService(string sourcePath, string projectPath, string packagesPath, string binPath)
		{
			_sourcePath = Path.GetFullPath(sourcePath ?? "source");
			_projectPath = Path.GetFullPath(projectPath ?? "website");
			_packagesPath = Path.GetFullPath(packagesPath ?? Path.Combine(_projectPath, "packages"));
			_binPath = Path.GetFullPath(binPath ?? Path.Combine(_projectPath, "bin"));

			_projectManager = CreateProjectManager();
		}

		public ILogger Logger
		{
			get { return _projectManager.Logger; }
			set { _projectManager.Logger = value; }
		}

		#region IPackageService Members

		public ModuleDescriptor[] GetModules()
		{
			return ManifestModuleCatalog.GetModuleManifests(_projectPath).Select(ConvertToModuleDescriptor).ToArray();
		}

		public PackageDescriptor[] GetPackages()
		{
			return _projectManager.LocalRepository.GetPackages().Select(ConvertToPackageDescriptor).ToArray();
		}

		public void Install(string packageId, string version)
		{
			var packageVersion = string.IsNullOrEmpty(version) ? null : new SemanticVersion(version);
			_projectManager.AddPackageReference(packageId, packageVersion, false, true);
		}

		public void Update(string packageId, string version)
		{
			var packageVersion = string.IsNullOrEmpty(version) ? null : new SemanticVersion(version);
			_projectManager.UpdatePackageReference(packageId, packageVersion, true, true);
		}

		public void Uninstall(string packageId)
		{
			_projectManager.RemovePackageReference(packageId, false, false);
		}

		#endregion


		private ProjectManager CreateProjectManager()
		{
			var projectSystem = new WebsiteProjectSystem(_projectPath, _binPath);

			var projectManager = new ProjectManager(
				new WebsiteLocalPackageRepository(_sourcePath),
				new DefaultPackagePathResolver(_projectPath),
				projectSystem,
				new WebsitePackageRepository(_packagesPath, projectSystem)
				);

			// TODO: configure logger

			return projectManager;
		}

		private static PackageDescriptor ConvertToPackageDescriptor(IPackage package)
		{
			return new PackageDescriptor
			{
				Id = package.Id,
				Version = package.Version.ToString(),
			};
		}

		private static ModuleDescriptor ConvertToModuleDescriptor(ModuleManifest manifest)
		{
			return new ModuleDescriptor
			{
				Id = manifest.Id,
				Version = manifest.Version,
				Title = manifest.Title,
				Description = manifest.Description,
				Authors = manifest.Authors,
				Owners = manifest.Owners,
				LicenseUrl = manifest.LicenseUrl,
				ProjectUrl = manifest.ProjectUrl,
				IconUrl = manifest.IconUrl,
				RequireLicenseAcceptance = manifest.RequireLicenseAcceptance,
				ReleaseNotes = manifest.ReleaseNotes,
				Copyright = manifest.Copyright,
				Tags = manifest.Tags,
				Dependencies = manifest.Dependencies,
			};
		}
	}
}
