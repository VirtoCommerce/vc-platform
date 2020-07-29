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
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;
using VirtoCommerce.Platform.Modules.External;

namespace VirtoCommerce.Platform.Modules
{
    public class ModuleInstaller : IModuleInstaller
    {
        private const string _packageFileExtension = ".zip";
        private readonly LocalStorageModuleCatalogOptions _options;
        private readonly IExternalModulesClient _externalClient;
        private readonly ITransactionFileManager _fileManager;
        private readonly IExternalModuleCatalog _extModuleCatalog;
        private readonly IFileSystem _fileSystem;
        private readonly IZipFileWrapper _zipFileWrapper;

        public ModuleInstaller(IExternalModuleCatalog extModuleCatalog, IExternalModulesClient externalClient, ITransactionFileManager txFileManager, IOptions<LocalStorageModuleCatalogOptions> localOptions, IFileSystem fileSystem, IZipFileWrapper zipFileWrapper)
        {
            _extModuleCatalog = extModuleCatalog;
            _externalClient = externalClient;
            _options = localOptions.Value;
            _fileManager = txFileManager;
            _fileSystem = fileSystem;
            _zipFileWrapper = zipFileWrapper;
        }

        #region IModuleInstaller Members
        public void Install(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
        {
            var isValid = true;
            //Dependency and version validation
            foreach (var module in modules.Where(x => !x.IsInstalled))
            {
                //Check platform version
                if (!module.PlatformVersion.IsCompatibleWith(PlatformVersion.CurrentVersion))
                {
                    Report(progress, ProgressMessageLevel.Error, string.Format("Target Platform version {0} is incompatible with current {1}", module.PlatformVersion, PlatformVersion.CurrentVersion));
                    isValid = false;
                }
           
                var allInstalledModules = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).ToArray();
                //Check that incompatible modules does not installed
                if (!module.Incompatibilities.IsNullOrEmpty())
                {
                    var installedIncompatibilities = allInstalledModules.Select(x => x.Identity).Join(module.Incompatibilities, x => x.Id, y => y.Id, (x, y) => new { x, y })
                                                          .Where(g => g.y.Version.IsCompatibleWith(g.x.Version)).Select(g => g.x)
                                                          .ToArray();
                    if (installedIncompatibilities.Any())
                    {
                        Report(progress, ProgressMessageLevel.Error, string.Format("{0} is incompatible with installed {1}. You should uninstall these modules first.", module, string.Join(", ", installedIncompatibilities.Select(x => x.ToString()))));
                        isValid = false;
                    }
                }

                //Check that installable version compatible with already installed
                var alreadyInstalledModule = allInstalledModules.FirstOrDefault(x => x.Id.EqualsInvariant(module.Id));
                if (alreadyInstalledModule != null && !alreadyInstalledModule.Version.IsCompatibleWithBySemVer(module.Version))
                {
                    //Allow downgrade or install not compatible version only if all dependencies will be compatible with installed version
                    var modulesHasIncompatibleDependecies = allInstalledModules.Where(x => x.DependsOn.Contains(module.Id, StringComparer.OrdinalIgnoreCase))
                                                          .Where(x => x.Dependencies.Any(d => !module.Version.IsCompatibleWithBySemVer(d.Version)));

                    if (modulesHasIncompatibleDependecies.Any())
                    {
                        Report(progress, ProgressMessageLevel.Error, string.Format("{0} is incompatible with installed {1} is required  by {2} ", module, alreadyInstalledModule, string.Join(", ", modulesHasIncompatibleDependecies.Select(x => x.ToString()))));
                        isValid = false;
                    }
                }
                //Check that dependencies for installable modules 
                var missedDependencies = _extModuleCatalog.CompleteListWithDependencies(new[] { module }).OfType<ManifestModuleInfo>()
                                                       .Where(x => !x.IsInstalled).Except(modules);
                if (missedDependencies.Any())
                {
                    Report(progress, ProgressMessageLevel.Error, string.Format("{0} dependencies required for {1}", string.Join(" ", missedDependencies), module));
                    isValid = false;
                }
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
                            var existModule = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled && x.Id == newModule.Id).First();
                            var dstModuleDir = Path.Combine(_options.DiscoveryPath, existModule.Id);
                            _fileManager.SafeDelete(dstModuleDir);
                            Report(progress, ProgressMessageLevel.Info, "Updating '{0}' -> '{1}'", existModule, newModule);
                            InnerInstall(newModule, progress);
                            existModule.IsInstalled = false;
                            newModule.IsInstalled = true;
                            changedModulesLog.AddRange(new[] { existModule, newModule });
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
            //Dependency and version validation
            foreach (var module in modules)
            {
                var dependingModules = _extModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled)
                                                    .Where(x => x.DependsOn.Contains(module.Id))
                                                    .Except(modules);
                //If module being uninstalled has depending modules and they are not contained in uninstall list
                foreach (var dependingModule in dependingModules)
                {
                    Report(progress, ProgressMessageLevel.Error, "Unable to uninstall '{0}' because '{1}' depends on it", module, dependingModule);
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
                        foreach (var uninstallingModule in modules)
                        {
                            Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'", uninstallingModule);
                            //Call module Uninstall method
                            if (uninstallingModule.ModuleInstance != null)
                            {
                                Report(progress, ProgressMessageLevel.Info, "Executing module.Uninstall() method");
                                uninstallingModule.ModuleInstance.Uninstall();
                            }
                            var moduleDir = Path.Combine(_options.DiscoveryPath, uninstallingModule.Id);
                            if (Directory.Exists(moduleDir))
                            {
                                Report(progress, ProgressMessageLevel.Info, "Deleting module {0} folder", moduleDir);
                                _fileManager.SafeDelete(moduleDir);
                            }
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


        private void InnerInstall(ManifestModuleInfo module, IProgress<ProgressMessage> progress)
        {
            var dstModuleDir = Path.Combine(_options.DiscoveryPath, module.Id);
            var moduleZipPath = Path.Combine(dstModuleDir, GetModuleZipFileName(module.Id, module.Version.ToString()));

            _fileManager.CreateDirectory(dstModuleDir);

            //download  module archive from web
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

            _zipFileWrapper.Extract(moduleZipPath, dstModuleDir);
            
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
