using System;
using System.IO;
using NuGet;
using OptimizedZipPackage = VirtoCommerce.PackagingModule.Data.Repositories.NuGet.OptimizedZipPackage;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	class WebsiteOptimizedZipPackage : OptimizedZipPackage
	{
		public WebsiteOptimizedZipPackage(IFileSystem fileSystem, string packagePath)
			: base(fileSystem, packagePath)
		{
		}

		protected override string TransformPath(string path)
		{
			var result = path;

			const string content = @"content\";

			if (path.StartsWith(content, StringComparison.OrdinalIgnoreCase))
				result = Path.Combine(content, Id, path.Substring(content.Length));

			return result;
		}
	}
}
