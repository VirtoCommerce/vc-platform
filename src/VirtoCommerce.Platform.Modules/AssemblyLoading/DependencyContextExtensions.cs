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
        public static IEnumerable<ManagedLibrary> ExtractDependenciesFromPath(this string depsFilePath)
        {
            var reader = new DependencyContextJsonReader();
            using (var file = File.OpenRead(depsFilePath))
            {
                var deps = reader.Read(file);
                return ExtractDependencies(deps);
            }
        }

        /// <summary>
        /// Get dependency list form pre-parsed <see cref="DependencyContext" />.
        /// </summary>
        /// <param name="dependencyContext">The dependency context.</param>
        /// <returns>Returns all assembly dependencies.</returns>
        public static IEnumerable<ManagedLibrary> ExtractDependencies(this DependencyContext dependencyContext)
        {
            var ridGraph = dependencyContext.RuntimeGraph.Any()
               ? dependencyContext.RuntimeGraph
               : DependencyContext.Default.RuntimeGraph;

            //var rid = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier(); // TODO: (AK) This function removed from CORE3. Consider to replace to something(https://github.com/dotnet/core-setup/issues/5213)
            var fallbackRid = GetFallbackRid();
            var fallbackGraph = /*ridGraph.FirstOrDefault(g => g.Runtime == rid)
                ?? */ridGraph.FirstOrDefault(g => g.Runtime == fallbackRid)
                ?? new RuntimeFallbacks("any");

            return ResolveRuntimeAssemblies(dependencyContext, fallbackGraph);
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

            switch (RuntimeInformation.OSArchitecture)
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

        private static IEnumerable<ManagedLibrary> ResolveRuntimeAssemblies(DependencyContext depContext, RuntimeFallbacks runtimeGraph)
        {
            var rids = GetRids(runtimeGraph);
            return depContext.RuntimeLibraries.SelectMany(x => SelectAssets(rids, x.RuntimeAssemblyGroups).Select(assetPath => ManagedLibrary.CreateFromPackage(x.Name, x.Version, assetPath)));
        }

        private static IEnumerable<string> GetRids(RuntimeFallbacks runtimeGraph)
        {
            return new[] { runtimeGraph.Runtime }.Concat(runtimeGraph?.Fallbacks ?? Enumerable.Empty<string>());
        }

        private static IEnumerable<string> SelectAssets(IEnumerable<string> rids, IEnumerable<RuntimeAssetGroup> groups)
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
            return groups.GetDefaultAssets();
        }
    }
}
