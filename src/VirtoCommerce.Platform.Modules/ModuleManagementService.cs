using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Transactions;
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
    IOptions<ExternalModuleCatalogOptions> externalOptions) : IModuleManagementService
{
    private const string PackageFileExtension = ".zip";

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly LocalStorageModuleCatalogOptions _localOptions = localOptions.Value;
    private readonly ExternalModuleCatalogOptions _externalOptions = externalOptions.Value;
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

    public IList<ManifestModuleInfo> GetDependencies(IList<ManifestModuleInfo> selectedModules)
    {
        EnsureInitialized();
        return ModuleDiscovery.GetDependencies(selectedModules, _mergedModules);
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

    public void InstallModules(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
    {
        EnsureInitialized();

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

    public void UninstallModules(IList<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
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

    public IList<ManifestModuleInfo> GetDependents(IList<ManifestModuleInfo> modules)
    {
        EnsureInitialized();

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
                retVal.AddRange(GetDependents(dependingModules));
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
