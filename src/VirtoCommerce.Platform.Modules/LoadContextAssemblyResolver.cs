using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Serilog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules
{
    public class LoadContextAssemblyResolver : IAssemblyResolver
    {
        private readonly ILogger _logger;
        private readonly bool _isDevelopmentEnvironment;
        private readonly Dictionary<string, Assembly> _loadedAssemblies = new();
        private readonly ConcurrentDictionary<string, List<string>> _nativePathsByName = new();

        private readonly IList<string> _ignoredAssemblies =
        [
            "AspNet.Security.OpenIdConnect.Extensions",
            "AspNet.Security.OpenIdConnect.Primitives",
            "AspNet.Security.OpenIdConnect.Server",
            "OpenIddict.Mvc",
            "CryptoHelper",
            "Microsoft.EntityFrameworkCore.Design",
        ];

        public LoadContextAssemblyResolver(ILogger logger, bool isDevelopmentEnvironment)
        {
            _logger = logger;
            _isDevelopmentEnvironment = isDevelopmentEnvironment;
            AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNativeLibrary;
        }

        /// <summary>
        /// Loads specified assembly and all its nested dependencies to default AssemblyLoadContext (ALC).
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly to load.</param>
        /// <exception cref="ModuleInitializeException">If cannot load assembly or its dependencies.</exception>
        public Assembly LoadAssemblyFrom(string assemblyPath)
        {
            var assemblyUri = GetFileUri(assemblyPath);

            if (assemblyUri == null)
            {
                throw new ArgumentException("The argument must be a valid absolute Uri to an assembly file.", nameof(assemblyPath));
            }

            if (!File.Exists(assemblyUri.LocalPath))
            {
                throw new FileNotFoundException(assemblyUri.LocalPath);
            }

            var assembly = LoadAssemblyWithReferences(assemblyUri.LocalPath, BuildLoadContext(assemblyUri));
            return assembly;
        }


        private ManagedAssemblyLoadContext BuildLoadContext(Uri assemblyUri)
        {
            var assemblyDirectory = Path.GetDirectoryName(assemblyUri.LocalPath);
            var runtimeConfigFilePath = Path.ChangeExtension(assemblyUri.LocalPath, ".runtimeconfig.json");

            return new ManagedAssemblyLoadContext
            {
                PlatformPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                BasePath = assemblyDirectory,
                AdditionalProbingPaths = runtimeConfigFilePath.TryGetAdditionalProbingPathFromRuntimeConfig(_isDevelopmentEnvironment, out _),
            };
        }

        private Assembly LoadAssemblyWithReferences(string assemblyPath, ManagedAssemblyLoadContext loadContext)
        {
            var depsFilePath = Path.ChangeExtension(assemblyPath, ".deps.json");

            if (!File.Exists(depsFilePath))
            {
                throw new ModuleInitializeException($"Cannot find \".deps.json\" file for \"{assemblyPath}\".");
            }

            var mainAssemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            Assembly mainAssembly = null;

            // Load all assembly references which we could get through .deps.json file
            foreach (var dependency in depsFilePath.ExtractDependenciesFromPath())
            {
                if (dependency.IsNative)
                {
                    RegisterNativeLibrary(dependency, loadContext);
                    continue;
                }

                try
                {
                    var loadedAssembly = LoadAssemblyCached(dependency, loadContext);
                    if (loadedAssembly == null)
                    {
                        // Temporary workaround to ensure seamless update to OpenIddictV3:
                        // skips unused OpenIddictV2 assemblies that might not be present on the machine from being loaded by modules (in Platform.Security package)
                        // will be removed later.
                        if (_ignoredAssemblies.ContainsIgnoreCase(dependency.Name))
                        {
                            continue;
                        }

                        throw GenerateAssemblyLoadException(dependency, assemblyPath);
                    }

                    if (mainAssemblyName.EqualsIgnoreCase(loadedAssembly.GetName().Name))
                    {
                        mainAssembly = loadedAssembly;
                    }
                }
                catch (Exception ex) when (ex is not ModuleInitializeException)
                {
                    throw GenerateAssemblyLoadException(dependency, assemblyPath, ex);
                }
            }

            return mainAssembly;
        }

        private static ModuleInitializeException GenerateAssemblyLoadException(Library dependency, string assemblyPath, Exception innerException = null)
        {
            return new ModuleInitializeException($"Cannot load dependency '{dependency.AssetPath}' for assembly '{assemblyPath}'.", innerException);
        }

        /// <summary>
        /// Loads assembly to AssemblyLoadContext.Default or gets it from own assembly cache.
        /// <para>
        /// Note that only one version of assembly would be loaded and cached by AssemblyName.Name, for all other versions returns cached assembly.
        /// </para>
        /// </summary>
        /// <param name="managedLibrary">Library object containing library name and paths.</param>
        /// <param name="loadContext">ManagedAssemblyLoadContext object.</param>
        /// <returns>Returns loaded assembly (could be cached).</returns>
        private Assembly LoadAssemblyCached(Library managedLibrary, ManagedAssemblyLoadContext loadContext)
        {
            if (_loadedAssemblies.TryGetValue(managedLibrary.Name, out var assembly))
            {
                return assembly;
            }

            var loadedAssembly = LoadAssemblyInternal(managedLibrary, loadContext);
            if (loadedAssembly != null)
            {
                _loadedAssemblies.Add(managedLibrary.Name, loadedAssembly);
            }

            return loadedAssembly;
        }

        /// <summary>
        /// Performs loading into AssemblyLoadContext.Default using LoadFromAssemblyName for TPA assemblies and LoadFromAssemblyPath for other dependencies.
        /// <para>
        /// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/8f5c28fa70f0869a1af2e2904536268f184e71de/src/Plugins/Loader/ManagedLoadContext.cs Load method,
        /// but avoided FileNotFoundException from LoadFromAssemblyName trying only load TPA assemblies that way.
        /// </para>
        /// </summary>
        /// <param name="managedLibrary">Library object containing assembly name and paths.</param>
        /// <param name="loadContext">ManagedAssemblyLoadContext object.</param>
        /// <returns>Returns loaded assembly.</returns>
        private static Assembly LoadAssemblyInternal(Library managedLibrary, ManagedAssemblyLoadContext loadContext)
        {
            // To avoid FileNotFoundException for assemblies that are included in TPA - we load them using AssemblyLoadContext.Default.LoadFromAssemblyName.
            if (Tpa.ContainsAssembly(managedLibrary.FileName))
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(managedLibrary.Name));
            }

            if (TryGetFullPath(managedLibrary, loadContext, out var path))
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            }

            return null;
        }

        private void RegisterNativeLibrary(Library library, ManagedAssemblyLoadContext loadContext)
        {
            if (!TryGetFullPath(library, loadContext, out var path))
            {
                return;
            }

            if (!_nativePathsByName.TryGetValue(library.Name, out var nativePaths))
            {
                nativePaths = [];
                _nativePathsByName[library.Name] = nativePaths;
            }

            if (!nativePaths.Contains(path))
            {
                nativePaths.Add(path);
                _logger.Debug("Registered native library: {LibraryPath}", path);
            }
        }

        private static bool TryGetFullPath(Library library, ManagedAssemblyLoadContext loadContext, out string fullPath)
        {
            var paths = library.IsNative
                ? GetNativeLibraryPaths(library, loadContext)
                : GetManagedLibraryPaths(library, loadContext);

            foreach (var path in paths.Select(Path.GetFullPath))
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                fullPath = path;
                return true;
            }

            fullPath = null;
            return false;
        }

        private static IEnumerable<string> GetNativeLibraryPaths(Library library, ManagedAssemblyLoadContext loadContext)
        {
            yield return Path.Combine(loadContext.PlatformPath, library.AssetPath);

            yield return Path.Combine(loadContext.BasePath, library.AssetPath);

            foreach (var searchPath in loadContext.AdditionalProbingPaths)
            {
                yield return Path.Combine(searchPath, library.AdditionalProbingPath);
            }
        }

        private static IEnumerable<string> GetManagedLibraryPaths(Library library, ManagedAssemblyLoadContext loadContext)
        {
            yield return Path.Combine(loadContext.BasePath, library.AppLocalPath);

            foreach (var searchPath in loadContext.AdditionalProbingPaths)
            {
                yield return Path.Combine(searchPath, library.AdditionalProbingPath);
            }

            foreach (var ext in PlatformInformation.ManagedAssemblyExtensions)
            {
                yield return Path.Combine(loadContext.BasePath, library.Name + ext);
            }
        }

        private IntPtr ResolveNativeLibrary(Assembly assembly, string name)
        {
            _logger.Debug("Resolving native library '{LibraryName}' for assembly '{AssemblyName}'", name, assembly.FullName);

            if (!TryGetNativeLibraryPaths(name, out var nativePaths))
            {
                _logger.Debug("Not found");
            }
            else
            {
                foreach (var nativePath in nativePaths)
                {
                    _logger.Debug("Loading '{LibraryPath}'", nativePath);

                    if (NativeLibrary.TryLoad(nativePath, out var handle))
                    {
                        _logger.Debug("Succeeded");

                        // Replace the list with a single path to avoid multiple attempts to load the same library
                        _nativePathsByName.AddOrUpdate(name, [nativePath], (_, oldList) => oldList.Count == 1 ? oldList : [nativePath]);

                        return handle;
                    }

                    _logger.Debug("Failed");
                }
            }

            return IntPtr.Zero;
        }

        private bool TryGetNativeLibraryPaths(string name, out List<string> nativePaths)
        {
            if (_nativePathsByName.TryGetValue(name, out nativePaths))
            {
                return true;
            }

            if (!PlatformInformation.NativeLibraryPrefixes.IsNullOrEmpty())
            {
                foreach (var prefix in PlatformInformation.NativeLibraryPrefixes)
                {
                    if (!name.StartsWith(prefix) && _nativePathsByName.TryGetValue(prefix + name, out nativePaths))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static Uri GetFileUri(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            if (!Uri.TryCreate(filePath, UriKind.Absolute, out var uri))
            {
                return null;
            }

            if (!uri.IsFile)
            {
                return null;
            }

            return uri;
        }
    }
}
