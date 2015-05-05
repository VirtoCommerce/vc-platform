using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.Packaging
{
    public class ZipPackageService : IPackageService
    {
        private const string _packageFileExtension = ".zip";
        private const string _packageFilePattern = "*" + _packageFileExtension;

        private readonly IModuleManifestProvider _manifestProvider;
        private readonly string _installedPackagesPath;
        private readonly string _sourcePackagesPath;

        public ZipPackageService(IModuleManifestProvider manifestProvider, string installedPackagesPath, string sourcePackagesPath)
        {
            _manifestProvider = manifestProvider;
            _installedPackagesPath = installedPackagesPath;
            _sourcePackagesPath = sourcePackagesPath;
        }

        #region IPackageService Members

        public ModuleDescriptor OpenPackage(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var manifest = ReadPackageManifest(fullPath);
            var result = ConvertToModuleDescriptor(manifest);

            return result;
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

        public void Install(string packageId, string version, IProgress<ProgressMessage> progress)
        {
            var packageIdAndVersion = string.Join(" ", packageId, version);
            Report(progress, ProgressMessageLevel.Info, "Installing '{0}'.", packageIdAndVersion);

            var installedModuleIds = GetModules().Select(m => m.Id).ToList();

            // Check if already installed
            if (installedModuleIds.Contains(packageId))
            {
                Report(progress, ProgressMessageLevel.Error, "'{0}' is already installed.", packageId);
            }
            else
            {
                var sourcePackageFileName = GetPackageFileName(packageId, version);
                var sourcePackageFilePath = Path.Combine(_sourcePackagesPath, sourcePackageFileName);
                var sourcePackage = OpenPackage(sourcePackageFilePath);

                if (sourcePackage == null)
                {
                    Report(progress, ProgressMessageLevel.Error, "Cannot open package '{0}'.", sourcePackageFilePath);
                }
                else
                {
                    if (CheckDependencies(sourcePackage, installedModuleIds, progress))
                    {
                        // Unpack all files
                        var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, sourcePackage.Id);
                        Report(progress, ProgressMessageLevel.Debug, "Copying files to '{0}'.", moduleDirectoryPath);
                        ProcessPackage(sourcePackageFilePath, moduleDirectoryPath, PackageAction.Install);

                        // Copy package to installed packages directory
                        var installedPackageFilePath = Path.Combine(_installedPackagesPath, sourcePackageFileName);
                        Report(progress, ProgressMessageLevel.Debug, "Copying package '{0}' to '{1}'.", sourcePackageFilePath, installedPackageFilePath);
                        EnsureDirectoryExists(installedPackageFilePath);
                        File.Copy(sourcePackageFilePath, installedPackageFilePath, true);

                        Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0}'.", packageIdAndVersion);
                    }
                }
            }
        }

        public void Update(string packageId, string version, IProgress<ProgressMessage> progress)
        {
            Report(progress, ProgressMessageLevel.Info, "Updating '{0}' to version '{1}'.", packageId, version);

            var modules = GetModules();
            var installedModuleIds = modules.Select(m => m.Id).ToList();
            var module = modules.FirstOrDefault(m => m.Id == packageId);

            if (module == null)
            {
                Report(progress, ProgressMessageLevel.Error, "'{0}' is not installed.", packageId);
            }
            else
            {
                var oldPackageFileName = GetPackageFileName(module.Id, module.Version);
                var oldPackageFilePath = Path.Combine(_installedPackagesPath, oldPackageFileName);
                var oldPackage = OpenPackage(oldPackageFilePath);

                if (oldPackage == null)
                {
                    Report(progress, ProgressMessageLevel.Error, "Cannot open old package '{0}'.", oldPackageFilePath);
                }
                else
                {
                    var newPackageFileName = GetPackageFileName(packageId, version);
                    var newPackageFilePath = Path.Combine(_sourcePackagesPath, newPackageFileName);
                    var newPackage = OpenPackage(newPackageFilePath);

                    if (newPackage == null)
                    {
                        Report(progress, ProgressMessageLevel.Error, "Cannot open new package '{0}'.", newPackageFilePath);
                    }
                    else
                    {
                        if (CheckDependencies(newPackage, installedModuleIds, progress))
                        {
                            // Unpack all files
                            var moduleDirectoryPath = Path.Combine(_manifestProvider.RootPath, newPackage.Id);

                            Report(progress, ProgressMessageLevel.Debug, "Copying new files to '{0}'.", moduleDirectoryPath);
                            var newFiles = ProcessPackage(newPackageFilePath, moduleDirectoryPath, PackageAction.Install);

                            Report(progress, ProgressMessageLevel.Debug, "Deleting old files from '{0}'.", moduleDirectoryPath);
                            var oldFiles = ProcessPackage(oldPackageFilePath, moduleDirectoryPath, PackageAction.Uninstall);
                            var filesToDelete = oldFiles.Except(newFiles).ToList();
                            DeleteFiles(filesToDelete, moduleDirectoryPath);

                            // Delete old package from installed packages directory
                            Report(progress, ProgressMessageLevel.Debug, "Deleting old package '{0}'.", oldPackageFilePath);
                            File.Delete(oldPackageFilePath);

                            // Copy new package to installed packages directory
                            var installedPackageFilePath = Path.Combine(_installedPackagesPath, newPackageFileName);
                            Report(progress, ProgressMessageLevel.Debug, "Copying new package '{0}' to '{1}'.", newPackageFilePath, installedPackageFilePath);
                            EnsureDirectoryExists(installedPackageFilePath);
                            File.Copy(newPackageFilePath, installedPackageFilePath, true);

                            Report(progress, ProgressMessageLevel.Info, "Successfully updated '{0}' to version '{1}'.", packageId, version);
                        }
                    }
                }
            }
        }

        public void Uninstall(string packageId, IProgress<ProgressMessage> progress)
        {
            Report(progress, ProgressMessageLevel.Info, "Uninstalling '{0}'.", packageId);

            var modules = GetModules();
            var module = modules.FirstOrDefault(m => m.Id == packageId);

            if (module != null)
            {
                // Check dependent modules
                var dependentModules = modules
                    .Where(m => m.Dependencies != null && m.Dependencies.Contains(packageId))
                    .ToList();

                dependentModules.ForEach(m => Report(progress, ProgressMessageLevel.Error, "'{0}' depends on '{1}'.", m.Id, module.Id));

                if (!dependentModules.Any())
                {
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
                }
            }

            Report(progress, ProgressMessageLevel.Info, "Successfully uninstalled '{0}'.", packageId);
        }

        #endregion


        private static bool CheckDependencies(ModuleDescriptor package, IEnumerable<string> installedModuleIds, IProgress<ProgressMessage> progress)
        {
            var dependenciesSatisfied = (package.Dependencies == null);

            if (!dependenciesSatisfied)
            {
                var missingModuleIds = package.Dependencies.Except(installedModuleIds).ToList();
                missingModuleIds.ForEach(id => Report(progress, ProgressMessageLevel.Error, "Dependency is not installed: '{0}'.", id));
                dependenciesSatisfied = !missingModuleIds.Any();
            }

            return dependenciesSatisfied;
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
                foreach (var entry in package.Entries)
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

        private static ModuleDescriptor ConvertToModuleDescriptor(ModuleManifest manifest, List<string> installedPackages = null)
        {
            ModuleDescriptor result = null;

            if (manifest != null)
            {
                result = new ModuleDescriptor
                {
                    Id = manifest.Id,
                    Version = manifest.Version,
                    Title = manifest.Title,
                    Description = manifest.Description,
                    Authors = manifest.Authors,
                    Owners = manifest.Owners,
                    RequireLicenseAcceptance = manifest.RequireLicenseAcceptance,
                    ReleaseNotes = manifest.ReleaseNotes,
                    Copyright = manifest.Copyright,
                    Tags = manifest.Tags,
                    Dependencies = manifest.Dependencies,
                };

                if (manifest.LicenseUrl != null)
                    result.LicenseUrl = new Uri(manifest.LicenseUrl);

                if (manifest.ProjectUrl != null)
                    result.ProjectUrl = new Uri(manifest.ProjectUrl);

                if (manifest.IconUrl != null)
                    result.IconUrl = new Uri(manifest.IconUrl);

                if (installedPackages != null && installedPackages.Any())
                {
                    var packageFileName = GetPackageFileName(manifest.Id, manifest.Version);
                    result.IsRemovable = installedPackages.Contains(packageFileName, StringComparer.OrdinalIgnoreCase);
                }
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
