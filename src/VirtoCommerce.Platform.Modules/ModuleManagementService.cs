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
/// Uses static classes (ModuleDiscovery, ModulePackageInstaller, ModuleRegistry) for core logic.
/// </summary>
public class ModuleManagementService(
    IHttpClientFactory httpClientFactory,
    IOptions<LocalStorageModuleCatalogOptions> localOptions,
    IOptions<ExternalModuleCatalogOptions> externalOptions,
    ILogger<ModuleManagementService> logger) : IModuleManagementService
{
    private const string PackageFileExtension = ".zip";

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly LocalStorageModuleCatalogOptions _localOptions = localOptions.Value;
    private readonly ExternalModuleCatalogOptions _externalOptions = externalOptions.Value;
    private readonly ILogger<ModuleManagementService> _logger = logger;
    private readonly Lock _lockObject = new();

    private List<ManifestModuleInfo> _mergedModules;

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
        return ModuleDiscovery.GetDependencies(modules, _mergedModules);
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
        ModuleDiscovery.GetDependencies([module], _mergedModules);
        _mergedModules.Add(module);
        return module;
    }

    public void InstallModules(IList<ModuleInstallRequest> modules, IProgress<ProgressMessage> progress)
    {
        EnsureInitialized();

        var resolved = new List<ManifestModuleInfo>();
        foreach (var request in modules)
        {
            var module = ResolveModuleForInstall(request.Id, request.Version);
            if (module != null)
            {
                resolved.Add(module);
            }
            else
            {
                Report(progress, ProgressMessageLevel.Error, "Module '{0}' version '{1}' not found", request.Id, request.Version ?? "latest");
            }
        }

        if (resolved.Count > 0)
        {
            InstallModulesInternal(resolved, progress);
        }
    }

    public void UninstallModules(IList<string> moduleIds, IProgress<ProgressMessage> progress)
    {
        EnsureInitialized();

        var resolved = new List<ManifestModuleInfo>();
        foreach (var id in moduleIds)
        {
            var module = _mergedModules.FirstOrDefault(x => x.Id.EqualsIgnoreCase(id) && x.IsInstalled);
            if (module != null)
            {
                resolved.Add(module);
            }
            else
            {
                Report(progress, ProgressMessageLevel.Error, "Installed module '{0}' not found", id);
            }
        }

        if (resolved.Count > 0)
        {
            UninstallModulesInternal(resolved, progress);
        }
    }

    private List<ManifestModuleInfo> ResolveModulesById(IList<string> moduleIds)
    {
        var result = new List<ManifestModuleInfo>();

        foreach (var id in moduleIds)
        {
            // Prefer installed version; fall back to latest available
            var module = _mergedModules.FirstOrDefault(x => x.Id.EqualsIgnoreCase(id) && x.IsInstalled)
                ?? _mergedModules.Where(x => x.Id.EqualsIgnoreCase(id)).OrderByDescending(x => x.Version).FirstOrDefault();

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
                .Where(x => x.Id.EqualsIgnoreCase(moduleId) && !x.IsInstalled)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault();
        }

        // Specific version — find existing or register custom
        var identity = new ModuleIdentity(moduleId, SemanticVersion.Parse(version));
        var existing = _mergedModules.FirstOrDefault(x => x.Identity.Equals(identity));
        if (existing != null)
        {
            return existing;
        }

        // Register custom version
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        if (packageUrl == null)
        {
            return null;
        }

        var sourceModule = _mergedModules
            .Where(x => x.Id.EqualsIgnoreCase(moduleId))
            .OrderByDescending(x => x.Version)
            .FirstOrDefault();

        if (sourceModule == null)
        {
            return null;
        }

        var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        moduleInfo.LoadFromExternalManifest(
            new ExternalModuleManifest { Id = sourceModule.Id, Title = sourceModule.Title, Description = sourceModule.Description },
            new ExternalModuleManifestVersion { Version = version, PackageUrl = packageUrl, PlatformVersion = PlatformVersion.CurrentVersion.ToString() });

        AddUploadedModule(moduleInfo);
        moduleInfo.Ref = packageUrl;

        return moduleInfo;
    }

    private void InstallModulesInternal(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        var isValid = true;

        foreach (var module in modules.Where(x => !x.IsInstalled))
        {
            var allInstalledModules = ModuleRegistry.GetAllModules().ToList();
            var errors = ModuleDiscovery.ValidateInstall(module, allInstalledModules, PlatformVersion.CurrentVersion);
            foreach (var error in errors)
            {
                Report(progress, ProgressMessageLevel.Error, error);
                isValid = false;
            }

            isValid &= !HasMissedDependencies(module, modules, progress);
        }

        if (!isValid)
        {
            return;
        }

        var installedModulesIds = ModuleRegistry.GetAllModules().Select(x => x.Id).ToArray();
        var updatableModules = modules.Where(x => installedModulesIds.Contains(x.Id));
        var installableModules = modules.Except(updatableModules);
        var changedModulesLog = new List<ManifestModuleInfo>();

        using var scope = new TransactionScope(TransactionScopeOption.Required, TransactionManager.MaximumTimeout);
        try
        {
            foreach (var installableModule in installableModules)
            {
                Report(progress, ProgressMessageLevel.Info, "Installing '{0}'", installableModule);
                InnerInstall(installableModule, progress);
                changedModulesLog.Add(installableModule);
                installableModule.IsInstalled = true;
            }

            foreach (var newModule in updatableModules)
            {
                var existModule = ModuleRegistry.GetModule(newModule.Id);
                var dstModuleDir = Path.Combine(_localOptions.DiscoveryPath, existModule.Id);
                ModulePackageInstaller.Uninstall(dstModuleDir);
                Report(progress, ProgressMessageLevel.Info, "Updating '{0}' -> '{1}'", existModule, newModule);
                InnerInstall(newModule, progress);
                existModule.IsInstalled = false;
                newModule.IsInstalled = true;
                changedModulesLog.AddRange([existModule, newModule]);
            }

            scope.Complete();
            InvalidateProbingFolder();
        }
        catch (Exception ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.ToString());
            Report(progress, ProgressMessageLevel.Error, "Rollback all changes...");
            foreach (var changedModule in changedModulesLog)
            {
                changedModule.IsInstalled = !changedModule.IsInstalled;
            }
        }
    }

    private void UninstallModulesInternal(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        var isValid = true;
        var modulesList = modules.ToList();
        var uninstallIds = modulesList.Select(m => m.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var module in modulesList)
        {
            var installedModules = ModuleRegistry.GetAllModules().ToList();
            var errors = ModuleDiscovery.ValidateUninstall(module.Id, installedModules, uninstallIds);
            foreach (var error in errors)
            {
                Report(progress, ProgressMessageLevel.Error, error);
                isValid = false;
            }
        }

        if (!isValid)
        {
            return;
        }

        var changedModulesLog = new List<ManifestModuleInfo>();
        using var scope = new TransactionScope();
        try
        {
            foreach (var uninstallingModule in modules)
            {
                Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'", uninstallingModule);
                if (uninstallingModule.ModuleInstance != null)
                {
                    Report(progress, ProgressMessageLevel.Info, "Executing module.Uninstall() method");
                    uninstallingModule.ModuleInstance.Uninstall();
                }

                var moduleDir = Path.Combine(_localOptions.DiscoveryPath, uninstallingModule.Id);
                Report(progress, ProgressMessageLevel.Info, "Deleting module {0} folder", moduleDir);
                ModulePackageInstaller.Uninstall(moduleDir);
                Report(progress, ProgressMessageLevel.Info, "'{0}' uninstalled successfully.", uninstallingModule);
                uninstallingModule.IsInstalled = false;
                changedModulesLog.Add(uninstallingModule);
            }

            scope.Complete();
            InvalidateProbingFolder();
        }
        catch (Exception ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.ToString());
            Report(progress, ProgressMessageLevel.Error, "Rollback all changes...");
            foreach (var changedModule in changedModulesLog)
            {
                changedModule.IsInstalled = !changedModule.IsInstalled;
            }
        }
    }

    public IList<ManifestModuleInfo> GetDependents(IList<string> moduleIds)
    {
        EnsureInitialized();

        var modules = _mergedModules
            .Where(x => x.IsInstalled && moduleIds.Any(id => x.Id.EqualsIgnoreCase(id)))
            .ToList();

        return GetDependentsRecursive(modules);
    }

    private List<ManifestModuleInfo> GetDependentsRecursive(IList<ManifestModuleInfo> modules)
    {
        var retVal = new List<ManifestModuleInfo>();
        foreach (var module in modules)
        {
            retVal.Add(module);
            var dependingModules = _mergedModules
                .Where(x => x.IsInstalled)
                .Where(x => x.DependsOn.ContainsIgnoreCase(module.Id))
                .ToList();

            if (dependingModules.Count > 0)
            {
                retVal.AddRange(GetDependentsRecursive(dependingModules));
            }
        }

        return retVal;
    }

    public IList<ManifestModuleInfo> GetAutoInstallModules(string[] moduleBundles)
    {
        EnsureInitialized();

        var modules = new List<ManifestModuleInfo>();
        var moduleVersionGroups = _mergedModules
            .Where(x => x.Groups.Intersect(moduleBundles, StringComparer.OrdinalIgnoreCase).Any())
            .GroupBy(x => x.Id);

        foreach (var moduleVersionGroup in moduleVersionGroups)
        {
            var alreadyInstalledModule = _mergedModules.FirstOrDefault(x => x.IsInstalled && x.Id.EqualsIgnoreCase(moduleVersionGroup.Key));
            if (alreadyInstalledModule == null)
            {
                var latestVersion = moduleVersionGroup.OrderBy(x => x.Version).LastOrDefault();
                if (latestVersion != null)
                {
                    modules.Add(latestVersion);
                }
            }
        }

        return ModuleDiscovery.GetDependencies(modules, _mergedModules)
            .Where(x => !x.IsInstalled)
            .ToList();
    }

    public async Task<bool> ValidateModuleVersionAsync(string moduleId, string version)
    {
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        return packageUrl != null && await CheckPackageExistsAsync(packageUrl);
    }

    public async Task<ManifestModuleInfo> RegisterCustomModuleVersionAsync(string moduleId, string version)
    {
        var packageUrl = BuildCustomPackageUrl(moduleId, version);
        if (packageUrl == null || !await CheckPackageExistsAsync(packageUrl))
        {
            return null;
        }

        // Check if already registered
        var identity = new ModuleIdentity(moduleId, SemanticVersion.Parse(version));
        var existing = _mergedModules.FirstOrDefault(x => x.Identity.Equals(identity));
        if (existing != null)
        {
            return existing;
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

        AddUploadedModule(moduleInfo);
        moduleInfo.Ref = packageUrl;

        return moduleInfo;
    }

    private string BuildCustomPackageUrl(string moduleId, string version)
    {
        EnsureInitialized();

        var module = _mergedModules
            .Where(x => x.Id.EqualsIgnoreCase(moduleId) && !string.IsNullOrEmpty(x.Ref))
            .OrderByDescending(x => x.Version)
            .FirstOrDefault();

        if (module == null)
        {
            return null;
        }

        var currentVersion = module.Version.ToString();
        var customPackageUrl = module.Ref.Replace(currentVersion, version);

        return Uri.IsWellFormedUriString(customPackageUrl, UriKind.Absolute) ? customPackageUrl : null;
    }

    private async Task<bool> CheckPackageExistsAsync(string packageUrl)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Virto Commerce Manager");

            using var request = new HttpRequestMessage(HttpMethod.Head, packageUrl);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "HEAD check failed for {PackageUrl}", packageUrl);
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

            var httpClient = _httpClientFactory.CreateClient();
            var externalModules = ModulePackageInstaller.LoadExternalModules(httpClient, _externalOptions);
            var installedModules = ModuleRegistry.GetAllModules().ToList();
            _mergedModules = ModuleDiscovery.MergeWithInstalled(externalModules, installedModules);
        }
    }

    private bool HasMissedDependencies(ManifestModuleInfo module, IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        var result = true;

        try
        {
            result = ModuleDiscovery.GetDependencies([module], _mergedModules)
                .Where(x => !x.IsInstalled)
                .Except(modules)
                .Any();
        }
        catch (MissedModuleException ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.Message);
        }

        return result;
    }

    private void InvalidateProbingFolder()
    {
        ModuleCopier.InvalidateProbingFolder(_localOptions.ProbingPath);
    }

    private void InnerInstall(ManifestModuleInfo module, IProgress<ProgressMessage> progress)
    {
        var dstModuleDir = Path.Combine(_localOptions.DiscoveryPath, module.Id);
        var moduleZipPath = Path.Combine(dstModuleDir, string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", module.Id, module.Version, PackageFileExtension));

        if (!Directory.Exists(dstModuleDir))
        {
            Directory.CreateDirectory(dstModuleDir);
        }

        if (Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
        {
            Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
            var httpClient = _httpClientFactory.CreateClient();
            ModulePackageInstaller.Download(new Uri(module.Ref), moduleZipPath, httpClient, _externalOptions);
        }
        else if (File.Exists(module.Ref))
        {
            moduleZipPath = module.Ref;
        }

        if (File.Exists(moduleZipPath))
        {
            ModulePackageInstaller.Install(moduleZipPath, dstModuleDir);
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
