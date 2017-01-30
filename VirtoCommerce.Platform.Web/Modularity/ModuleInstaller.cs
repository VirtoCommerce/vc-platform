using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Transactions;
using ChinhDo.Transactions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ModuleInstaller : IModuleInstaller
    {
        private const string _packageFileExtension = ".zip";
        private readonly TxFileManager _txFileManager = new TxFileManager();

        private readonly string _modulesPath;
        private readonly IModuleCatalog _moduleCatalog;

        public ModuleInstaller(string modulesPath, IModuleCatalog moduleCatalog)
        {
            _modulesPath = modulesPath;
            _moduleCatalog = moduleCatalog;
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
                //Check that installable version compatible with already installed
                var alreadyInstalledModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).FirstOrDefault(x => x.Id.EqualsInvariant(module.Id));
                if (alreadyInstalledModule != null && !alreadyInstalledModule.Version.IsCompatibleWithBySemVer(module.Version))
                {
                    //Allow downgrade or install not compatible version only if all dependencies will be compatible with installed version
                    var modulesHasIncompatibleDependecies = _moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                                          .Where(x => x.IsInstalled)
                                                          .Where(x => x.DependsOn.Contains(module.Id, StringComparer.OrdinalIgnoreCase))
                                                          .Where(x => x.Dependencies.Any(d => !module.Version.IsCompatibleWithBySemVer(d.Version)));

                    if (modulesHasIncompatibleDependecies.Any())
                    {
                        Report(progress, ProgressMessageLevel.Error, string.Format("{0} is incompatible with installed {1} is required  by {2} ", module, alreadyInstalledModule, string.Join(", ", modulesHasIncompatibleDependecies.Select(x => x.ToString()))));
                        isValid = false;
                    }
                }
                //Check that dependencies for installable modules 
                var missedDependencies = _moduleCatalog.CompleteListWithDependencies(new[] { module }).OfType<ManifestModuleInfo>()
                                                       .Where(x => !x.IsInstalled).Except(modules);
                if (missedDependencies.Any())
                {
                    Report(progress, ProgressMessageLevel.Error, string.Format("{0} dependencies required for {1}", string.Join(" ", missedDependencies), module));
                    isValid = false;
                }
            }

            if (isValid)
            {
                var installedModulesIds = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).Select(x => x.Id).ToArray();
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
                            var existModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled && x.Id == newModule.Id).First();
                            var dstModuleDir = Path.Combine(_modulesPath, existModule.Id);
                            if (Directory.Exists(dstModuleDir))
                            {
                                SafeDeleteDirectory(dstModuleDir);
                            }
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
                var dependingModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled)
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
                            var moduleDir = Path.Combine(_modulesPath, uninstallingModule.Id);
                            if (Directory.Exists(moduleDir))
                            {
                                Report(progress, ProgressMessageLevel.Info, "Deleting module {0} folder", moduleDir);
                                SafeDeleteDirectory(moduleDir);
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
            var dstModuleDir = Path.Combine(_modulesPath, module.Id);
            var moduleZipPath = Path.Combine(dstModuleDir, GetModuleZipFileName(module.Id, module.Version.ToString()));

            if (!Directory.Exists(dstModuleDir))
            {
                _txFileManager.CreateDirectory(dstModuleDir);
            }

            //download  module archive from web
            if (Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
            {
                var moduleUrl = new Uri(module.Ref);

                using (var webClient = new WebClient())
                {
                    webClient.AddAuthorizationTokenForGitHub(moduleUrl);

                    using (var fileStream = File.OpenWrite(moduleZipPath))
                    using (var webStream = webClient.OpenRead(moduleUrl))
                    {
                        Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
                        webStream.CopyTo(fileStream);
                    }
                }
            }
            else if (File.Exists(module.Ref))
            {
                moduleZipPath = module.Ref;
            }

            using (var zipStream = File.Open(moduleZipPath, FileMode.Open))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries.Where(e => !string.IsNullOrEmpty(e.Name)))
                {
                    //Report(progress, ProgressMessageLevel.Info, "Extracting '{0}' ", entry.FullName);
                    var filePath = Path.Combine(dstModuleDir, entry.FullName);
                    //Create directory if not exist
                    var directoryPath = Path.GetDirectoryName(filePath);
                    if (!_txFileManager.DirectoryExists(directoryPath))
                    {
                        _txFileManager.CreateDirectory(directoryPath);
                    }
                    using (var entryStream = entry.Open())
                    using (var fileStream = File.Create(filePath))
                    {
                        entryStream.CopyTo(fileStream);
                    }
                    File.SetLastWriteTime(filePath, entry.LastWriteTime.LocalDateTime);
                }
            }

            Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0}'.", module);
        }

        private void SafeDeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                //try delete whole directory
                try
                {
                    _txFileManager.DeleteDirectory(directoryPath);
                }
                //Because some folder can be locked by ASP.NET Bundles file monitor we should ignore IOException
                catch (IOException)
                {
                    //If fail need to delete directory content first
                    //Files                 
                    foreach (var file in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
                    {
                        _txFileManager.Delete(file);
                    }
                    //Dirs
                    foreach (var subDirectory in Directory.EnumerateDirectories(directoryPath, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            _txFileManager.DeleteDirectory(subDirectory);
                        }
                        catch (IOException)
                        {
                        }
                    }
                    //Then try to delete main directory itself
                    try
                    {
                        _txFileManager.DeleteDirectory(directoryPath);
                    }
                    catch (IOException)
                    {
                    }
                }
            }
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
