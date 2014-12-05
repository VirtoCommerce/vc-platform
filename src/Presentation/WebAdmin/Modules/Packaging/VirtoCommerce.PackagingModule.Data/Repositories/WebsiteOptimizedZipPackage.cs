using System;
using System.IO;
using NuGet;
using VirtoCommerce.Framework.Web.Modularity;
using OptimizedZipPackage = VirtoCommerce.PackagingModule.Data.Repositories.NuGet.OptimizedZipPackage;
using Package = System.IO.Packaging.Package;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	class WebsiteOptimizedZipPackage : OptimizedZipPackage
	{
		public ModuleManifest Manifest { get; private set; }

		public WebsiteOptimizedZipPackage(IFileSystem fileSystem, string packagePath)
			: base(fileSystem, packagePath)
		{
			EnsureModuleManifest();
		}

		protected override string TransformPath(string path)
		{
			var result = path;

			const string content = @"content\";

			if (path.StartsWith(content, StringComparison.OrdinalIgnoreCase))
				result = Path.Combine(content, "Modules", Id, path.Substring(content.Length));

			return result;
		}


		private void EnsureModuleManifest()
		{
			using (var stream = GetStream())
			{
				var package = Package.Open(stream);
				var uri = new Uri("/content/module.manifest", UriKind.Relative);

				if (package.PartExists(uri))
				{
					var manifestPart = package.GetPart(uri);

					using (var manifestStream = manifestPart.GetStream())
					{
						Manifest = ManifestReader.Read(manifestStream);
					}
				}
			}
		}
	}
}
