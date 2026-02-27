using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Discovers and instantiates <see cref="IPlatformStartup"/> implementations from module assemblies.
    /// Designed to run early in the platform lifecycle (Program.cs level) before DI is configured.
    /// </summary>
    public static class PlatformStartupDiscovery
    {
        /// <summary>
        /// Scans module manifests in the discovery path for those declaring a startupType,
        /// loads their assemblies from the probing path, and instantiates the startup types.
        /// Returns an empty list if paths are missing or no startup types are found.
        /// </summary>
        /// <param name="discoveryPath">Path to scan for module.manifest files (e.g., "modules/").</param>
        /// <param name="probingPath">Path where module assemblies are located (e.g., "app_data/modules/").</param>
        /// <returns>List of discovered startup instances ordered by <see cref="IPlatformStartup.Priority"/>.</returns>
        public static IReadOnlyList<IPlatformStartup> DiscoverStartups(string discoveryPath, string probingPath)
        {
            var startups = new List<IPlatformStartup>();

            if (string.IsNullOrEmpty(discoveryPath) || !Directory.Exists(discoveryPath))
            {
                return startups;
            }

            if (string.IsNullOrEmpty(probingPath) || !Directory.Exists(probingPath))
            {
                return startups;
            }

            // Register a permanent handler to resolve assemblies from the probing path.
            // This is needed because startup assemblies are loaded into the default ALC
            // before the full module loader (LoadContextAssemblyResolver) is configured,
            // so their transitive dependencies won't be found otherwise.
            // The handler must remain active because startup methods (ConfigureAppConfiguration,
            // ConfigureHostServices, etc.) are invoked later during host building and may
            // trigger assembly loads at that point.
            var fullProbingPath = Path.GetFullPath(probingPath);
            AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
            {
                var candidatePath = Path.Combine(fullProbingPath, assemblyName.Name + ".dll");
                if (File.Exists(candidatePath))
                {
                    return context.LoadFromAssemblyPath(candidatePath);
                }

                return null;
            };

            foreach (var manifestFile in Directory.EnumerateFiles(discoveryPath, "module.manifest", SearchOption.AllDirectories))
            {
                // Exclude manifests from built modules artifacts
                if (manifestFile.Contains("artifacts"))
                {
                    continue;
                }

                ModuleManifest manifest;
                try
                {
                    manifest = ManifestReader.Read(manifestFile);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Warning: Failed to read module manifest {manifestFile}: {ex.Message}");
                    continue;
                }

                if (string.IsNullOrEmpty(manifest?.StartupType) || string.IsNullOrEmpty(manifest.AssemblyFile))
                {
                    continue;
                }

                var assemblyPath = Path.GetFullPath(Path.Combine(probingPath, manifest.AssemblyFile));
                if (!File.Exists(assemblyPath))
                {
                    // Assembly not yet in probing path (first run). Skip gracefully.
                    continue;
                }

                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                    var startupType = assembly.GetType(manifest.StartupType);

                    if (startupType == null)
                    {
                        // Try partial match (same pattern as ModuleInitializer.TryResolveModuleTypeFromAssembly)
                        startupType = assembly.GetTypes()
                            .FirstOrDefault(t => typeof(IPlatformStartup).IsAssignableFrom(t)
                                && t.AssemblyQualifiedName?.StartsWith(manifest.StartupType) == true);
                    }

                    if (startupType != null
                        && typeof(IPlatformStartup).IsAssignableFrom(startupType)
                        && Activator.CreateInstance(startupType) is IPlatformStartup startup)
                    {
                        startups.Add(startup);
                    }
                    else
                    {
                        Console.Error.WriteLine(
                            $"Warning: Startup type '{manifest.StartupType}' in module '{manifest.Id}' does not implement IPlatformStartup or could not be instantiated.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(
                        $"Warning: Failed to load startup type '{manifest.StartupType}' from '{manifest.AssemblyFile}' (module '{manifest.Id}'): {ex.Message}");
                }
            }

            return startups.OrderBy(s => s.Priority).ToList();
        }
    }
}
