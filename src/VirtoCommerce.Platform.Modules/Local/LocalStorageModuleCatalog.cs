using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.AssemblyLoading;

namespace VirtoCommerce.Platform.Modules
{
    public class LocalStorageModuleCatalog : ModuleCatalog, ILocalModuleCatalog
    {
        private readonly LocalStorageModuleCatalogOptions _options;
        private readonly ILogger<LocalStorageModuleCatalog> _logger;

        public LocalStorageModuleCatalog(IOptions<LocalStorageModuleCatalogOptions> options, ILogger<LocalStorageModuleCatalog> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        protected override void InnerLoad()
        {
            var discoveryPath = _options.DiscoveryPath;

            if (string.IsNullOrEmpty(_options.ProbingPath))
                throw new InvalidOperationException("The ProbingPath cannot contain a null value or be empty");
            if (string.IsNullOrEmpty(_options.DiscoveryPath))
                throw new InvalidOperationException("The DiscoveryPath cannot contain a null value or be empty");


            var bNeedToCopyAssemblies = _options.RefreshProbingFolderOnStart;

            if (!Directory.Exists(_options.ProbingPath))
            {
                bNeedToCopyAssemblies = true; // Force to refresh assemblies anyway, even if RefreshProbeFolderOnStart set to false, because the probing path is absent
                Directory.CreateDirectory(_options.ProbingPath);
            }

            if (!discoveryPath.EndsWith(PlatformInformation.DirectorySeparator))
                discoveryPath += PlatformInformation.DirectorySeparator;

            if (bNeedToCopyAssemblies) CopyAssemblies(discoveryPath, _options.ProbingPath);

            foreach (var pair in GetModuleManifests())
            {
                var manifest = pair.Value;
                var manifestPath = pair.Key;

                if (bNeedToCopyAssemblies)
                {
                    var modulePath = Path.GetDirectoryName(manifestPath);
                    CopyAssemblies(modulePath, _options.ProbingPath);
                }

                var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                moduleInfo.LoadFromManifest(manifest);
                moduleInfo.FullPhysicalPath = Path.GetDirectoryName(manifestPath);

                // Modules without assembly file don't need initialization
                if (string.IsNullOrEmpty(manifest.AssemblyFile))
                {
                    moduleInfo.State = ModuleState.Initialized;
                }
                else
                {
                    //Set module assembly physical path for future loading by IModuleTypeLoader instance
                    moduleInfo.Ref = GetFileAbsoluteUri(_options.ProbingPath, manifest.AssemblyFile);
                }

                moduleInfo.IsInstalled = true;
                AddModule(moduleInfo);
            }
        }

        public override IEnumerable<ModuleInfo> CompleteListWithDependencies(IEnumerable<ModuleInfo> modules)
        {
            IEnumerable<ModuleInfo> result;

            try
            {
                var passedModules = modules as ModuleInfo[] ?? modules.ToArray();
                result = base.CompleteListWithDependencies(passedModules).ToArray();
            }
            catch (MissedModuleException)
            {
                // Do not throw if module was missing
                // Use ValidateDependencyGraph to validate & write and error of module missing
                result = Enumerable.Empty<ModuleInfo>();
            }

            return result;
        }

        protected override void ValidateDependencyGraph()
        {
            var modules = Modules.OfType<ManifestModuleInfo>();
            var manifestModules = modules as ManifestModuleInfo[] ?? modules.ToArray();
            try
            {
                base.ValidateDependencyGraph();
            }
            catch (MissedModuleException exception)
            {
                foreach (var module in manifestModules)
                {
                    if (exception.MissedDependenciesMatrix.Keys.Contains(module.ModuleName))
                    {
                        var errorMessage = $"A module declared a dependency on another module which is not declared to be loaded. Missing module(s): {string.Join(", ", exception.MissedDependenciesMatrix[module.ModuleName])}";
                        if (!module.Errors.Any(x => x.Contains(errorMessage)))
                        {
                            module.Errors.Add(errorMessage);
                        }
                    }
                }
            }
        }

        public override void Validate()
        {
            var modules = Modules.OfType<ManifestModuleInfo>();
            var manifestModules = modules as ManifestModuleInfo[] ?? modules.ToArray();

            base.Validate();

            //Dependencies and platform version validation
            foreach (var module in manifestModules)
            {
                //Check platform version
                if (!module.PlatformVersion.IsCompatibleWith(PlatformVersion.CurrentVersion))
                {
                    module.Errors.Add($"Module platform version {module.PlatformVersion} is incompatible with current {PlatformVersion.CurrentVersion}");
                }

                //Check that incompatible modules does not installed
                if (!module.Incompatibilities.IsNullOrEmpty())
                {
                    var installedIncompatibilities = manifestModules.Select(x => x.Identity).Join(module.Incompatibilities, x => x.Id, y => y.Id, (x, y) => new { x, y })
                                                            .Where(g => g.y.Version.IsCompatibleWith(g.x.Version)).Select(g => g.x)
                                                            .ToArray();
                    if (installedIncompatibilities.Any())
                    {
                        module.Errors.Add($"{ module } is incompatible with installed { string.Join(", ", installedIncompatibilities.Select(x => x.ToString())) }. You should uninstall these modules first.");
                    }
                }

                foreach (var declaredDependency in module.Dependencies)
                {
                    var installedDependency = manifestModules.FirstOrDefault(x => x.Id.EqualsInvariant(declaredDependency.Id));
                    if (installedDependency != null && !declaredDependency.Version.IsCompatibleWithBySemVer(installedDependency.Version))
                    {
                        module.Errors.Add($"Module dependency {declaredDependency} is incompatible with installed {installedDependency.Version}");
                    }
                }
            }
        }


        private IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            var result = new Dictionary<string, ModuleManifest>();

            if (Directory.Exists(_options.DiscoveryPath))
            {
                foreach (var manifestFile in Directory.EnumerateFiles(_options.DiscoveryPath, "module.manifest", SearchOption.AllDirectories))
                {
                    if (!manifestFile.Contains("artifacts"))
                    {
                        var manifest = ManifestReader.Read(manifestFile);
                        result.Add(manifestFile, manifest);
                    }
                }
            }
            return result;
        }

        private void CopyAssemblies(string sourceParentPath, string targetDirectoryPath)
        {
            if (sourceParentPath != null)
            {
                var sourceDirectoryPath = Path.Combine(sourceParentPath, "bin");

                if (Directory.Exists(sourceDirectoryPath))
                {
                    foreach (var sourceFilePath in Directory.EnumerateFiles(sourceDirectoryPath, "*.*", SearchOption.AllDirectories))
                    {
                        // Copy all assembly related files except assemblies that are inlcuded in TPA list
                        if (IsAssemblyRelatedFile(sourceFilePath) && !(IsAssemblyFile(sourceFilePath) && TPA.ContainsAssembly(Path.GetFileName(sourceFilePath))))
                        {
                            // Copy localization resource files to related subfolders
                            var targetFilePath = Path.Combine(
                                IsLocalizationFile(sourceFilePath) ? Path.Combine(targetDirectoryPath, Path.GetFileName(Path.GetDirectoryName(sourceFilePath)))
                                : targetDirectoryPath,
                                Path.GetFileName(sourceFilePath));
                            CopyFile(sourceFilePath, targetFilePath);
                        }
                    }
                }
            }
        }

        private void CopyFile(string sourceFilePath, string targetFilePath)
        {
            var sourceFileInfo = new FileInfo(sourceFilePath);
            var targetFileInfo = new FileInfo(targetFilePath);

            var sourceFileVersionInfo = FileVersionInfo.GetVersionInfo(sourceFilePath);
            var sourceVersion = new Version(sourceFileVersionInfo.FileMajorPart, sourceFileVersionInfo.FileMinorPart, sourceFileVersionInfo.FileBuildPart, sourceFileVersionInfo.FilePrivatePart);
            var targetVersion = sourceVersion;

            if (targetFileInfo.Exists)
            {
                var targetFileVersionInfo = FileVersionInfo.GetVersionInfo(targetFilePath);
                targetVersion = new Version(targetFileVersionInfo.FileMajorPart, targetFileVersionInfo.FileMinorPart, targetFileVersionInfo.FileBuildPart, targetFileVersionInfo.FilePrivatePart);
            }

            var versionsAreSameButLaterDate = (sourceVersion == targetVersion && targetFileInfo.Exists && sourceFileInfo.Exists && targetFileInfo.LastWriteTimeUtc < sourceFileInfo.LastWriteTimeUtc);
            if (!targetFileInfo.Exists || sourceVersion > targetVersion || versionsAreSameButLaterDate)
            {
                var targetDirectoryPath = Path.GetDirectoryName(targetFilePath);
                Directory.CreateDirectory(targetDirectoryPath);

                try
                {
                    File.Copy(sourceFilePath, targetFilePath, true);
                }
                catch (IOException)
                {
                    // VP-3719: Need to catch to avoid possible problem when different instances are trying to update the same file with the same version but different dates in the probing folder.
                    // We should not fail platform sart in that case - just add warning into the log. In case of unability to place newer version - should fail platform start.
                    if (versionsAreSameButLaterDate)
                    {
                        _logger.LogWarning($"File '{targetFilePath}' was not updated by '{sourceFilePath}' of the same version but later modified date, because probably it was used by another process");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private bool IsAssemblyRelatedFile(string path)
        {
            return _options.AssemblyFileExtensions.Union(_options.AssemblyServiceFileExtensions).Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsAssemblyFile(string path)
        {
            return _options.AssemblyFileExtensions.Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsLocalizationFile(string path)
        {
            return _options.LocalizationFileExtensions.Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetFileAbsoluteUri(string rootPath, string relativePath)
        {
            var builder = new UriBuilder
            {
                Host = string.Empty,
                Scheme = Uri.UriSchemeFile,
                Path = Path.GetFullPath(Path.Combine(rootPath, relativePath))
            };

            return builder.Uri.ToString();
        }
    }
}
