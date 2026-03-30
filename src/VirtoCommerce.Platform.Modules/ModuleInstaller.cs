using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Transactions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.External;

namespace VirtoCommerce.Platform.Modules
{
    [Obsolete("Use ModulePackageInstaller and ModuleDiscovery classes instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class ModuleInstaller : IModuleInstaller
    {
        private const string _packageFileExtension = ".zip";
        private readonly LocalStorageModuleCatalogOptions _options;
        private readonly IExternalModulesClient _externalClient;
        private readonly IExternalModuleCatalog _extModuleCatalog;
        private readonly IFileSystem _fileSystem;

        public ModuleInstaller(IExternalModuleCatalog extModuleCatalog, IExternalModulesClient externalClient, IOptions<LocalStorageModuleCatalogOptions> localOptions, IFileSystem fileSystem)
        {
            _extModuleCatalog = extModuleCatalog;
            _externalClient = externalClient;
            _options = localOptions.Value;
            _fileSystem = fileSystem;
        }

        #region IModuleInstaller Members
        public void Install(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
        {
            var isValid = true;
            //Dependency and version validation
            foreach (var module in modules.Where(x => !x.IsInstalled))
            {
                var allInstalledModules = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).ToList();
                var errors = ModuleBootstrapper.Instance.ValidateInstall(module, allInstalledModules, PlatformVersion.CurrentVersion);
                foreach (var error in errors)
                {
                    Report(progress, ProgressMessageLevel.Error, error);
                    isValid = false;
                }

                //Check the dependencies for installable modules
                isValid &= !HasMissedDependencies(module, modules, progress);
            }

            if (isValid)
            {
                var installedModulesIds = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).Select(x => x.Id).ToArray();
                var updatableModules = modules.Where(x => installedModulesIds.Contains(x.Id));
                var installableModules = modules.Except(updatableModules);
                var changedModulesLog = new List<ManifestModuleInfo>();
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionManager.MaximumTimeout))
                {
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
                            var existModule = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().First(x => x.IsInstalled && x.Id == newModule.Id);
                            var dstModuleDir = Path.Combine(_options.DiscoveryPath, existModule.Id);
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
                        //Revert changed modules state
                        foreach (var changedModule in changedModulesLog)
                        {
                            changedModule.IsInstalled = !changedModule.IsInstalled;
                        }
                    }
                }
            }
        }

        public void Uninstall(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
        {
            var isValid = true;
            var modulesList = modules.ToList();
            var uninstallIds = modulesList.Select(x => x.Id).ToArray();

            //Dependency and version validation
            foreach (var module in modulesList)
            {
                var installedModules = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).ToList();
                var errors = ModuleBootstrapper.Instance.ValidateUninstall(module.Id, installedModules, uninstallIds);
                foreach (var error in errors)
                {
                    Report(progress, ProgressMessageLevel.Error, error);
                    isValid = false;
                }
            }

            if (isValid)
            {
                var changedModulesLog = new List<ManifestModuleInfo>();
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        foreach (var uninstallingModule in modulesList)
                        {
                            Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'", uninstallingModule);
                            //Call module Uninstall method
                            if (uninstallingModule.ModuleInstance != null)
                            {
                                Report(progress, ProgressMessageLevel.Info, "Executing module.Uninstall() method");
                                uninstallingModule.ModuleInstance.Uninstall();
                            }
                            var moduleDir = Path.Combine(_options.DiscoveryPath, uninstallingModule.Id);
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
                        //Revert changed modules state
                        foreach (var changedModule in changedModulesLog)
                        {
                            changedModule.IsInstalled = !changedModule.IsInstalled;
                        }
                    }
                }
            }
        }
        #endregion

        private bool HasMissedDependencies(ManifestModuleInfo module, IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
        {
            var result = true;

            try
            {
                result = _extModuleCatalog.CompleteListWithDependencies([module])
                    .OfType<ManifestModuleInfo>()
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
            var dstModuleDir = Path.Combine(_options.DiscoveryPath, module.Id);
            var moduleZipPath = Path.Combine(dstModuleDir, GetModuleZipFileName(module.Id, module.Version.ToString()));

            if (!Directory.Exists(dstModuleDir))
            {
                Directory.CreateDirectory(dstModuleDir);
            }

            // Download module archive from web
            if (Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
            {
                var moduleUrl = new Uri(module.Ref);

                using (var fileStream = _fileSystem.File.OpenWrite(moduleZipPath))
                using (var webStream = _externalClient.OpenRead(moduleUrl))
                {
                    Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
                    webStream.CopyTo(fileStream);
                }
            }
            else if (_fileSystem.File.Exists(module.Ref))
            {
                moduleZipPath = module.Ref;
            }

            // Extract the downloaded/local package using ModulePackageInstaller
            if (File.Exists(moduleZipPath))
            {
                ModulePackageInstaller.Install(moduleZipPath, dstModuleDir);
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

        private static string GetModuleZipFileName(string moduleId, string version)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", moduleId, version, _packageFileExtension);
        }


    }
}
