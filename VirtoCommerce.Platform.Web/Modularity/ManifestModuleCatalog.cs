using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Properties;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ManifestModuleCatalog : ModuleCatalog
    {
        private readonly string _modulesLocalPath;
        private readonly string _contentVirtualPath;
        private readonly string _assembliesPath;
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
                Directory.CreateDirectory(_assembliesPath);

            if (!contentPhysicalPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                contentPhysicalPath += "\\";

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

        public override void Validate()
        {

            base.Validate();

            var modules = Modules.OfType<ManifestModuleInfo>();
            //Dependencies and platform version validation
            foreach (var module in modules)
            {
                //Check platform version
                if (!module.PlatformVersion.IsCompatibleWith(PlatformVersion.CurrentVersion))
                {
                    module.Errors.Add(string.Format("module platform version {0} is incompatible with current {1}", module.PlatformVersion, PlatformVersion.CurrentVersion));
                }
           
                foreach (var declaredDependency in module.Dependencies)
                {
                    var installedDependency = modules.First(x => x.Id.EqualsInvariant(declaredDependency.Id));
                    if (!declaredDependency.Version.IsCompatibleWithBySemVer(installedDependency.Version))
                    {
                        module.Errors.Add(string.Format("module dependency {0} is incompatible with installed {1}", declaredDependency, installedDependency.Version));
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

        private static void CopyAssemblies(string sourceParentPath, string targetDirectoryPath)
        {
            if (sourceParentPath != null)
            {
                var sourceDirectoryPath = Path.Combine(sourceParentPath, "bin\\");

                if (Directory.Exists(sourceDirectoryPath))
                {
                    var sourceDirectoryUri = new Uri(sourceDirectoryPath);

                    foreach (var sourceFilePath in Directory.EnumerateFiles(sourceDirectoryPath))
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

        private void ConvertVirtualPath(IEnumerable<ManifestBundleItem> items, string moduleVirtualPath)
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
