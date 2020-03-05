using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules
{

    public class LoadContextAssemblyResolver : IAssemblyResolver
    {
        private readonly ILogger<LoadContextAssemblyResolver> _logger;
        private readonly Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();
        private readonly bool _isDevelopmentEnvironment;

        public LoadContextAssemblyResolver(ILogger<LoadContextAssemblyResolver> logger, bool isDevelopmentEnvironment)
        {
            _logger = logger;
            _isDevelopmentEnvironment = isDevelopmentEnvironment;
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

            return new ManagedAssemblyLoadContext()
            {
                BasePath = assemblyDirectory,
                AdditionalProdingPath = runtimeConfigFilePath.TryGetAdditionalProbingPathFromRuntimeConfig(_isDevelopmentEnvironment, out _),
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

            // Load all assembly referencies which we could get through .deps.json file
            foreach (var dependency in depsFilePath.ExtractDependenciesFromPath())
            {
                try
                {
                    var loadedAssembly = LoadAssemblyCached(dependency, loadContext) ?? throw GenerateAssemblyLoadException(dependency.Name.Name, assemblyPath);
                    if (mainAssemblyName.EqualsInvariant(loadedAssembly.GetName().Name))
                    {
                        mainAssembly = loadedAssembly;
                    }
                }
                catch (Exception ex) when (!(ex is ModuleInitializeException))
                {
                    throw GenerateAssemblyLoadException(dependency.Name.Name, assemblyPath, ex);
                }
            }

            return mainAssembly;
        }

        private ModuleInitializeException GenerateAssemblyLoadException(string assemblyPath, string modulePath, Exception innerException = null)
        {
            return new ModuleInitializeException($"Cannot load \"{assemblyPath}\" for module \"{modulePath}\".", innerException);
        }

        /// <summary>
        /// Loads assembly to AssemblyLoadContext.Default or gets it from own assembly cache.
        /// <para>
        /// Note that only one version of assembly would be loaded and cached by AssemblyName.Name, for all other versions returns cached assembly.
        /// </para>
        /// </summary>
        /// <param name="managedLibrary">ManagedLibrary object containing library name and paths.</param>
        /// <param name="loadContext">ManagedAssemblyLoadContext object.</param>
        /// <returns>Retures loaded assembly (could be cached).</returns>
        private Assembly LoadAssemblyCached(ManagedLibrary managedLibrary, ManagedAssemblyLoadContext loadContext)
        {
            var assemblyName = managedLibrary.Name;
            if (_loadedAssemblies.ContainsKey(assemblyName.Name))
            {
                return _loadedAssemblies[assemblyName.Name];
            }

            var loadedAssembly = LoadAssemblyInternal(managedLibrary, loadContext);
            if (loadedAssembly != null)
            {
                _loadedAssemblies.Add(assemblyName.Name, loadedAssembly);
            }
            return loadedAssembly;
        }

        /// <summary>
        /// Performs loading into AssemblyLoadContext.Default using LoadFromAssemblyName for TPA assemblies and LoadFromAssemblyPath for other dependecies.
        /// <para>
        /// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/8f5c28fa70f0869a1af2e2904536268f184e71de/src/Plugins/Loader/ManagedLoadContext.cs Load method,
        /// but avoided FileNotFoundException from LoadFromAssemblyName trying only load TPA assemblies that way.
        /// </para>
        /// </summary>
        /// <param name="managedLibrary">ManagedLibrary object containing assembly name and paths.</param>
        /// <param name="loadContext">ManagedAssemblyLoadContext object.</param>
        /// <returns>Returns loaded assembly.</returns>
        private Assembly LoadAssemblyInternal(ManagedLibrary managedLibrary, ManagedAssemblyLoadContext loadContext)
        {
            // To avoid FileNotFoundException for assemblies that are included in TPA - we load them using AssemblyLoadContext.Default.LoadFromAssemblyName.
            var assemblyFileName = Path.GetFileName(managedLibrary.AppLocalPath);
            if (TPA.ContainsAssembly(assemblyFileName))
            {
                var defaultAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(managedLibrary.Name);
                if (defaultAssembly != null)
                {
                    return defaultAssembly;
                }
            }

            if (SearchForLibrary(managedLibrary, loadContext, out var path))
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            }

            return null;
        }

        private bool SearchForLibrary(ManagedLibrary library, ManagedAssemblyLoadContext loadContext, out string path)
        {
            // 1. Check for in _basePath + app local path
            var localFile = Path.Combine(loadContext.BasePath, library.AppLocalPath);
            if (File.Exists(localFile))
            {
                path = localFile;
                return true;
            }

            // 2. Search additional probing paths
            foreach (var searchPath in loadContext.AdditionalProdingPath)
            {
                var candidate = Path.Combine(searchPath, library.AdditionalProbingPath);
                if (File.Exists(candidate))
                {
                    path = candidate;
                    return true;
                }
            }

            // 3. Search in base path
            foreach (var ext in PlatformInformation.ManagedAssemblyExtensions)
            {
                var local = Path.Combine(loadContext.BasePath, library.Name.Name + ext);
                if (File.Exists(local))
                {
                    path = local;
                    return true;
                }
            }

            path = null;
            return false;
        }

        private static Uri GetFileUri(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            if (!Uri.TryCreate(filePath, UriKind.Absolute, out Uri uri))
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
