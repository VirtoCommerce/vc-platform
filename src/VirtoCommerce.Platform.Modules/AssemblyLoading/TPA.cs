using System;
using System.Collections.Generic;
using System.IO;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    internal static class Tpa
    {
        private static readonly HashSet<string> _tpaFileNames = GetTpaFileNames();

        /// <summary>
        /// Gets the list of Trusted Platform Assemblies (TPA).
        /// <para>
        /// https://github.com/dotnet/coreclr/issues/919#issuecomment-285928695
        /// https://docs.microsoft.com/en-US/dotnet/core/tutorials/netcore-hosting#step-5---preparing-appdomain-settings
        /// </para>
        /// </summary>
        /// <returns>Returns the list of TPA paths.</returns>
        private static HashSet<string> GetTpaFileNames()
        {
            if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is not string filePaths)
            {
                return [];
            }

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var path in filePaths.Split(Path.PathSeparator))
            {
                var fileName = Path.GetFileName(path);
                result.Add(fileName);

                // Also add resource assemblies to TPA list
                var extension = Path.GetExtension(fileName);
                var resourcesFileName = Path.ChangeExtension(fileName, ".resources" + extension);
                result.Add(resourcesFileName);
            }

            return result;
        }

        /// <summary>
        /// Checks whether we have assembly with the same file name in TPA list.
        /// </summary>
        /// <param name="assemblyFileName">File name with extension and without path.</param>
        /// <returns>True if TPA list contains assembly with the same file name, otherwise false.</returns>
        public static bool ContainsAssembly(string assemblyFileName)
        {
            return _tpaFileNames.Contains(assemblyFileName);
        }
    }
}
