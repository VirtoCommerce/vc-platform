using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Web.Resources;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ManifestModuleCatalog : ModuleCatalog
    {
        private readonly string _modulesLocalPath;
        private readonly string _contentVirtualPath;
        private readonly string _assembliesPath;
        private static readonly string _localizationFilePattern = "*.resources.dll";
        private static readonly string[] _assemblyFileExtensions = { ".dll", ".pdb", ".exe", ".xml" };

        public ManifestModuleCatalog(string modulesLocalPath, string contentVirtualPath, string assembliesPath)
        {
            _modulesLocalPath = modulesLocalPath;
            if (contentVirtualPath != null)
            {
                _contentVirtualPath = contentVirtualPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            _assembliesPath = assembliesPath;
        }

        protected override void InnerLoad()
        {
            var contentPhysicalPath = _modulesLocalPath;

            if (string.IsNullOrEmpty(_assembliesPath))
                throw new InvalidOperationException("The AssembliesPath cannot contain a null value or be empty");
            if (string.IsNullOrEmpty(_contentVirtualPath))
                throw new InvalidOperationException("The ContentVirtualPath cannot contain a null value or be empty");
            if (string.IsNullOrEmpty(contentPhysicalPath))
                throw new InvalidOperationException("The ContentPhysicalPath cannot contain a null value or be empty");

            if (!Directory.Exists(_assembliesPath))
            {
                Directory.CreateDirectory(_assembliesPath);
            }
            var separator = Path.DirectorySeparatorChar.ToString();

            if (!contentPhysicalPath.EndsWith(separator, StringComparison.OrdinalIgnoreCase))
            {
                contentPhysicalPath += separator;
            }

            var rootUri = new Uri(contentPhysicalPath);

            CopyAssemblies(_modulesLocalPath, _assembliesPath);

            foreach (var pair in GetModuleManifests())
            {
                var manifest = pair.Value;
                var manifestPath = pair.Key;

                var modulePath = Path.GetDirectoryName(manifestPath);

                CopyAssemblies(modulePath, _assembliesPath);

                var moduleVirtualPath = GetModuleVirtualPath(rootUri, modulePath);
                ConvertVirtualPath(manifest.Scripts, moduleVirtualPath);
                ConvertVirtualPath(manifest.Styles, moduleVirtualPath);

                var moduleInfo = new ManifestModuleInfo(manifest) { FullPhysicalPath = Path.GetDirectoryName(manifestPath) };

                // Modules without assembly file don't need initialization
                if (string.IsNullOrEmpty(manifest.AssemblyFile))
                    moduleInfo.State = ModuleState.Initialized;
                else
                    moduleInfo.Ref = GetFileAbsoluteUri(_assembliesPath, manifest.AssemblyFile);

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
                        module.Errors.Add(string.Format(ModularityResources.DependencyOnMissingModule, string.Join(", ", exception.MissedDependenciesMatrix[module.ModuleName])));
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
                    module.Errors.Add(string.Format(ModularityResources.PlatformVersionIsIncompatible, module.PlatformVersion, PlatformVersion.CurrentVersion));
                }

                //Check that incompatible modules does not installed
                if (!module.Incompatibilities.IsNullOrEmpty())
                {
                    var installedIncompatibilities = manifestModules.Select(x => x.Identity).Join(module.Incompatibilities, x => x.Id, y => y.Id, (x, y) => new { x, y })
                                                            .Where(g => g.y.Version.IsCompatibleWith(g.x.Version)).Select(g => g.x)
                                                            .ToArray();
                    if (installedIncompatibilities.Any())
                    {
                        module.Errors.Add(string.Format(ModularityResources.ModuleIsIncompatible, module, string.Join(", ", installedIncompatibilities.Select(x => x.ToString()))));
                    }
                }

                foreach (var declaredDependency in module.Dependencies)
                {
                    var installedDependency = manifestModules.FirstOrDefault(x => x.Id.EqualsInvariant(declaredDependency.Id));
                    if (installedDependency != null && !declaredDependency.Version.IsCompatibleWithBySemVer(installedDependency.Version))
                    {
                        module.Errors.Add(string.Format(ModularityResources.ModuleDependencyIsIncompatible, declaredDependency, installedDependency.Version));
                    }
                }
            }
        }


        private IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            var result = new Dictionary<string, ModuleManifest>();

            if (Directory.Exists(_modulesLocalPath))
            {
                foreach (var manifestFile in Directory.EnumerateFiles(_modulesLocalPath, "module.manifest", SearchOption.AllDirectories))
                {
                    var manifest = ManifestReader.Read(manifestFile);
                    result.Add(manifestFile, manifest);
                }
            }
            return result;
        }

        private static List<string> GetAssembliesFiles(string sourceDirectoryPath)
        {
            var result = new List<string>();

            result.AddRange(Directory.EnumerateFiles(sourceDirectoryPath));
            result.AddRange(Directory.EnumerateDirectories(sourceDirectoryPath).SelectMany(
                directory => Directory.EnumerateFiles(directory, _localizationFilePattern)));

            return result;
        }

        private static void CopyAssemblies(string sourceParentPath, string targetDirectoryPath)
        {
            if (sourceParentPath != null)
            {
                var separator = Path.DirectorySeparatorChar;
                var sourceDirectoryPath = Path.Combine(sourceParentPath, $"bin{separator}");

                if (Directory.Exists(sourceDirectoryPath))
                {
                    var sourceDirectoryUri = new Uri(sourceDirectoryPath);

                    foreach (var sourceFilePath in GetAssembliesFiles(sourceDirectoryPath))
                    {
                        if (IsAssemblyFile(sourceFilePath))
                        {
                            var relativePath = MakeRelativePath(sourceDirectoryUri, sourceFilePath);
                            var targetFilePath = Path.Combine(targetDirectoryPath, relativePath);

                            CopyFile(sourceFilePath, targetFilePath);
                        }
                    }
                }
            }
        }

        private static void CopyFile(string sourceFilePath, string targetFilePath)
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

            if (!targetFileInfo.Exists || sourceVersion > targetVersion || (sourceVersion == targetVersion && targetFileInfo.LastWriteTimeUtc < sourceFileInfo.LastWriteTimeUtc))
            {
                var targetDirectoryPath = Path.GetDirectoryName(targetFilePath);
                Directory.CreateDirectory(targetDirectoryPath);
                File.Copy(sourceFilePath, targetFilePath, true);
            }
        }

        private static bool IsAssemblyFile(string path)
        {
            var fileExtension = Path.GetExtension(path);
            return fileExtension != null && _assemblyFileExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        private string GetModuleVirtualPath(Uri rootUri, string modulePath)
        {
            var moduleRelativePath = MakeRelativePath(rootUri, modulePath);
            var moduleVirtualPath = string.Join("/", _contentVirtualPath, moduleRelativePath);

            return moduleVirtualPath;
        }

        private static string MakeRelativePath(Uri rootUri, string fullPath)
        {
            var fullUri = new Uri(fullPath);
            var relativePath = rootUri.MakeRelativeUri(fullUri).ToString();
            return relativePath;
        }

        private static void ConvertVirtualPath(IEnumerable<ManifestBundleItem> items, string moduleVirtualPath)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    const string moduleRoot = "$/";
                    if (item.VirtualPath.StartsWith(moduleRoot, StringComparison.OrdinalIgnoreCase))
                        item.VirtualPath = string.Join("/", moduleVirtualPath, item.VirtualPath.Substring(moduleRoot.Length));
                }
            }
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
