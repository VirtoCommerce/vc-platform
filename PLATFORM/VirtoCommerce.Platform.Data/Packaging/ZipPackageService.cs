using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.Packaging
{
    public class ZipPackageService : IPackageService
    {
        private const string _packageFileExtension = ".zip";
        private const string _packageFilePattern = "*" + _packageFileExtension;

        private readonly IModuleCatalog _moduleCatalog;
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly string _installedPackagesPath;

        public ZipPackageService(IModuleCatalog moduleCatalog, IModuleManifestProvider manifestProvider, string installedPackagesPath)
        {
            _moduleCatalog = moduleCatalog;
            _manifestProvider = manifestProvider;
            _installedPackagesPath = installedPackagesPath;
        }

        #region IPackageService Members

        public ModuleDescriptor OpenPackage(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var manifest = ReadPackageManifest(fullPath);
            var result = ConvertToModuleDescriptor(manifest);

            return result;
        }

        public string[] GetDependencyErrors(ModuleDescriptor package)
        {
            var dependencyErrors = GetDependencyErrors(package, GetModules());
            return dependencyErrors.ToArray();
        }

        public ModuleDescriptor[] GetModules()
        {
            List<string> installedPackages = null;

            if (Directory.Exists(_installedPackagesPath))
            {
                installedPackages = Directory.EnumerateFiles(_installedPackagesPath, _packageFilePattern)
                    .Select(Path.GetFileName)
                    .ToList();
            }

            var result = _manifestProvider.GetModuleManifests().Values
                .Select(m => ConvertToModuleDescriptor(m, installedPackages))
                .OrderBy(m => m.Title)
                .ToArray();

            return result;
        }

        public void Install(string sourcePackageFilePath, IProgress<ProgressMessage> progress)
        {
            sourcePackageFilePath = Path.GetFullPath(sourcePackageFilePath);
            Report(progress, ProgressMessageLevel.Info, "Installing '{0}'.", sourcePackageFilePath);

            var sourcePackage = OpenPackage(sourcePackageFilePath);

            if (sourcePackage == null)
            {
                Report(progress, ProgressMessageLevel.Error, "Cannot open package '{0}'.", sourcePackageFilePath);
            }
            else
            {
                var installedModules = GetModules();

                // Check if already installed
                if (installedModules.Any(m => m.Id == sourcePackage.Id))
                {
                    Report(progress, ProgressMessageLevel.Error, "'{0}' is already installed.", sourcePackage.Id);
                }
                else if (CheckDependencies(sourcePackage, installedModules, progress))
                {
                    // Unpack all files
                    var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, sourcePackage.Id);
                    Report(progress, ProgressMessageLevel.Debug, "Copying files to '{0}'.", moduleDirectoryPath);
                    ProcessPackage(sourcePackageFilePath, moduleDirectoryPath, PackageAction.Install);

                    // Copy package to installed packages directory
                    var installedPackageFilePath = Path.Combine(_installedPackagesPath, GetPackageFileName(sourcePackage.Id, sourcePackage.Version));
                    Report(progress, ProgressMessageLevel.Debug, "Copying package '{0}' to '{1}'.", sourcePackageFilePath, installedPackageFilePath);
                    EnsureDirectoryExists(installedPackageFilePath);
                    File.Copy(sourcePackageFilePath, installedPackageFilePath, true);

                    Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0} {1}'.", sourcePackage.Id, sourcePackage.Version);
                }
            }

            _manifestProvider.ClearCache();
        }

        public void Update(string packageId, string newPackageFilePath, IProgress<ProgressMessage> progress)
        {
            newPackageFilePath = Path.GetFullPath(newPackageFilePath);
            Report(progress, ProgressMessageLevel.Info, "Updating '{0}' with '{1}'.", packageId, newPackageFilePath);

            var newPackage = OpenPackage(newPackageFilePath);
            if (newPackage == null)
            {
                Report(progress, ProgressMessageLevel.Error, "Cannot open new package '{0}'.", newPackageFilePath);
            }
            else if (newPackage.Id != packageId)
            {
                Report(progress, ProgressMessageLevel.Error, "New package ID '{0}' does not equal to old package ID '{1}'.", newPackage.Id, packageId);
            }
            else
            {
                var installedModules = GetModules();
                var module = installedModules.FirstOrDefault(m => m.Id == packageId);

                if (module == null)
                {
                    Report(progress, ProgressMessageLevel.Error, "'{0}' is not installed.", packageId);
                }
                else if (newPackage.Version == module.Version)
                {
                    Report(progress, ProgressMessageLevel.Error, "New package version '{0}' is the same as installed version.", newPackage.Version);
                }
                else
                {
                    var oldPackageFilePath = Path.Combine(_installedPackagesPath, GetPackageFileName(module.Id, module.Version));
                    var oldPackage = OpenPackage(oldPackageFilePath);

                    if (oldPackage == null)
                    {
                        Report(progress, ProgressMessageLevel.Error, "Cannot open old package '{0}'.", oldPackageFilePath);
                    }
                    else
                    {
                        if (CheckDependencies(newPackage, installedModules, progress))
                        {
                            // Unpack all files
                            var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, newPackage.Id);

                            Report(progress, ProgressMessageLevel.Debug, "Copying new files to '{0}'.", moduleDirectoryPath);
                            var newFiles = ProcessPackage(newPackageFilePath, moduleDirectoryPath, PackageAction.Install);

                            Report(progress, ProgressMessageLevel.Debug, "Deleting old files from '{0}'.", moduleDirectoryPath);
                            var oldFiles = ProcessPackage(oldPackageFilePath, moduleDirectoryPath, PackageAction.Uninstall);
                            var filesToDelete = oldFiles.Except(newFiles, StringComparer.OrdinalIgnoreCase).ToList();
                            DeleteFiles(filesToDelete, moduleDirectoryPath);

                            // Delete old package from installed packages directory
                            Report(progress, ProgressMessageLevel.Debug, "Deleting old package '{0}'.", oldPackageFilePath);
                            File.Delete(oldPackageFilePath);

                            // Copy new package to installed packages directory
                            var installedPackageFilePath = Path.Combine(_installedPackagesPath, GetPackageFileName(newPackage.Id, newPackage.Version));
                            Report(progress, ProgressMessageLevel.Debug, "Copying new package '{0}' to '{1}'.", newPackageFilePath, installedPackageFilePath);
                            EnsureDirectoryExists(installedPackageFilePath);
                            File.Copy(newPackageFilePath, installedPackageFilePath, true);

                            Report(progress, ProgressMessageLevel.Info, "Successfully updated '{0}' to version '{1}'.", packageId, newPackage.Version);
                        }
                    }
                }
            }

            _manifestProvider.ClearCache();
        }

        public void Uninstall(string packageId, IProgress<ProgressMessage> progress)
        {
            Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'.", packageId);

            var modules = GetModules();
            var module = modules.FirstOrDefault(m => m.Id == packageId);

            if (module == null)
            {
                Report(progress, ProgressMessageLevel.Info, "'{0}' is not installed.", packageId);
            }
            else
            {
                // Check dependent modules
                var dependentModules = modules
                    .Where(m => m.Dependencies != null && m.Dependencies.Any(d => d.Id == packageId))
                    .ToList();

                dependentModules.ForEach(m => Report(progress, ProgressMessageLevel.Error, "'{0}' depends on '{1}'.", m.Id, module.Id));

                if (!dependentModules.Any())
                {
                    // Call Uninstall() for module instance
                    if (_moduleCatalog != null)
                    {
                        var moduleInstance = _moduleCatalog.Modules
                            .Where(m => m.ModuleName == packageId)
                            .Select(m => m.ModuleInstance)
                            .FirstOrDefault();

                        if (moduleInstance != null)
                        {
                            moduleInstance.Uninstall();
                        }
                    }

                    // Delete files
                    var installedPackageFileName = GetPackageFileName(module.Id, module.Version);
                    var installedPackageFilePath = Path.Combine(_installedPackagesPath, installedPackageFileName);
                    var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, module.Id);

                    Report(progress, ProgressMessageLevel.Debug, "Deleting files from '{0}'.", moduleDirectoryPath);

                    var filePaths = ProcessPackage(installedPackageFilePath, moduleDirectoryPath, PackageAction.Uninstall);
                    DeleteFiles(filePaths, moduleDirectoryPath);

                    // Delete package from installed packages directory
                    Report(progress, ProgressMessageLevel.Debug, "Deleting package '{0}'.", installedPackageFilePath);
                    File.Delete(installedPackageFilePath);

                    Report(progress, ProgressMessageLevel.Info, "Successfully uninstalled '{0}'.", packageId);
                }
            }

            _manifestProvider.ClearCache();
        }

        #endregion


        private static bool CheckDependencies(ModuleDescriptor package, IEnumerable<ModuleIdentity> installedModules, IProgress<ProgressMessage> progress)
        {
            var dependencyErrors = GetDependencyErrors(package, installedModules);
            dependencyErrors.ForEach(e => Report(progress, ProgressMessageLevel.Error, e));
            return !dependencyErrors.Any();
        }

        private static List<string> GetDependencyErrors(ModuleDescriptor package, IEnumerable<ModuleIdentity> installedModules)
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

        private static List<ModuleIdentity> GetMissingDependencies(IEnumerable<ModuleIdentity> dependencies, IEnumerable<ModuleIdentity> installedModules)
        {
            var result = new List<ModuleIdentity>();

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

        private ModuleManifest ReadPackageManifest(string packageFilePath)
        {
            ModuleManifest result = null;

            if (File.Exists(packageFilePath))
            {
                using (var packageStream = File.Open(packageFilePath, FileMode.Open))
                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Read))
                {
                    var entry = package.GetEntry(_manifestProvider.ManifestFileName);
                    if (entry != null)
                    {
                        using (var manifestStream = entry.Open())
                        {
                            result = ManifestReader.Read(manifestStream);
                        }
                    }
                }
            }

            return result;
        }

        private static List<string> ProcessPackage(string packageFilePath, string targetDirectoryPath, PackageAction action)
        {
            var files = new List<string>();

            using (var packageStream = File.Open(packageFilePath, FileMode.Open))
            using (var package = new ZipArchive(packageStream, ZipArchiveMode.Read))
            {
                foreach (var entry in package.Entries.Where(e => !string.IsNullOrEmpty(e.Name)))
                {
                    var filePath = Path.Combine(targetDirectoryPath, entry.FullName);
                    files.Add(filePath);

                    switch (action)
                    {
                        case PackageAction.Install:
                            EnsureDirectoryExists(filePath);
                            using (var entryStream = entry.Open())
                            using (var fileStream = File.Create(filePath))
                            {
                                entryStream.CopyTo(fileStream);
                            }
                            File.SetLastWriteTime(filePath, entry.LastWriteTime.LocalDateTime);
                            break;
                    }
                }
            }

            return files;
        }

        private static void EnsureDirectoryExists(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);

            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private static void DeleteFiles(IEnumerable<string> filePaths, string rootDirectoryPath)
        {
            var parentDirectories = new List<string>();

            foreach (var filePath in filePaths)
            {
                AddParentDirectories(filePath, rootDirectoryPath, parentDirectories);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            DeleteEmptyDirectories(parentDirectories);
        }

        private static void AddParentDirectories(string filePath, string rootDirectoryPath, List<string> parentDirectories)
        {
            if (parentDirectories != null)
            {
                var directoryPath = Path.GetDirectoryName(filePath);

                while (directoryPath != null
                    && directoryPath.StartsWith(rootDirectoryPath, StringComparison.OrdinalIgnoreCase)
                    && !parentDirectories.Contains(directoryPath, StringComparer.OrdinalIgnoreCase))
                {
                    parentDirectories.Add(directoryPath);
                    directoryPath = Path.GetDirectoryName(directoryPath);
                }
            }
        }

        private static void DeleteEmptyDirectories(List<string> directories)
        {
            directories.Sort();
            directories.Reverse();

            foreach (var directoryPath in directories)
            {
                if (Directory.Exists(directoryPath) && !Directory.EnumerateFileSystemEntries(directoryPath).Any())
                {
                    Directory.Delete(directoryPath);
                }
            }
        }

        private ModuleDescriptor ConvertToModuleDescriptor(ModuleManifest manifest, List<string> installedPackages = null)
        {
            ModuleDescriptor result = null;

            if (manifest != null)
            {
                result = new ModuleDescriptor
                {
                    Id = manifest.Id,
                    Version = manifest.Version,
                    PlatformVersion = manifest.PlatformVersion,
                    Title = manifest.Title,
                    Description = manifest.Description,
                    Authors = manifest.Authors,
                    Owners = manifest.Owners,
                    RequireLicenseAcceptance = manifest.RequireLicenseAcceptance,
                    ReleaseNotes = manifest.ReleaseNotes,
                    Copyright = manifest.Copyright,
                    Tags = manifest.Tags,
                };

                if (manifest.Dependencies != null)
                    result.Dependencies = manifest.Dependencies.Select(d => new ModuleIdentity { Id = d.Id, Version = d.Version });

                if (manifest.LicenseUrl != null)
                    result.LicenseUrl = new Uri(manifest.LicenseUrl);

                if (manifest.ProjectUrl != null)
                    result.ProjectUrl = new Uri(manifest.ProjectUrl);

                if (manifest.IconUrl != null)
                    result.IconUrl = new Uri(manifest.IconUrl, UriKind.RelativeOrAbsolute);

                if (installedPackages != null && installedPackages.Any())
                {
                    var packageFileName = GetPackageFileName(manifest.Id, manifest.Version);
                    result.IsRemovable = installedPackages.Contains(packageFileName, StringComparer.OrdinalIgnoreCase);
                }

                if (_moduleCatalog != null && _moduleCatalog.Modules != null)
                    result.ModuleInfo = _moduleCatalog.Modules.FirstOrDefault(x => x.ModuleName == result.Id);
            }

            return result;
        }

        private static string GetPackageFileName(string packageId, string version)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", packageId, version, _packageFileExtension);
        }

        private enum PackageAction
        {
            Install,
            Uninstall,
        }
    }
}
