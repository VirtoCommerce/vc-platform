using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.AssemblyLoading;
using VirtoCommerce.Platform.Modules.Local;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Single entry point for the module loading pipeline.
/// <para>
/// Usage (platform):
/// <code>
/// ModuleBootstrapper.Instance = new ModuleBootstrapper(loggerFactory, options)
///     .Discover(platformVersion)
///     .Copy(RuntimeInformation.ProcessArchitecture)
///     .Load(isDevelopment);
/// </code>
/// </para>
/// <para>
/// Usage (vc-build, design-time only):
/// <code>
/// var bootstrapper = new ModuleBootstrapper(loggerFactory, options)
///     .Discover(platformVersion);
/// var modules = bootstrapper.GetModules();
/// </code>
/// </para>
/// </summary>
public class ModuleBootstrapper : IModuleService
{
    #region Static Instance

    /// <summary>
    /// Static accessor for cross-phase use (Program.cs → Startup.cs → controllers).
    /// Set once in Program.Main after the pipeline completes.
    /// </summary>
    public static ModuleBootstrapper Instance { get; set; }

    #endregion

    #region Fields

    private readonly ILoggerFactory _loggerFactory;
    private readonly LocalStorageModuleCatalogOptions _options;

    // Registry state
    private IList<ManifestModuleInfo> _modules = [];
    private Dictionary<string, ManifestModuleInfo> _modulesById = new(StringComparer.OrdinalIgnoreCase);

    // Startup discovery state
    private IList<IPlatformStartup> _startups = [];

    // Assembly loading state (process-global caches)
    private readonly ConcurrentDictionary<string, Assembly> _loadedAssemblies = new();
    private readonly ConcurrentDictionary<string, List<string>> _nativePathsByName = new();
    private bool _nativeResolverRegistered;

    private readonly IList<string> _ignoredAssemblies =
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

    private const string _rebuildMarkerFileName = ".rebuild";

    #endregion

    #region Constructor
    /// <summary>
    /// Create a new bootstrapper with all dependencies.
    /// </summary>
    /// <param name="loggerFactory">Logger factory for all module loading operations.</param>
    /// <param name="options">Local module catalog options (discovery path, probing path, etc.).</param>
    /// <param name="ignoredAssemblies">Optional list of assemblies to ignore during module loading.</param>
    public ModuleBootstrapper(
        ILoggerFactory loggerFactory,
        LocalStorageModuleCatalogOptions options,
        string[] ignoredAssemblies = null)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(options);

        _loggerFactory = loggerFactory;
        _options = options;

        if (ignoredAssemblies != null)
        {
            _ignoredAssemblies = ignoredAssemblies.ToList();
        }
    }

    #endregion

    #region Pipeline — Fluent API

    /// <summary>
    /// Phase 1: Read manifests from discovery path, sort by dependencies, validate against platform version,
    /// and populate the module registry (GetModules/GetInstalledModules/GetFailedModules).
    /// </summary>
    public ModuleBootstrapper Discover(SemanticVersion platformVersion)
    {
        ArgumentNullException.ThrowIfNull(platformVersion);

        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        // 1. Read manifests
        var modules = ReadLocalManifests();

        // 2. Sort by dependencies
        modules = SortModulesByDependency(modules);

        // 3. Validate
        ValidateModulesInternal(modules, platformVersion);

        // 4. Populate registry
        RegisterModules(modules);

        logger.LogInformation("Discovered modules: {ModuleCount}, with errors: {ErrorCount}",
            modules.Count, modules.Count(x => x.Errors.Count > 0));

        return this;
    }

    /// <summary>
    /// Phase 2: Copy module assemblies to the probing path.
    /// Platform-only — vc-build skips this phase.
    /// </summary>
    public ModuleBootstrapper Copy(Architecture arch)
    {
        CopyModulesInternal(_modules, arch);
        return this;
    }

    /// <summary>
    /// Phase 3: Load module assemblies, refresh the registry (update error state),
    /// and discover IPlatformStartup implementations.
    /// </summary>
    public ModuleBootstrapper Load(bool isDevelopmentEnvironment)
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        // Load assemblies
        LoadModulesInternal(_modules, isDevelopmentEnvironment);

        // Refresh registry (modules may have new errors from load failures)
        RegisterModules(_modules);

        // Discover IPlatformStartup implementations
        DiscoverStartupsInternal(_modules);

        logger.LogInformation("Loaded modules: {ModuleCount}, with errors: {ErrorCount}",
            _modules.Count, _modules.Count(x => x.Errors.Count > 0));

        return this;
    }

    #endregion

    #region IModuleService

    /// <inheritdoc />
    public IList<ManifestModuleInfo> GetModules()
    {
        return _modules;
    }

    /// <inheritdoc />
    public IList<ManifestModuleInfo> GetInstalledModules()
    {
        return _modules.Where(x => x.IsInstalled && x.Errors.Count == 0).ToList();
    }

    /// <inheritdoc />
    public IList<ManifestModuleInfo> GetFailedModules()
    {
        return _modules.Where(x => x.Errors.Count > 0).ToList();
    }

    /// <inheritdoc />
    public bool IsInstalled(string moduleId)
    {
        return _modulesById.TryGetValue(moduleId, out var module) && module.IsInstalled;
    }

    /// <inheritdoc />
    public bool IsInstalled(string moduleId, string minVersion)
    {
        if (!_modulesById.TryGetValue(moduleId, out var module) || !module.IsInstalled)
        {
            return false;
        }

        var requiredVersion = SemanticVersion.Parse(minVersion);
        return module.Version >= requiredVersion;
    }

    /// <inheritdoc />
    public ManifestModuleInfo GetModule(string moduleId)
    {
        _modulesById.TryGetValue(moduleId, out var module);
        return module;
    }

    #endregion

    #region Validation & External Manifest

    /// <summary>
    /// Validate that a module can be installed: platform version, dependencies, incompatibilities.
    /// Returns a list of error messages (empty if valid).
    /// </summary>
    public IList<string> ValidateInstall(
        ManifestModuleInfo moduleToInstall,
        IEnumerable<ManifestModuleInfo> installedModules,
        SemanticVersion platformVersion)
    {
        ArgumentNullException.ThrowIfNull(moduleToInstall);
        ArgumentNullException.ThrowIfNull(installedModules);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var errors = new List<string>();
        var installed = installedModules as IList<ManifestModuleInfo> ?? installedModules.ToList();

        if (!moduleToInstall.PlatformVersion.IsCompatibleWith(platformVersion) ||
            !moduleToInstall.PlatformVersion.IsCompatibleWithBySemVer(platformVersion))
        {
            errors.Add($"Target platform version {moduleToInstall.PlatformVersion} is incompatible with current {platformVersion}");
        }

        if (moduleToInstall.Incompatibilities?.Count > 0)
        {
            var installedIncompatibleModules = installed
                .Where(x => moduleToInstall.Incompatibilities.Any(incompatible =>
                    incompatible.Id.EqualsIgnoreCase(x.Id) &&
                    incompatible.Version.IsCompatibleWith(x.Version)))
                .ToList();

            if (installedIncompatibleModules.Count > 0)
            {
                errors.Add($"{moduleToInstall} is incompatible with installed {string.Join(", ", installedIncompatibleModules)}");
            }
        }

        return errors;
    }

    /// <summary>
    /// Validate that a module can be uninstalled: check no installed modules depend on it.
    /// Returns list of error messages (empty if valid).
    /// </summary>
    public IList<string> ValidateUninstall(
        string moduleId,
        IEnumerable<ManifestModuleInfo> installedModules,
        IEnumerable<string> excludeModuleIds = null)
    {
        var installed = installedModules as IList<ManifestModuleInfo> ?? installedModules.ToList();
        var excludeIds = excludeModuleIds as IList<string> ?? excludeModuleIds?.ToList();

        var dependingModules = installed
            .Where(x =>
                x.DependsOn.ContainsIgnoreCase(moduleId) &&
                (excludeIds == null || !excludeIds.ContainsIgnoreCase(x.Id)));

        return dependingModules
            .Select(x => $"Unable to uninstall '{moduleId}' because '{x.Id}' depends on it")
            .ToList();
    }

    /// <summary>
    /// Returns the given modules plus all their transitive dependencies (prerequisites), sorted in dependency order.
    /// </summary>
    public IList<ManifestModuleInfo> GetDependencies(
        IEnumerable<ManifestModuleInfo> selectedModules,
        IEnumerable<ManifestModuleInfo> allAvailableModules)
    {
        ArgumentNullException.ThrowIfNull(selectedModules);
        ArgumentNullException.ThrowIfNull(allAvailableModules);

        var available = allAvailableModules as IList<ManifestModuleInfo> ?? allAvailableModules.ToList();
        var completeList = new List<ManifestModuleInfo>();
        var pendingList = new List<ManifestModuleInfo>(selectedModules);

        while (pendingList.Count > 0)
        {
            var moduleInfo = pendingList[0];
            pendingList.RemoveAt(0);

            if (moduleInfo.Dependencies != null)
            {
                foreach (var dependency in moduleInfo.Dependencies)
                {
                    var candidates = available
                        .Where(x => x.Id.EqualsIgnoreCase(dependency.Id))
                        .Where(x => dependency.Version.IsCompatibleWithBySemVer(x.Version))
                        .OrderByDescending(x => x.Version)
                        .ToList();

                    var resolved = candidates.FirstOrDefault(x => x.IsInstalled) ?? candidates.FirstOrDefault();

                    if (resolved != null && !completeList.Contains(resolved) && !pendingList.Contains(resolved))
                    {
                        pendingList.Add(resolved);
                    }
                }
            }

            if (!completeList.Contains(moduleInfo))
            {
                completeList.Add(moduleInfo);
            }
        }

        return SortModulesByDependency(completeList).ToList();
    }

    /// <summary>
    /// Parse external module manifest JSON into a list of ManifestModuleInfo.
    /// </summary>
    public IList<ManifestModuleInfo> ParseExternalManifest(
        string manifestJson,
        SemanticVersion platformVersion,
        bool includePrerelease = false)
    {
        ArgumentNullException.ThrowIfNull(manifestJson);
        ArgumentNullException.ThrowIfNull(platformVersion);

        var manifests = JsonSerializer.Deserialize<List<ExternalModuleManifest>>(manifestJson);
        if (manifests.IsNullOrEmpty())
        {
            return [];
        }

        var result = new List<ManifestModuleInfo>();
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        foreach (var manifest in manifests)
        {
            if (manifest.Versions == null)
            {
                logger.LogWarning("Module {ModuleId} has invalid format, missing 'versions'", manifest.Id);
                continue;
            }

            var latestStable = GetLatestCompatibleVersion(manifest, platformVersion, prerelease: false);
            if (latestStable != null)
            {
                result.Add(latestStable);
            }

            if (includePrerelease)
            {
                var latestPrerelease = GetLatestCompatibleVersion(manifest, platformVersion, prerelease: true);
                if (latestPrerelease != null)
                {
                    result.Add(latestPrerelease);
                }
            }
        }

        logger.LogDebug("Parsed {ModuleCount} modules from the external manifest", result.Count);
        return result;
    }

    /// <summary>
    /// Merge external modules with locally installed modules.
    /// </summary>
    public IList<ManifestModuleInfo> MergeWithInstalled(
        IEnumerable<ManifestModuleInfo> externalModules,
        IEnumerable<ManifestModuleInfo> installedModules)
    {
        ArgumentNullException.ThrowIfNull(externalModules);
        ArgumentNullException.ThrowIfNull(installedModules);

        var installed = installedModules as IList<ManifestModuleInfo> ?? installedModules.ToList();
        var installedModulesById = installed.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        var result = new List<ManifestModuleInfo>();

        foreach (var externalModule in externalModules)
        {
            if (installedModulesById.TryGetValue(externalModule.Id, out var installedModule))
            {
                if (externalModule.Equals(installedModule))
                {
                    externalModule.IsInstalled = installedModule.IsInstalled;
                    externalModule.Errors.AddRange(installedModule.Errors);
                }
                else if (installedModule.Version > externalModule.Version)
                {
                    continue;
                }
            }

            result.Add(externalModule);
        }

        foreach (var installedModule in installed)
        {
            if (!result.Contains(installedModule))
            {
                result.Add(installedModule);
            }
        }

        return result;
    }

    #endregion

    #region Probing Folder Management

    /// <summary>
    /// Write a marker file so the next startup rebuilds the probing folder from scratch.
    /// Called at runtime after install/uninstall when loaded assemblies are locked.
    /// </summary>
    public void InvalidateProbingFolder()
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        if (!Directory.Exists(_options.ProbingPath))
        {
            Directory.CreateDirectory(_options.ProbingPath);
        }

        var markerPath = Path.Combine(_options.ProbingPath, _rebuildMarkerFileName);
        File.WriteAllBytes(markerPath, []);

        logger.LogInformation("Probing folder marked for rebuild on next startup");
    }

    #endregion

    #region Module Initialization

    /// <summary>
    /// Run IModule.Initialize() on all modules in dependency order.
    /// </summary>
    public void InitializeModules(
        IServiceCollection serviceCollection,
        IConfiguration configuration = null,
        IHostEnvironment hostEnvironment = null,
#pragma warning disable VC0014
        IModuleCatalog moduleCatalog = null
#pragma warning restore VC0014
        )
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        foreach (var moduleInfo in _modules)
        {
            if (moduleInfo.Errors.Count > 0)
            {
                logger.LogWarning("Skipping module {ModuleId} (has errors)", moduleInfo.Id);
                continue;
            }

            if (moduleInfo.Assembly == null)
            {
                logger.LogDebug("Skipping module {ModuleId} (no assembly)", moduleInfo.Id);
                continue;
            }

            try
            {
                logger.LogDebug("Initializing module {ModuleId} {ModuleVersion}", moduleInfo.Id, moduleInfo.Version);

                var instance = CreateModuleInstance(moduleInfo);
                moduleInfo.ModuleInstance = instance;

                if (instance is IHasConfiguration configurable && configuration != null)
                {
                    configurable.Configuration = configuration;
                }

                if (instance is IHasHostEnvironment hasHost && hostEnvironment != null)
                {
                    hasHost.HostEnvironment = hostEnvironment;
                }

#pragma warning disable VC0014
                if (instance is IHasModuleCatalog hasModuleCatalog && moduleCatalog != null)
                {
                    hasModuleCatalog.ModuleCatalog = moduleCatalog;
                }
#pragma warning restore VC0014

                instance.Initialize(serviceCollection);
                moduleInfo.State = ModuleState.Initialized;
            }
            catch (Exception ex)
            {
                moduleInfo.Errors.Add(ex.ToString());
                logger.LogError(ex, "Failed to initialize module {ModuleId}", moduleInfo.Id);
            }
        }
    }

    /// <summary>
    /// Run IModule.PostInitialize() on all initialized modules.
    /// </summary>
    public void PostInitializeModules(IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        foreach (var moduleInfo in _modules)
        {
            if (moduleInfo.ModuleInstance == null || moduleInfo.Errors.Count > 0)
            {
                continue;
            }

            try
            {
                logger.LogDebug("Post-initializing module {ModuleId}", moduleInfo.Id);
                moduleInfo.ModuleInstance.PostInitialize(appBuilder);
            }
            catch (Exception ex)
            {
                moduleInfo.Errors.Add(ex.ToString());
                logger.LogError(ex, "Failed to post-initialize module {ModuleId}", moduleInfo.Id);
            }
        }
    }

    #endregion

    #region Startup Discovery

    /// <summary>
    /// Get previously discovered IPlatformStartup implementations.
    /// </summary>
    public IReadOnlyList<IPlatformStartup> Startups => (IReadOnlyList<IPlatformStartup>)_startups;

    public void RunConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment environment)
    {
        foreach (var startup in _startups)
        {
            startup.ConfigureAppConfiguration(builder, environment);
        }
    }

    public void RunConfigureHostServices(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var startup in _startups)
        {
            startup.ConfigureHostServices(services, configuration);
        }
    }

    public void RunConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var startup in _startups)
        {
            startup.ConfigureServices(services, configuration);
        }
    }

    public void RunConfigure(IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        foreach (var startup in _startups)
        {
            startup.Configure(applicationBuilder, configuration);
        }
    }

    #endregion

    #region Private — Discovery

    internal IList<ManifestModuleInfo> ReadLocalManifests()
    {
        var discoveryPath = _options.DiscoveryPath;
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();
        logger.LogInformation("Loading modules");

        if (!discoveryPath.EndsWith(Path.DirectorySeparatorChar))
        {
            discoveryPath += Path.DirectorySeparatorChar;
        }

        var result = new List<ManifestModuleInfo>();

        if (!Directory.Exists(discoveryPath))
        {
            logger.LogWarning("Discovery path not found: {DiscoveryPath}", discoveryPath);
            return result;
        }

        foreach (var manifestFile in Directory.EnumerateFiles(discoveryPath, "module.manifest", SearchOption.AllDirectories))
        {
            var relativePath = manifestFile.Substring(discoveryPath.Length);
            if (relativePath.Contains("artifacts"))
            {
                continue;
            }

            var moduleInfo = ReadManifest(manifestFile);
            if (moduleInfo != null)
            {
                result.Add(moduleInfo);
            }
        }

        logger.LogDebug("Found {ModuleCount} module manifests in {DiscoveryPath}", result.Count, discoveryPath);
        return result;
    }

    internal ManifestModuleInfo ReadManifest(string manifestFilePath)
    {
        try
        {
            var manifest = ManifestReader.Read(manifestFilePath);
            var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
            moduleInfo.LoadFromManifest(manifest);
            moduleInfo.FullPhysicalPath = Path.GetDirectoryName(manifestFilePath);

            if (string.IsNullOrEmpty(moduleInfo.AssemblyFile))
            {
                moduleInfo.State = ModuleState.Initialized;
            }

            moduleInfo.IsInstalled = true;
            return moduleInfo;
        }
        catch (Exception ex)
        {
            var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();
            logger.LogError(ex, "Error reading manifest {ManifestPath}", manifestFilePath);
            return null;
        }
    }

    #endregion

    #region Private — Sorting

    internal IList<ManifestModuleInfo> SortModulesByDependency(IEnumerable<ManifestModuleInfo> modules)
    {
        var moduleList = modules as IList<ManifestModuleInfo> ?? modules.ToList();

        if (moduleList.Count == 0)
        {
            return [];
        }

        var ignoreCase = StringComparer.OrdinalIgnoreCase;
        var boostedIds = new HashSet<string>(_options.ModuleSequenceBoost ?? [], ignoreCase);

        var remaining = moduleList
            .GroupBy(x => x.Id, ignoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(x => x.IsInstalled).ThenByDescending(x => x.Version).First(),
                ignoreCase);

        var allIds = remaining.Keys.ToHashSet(ignoreCase);
        var resultIds = new HashSet<string>(ignoreCase);
        var result = new List<ManifestModuleInfo>(remaining.Count);

        while (remaining.Count > 0)
        {
            var layer = remaining.Values
                .Where(x => x.Dependencies.All(d => resultIds.Contains(d.Id) || !allIds.Contains(d.Id)))
                .OrderBy(x => boostedIds.Contains(x.Id) ? 0 : 1)
                .ThenBy(x => x.Id, ignoreCase)
                .ToList();

            if (layer.Count == 0)
            {
                foreach (var module in remaining.Values.OrderBy(x => x.Id))
                {
                    module.Errors.Add("This module either has a loop in the dependencies or it depends on a module with such a loop.");
                    result.Add(module);
                }

                break;
            }

            foreach (var module in layer)
            {
                result.Add(module);
                resultIds.Add(module.Id);
                remaining.Remove(module.Id);
            }
        }

        return result;
    }

    #endregion

    #region Private — Copying

    private void CopyModulesInternal(IList<ManifestModuleInfo> modules, Architecture environmentArchitecture)
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();
        var probingPath = _options.ProbingPath;
        var refreshProbing = _options.RefreshProbingFolderOnStart;
        var fileCopyPolicy = new FileCopyPolicy(new FileMetadataProvider(), Options.Create(_options));

        var markerPath = Path.Combine(probingPath, _rebuildMarkerFileName);
        if (File.Exists(markerPath))
        {
            logger.LogInformation("Rebuild marker found — clearing probing folder for clean rebuild");

            foreach (var entry in new DirectoryInfo(probingPath).EnumerateFileSystemInfos())
            {
                if (entry is DirectoryInfo directory)
                {
                    directory.Delete(recursive: true);
                }
                else
                {
                    entry.Delete();
                }
            }

            refreshProbing = true;
        }

        if (!Directory.Exists(probingPath))
        {
            Directory.CreateDirectory(probingPath);
            refreshProbing = true;
        }

        if (refreshProbing)
        {
            logger.LogDebug("Copying modules from {From} to {To}, count: {Count}, architecture: {Architecture}",
                _options.DiscoveryPath, probingPath, modules.Count, environmentArchitecture);

            foreach (var module in modules)
            {
                if (module.FullPhysicalPath != null)
                {
                    CopyModule(module.FullPhysicalPath, probingPath, environmentArchitecture, fileCopyPolicy, logger);
                }
            }

            logger.LogDebug("Module copying completed");
        }
    }

    private static void CopyModule(string sourceDirectoryPath, string targetDirectoryPath, Architecture environmentArchitecture, FileCopyPolicy fileCopyPolicy, ILogger logger)
    {
        var sourceBinPath = Path.Combine(sourceDirectoryPath, "bin");
        if (!Directory.Exists(sourceBinPath))
        {
            logger.LogDebug("No bin directory for module at {Path}, skipping", sourceDirectoryPath);
            return;
        }

        logger.LogDebug("Copying assemblies from {SourceBinPath}", sourceBinPath);

        foreach (var sourceFilePath in Directory.EnumerateFiles(sourceBinPath, "*", SearchOption.AllDirectories))
        {
            var sourceRelativePath = Path.GetRelativePath(sourceBinPath, sourceFilePath);
            var targetRelativePath = fileCopyPolicy.GetTargetRelativePath(sourceRelativePath);

            if (targetRelativePath == null)
            {
                continue;
            }

            var targetFilePath = Path.Combine(targetDirectoryPath, targetRelativePath);
            CopyFile(sourceFilePath, targetFilePath, environmentArchitecture, fileCopyPolicy, logger);
        }
    }

    private static void CopyFile(string sourceFilePath, string targetFilePath, Architecture environmentArchitecture, FileCopyPolicy fileCopyPolicy, ILogger logger)
    {
        if (!fileCopyPolicy.IsCopyRequired(sourceFilePath, targetFilePath, environmentArchitecture, out var result))
        {
            return;
        }

        logger.LogDebug("Updating {TargetFile}: NewVersion={NewVersion}, NewArchitecture={NewArchitecture}, NewDate={NewDate}",
            Path.GetFileName(targetFilePath), result.NewVersion, result.NewArchitecture, result.NewDate);

        var targetDirectoryPath = Path.GetDirectoryName(targetFilePath);
        if (targetDirectoryPath != null && !Directory.Exists(targetDirectoryPath))
        {
            Directory.CreateDirectory(targetDirectoryPath);
        }

        try
        {
            File.Copy(sourceFilePath, targetFilePath, true);
        }
        catch (IOException)
        {
            if (result.NewDate)
            {
                logger.LogWarning("File '{TargetFile}' was not updated by '{SourceFile}' of the same version but later modified date, because probably it was used by another process",
                    targetFilePath, sourceFilePath);
            }
            else
            {
                throw;
            }
        }
    }

    #endregion

    #region Private — Loading

    private void LoadModulesInternal(IList<ManifestModuleInfo> modules, bool isDevelopmentEnvironment)
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        if (!_nativeResolverRegistered)
        {
            AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNativeLibrary;
            _nativeResolverRegistered = true;
        }

        foreach (var module in modules)
        {
            LoadModule(module, isDevelopmentEnvironment, logger);
        }
    }

    private void LoadModule(ManifestModuleInfo module, bool isDevelopmentEnvironment, ILogger logger)
    {
        if (string.IsNullOrEmpty(module.AssemblyFile))
        {
            return;
        }

        var moduleAssemblyPath = Path.GetFullPath(Path.Combine(_options.ProbingPath, module.AssemblyFile));

        if (!File.Exists(moduleAssemblyPath))
        {
            logger.LogWarning("Assembly file not found for module {ModuleId}: {ModuleRef}", module.Id, moduleAssemblyPath);
            module.Errors.Add($"Assembly file not found: {moduleAssemblyPath}");
            return;
        }

        module.Ref = GetFileAbsoluteUri(moduleAssemblyPath).ToString();

        try
        {
            logger.LogDebug("Loading module {ModuleId} {ModuleVersion}", module.Id, module.Version);
            var assembly = LoadAssemblyWithReferences(moduleAssemblyPath, isDevelopmentEnvironment, logger);
            module.Assembly = assembly;
            module.State = ModuleState.ReadyForInitialization;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load {ModuleId}", module.Id);
            module.Errors.Add($"Failed to load assembly: {ex.Message}");
        }
    }

    private Assembly LoadAssemblyWithReferences(string assemblyPath, bool isDevelopmentEnvironment, ILogger logger)
    {
        var loadContext = BuildLoadContext(assemblyPath, isDevelopmentEnvironment);
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
                RegisterNativeLibrary(dependency, loadContext, logger);
                continue;
            }

            try
            {
                var loadedAssembly = _loadedAssemblies.GetOrAdd(dependency.Name, _ => LoadAssemblyInternal(dependency, loadContext, logger));
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

    private static ManagedAssemblyLoadContext BuildLoadContext(string assemblyPath, bool isDevelopmentEnvironment)
    {
        var runtimeConfigFilePath = Path.ChangeExtension(assemblyPath, ".runtimeconfig.json");

        return new ManagedAssemblyLoadContext
        {
            PlatformPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            BasePath = Path.GetDirectoryName(assemblyPath),
            AdditionalProbingPaths = runtimeConfigFilePath.TryGetAdditionalProbingPathFromRuntimeConfig(isDevelopmentEnvironment, out _),
        };
    }

    private static Assembly LoadAssemblyInternal(Library managedLibrary, ManagedAssemblyLoadContext loadContext, ILogger logger)
    {
        if (Tpa.ContainsAssembly(managedLibrary.FileName))
        {
            logger.LogTrace("Loading managed library: {Name}", managedLibrary.Name);
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(managedLibrary.Name));
        }

        if (TryGetFullPath(managedLibrary, loadContext, out var path))
        {
            logger.LogTrace("Loading managed library: {Path}", path);
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        }

        return null;
    }

    private void RegisterNativeLibrary(Library library, ManagedAssemblyLoadContext loadContext, ILogger logger)
    {
        if (!TryGetFullPath(library, loadContext, out var path))
        {
            return;
        }

        var nativePaths = _nativePathsByName.GetOrAdd(library.Name, _ => []);

        if (!nativePaths.Contains(path))
        {
            nativePaths.Add(path);
            logger.LogTrace("Registered native library: {LibraryPath}", path);
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

    private IntPtr ResolveNativeLibrary(Assembly assembly, string name)
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();
        logger.LogTrace("Resolving native library '{LibraryName}' for assembly '{AssemblyName}'", name, assembly.FullName);

        if (!TryGetNativeLibraryPaths(name, out var nativePaths))
        {
            logger.LogTrace("Not found");
            return IntPtr.Zero;
        }

        foreach (var nativePath in nativePaths)
        {
            logger.LogTrace("Loading '{LibraryPath}'", nativePath);

            if (NativeLibrary.TryLoad(nativePath, out var handle))
            {
                logger.LogTrace("Succeeded");
                _nativePathsByName.AddOrUpdate(name, [nativePath], (_, oldList) => oldList.Count == 1 ? oldList : [nativePath]);
                return handle;
            }

            logger.LogTrace("Failed");
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

    #endregion

    #region Private — Validation

    internal void ValidateModulesInternal(IList<ManifestModuleInfo> modules, SemanticVersion platformVersion)
    {
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        foreach (var module in modules)
        {
            if (!module.PlatformVersion.IsCompatibleWith(platformVersion) ||
                !module.PlatformVersion.IsCompatibleWithBySemVer(platformVersion))
            {
                module.Errors.Add($"Module requires platform version {module.PlatformVersion}, which is incompatible with current version {platformVersion}");
            }

            if (module.Incompatibilities?.Count > 0)
            {
                var installedIncompatibilities = modules
                    .Where(x => module.Incompatibilities.Any(incompatible =>
                        incompatible.Id.EqualsIgnoreCase(x.Id) &&
                        incompatible.Version.IsCompatibleWith(x.Version)))
                    .ToList();

                if (installedIncompatibilities.Count > 0)
                {
                    module.Errors.Add($"{module} is incompatible with installed {string.Join(", ", installedIncompatibilities)}. You should uninstall these modules first.");
                }
            }

            if (module.Dependencies?.Count > 0)
            {
                foreach (var dependency in module.Dependencies)
                {
                    if (dependency.Optional)
                    {
                        continue;
                    }

                    var installedDependency = modules.FirstOrDefault(x => x.Id.EqualsIgnoreCase(dependency.Id));
                    if (installedDependency == null)
                    {
                        module.Errors.Add($"Module dependency {dependency.Id} {dependency.Version} is not installed");
                    }
                    else if (!dependency.Version.IsCompatibleWithBySemVer(installedDependency.Version))
                    {
                        module.Errors.Add($"Module dependency {dependency.Id} {dependency.Version} is incompatible with installed {installedDependency.Version}");
                    }
                }
            }
        }

        // Cascade errors to dependents
        var failedIds = modules
            .Where(x => x.Errors.Count > 0)
            .Select(x => x.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var module in modules.Where(x => x.Dependencies?.Count > 0 && x.Errors.Count == 0))
            {
                var failedDependency = module.Dependencies.FirstOrDefault(x =>
                    !x.Optional &&
                    failedIds.Contains(x.Id));

                if (failedDependency != null)
                {
                    module.Errors.Add($"Module skipped because its dependency '{failedDependency.Id}' has errors");
                    failedIds.Add(module.Id);
                    changed = true;
                }
            }
        }

        var errorCount = modules.Count(x => x.Errors.Count > 0);
        if (errorCount > 0)
        {
            foreach (var module in modules.Where(x => x.Errors.Count > 0))
            {
                logger.LogWarning("Module {ModuleId} has errors: {Errors}", module.Id, string.Join("; ", module.Errors));
            }

            logger.LogWarning("{ErrorCount} modules failed validation (including cascaded dependents)", errorCount);
        }
    }

    #endregion

    #region Private — Registry

    public void RegisterModules(IList<ManifestModuleInfo> modules)
    {
        _modules = modules;
        _modulesById = modules.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
    }

    #endregion

    #region Private — Startup Discovery

    private void DiscoverStartupsInternal(IList<ManifestModuleInfo> modules)
    {
        var startups = new List<IPlatformStartup>();
        var logger = _loggerFactory.CreateLogger<ModuleBootstrapper>();

        foreach (var module in modules)
        {
            if (string.IsNullOrEmpty(module.StartupType) || module.Assembly == null)
            {
                continue;
            }

            try
            {
                var startupType = module.Assembly.GetType(module.StartupType) ??
                                  FindTypeByName(module.Assembly, module.StartupType);

                if (startupType == null)
                {
                    logger.LogWarning("Startup type '{StartupType}' not found in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (!typeof(IPlatformStartup).IsAssignableFrom(startupType))
                {
                    logger.LogWarning("Type '{StartupType}' does not implement IPlatformStartup in {ModuleId}", module.StartupType, module.Id);
                    continue;
                }

                if (Activator.CreateInstance(startupType) is IPlatformStartup instance)
                {
                    startups.Add(instance);
                    logger.LogInformation("Discovered {StartupTypeName} from {ModuleId}", startupType.Name, module.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading startup type from {ModuleId}", module.Id);
            }
        }

        _startups = startups;
        logger.LogDebug("Platform startup extensions: {StartupCount}", startups.Count);
    }

    private static Type FindTypeByName(Assembly assembly, string typeName)
    {
        return assembly.GetTypes()
            .FirstOrDefault(x =>
                x.FullName == typeName ||
                x.Name == typeName ||
                x.AssemblyQualifiedName?.StartsWith(typeName, StringComparison.Ordinal) == true);
    }

    #endregion

    #region Private — Module Instance Creation

    internal static IModule CreateModuleInstance(ManifestModuleInfo moduleInfo)
    {
        if (moduleInfo.Assembly == null)
        {
            throw new ModuleInitializeException($"Assembly not loaded for module {moduleInfo.Id}");
        }

        var moduleTypes = moduleInfo.Assembly.GetTypes()
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsAbstract)
            .ToArray();

        if (moduleTypes.Length == 0)
        {
            throw new ModuleInitializeException($"No IModule implementation found in assembly {moduleInfo.Assembly.FullName}");
        }

        Type moduleType;

        if (moduleTypes.Length == 1)
        {
            moduleType = moduleTypes[0];
        }
        else
        {
            moduleType = moduleTypes.FirstOrDefault(x => x.AssemblyQualifiedName?.StartsWith(moduleInfo.ModuleType) == true);
            if (moduleType == null)
            {
                throw new ModuleInitializeException($"Cannot resolve IModule type '{moduleInfo.ModuleType}' from assembly {moduleInfo.Assembly.FullName}");
            }
        }

        if (Activator.CreateInstance(moduleType) is not IModule instance)
        {
            throw new ModuleInitializeException($"Failed to create instance of {moduleType.FullName}");
        }

        instance.ModuleInfo = moduleInfo;

        return instance;
    }

    #endregion

    #region Private — External Manifest Helpers

    private static ManifestModuleInfo GetLatestCompatibleVersion(
        ExternalModuleManifest manifest,
        SemanticVersion platformVersion,
        bool prerelease)
    {
        var latestVersion = manifest.Versions
            .OrderByDescending(x => x.SemanticVersion)
            .FirstOrDefault(x =>
                x.PlatformSemanticVersion.Major == platformVersion.Major &&
                x.VersionTag.IsNullOrEmpty() != prerelease);

        if (latestVersion == null)
        {
            return null;
        }

        var result = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        result.LoadFromExternalManifest(manifest, latestVersion);

        return result;
    }

    #endregion
}
