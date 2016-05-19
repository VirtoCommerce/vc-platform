using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.Modularity
{
    public class ModuleInstallerImpl : IModuleInstaller
    {
        private const string _packageFileExtension = ".zip";
        private const string _packageFilePattern = "*" + _packageFileExtension;

        private readonly string _modulesPath;

        public ModuleInstallerImpl(string modulesPath)
        {
            _modulesPath = modulesPath;
        }

        #region IModuleInstaller Members
        public void Install(IEnumerable<ManifestModuleInfo> moduleInfos, IProgress<ProgressMessage> progress)
        {
            foreach (var module in moduleInfos)
            {
                var dstModuleDir = Path.Combine(_modulesPath, module.Id);
                var dstModuleZipPath = Path.Combine(_modulesPath, module.Id, GetModuleZipFileName(module.Id, module.Version));
                Report(progress, ProgressMessageLevel.Info, "Installing '{0}' from {1}", module.Id, module.Ref);

                if(Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
                {
                    if (!Directory.Exists(dstModuleDir))
                    {
                        Directory.CreateDirectory(dstModuleDir);
                    }
    
                    using (var client = new WebClient())
                    using (var fileStream = File.Create(dstModuleZipPath))
                    using (var webStream = client.OpenRead(new Uri(module.Ref)))
                    {
                        Report(progress, ProgressMessageLevel.Info, "Downloading '{0}' ", module.Ref);
                        webStream.CopyTo(fileStream);
                    }
                   
                    using (var zipStream = File.Open(dstModuleZipPath, FileMode.Open))
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
                    Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0} {1}'.", module.Id, module.Version);
                } 
            }
        }

        public void Uninstall(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress)
        {
            throw new NotImplementedException();
        }

        //public void Update(ManifestModuleInfo moduleInfo, IProgress<ProgressMessage> progress)
        //{
        //    newPackageFilePath = Path.GetFullPath(newPackageFilePath);
        //    Report(progress, ProgressMessageLevel.Info, "Updating '{0}' with '{1}'.", packageId, newPackageFilePath);

        //    var newPackage = LoadModule(newPackageFilePath);
        //    if (newPackage == null)
        //    {
        //        Report(progress, ProgressMessageLevel.Error, "Cannot open new package '{0}'.", newPackageFilePath);
        //    }
        //    else if (newPackage.Id != packageId)
        //    {
        //        Report(progress, ProgressMessageLevel.Error, "New package ID '{0}' does not equal to old package ID '{1}'.", newPackage.Id, packageId);
        //    }
        //    else
        //    {
        //        var installedModules = GetAllModules();
        //        var module = installedModules.FirstOrDefault(m => m.Id == packageId);

        //        if (module == null)
        //        {
        //            Report(progress, ProgressMessageLevel.Error, "'{0}' is not installed.", packageId);
        //        }
        //        else if (newPackage.Version == module.Version)
        //        {
        //            Report(progress, ProgressMessageLevel.Error, "New package version '{0}' is the same as installed version.", newPackage.Version);
        //        }
        //        else
        //        {
        //            var oldPackageFilePath = Path.Combine(_installedPackagesPath, GetPackageFileName(module.Id, module.Version));
        //            var oldPackage = LoadModule(oldPackageFilePath);

        //            if (oldPackage == null)
        //            {
        //                Report(progress, ProgressMessageLevel.Error, "Cannot open old package '{0}'.", oldPackageFilePath);
        //            }
        //            else
        //            {
        //                if (CheckDependencies(newPackage, installedModules, progress))
        //                {
        //                    // Unpack all files
        //                    var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, newPackage.Id);

        //                    Report(progress, ProgressMessageLevel.Debug, "Copying new files to '{0}'.", moduleDirectoryPath);
        //                    var newFiles = ProcessPackage(newPackageFilePath, moduleDirectoryPath, PackageAction.Install);

        //                    Report(progress, ProgressMessageLevel.Debug, "Deleting old files from '{0}'.", moduleDirectoryPath);
        //                    var oldFiles = ProcessPackage(oldPackageFilePath, moduleDirectoryPath, PackageAction.Uninstall);
        //                    var filesToDelete = oldFiles.Except(newFiles, StringComparer.OrdinalIgnoreCase).ToList();
        //                    DeleteFiles(filesToDelete, moduleDirectoryPath);

        //                    // Delete old package from installed packages directory
        //                    Report(progress, ProgressMessageLevel.Debug, "Deleting old package '{0}'.", oldPackageFilePath);
        //                    File.Delete(oldPackageFilePath);

        //                    // Copy new package to installed packages directory
        //                    var installedPackageFilePath = Path.Combine(_installedPackagesPath, GetPackageFileName(newPackage.Id, newPackage.Version));
        //                    Report(progress, ProgressMessageLevel.Debug, "Copying new package '{0}' to '{1}'.", newPackageFilePath, installedPackageFilePath);
        //                    EnsureDirectoryExists(installedPackageFilePath);
        //                    File.Copy(newPackageFilePath, installedPackageFilePath, true);

        //                    Report(progress, ProgressMessageLevel.Info, "Successfully updated '{0}' to version '{1}'.", packageId, newPackage.Version);
        //                }
        //            }
        //        }
        //    }

        //    _manifestProvider.ClearCache();
        //}

        //public void Uninstall(string packageId, IProgress<ProgressMessage> progress)
        //{
        //    Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'.", packageId);

        //    var modules = GetAllModules();
        //    var module = modules.FirstOrDefault(m => m.Id == packageId);

        //    if (module == null)
        //    {
        //        Report(progress, ProgressMessageLevel.Info, "'{0}' is not installed.", packageId);
        //    }
        //    else
        //    {
        //        // Check dependent modules
        //        var dependentModules = modules
        //            .Where(m => m.Dependencies != null && m.Dependencies.Any(d => d.Id == packageId))
        //            .ToList();

        //        dependentModules.ForEach(m => Report(progress, ProgressMessageLevel.Error, "'{0}' depends on '{1}'.", m.Id, module.Id));

        //        if (!dependentModules.Any())
        //        {
        //            // Call Uninstall() for module instance
        //            if (_moduleCatalog != null)
        //            {
        //                var moduleInstance = _moduleCatalog.Modules
        //                    .Where(m => m.ModuleName == packageId)
        //                    .Select(m => m.ModuleInstance)
        //                    .FirstOrDefault();

        //                if (moduleInstance != null)
        //                {
        //                    moduleInstance.Uninstall();
        //                }
        //            }

        //            // Delete files
        //            var installedPackageFileName = GetPackageFileName(module.Id, module.Version);
        //            var installedPackageFilePath = Path.Combine(_installedPackagesPath, installedPackageFileName);
        //            var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, module.Id);

        //            Report(progress, ProgressMessageLevel.Debug, "Deleting files from '{0}'.", moduleDirectoryPath);

        //            var filePaths = ProcessPackage(installedPackageFilePath, moduleDirectoryPath, PackageAction.Uninstall);
        //            DeleteFiles(filePaths, moduleDirectoryPath);

        //            // Delete package from installed packages directory
        //            Report(progress, ProgressMessageLevel.Debug, "Deleting package '{0}'.", installedPackageFilePath);
        //            File.Delete(installedPackageFilePath);

        //            Report(progress, ProgressMessageLevel.Info, "Successfully uninstalled '{0}'.", packageId);
        //        }
        //    }

        //    _manifestProvider.ClearCache();
        //}

        #endregion

        private static IEnumerable<ManifestModuleInfo> LoadModulesFromUpdateServer(string updateServerUrl)
        {
            var retVal = new List<ManifestModuleInfo>();
            using (WebClient webClient = new WebClient())
            using (var stream = webClient.OpenRead(updateServerUrl))
            {
                retVal.AddRange(stream.DeserializeJson<List<ManifestModuleInfo>>());
            }
            return retVal;
        }

        private static bool CheckDependencies(ManifestModuleInfo package, IEnumerable<ManifestModuleInfo> installedModules, IProgress<ProgressMessage> progress)
        {
            var dependencyErrors = GetDependencyErrors(package, installedModules);
            dependencyErrors.ForEach(e => Report(progress, ProgressMessageLevel.Error, e));
            return !dependencyErrors.Any();
        }

        private static List<string> GetDependencyErrors(ManifestModuleInfo package, IEnumerable<ManifestModuleInfo> installedModules)
        {
            var errors = new List<string>();

            if (!SemanticVersion.Parse(package.PlatformVersion).IsCompatibleWith(PlatformVersion.CurrentVersion))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, "Required platform version: '{0}'.", package.PlatformVersion));
            }
            else
            {
                var missingDependencies = GetMissingDependencies(package.Dependencies, installedModules);
                missingDependencies.ForEach(d => errors.Add(string.Format(CultureInfo.CurrentCulture, "Dependency is not installed: '{0} {1}'.", d.Id, d.Version)));
            }

            return errors;
        }

        private static List<ManifestDependency> GetMissingDependencies(IEnumerable<ManifestDependency> dependencies, IEnumerable<ManifestModuleInfo> installedModules)
        {
            var result = new List<ManifestDependency>();

            if (dependencies != null)
            {
                var modules = installedModules.ToList();

                foreach (var dependency in dependencies)
                {
                    var isMissing = true;
                    var installedModule = modules.FirstOrDefault(m => m.Id == dependency.Id);

                    if (installedModule != null)
                    {
                        isMissing = !SemanticVersion.Parse(dependency.Version).IsCompatibleWith(SemanticVersion.Parse(installedModule.Version));
                    }

                    if (isMissing)
                    {
                        result.Add(dependency);
                    }
                }
            }

            return result;
        }


        private static void Report(IProgress<ProgressMessage> progress, ProgressMessageLevel level, string format, params object[] args)
        {
            if (progress != null)
            {
                var message = string.Format(CultureInfo.CurrentCulture, format, args);
                progress.Report(new ProgressMessage { Level = level, Message = message });
            }
        }

        //private ModuleManifest ReadPackageManifest(string packageFilePath)
        //{
        //    ModuleManifest result = null;

        //    if (File.Exists(packageFilePath))
        //    {
        //        using (var packageStream = File.Open(packageFilePath, FileMode.Open))
        //        using (var package = new ZipArchive(packageStream, ZipArchiveMode.Read))
        //        {
        //            var entry = package.GetEntry(_manifestProvider.ManifestFileName);
        //            if (entry != null)
        //            {
        //                using (var manifestStream = entry.Open())
        //                {
        //                    result = ManifestReader.Read(manifestStream);
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}       

    
        private static string GetModuleZipFileName(string moduleId, string version)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", moduleId, version, _packageFileExtension);
        }

   
    }
}
