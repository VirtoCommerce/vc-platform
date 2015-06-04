using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Platform.Core.Properties;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleCatalog : ModuleCatalog
    {
        private readonly string _contentVirtualPath;
        private readonly string _assembliesPath;
        private static readonly string[] _assemblyFileExtensions = { ".dll", ".pdb", ".exe" };

        public IModuleManifestProvider ManifestProvider { get; private set; }

        public ManifestModuleCatalog(IModuleManifestProvider manifestProvider, string contentVirtualPath, string assembliesPath)
        {
            ManifestProvider = manifestProvider;
            if (contentVirtualPath != null)
            {
                _contentVirtualPath = contentVirtualPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            _assembliesPath = assembliesPath;
        }

        protected override void InnerLoad()
        {
            var contentPhysicalPath = ManifestProvider.RootPath;

            if (string.IsNullOrEmpty(_assembliesPath))
                throw new InvalidOperationException(Resources.AssembliesPathCannotBeNullOrEmpty);
            if (string.IsNullOrEmpty(_contentVirtualPath))
                throw new InvalidOperationException(Resources.ContentPathCannotBeNullOrEmpty);
            if (string.IsNullOrEmpty(contentPhysicalPath))
                throw new InvalidOperationException(Resources.ContentPathCannotBeNullOrEmpty);

            if (!Directory.Exists(_assembliesPath))
                Directory.CreateDirectory(_assembliesPath);

            if (!contentPhysicalPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                contentPhysicalPath += "\\";

            var rootUri = new Uri(contentPhysicalPath);

            CopyAssemblies(ManifestProvider.RootPath, _assembliesPath);

            foreach (var pair in ManifestProvider.GetModuleManifests())
            {
                var manifest = pair.Value;
                var manifestPath = pair.Key;

                var modulePath = Path.GetDirectoryName(manifestPath);
                CopyAssemblies(modulePath, _assembliesPath);

                var moduleVirtualPath = GetModuleVirtualPath(rootUri, modulePath);
                ConvertVirtualPath(manifest.Scripts, moduleVirtualPath);
                ConvertVirtualPath(manifest.Styles, moduleVirtualPath);

                var moduleInfo = new ManifestModuleInfo(manifest, modulePath);

                // Modules without assembly file don't need initialization
                if (string.IsNullOrEmpty(manifest.AssemblyFile))
                    moduleInfo.State = ModuleState.Initialized;
                else
                    moduleInfo.Ref = GetFileAbsoluteUri(_assembliesPath, manifest.AssemblyFile);

                AddModule(moduleInfo);
            }
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

            if (!targetFileInfo.Exists || targetFileInfo.LastWriteTimeUtc < sourceFileInfo.LastWriteTimeUtc)
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
