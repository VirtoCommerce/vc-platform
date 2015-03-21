using System.Globalization;
using System.IO;
using NuGet;
using NuGet.Resources;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	public class WebsiteLocalPackageRepository : LocalPackageRepository
	{
		public WebsiteLocalPackageRepository(string physicalPath)
			: base(physicalPath)
		{
		}

		protected override IPackage OpenPackage(string path)
		{
			WebsiteOptimizedZipPackage package = null;

			if (FileSystem.FileExists(path))
			{
				if (Path.GetExtension(path) == Constants.PackageExtension)
				{
					try
					{
						package = new WebsiteOptimizedZipPackage(FileSystem, path);
					}
					catch (FileFormatException exception)
					{
						throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, NuGetResources.ErrorReadingPackage, new object[] { path }), exception);
					}

					package.Published = FileSystem.GetLastModified(path);
				}
			}

			return package;
		}
	}
}
