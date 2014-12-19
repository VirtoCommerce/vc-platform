using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VirtoCommerce.Framework.Web.Properties;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestModuleCatalog : ModuleCatalog
	{
		private static readonly string[] _assemblyFileExtensions = { ".dll", ".pdb", ".exe" };

		public string AssembliesPath { get; set; }
		public string ContentVirtualPath { get; set; }
		public string ContentPhysicalPath { get; set; }

		public static IDictionary<string, ModuleManifest> GetModuleManifests(string rootPath)
		{
			var result = new Dictionary<string, ModuleManifest>();

			if (Directory.Exists(rootPath))
				result = Directory.EnumerateFiles(rootPath, "module.manifest", SearchOption.AllDirectories).ToDictionary(path => path, ManifestReader.Read);

			return result;
		}

		protected override void InnerLoad()
		{
			if (string.IsNullOrEmpty(AssembliesPath))
				throw new InvalidOperationException(Resources.AssembliesPathCannotBeNullOrEmpty);
			if (string.IsNullOrEmpty(ContentVirtualPath))
				throw new InvalidOperationException(Resources.ContentPathCannotBeNullOrEmpty);
			if (string.IsNullOrEmpty(ContentPhysicalPath))
				throw new InvalidOperationException(Resources.ContentPathCannotBeNullOrEmpty);

			if (!Directory.Exists(ContentPhysicalPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, ContentPhysicalPath));

			if (!Directory.Exists(AssembliesPath))
				Directory.CreateDirectory(AssembliesPath);

			ContentVirtualPath = ContentVirtualPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			if (!ContentPhysicalPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
				ContentPhysicalPath += "\\";

			var rootUri = new Uri(ContentPhysicalPath);

			foreach (var pair in GetModuleManifests(ContentPhysicalPath))
			{
				var manifest = pair.Value;
				var manifestPath = pair.Key;

				var modulePath = Path.GetDirectoryName(manifestPath);
				CopyAssemblies(Path.Combine(modulePath, "bin\\"), AssembliesPath);

				var moduleVirtualPath = GetModuleVirtualPath(rootUri, modulePath);
				ConvertVirtualPath(manifest.Scripts, moduleVirtualPath);
				ConvertVirtualPath(manifest.Styles, moduleVirtualPath);

				var moduleInfo = new ManifestModuleInfo(manifest);

				// Modules without assembly file don't need initialization
				if (string.IsNullOrEmpty(manifest.AssemblyFile))
					moduleInfo.State = ModuleState.Initialized;
				else
					moduleInfo.Ref = GetFileAbsoluteUri(AssembliesPath, manifest.AssemblyFile);

				AddModule(moduleInfo);
			}
		}


		private static void CopyAssemblies(string sourceDirectoryPath, string targetDirectoryPath)
		{
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
			var moduleVirtualPath = string.Join("/", ContentVirtualPath, moduleRelativePath);

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
