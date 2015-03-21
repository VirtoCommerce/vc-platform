using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	public class WebsitePackageRepository : WebsiteLocalPackageRepository
	{
		private readonly IProjectSystem _projectSystem;

		public WebsitePackageRepository(string physicalPath, IProjectSystem projectSystem)
			: base(physicalPath)
		{
			_projectSystem = projectSystem;
		}

		public override void AddPackage(IPackage package)
		{
			base.AddPackage(package);

			var references = GetCompatibleAssemblyReferences(package);

			foreach (var reference in references)
			{
				var path = BuildAssemblyFilePath(package.Id, reference.Name);
				_projectSystem.AddFile(path, reference.GetStream());
			}
		}

		public override void RemovePackage(IPackage package)
		{
			base.RemovePackage(package);

			foreach (var reference in GetCompatibleAssemblyReferences(package))
			{
				var path = BuildAssemblyFilePath(package.Id, reference.Name);
				_projectSystem.DeleteFileAndParentDirectoriesIfEmpty(path);
			}
		}

		public override bool Exists(string packageId, SemanticVersion version)
		{
			var exists = base.Exists(packageId, version);

			if (exists)
			{
				var package = FindPackage(packageId, version);
				var references = GetCompatibleAssemblyReferences(package);
				exists = references.All(reference => _projectSystem.ReferenceExists(reference.Name));
			}

			return exists;
		}


		private IEnumerable<IPackageAssemblyReference> GetCompatibleAssemblyReferences(IPackage package)
		{
			IEnumerable<IPackageAssemblyReference> result;

			if (!VersionUtility.TryGetCompatibleItems(_projectSystem.TargetFramework, package.AssemblyReferences, out result))
				result = Enumerable.Empty<IPackageAssemblyReference>();

			return result;
		}

		private static string BuildAssemblyFilePath(string packageId, string fileName)
		{
			return Path.Combine(packageId, "bin", fileName);
		}
	}
}
