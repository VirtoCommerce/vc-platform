using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using NuGet;

namespace VirtoCommerce.Platform.Data.Packaging.Repositories.NuGet
{
    class ZipPackage
    {
        private static readonly string[] _excludePaths = { "_rels", "package" };

        internal static bool IsPackageFile(PackagePart part)
        {
            string path = UriUtility.GetPath(part.Uri);
            string directory = Path.GetDirectoryName(path);

            // We exclude any opc files and the manifest file (.nuspec)
            return !string.IsNullOrEmpty(directory) && !_excludePaths.Any(p => directory.StartsWith(p, StringComparison.OrdinalIgnoreCase))
                && !PackageHelper.IsManifest(path);
        }
    }
}
