using System.Collections.Generic;
using System.Linq;
using NuGet;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	public class ManifestPackageRepository : PackageRepositoryBase
	{
		private readonly string _source;
		private readonly IPackageRepository _repository;

		public override string Source { get { return _source; } }
		public override bool SupportsPrereleasePackages { get { return true; } }

		public ManifestPackageRepository(string source, IPackageRepository repository)
		{
			_source = source;
			_repository = repository;
		}


		public override IQueryable<IPackage> GetPackages()
		{
			var repositoryPackages = _repository.GetPackages().ToList();
			var packages = ManifestModuleCatalog.GetModuleManifests(_source).Values.Select(m => ConvertToManifestPackage(m, repositoryPackages)).ToArray();
			return packages.AsQueryable();
		}

		public override void AddPackage(IPackage package)
		{
			_repository.AddPackage(package);
		}

		public override void RemovePackage(IPackage package)
		{
			_repository.RemovePackage(package);
		}


		private IPackage ConvertToManifestPackage(ModuleManifest manifest, IEnumerable<IPackage> repositoryPackages)
		{
			var version = new SemanticVersion(string.IsNullOrEmpty(manifest.Version) ? "1.0.0.0" : manifest.Version);
			var repositoryPackage = repositoryPackages.FirstOrDefault(p => p.Id == manifest.Id && p.Version == version);

			return new ManifestPackage(manifest, repositoryPackage);
		}
	}
}
