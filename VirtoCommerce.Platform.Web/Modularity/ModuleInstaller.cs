﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Transactions;
using CacheManager.Core;
using ChinhDo.Transactions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ModuleInstaller : IModuleInstaller
    {
        private const string _packageFileExtension = ".zip";
        private const string _packageFilePattern = "*" + _packageFileExtension;
        private readonly IModuleCatalog _moduleCatalog;
        private TxFileManager _txFileManager = new TxFileManager();

        private readonly string _modulesPath;

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
                if (PlatformVersion.CurrentVersion.IsCompatibleWith(module.PlatformVersion))
                {
                    Report(progress, ProgressMessageLevel.Error, string.Format("Platform version {0}  is not compatible with installed {1}", module.PlatformVersion, PlatformVersion.CurrentVersion));
                    isValid = false;
                }
                //Check that installed version compatible with already installed
                var alreadyInstalledModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).FirstOrDefault(x => x.Id.EqualsInvariant(module.Id));
                if (alreadyInstalledModule != null && !alreadyInstalledModule.Version.IsCompatibleWith(module.Version))
                {
                    Report(progress, ProgressMessageLevel.Error, string.Format("{0}  is not compatible with installed {1}", module, alreadyInstalledModule));
                    isValid = false;
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
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        foreach (var installableModule in installableModules)
                        {
                            Report(progress, ProgressMessageLevel.Info, "Installing '{0}'", installableModule);
                            InnerInstall(installableModule, progress);
                        }

                        foreach (var updatableModule in updatableModules)
                        {
                            var existModule = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.IsInstalled && x.Id == updatableModule.Id).First();
                            var dstModuleDir = Path.Combine(_modulesPath, existModule.Id);
                            if (Directory.Exists(dstModuleDir))
                            {
                                _txFileManager.DeleteDirectory(dstModuleDir);
                            }
                            Report(progress, ProgressMessageLevel.Info, "Updating '{0}' -> '{1}'", existModule, updatableModule);
                            InnerInstall(updatableModule, progress);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        //TODO: rollback all changes
                        Report(progress, ProgressMessageLevel.Error, ex.ToString());
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
                //If uninstalled module have depending modules and they not fully contains in uninstalling list 
                foreach (var dependingModule in dependingModules)
                {
                    Report(progress, ProgressMessageLevel.Error, "Unable to uninstall '{0}' because '{1}' depends on it", module, dependingModule);
                    isValid = false;
                }
            }

            if (isValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        foreach (var uninstallingModule in modules)
                        {
                            Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'", uninstallingModule);
                            //Call module Uninstall method
                            if (uninstallingModule.ModuleInstance != null)
                            {
                                uninstallingModule.ModuleInstance.Uninstall();
                            }
                            var dstModuleDir = Path.Combine(_modulesPath, uninstallingModule.Id);
                            if (Directory.Exists(dstModuleDir))
                            {
                                _txFileManager.DeleteDirectory(dstModuleDir);
                            }
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        Report(progress, ProgressMessageLevel.Error, ex.ToString());
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
                using (var client = new WebClient())
                using (var fileStream = File.OpenWrite(moduleZipPath))
                using (var webStream = client.OpenRead(new Uri(module.Ref)))
                {
                    Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
                    webStream.CopyTo(fileStream);
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
                    Report(progress, ProgressMessageLevel.Info, "Extracting '{0}' ", entry.FullName);
                    var filePath = Path.Combine(dstModuleDir, entry.FullName);

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
