using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Loads module assemblies and their dependencies into the default AssemblyLoadContext.
/// Static, no DI. Extracted from LoadContextAssemblyResolver.
/// </summary>
public static class ModuleAssemblyLoader
{
    private static readonly ConcurrentDictionary<string, Assembly> _loadedAssemblies = new();
    private static readonly ConcurrentDictionary<string, List<string>> _nativePathsByName = new();
    private static readonly object _initLock = new();
    private static bool _nativeResolverRegistered;
    private static bool _isDevelopmentEnvironment;

    private static readonly IList<string> _ignoredAssemblies =
    [
        "AspNet.Security.OpenIdConnect.Extensions",
        "AspNet.Security.OpenIdConnect.Primitives",
        "AspNet.Security.OpenIdConnect.Server",
        "OpenIddict.Mvc",
        "CryptoHelper",
        "Microsoft.EntityFrameworkCore.Design",
        "Microsoft.CodeAnalysis",
        "Microsoft.CodeAnalysis.CSharp",
        "Microsoft.CodeAnalysis.CSharp.Workspaces",
        "Microsoft.CodeAnalysis.Workspaces",
        "Mono.TextTemplating",
        "System.Composition",
        "System.Composition.AttributedModel",
        "System.Composition.Convention",
        "System.Composition.Hosting",
        "System.Composition.Runtime",
        "System.Composition.TypedParts"
    ];

    /// <summary>
    /// Initialize the assembly loader. Call once before loading modules.
    /// </summary>
    public static void Initialize(bool isDevelopmentEnvironment = false)
    {
        _isDevelopmentEnvironment = isDevelopmentEnvironment;

        lock (_initLock)
        {
            if (!_nativeResolverRegistered)
            {
                AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNativeLibrary;
                _nativeResolverRegistered = true;
            }
        }
    }

    /// <summary>
    /// Load a module's main assembly and all its dependencies from the probing path.
    /// Sets module.Assembly on success.
    /// </summary>
    public static Assembly LoadModule(ManifestModuleInfo module, string probingPath)
    {
        ArgumentNullException.ThrowIfNull(module);

        if (string.IsNullOrEmpty(module.Ref))
        {
            // No assembly reference set - module may not have an assembly
            return null;
        }

        var assemblyUri = GetFileUri(module.Ref);
        if (assemblyUri == null || !File.Exists(assemblyUri.LocalPath))
        {
            ModuleLogger.CreateLogger(typeof(ModuleAssemblyLoader)).LogWarning("Assembly not found for {ModuleId}: {ModuleRef}", module.Id, module.Ref);
            module.Errors.Add($"Assembly file not found: {module.Ref}");
            return null;
        }

        try
        {
            var assembly = LoadAssemblyWithReferences(assemblyUri.LocalPath);
            module.Assembly = assembly;
            module.State = ModuleState.ReadyForInitialization;
            ModuleLogger.CreateLogger(typeof(ModuleAssemblyLoader)).LogDebug("Loaded {ModuleId} {ModuleVersion}", module.Id, module.Version);
            return assembly;
        }
        catch (Exception ex)
        {
            ModuleLogger.CreateLogger(typeof(ModuleAssemblyLoader)).LogError(ex, "Failed to load {ModuleId}", module.Id);
            module.Errors.Add($"Failed to load assembly: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Load an assembly and all dependencies listed in its .deps.json file.
    /// </summary>
    public static Assembly LoadAssemblyWithReferences(string assemblyPath)
    {
        var loadContext = BuildLoadContext(assemblyPath);
        var depsFilePath = Path.ChangeExtension(assemblyPath, ".deps.json");

        if (!File.Exists(depsFilePath))
        {
            throw new ModuleInitializeException($"Cannot find \".deps.json\" file for \"{assemblyPath}\".");
        }

        var mainAssemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
        Assembly mainAssembly = null;

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
                    if (_ignoredAssemblies.ContainsIgnoreCase(dependency.Name))
                    {
                        continue;
                    }

                    throw new ModuleInitializeException($"Cannot load dependency '{dependency.AssetPath}' for assembly '{assemblyPath}'.");
                }

                if (mainAssemblyName.EqualsIgnoreCase(loadedAssembly.GetName().Name))
                {
                    mainAssembly = loadedAssembly;
                }
            }
            catch (Exception ex) when (ex is not ModuleInitializeException)
            {
                throw new ModuleInitializeException($"Cannot load dependency '{dependency.AssetPath}' for assembly '{assemblyPath}'.", ex);
            }
        }

        return mainAssembly;
    }

    private static ManagedAssemblyLoadContext BuildLoadContext(string assemblyPath)
    {
        var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
        var runtimeConfigFilePath = Path.ChangeExtension(assemblyPath, ".runtimeconfig.json");

        return new ManagedAssemblyLoadContext
        {
            PlatformPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            BasePath = assemblyDirectory,
            AdditionalProbingPaths = runtimeConfigFilePath.TryGetAdditionalProbingPathFromRuntimeConfig(_isDevelopmentEnvironment, out _),
        };
    }

    private static Assembly LoadAssemblyCached(Library managedLibrary, ManagedAssemblyLoadContext loadContext)
    {
        if (_loadedAssemblies.TryGetValue(managedLibrary.Name, out var assembly))
        {
            return assembly;
        }

        var loadedAssembly = LoadAssemblyInternal(managedLibrary, loadContext);
        if (loadedAssembly != null)
        {
            _loadedAssemblies.TryAdd(managedLibrary.Name, loadedAssembly);
        }

        return loadedAssembly;
    }

    private static Assembly LoadAssemblyInternal(Library managedLibrary, ManagedAssemblyLoadContext loadContext)
    {
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

    private static void RegisterNativeLibrary(Library library, ManagedAssemblyLoadContext loadContext)
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
        }
    }

    private static bool TryGetFullPath(Library library, ManagedAssemblyLoadContext loadContext, out string fullPath)
    {
        var paths = library.IsNative
            ? GetNativeLibraryPaths(library, loadContext)
            : GetManagedLibraryPaths(library, loadContext);

        foreach (var path in paths.Select(Path.GetFullPath))
        {
            if (File.Exists(path))
            {
                fullPath = path;
                return true;
            }
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

    private static IntPtr ResolveNativeLibrary(Assembly assembly, string name)
    {
        if (!_nativePathsByName.TryGetValue(name, out var nativePaths))
        {
            if (!PlatformInformation.NativeLibraryPrefixes.IsNullOrEmpty())
            {
                foreach (var prefix in PlatformInformation.NativeLibraryPrefixes)
                {
                    if (!name.StartsWith(prefix) && _nativePathsByName.TryGetValue(prefix + name, out nativePaths))
                    {
                        break;
                    }
                }
            }
        }

        if (nativePaths == null)
        {
            return IntPtr.Zero;
        }

        foreach (var nativePath in nativePaths)
        {
            if (NativeLibrary.TryLoad(nativePath, out var handle))
            {
                _nativePathsByName.AddOrUpdate(name, [nativePath], (_, oldList) => oldList.Count == 1 ? oldList : [nativePath]);
                return handle;
            }
        }

        return IntPtr.Zero;
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

        return uri.IsFile ? uri : null;
    }
}
