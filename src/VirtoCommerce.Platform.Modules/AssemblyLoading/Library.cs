// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/1001cdede224c0d335f21ec7b1a9f3fa7ad7fa84/src/Plugins/LibraryModel/Library.cs

using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.DependencyModel;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    [DebuggerDisplay("{Name}")]
    public class Library
    {
        /// <summary>
        /// Creates an instance of <see cref="Library" />.
        /// </summary>
        /// <param name="package">The NuGet package.</param>
        /// <param name="assetPath">The path within the NuGet package.</param>
        /// <param name="isNative">Indicates whether the library is a native library (true) or a managed library (false).</param>
        /// <returns></returns>
        public Library(RuntimeLibrary package, string assetPath, bool isNative)
        {
            Package = package;
            AssetPath = assetPath;
            IsNative = isNative;
            AdditionalProbingPath = Path.Combine(package.Name.ToLowerInvariant(), package.Version, assetPath);
            Name = Path.GetFileNameWithoutExtension(assetPath);
            FileName = Path.GetFileName(assetPath);

            // When the asset comes from "lib/$tfm/", Microsoft.NET.Sdk will flatten this during publish based on the most compatible TFM.
            // The SDK will not flatten managed libraries found under runtimes/
            AppLocalPath = assetPath.StartsWith("lib/")
                ? Path.GetFileName(assetPath)
                : assetPath;
        }

        public RuntimeLibrary Package { get; }

        public string AssetPath { get; }

        public bool IsNative { get; }

        /// <summary>
        /// Name of the library
        /// </summary>
        public string Name { get; }

        public string FileName { get; }

        /// <summary>
        /// Contains path to file within an additional probing path root. This is typically a combination
        /// of the NuGet package ID (lowercased), version, and path within the package.
        /// <para>
        /// For example, <c>microsoft.data.sqlite/1.0.0/lib/netstandard1.3/Microsoft.Data.Sqlite.dll</c>
        /// </para>
        /// </summary>
        public string AdditionalProbingPath { get; }

        /// <summary>
        /// Contains path to file within a deployed, framework-dependent application.
        /// <para>
        /// For most managed libraries, this will be the file name.
        /// For example, <c>MyPlugin1.dll</c>.
        /// </para>
        /// <para>
        /// For runtime-specific managed implementations, this may include a sub folder path.
        /// For example, <c>runtimes/win/lib/netcoreapp2.0/System.Diagnostics.EventLog.dll</c>
        /// </para>
        /// </summary>
        public string AppLocalPath { get; }
    }
}
