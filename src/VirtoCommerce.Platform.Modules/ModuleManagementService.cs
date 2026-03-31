using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Singleton service for module catalog management.
/// Caches the merged module list (external + installed) across requests.
/// Uses static classes (ModuleBootstrapper, ModulePackageInstaller) for core logic.
/// </summary>
public class ModuleManagementService(
    IHttpClientFactory httpClientFactory,
    IOptions<LocalStorageModuleCatalogOptions> localOptions,
    IOptions<ExternalModuleCatalogOptions> externalOptions,
    ILogger<ModuleManagementService> logger)
    : IModuleManagementService
{
    private readonly LocalStorageModuleCatalogOptions _localOptions = localOptions.Value;
    private readonly ExternalModuleCatalogOptions _externalOptions = externalOptions.Value;
    private readonly Lock _lockObject = new();

    private IList<ManifestModuleInfo> _mergedModules;

    public IList<ManifestModuleInfo> GetModules()
    {
        EnsureInitialized();
        return _mergedModules;
    }

    public void ReloadModules()
    {
        lock (_lockObject)
        {
            _mergedModules = null;
        }

        EnsureInitialized();
    }

    public IList<ManifestModuleInfo> GetDependencies(IList<string> moduleIds)
    {
        EnsureInitialized();
        var modules = ResolveModulesById(moduleIds);
        return ModuleBootstrapper.Instance.GetDependencies(modules, _mergedModules);
    }

    public ManifestModuleInfo AddUploadedModule(ManifestModuleInfo module)
    {
        EnsureInitialized();

        var existingModule = _mergedModules.FirstOrDefault(x => x.Equals(module));
        if (existingModule != null)
        {
            return existingModule;
        }

        // Force dependency validation
        ModuleBootstrapper.Instance.GetDependencies([module], _mergedModules);
        _mergedModules.Add(module);

        return module;
    }

    public void InstallModules(IList<ModuleInstallRequest> requests, IProgress<ProgressMessage> progress)
    {
        EnsureInitialized();

        var modulesToInstall = new List<ManifestModuleInfo>();

        foreach (var request in requests)
        {
            var module = ResolveModuleForInstall(request.Id, request.Version);
            if (module != null)
            {
                modulesToInstall.Add(module);
            }
            else
            {
                Report(progress, ProgressMessageLevel.Error, "Module '{0}' version '{1}' not found", request.Id, request.Version ?? "latest");
            }
        }

        if (modulesToInstall.Count > 0)
        {
            InstallModulesInternal(modulesToInstall, progress);
        }
    }

    public void UninstallModules(IList<string> moduleIds, IProgress<ProgressMessage> progress)
    {
        EnsureInitialized();

        var modulesToUninstall = new List<ManifestModuleInfo>();

        foreach (var id in moduleIds)
        {
            var module = _mergedModules.FirstOrDefault(x => x.IsInstalled && x.Id.EqualsIgnoreCase(id));
            if (module != null)
            {
                modulesToUninstall.Add(module);
            }
            else
            {
                Report(progress, ProgressMessageLevel.Error, "Installed module '{0}' not found", id);
            }
        }

        if (modulesToUninstall.Count > 0)
        {
            UninstallModulesInternal(modulesToUninstall, progress);
        }
    }

    private List<ManifestModuleInfo> ResolveModulesById(IList<string> moduleIds)
    {
        var result = new List<ManifestModuleInfo>();

        foreach (var id in moduleIds)
        {
            // Prefer installed version; fall back to latest available
            var module = _mergedModules.FirstOrDefault(x => x.IsInstalled && x.Id.EqualsIgnoreCase(id)) ??
                         _mergedModules.Where(x => x.Id.EqualsIgnoreCase(id)).OrderByDescending(x => x.Version).FirstOrDefault();

            if (module != null)
            {
                result.Add(module);
            }
        }

        return result;
    }

    private ManifestModuleInfo ResolveModuleForInstall(string moduleId, string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            // Latest non-installed version
            return _mergedModules
                .Where(x => !x.IsInstalled && x.Id.EqualsIgnoreCase(moduleId))
                .OrderByDescending(x => x.Version)
                .FirstOrDefault();
        }

        // Specific version — find existing or register custom
        var identity = new ModuleIdentity(moduleId, SemanticVersion.Parse(version));
        var existingModule = _mergedModules.FirstOrDefault(x => x.Identity.Equals(identity));
        if (existingModule != null)
        {
            return existingModule;
        }

        // Register custom version
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        if (packageUrl is null)
        {
            return null;
        }

        var sourceModule = _mergedModules
            .Where(x => x.Id.EqualsIgnoreCase(moduleId))
            .OrderByDescending(x => x.Version)
            .FirstOrDefault();

        if (sourceModule is null)
        {
            return null;
        }

        var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();

        module.LoadFromExternalManifest(
            new ExternalModuleManifest { Id = sourceModule.Id, Title = sourceModule.Title, Description = sourceModule.Description },
            new ExternalModuleManifestVersion { Version = version, PackageUrl = packageUrl, PlatformVersion = PlatformVersion.CurrentVersion.ToString() });

        module.Ref = packageUrl;

        AddUploadedModule(module);

        return module;
    }

    private void InstallModulesInternal(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        var failed = false;
        var installedModules = ModuleBootstrapper.Instance.GetModules();

        foreach (var module in modules.Where(x => !x.IsInstalled))
        {
            var errors = ModuleBootstrapper.Instance.ValidateInstall(module, installedModules, PlatformVersion.CurrentVersion);
            foreach (var error in errors)
            {
                Report(progress, ProgressMessageLevel.Error, error);
            }

            failed |= errors.Count > 0;
            failed |= HasMissingDependencies(module, modules, progress);
        }

        if (failed)
        {
            return;
        }

        var installedModuleIds = installedModules.Select(x => x.Id).ToArray();
        var modulesToUpdate = modules.Where(x => installedModuleIds.Contains(x.Id)).ToArray();
        var modulesToAdd = modules.Except(modulesToUpdate);

        var changedModules = new List<ManifestModuleInfo>();
        using var scope = new TransactionScope(TransactionScopeOption.Required, TransactionManager.MaximumTimeout);

        try
        {
            foreach (var module in modulesToAdd)
            {
                Report(progress, ProgressMessageLevel.Info, "Installing '{0}'", module);
                InstallModule(module, progress);
                changedModules.Add(module);
                module.IsInstalled = true;
            }

            foreach (var module in modulesToUpdate)
            {
                var existingModule = ModuleBootstrapper.Instance.GetModule(module.Id);
                var modulePath = Path.Combine(_localOptions.DiscoveryPath, existingModule.Id);
                ModulePackageInstaller.Uninstall(modulePath);
                Report(progress, ProgressMessageLevel.Info, "Updating '{0}' -> '{1}'", existingModule, module);
                InstallModule(module, progress);
                existingModule.IsInstalled = false;
                module.IsInstalled = true;
                changedModules.Add(existingModule);
                changedModules.Add(module);
            }

            scope.Complete();
            InvalidateProbingFolder();
        }
        catch (Exception ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.ToString());
            Report(progress, ProgressMessageLevel.Error, "Rollback all changes...");

            foreach (var changedModule in changedModules)
            {
                changedModule.IsInstalled = !changedModule.IsInstalled;
            }
        }
    }

    private void UninstallModulesInternal(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        var failed = false;
        var uninstallIds = modules.Select(x => x.Id).ToArray();
        var installedModules = ModuleBootstrapper.Instance.GetModules();

        foreach (var module in modules)
        {
            var errors = ModuleBootstrapper.Instance.ValidateUninstall(module.Id, installedModules, uninstallIds);
            foreach (var error in errors)
            {
                Report(progress, ProgressMessageLevel.Error, error);
            }

            failed |= errors.Count > 0;
        }

        if (failed)
        {
            return;
        }

        var changedModules = new List<ManifestModuleInfo>();
        using var scope = new TransactionScope();

        try
        {
            foreach (var module in modules)
            {
                Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'", module);

                if (module.ModuleInstance != null)
                {
                    Report(progress, ProgressMessageLevel.Info, "Executing module.Uninstall() method");
                    module.ModuleInstance.Uninstall();
                }

                var modulePath = Path.Combine(_localOptions.DiscoveryPath, module.Id);
                Report(progress, ProgressMessageLevel.Info, "Deleting module folder {0}", modulePath);
                ModulePackageInstaller.Uninstall(modulePath);
                Report(progress, ProgressMessageLevel.Info, "'{0}' uninstalled successfully.", module);
                module.IsInstalled = false;
                changedModules.Add(module);
            }

            scope.Complete();
            InvalidateProbingFolder();
        }
        catch (Exception ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.ToString());
            Report(progress, ProgressMessageLevel.Error, "Rollback all changes...");

            foreach (var changedModule in changedModules)
            {
                changedModule.IsInstalled = !changedModule.IsInstalled;
            }
        }
    }

    public IList<ManifestModuleInfo> GetDependents(IList<string> moduleIds)
    {
        EnsureInitialized();

        var result = new List<ManifestModuleInfo>();
        var queue = new Queue<ManifestModuleInfo>(_mergedModules.Where(x => x.IsInstalled && moduleIds.ContainsIgnoreCase(x.Id)));
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (queue.Count > 0)
        {
            var module = queue.Dequeue();

            if (!visited.Add(module.Id))
            {
                continue;
            }

            result.Add(module);

            foreach (var dependent in _mergedModules.Where(x => x.IsInstalled && x.DependsOn.ContainsIgnoreCase(module.Id)))
            {
                queue.Enqueue(dependent);
            }
        }

        return result;
    }

    public IList<ManifestModuleInfo> GetNotInstalledModulesFromGroups(IList<string> groups)
    {
        EnsureInitialized();

        var modules = new List<ManifestModuleInfo>();

        var moduleVersionGroups = _mergedModules
            .Where(x => x.Groups.Intersect(groups, StringComparer.OrdinalIgnoreCase).Any())
            .GroupBy(x => x.Id);

        foreach (var moduleVersionGroup in moduleVersionGroups)
        {
            var installedModule = _mergedModules.FirstOrDefault(x => x.IsInstalled && x.Id.EqualsIgnoreCase(moduleVersionGroup.Key));
            if (installedModule is null)
            {
                var latestVersion = moduleVersionGroup.OrderBy(x => x.Version).LastOrDefault();
                if (latestVersion != null)
                {
                    modules.Add(latestVersion);
                }
            }
        }

        return ModuleBootstrapper.Instance.GetDependencies(modules, _mergedModules)
            .Where(x => !x.IsInstalled)
            .ToList();
    }

    public async Task<bool> ValidateModuleVersionAsync(string moduleId, string version)
    {
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        return packageUrl != null && await PackageExistsAsync(packageUrl);
    }

    public async Task<ManifestModuleInfo> RegisterCustomModuleVersionAsync(string moduleId, string version)
    {
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        if (packageUrl is null || !await PackageExistsAsync(packageUrl))
        {
            return null;
        }

        // Check if already registered
        var moduleIdentity = new ModuleIdentity(moduleId, SemanticVersion.Parse(version));
        var existingModule = _mergedModules.FirstOrDefault(x => x.Identity.Equals(moduleIdentity));
        if (existingModule != null)
        {
            return existingModule;
        }

        // Find the source module to copy metadata from
        var sourceModule = _mergedModules
            .Where(x => x.Id.EqualsIgnoreCase(moduleId))
            .OrderByDescending(x => x.Version)
            .First();

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();

        moduleInfo.LoadFromExternalManifest(
            new ExternalModuleManifest { Id = sourceModule.Id, Title = sourceModule.Title, Description = sourceModule.Description },
            new ExternalModuleManifestVersion { Version = version, PackageUrl = packageUrl, PlatformVersion = PlatformVersion.CurrentVersion.ToString() });

        moduleInfo.Ref = packageUrl;

        AddUploadedModule(moduleInfo);

        return moduleInfo;
    }

    private string BuildCustomPackageUrl(string moduleId, string version)
    {
        EnsureInitialized();

        var module = _mergedModules
            .Where(x => !x.Ref.IsNullOrEmpty() && x.Id.EqualsIgnoreCase(moduleId))
            .OrderByDescending(x => x.Version)
            .FirstOrDefault();

        if (module is null)
        {
            return null;
        }

        var currentVersion = module.Version.ToString();
        var customPackageUrl = module.Ref.Replace(currentVersion, version);

        return Uri.IsWellFormedUriString(customPackageUrl, UriKind.Absolute) ? customPackageUrl : null;
    }

    private async Task<bool> PackageExistsAsync(string packageUrl)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Virto Commerce Manager");

            using var request = new HttpRequestMessage(HttpMethod.Head, packageUrl);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "HEAD check failed for {PackageUrl}", packageUrl);
            return false;
        }
    }

    private void EnsureInitialized()
    {
        if (_mergedModules != null)
        {
            return;
        }

        lock (_lockObject)
        {
            if (_mergedModules != null)
            {
                return;
            }

            var httpClient = httpClientFactory.CreateClient();
            var externalModules = ModulePackageInstaller.LoadExternalModules(httpClient, _externalOptions);
            var installedModules = ModuleBootstrapper.Instance.GetModules();
            _mergedModules = ModuleBootstrapper.Instance.MergeWithInstalled(externalModules, installedModules);
        }
    }

    private bool HasMissingDependencies(ManifestModuleInfo module, IList<ManifestModuleInfo> dependenciesToIgnore, IProgress<ProgressMessage> progress)
    {
        bool hasMissingDependencies;

        try
        {
            hasMissingDependencies = ModuleBootstrapper.Instance.GetDependencies([module], _mergedModules)
                .Where(x => !x.IsInstalled)
                .Except(dependenciesToIgnore)
                .Any();
        }
        catch (MissedModuleException ex)
        {
            hasMissingDependencies = true;
            Report(progress, ProgressMessageLevel.Error, ex.Message);
        }

        return hasMissingDependencies;
    }

    private static void InvalidateProbingFolder()
    {
        ModuleBootstrapper.Instance.InvalidateProbingFolder();
    }

    private void InstallModule(ManifestModuleInfo module, IProgress<ProgressMessage> progress)
    {
        var modulePath = Path.Combine(_localOptions.DiscoveryPath, module.Id);
        var moduleZipName = $"{module.Id}_{module.Version}.zip";
        var moduleZipPath = Path.Combine(modulePath, moduleZipName);

        if (!Directory.Exists(modulePath))
        {
            Directory.CreateDirectory(modulePath);
        }

        if (Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
        {
            Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
            var httpClient = httpClientFactory.CreateClient();
            ModulePackageInstaller.Download(new Uri(module.Ref), moduleZipPath, httpClient, _externalOptions);
        }
        else if (File.Exists(module.Ref))
        {
            moduleZipPath = module.Ref;
        }

        if (File.Exists(moduleZipPath))
        {
            ModulePackageInstaller.Install(moduleZipPath, modulePath);
        }
        else
        {
            throw new FileNotFoundException($"Module package not found: {moduleZipPath}");
        }

        Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0}'.", module);
    }

    private static void Report(IProgress<ProgressMessage> progress, ProgressMessageLevel level, string format, params object[] args)
    {
        if (progress != null)
        {
            var message = string.Format(CultureInfo.CurrentCulture, format, args);
            progress.Report(new ProgressMessage { Level = level, Message = message });
        }
    }
}
