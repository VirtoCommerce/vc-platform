using System.Collections.Generic;
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
				_projectSystem.AddReference(reference.Name, reference.GetStream());
		}

		public override void RemovePackage(IPackage package)
		{
			base.RemovePackage(package);

			foreach (var reference in GetCompatibleAssemblyReferences(package))
				_projectSystem.RemoveReference(reference.Name);
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


		private static IEnumerable<IPackageAssemblyReference> GetCompatibleAssemblyReferences(IPackage package)
		{
			//return Enumerable.Empty<IPackageAssemblyReference>();
			return package.AssemblyReferences;
		}
	}
}
