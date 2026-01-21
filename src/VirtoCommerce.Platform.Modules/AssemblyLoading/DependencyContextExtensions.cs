using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyModel;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    // Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/ce1b5cc94e8e9ce018f9304368cc69897cc70cca/src/Plugins/Loader/DependencyContextExtensions.cs
    public static class DependencyContextExtensions
    {
        /// <summary>
        /// Get dependency list from a .deps.json file.
        /// </summary>
        /// <param name="depsFilePath">The full path to the .deps.json file.</param>
        /// <returns>Returns all assembly dependencies.</returns>
        public static IEnumerable<Library> ExtractDependenciesFromPath(this string depsFilePath)
        {
            var reader = new DependencyContextJsonReader();
            using (var file = File.OpenRead(depsFilePath))
            {
                var deps = reader.Read(file);
                var dependencies = ExtractDependencies(deps).ToArray();
                return dependencies;
            }
        }

        /// <summary>
        /// Get dependency list form pre-parsed <see cref="DependencyContext" />.
        /// </summary>
        /// <param name="dependencyContext">The dependency context.</param>
        /// <returns>Returns all assembly dependencies.</returns>
        public static IEnumerable<Library> ExtractDependencies(this DependencyContext dependencyContext)
        {
            var ridGraph = dependencyContext.RuntimeGraph.Any()
               ? dependencyContext.RuntimeGraph
               : DependencyContext.Default.RuntimeGraph;

            var rid = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
            var fallbackRid = GetFallbackRid();

            var fallbackGraph = ridGraph.FirstOrDefault(g => g.Runtime == rid)
                ?? ridGraph.FirstOrDefault(g => g.Runtime == fallbackRid)
                ?? new RuntimeFallbacks("any");

            var rids = GetRids(fallbackGraph);

            return ResolveRuntimeAssemblies(dependencyContext, rids);
        }


        private static string GetFallbackRid()
        {
            // see https://github.com/dotnet/core-setup/blob/b64f7fffbd14a3517186b9a9d5cc001ab6e5bde6/src/corehost/common/pal.h#L53-L73

            string ridBase;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ridBase = "win10";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ridBase = "linux";

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ridBase = "osx.10.12";
            }
            else
            {
                return "any";
            }

            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    return ridBase + "-x86";
                case Architecture.X64:
                    return ridBase + "-x64";
                case Architecture.Arm:
                    return ridBase + "-arm";
                case Architecture.Arm64:
                    return ridBase + "-arm64";
            }

            return ridBase;
        }

        private static IEnumerable<Library> ResolveRuntimeAssemblies(DependencyContext depContext, List<string> rids)
        {
            foreach (var runtimeLibrary in depContext.RuntimeLibraries)
            {
                foreach (var assetPath in GetAssets(rids, runtimeLibrary.NativeLibraryGroups))
                {
                    yield return new Library(runtimeLibrary, assetPath, isNative: true);
                }

                foreach (var assetPath in GetAssets(rids, runtimeLibrary.RuntimeAssemblyGroups))
                {
                    yield return new Library(runtimeLibrary, assetPath, isNative: false);
                }
            }
        }

        private static List<string> GetRids(RuntimeFallbacks runtimeGraph)
        {
            var result = new List<string> { runtimeGraph.Runtime };
            result.AddRange(runtimeGraph.Fallbacks);

            return result;
        }

        private static IReadOnlyList<string> GetAssets(List<string> rids, IReadOnlyList<RuntimeAssetGroup> groups)
        {
            foreach (var rid in rids)
            {
                var group = groups.FirstOrDefault(g => g.Runtime == rid);
                if (group != null)
                {
                    return group.AssetPaths;
                }
            }

            // Return the RID-agnostic group
            return groups.GetDefaultAssets().ToArray();
        }
    }
}
