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
public static class ModuleLoader
{
    private static ILogger _logger;
    private static bool _isDevelopmentEnvironment;
    private static readonly ConcurrentDictionary<string, Assembly> _loadedAssemblies = new();
    private static readonly ConcurrentDictionary<string, List<string>> _nativePathsByName = new();

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
        "System.Composition.TypedParts",
    ];

    /// <summary>
    /// Initialize the assembly loader. Call once before loading modules.
    /// </summary>
    public static void Initialize(bool isDevelopmentEnvironment)
    {
        _logger = ModuleLogger.CreateLogger(typeof(ModuleLoader));
        _isDevelopmentEnvironment = isDevelopmentEnvironment;
        AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNativeLibrary;
    }

    public static void LoadModules(IList<ManifestModuleInfo> modules, string probingPath)
    {
        foreach (var module in modules)
        {
            LoadModule(module, probingPath);
        }
    }

    /// <summary>
    /// Load a module's main assembly and all its dependencies from the probing path.
    /// Sets module.Assembly on success.
    /// </summary>
    public static Assembly LoadModule(ManifestModuleInfo module, string probingPath)
    {
        ArgumentNullException.ThrowIfNull(module);

        if (string.IsNullOrEmpty(module.AssemblyFile))
        {
            // No assembly reference set - a module may not have an assembly
            return null;
        }

        var moduleAssemblyPath = Path.GetFullPath(Path.Combine(probingPath, module.AssemblyFile));

        if (!File.Exists(moduleAssemblyPath))
        {
            _logger.LogWarning("Assembly file not found for module {ModuleId}: {ModuleRef}", module.Id, moduleAssemblyPath);
            module.Errors.Add($"Assembly file not found: {moduleAssemblyPath}");
            return null;
        }

        module.Ref = GetFileAbsoluteUri(moduleAssemblyPath).ToString();

        try
        {
            _logger.LogDebug("Loading module {ModuleId} {ModuleVersion}", module.Id, module.Version);
            var assembly = LoadAssemblyWithReferences(moduleAssemblyPath);
            module.Assembly = assembly;
            module.State = ModuleState.ReadyForInitialization;

            return assembly;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load {ModuleId}", module.Id);
            module.Errors.Add($"Failed to load assembly: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Load an assembly and all dependencies listed in its .deps.json file.
    /// </summary>
    private static Assembly LoadAssemblyWithReferences(string assemblyPath)
    {
        var loadContext = BuildLoadContext(assemblyPath);
        var depsFilePath = Path.ChangeExtension(assemblyPath, ".deps.json");

        if (!File.Exists(depsFilePath))
        {
            throw new ModuleInitializeException($"Cannot find '.deps.json' file for assembly '{assemblyPath}'.");
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
                var loadedAssembly = _loadedAssemblies.GetOrAdd(dependency.Name, _ => LoadAssemblyInternal(dependency, loadContext));
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
        var runtimeConfigFilePath = Path.ChangeExtension(assemblyPath, ".runtimeconfig.json");

        return new ManagedAssemblyLoadContext
        {
            PlatformPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            BasePath = Path.GetDirectoryName(assemblyPath),
            AdditionalProbingPaths = runtimeConfigFilePath.TryGetAdditionalProbingPathFromRuntimeConfig(_isDevelopmentEnvironment, out _),
        };
    }

    /// <summary>
    /// Performs loading into AssemblyLoadContext.Default using LoadFromAssemblyName for TPA assemblies and LoadFromAssemblyPath for other dependencies.
    /// <para>
    /// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/8f5c28fa70f0869a1af2e2904536268f184e71de/src/Plugins/Loader/ManagedLoadContext.cs Load method,
    /// but avoided FileNotFoundException from LoadFromAssemblyName trying only load TPA assemblies that way.
    /// </para>
    /// </summary>
    private static Assembly LoadAssemblyInternal(Library managedLibrary, ManagedAssemblyLoadContext loadContext)
    {
        // To avoid FileNotFoundException for assemblies that are included in TPA - we load them using AssemblyLoadContext.Default.LoadFromAssemblyName.
        if (Tpa.ContainsAssembly(managedLibrary.FileName))
        {
            _logger.LogTrace("Loading managed library: {Name}", managedLibrary.Name);
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(managedLibrary.Name));
        }

        if (TryGetFullPath(managedLibrary, loadContext, out var path))
        {
            _logger.LogTrace("Loading managed library: {Path}", path);
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

        var nativePaths = _nativePathsByName.GetOrAdd(library.Name, _ => []);

        if (!nativePaths.Contains(path))
        {
            nativePaths.Add(path);
            _logger.LogTrace("Registered native library: {LibraryPath}", path);
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
        _logger.LogTrace("Resolving native library '{LibraryName}' for assembly '{AssemblyName}'", name, assembly.FullName);

        if (!TryGetNativeLibraryPaths(name, out var nativePaths))
        {
            _logger.LogTrace("Not found");
            return IntPtr.Zero;
        }

        foreach (var nativePath in nativePaths)
        {
            _logger.LogTrace("Loading '{LibraryPath}'", nativePath);

            if (NativeLibrary.TryLoad(nativePath, out var handle))
            {
                _logger.LogTrace("Succeeded");

                // Replace the list with a single path to avoid multiple attempts to load the same library
                _nativePathsByName.AddOrUpdate(name, [nativePath], (_, oldList) => oldList.Count == 1 ? oldList : [nativePath]);

                return handle;
            }

            _logger.LogTrace("Failed");
        }

        return IntPtr.Zero;
    }

    private static bool TryGetNativeLibraryPaths(string name, out List<string> nativePaths)
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

    private static Uri GetFileAbsoluteUri(string localPath)
    {
        var builder = new UriBuilder
        {
            Host = string.Empty,
            Scheme = Uri.UriSchemeFile,
            Path = localPath,
        };

        return builder.Uri;
    }
}
