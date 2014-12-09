using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Framework.Web.Properties;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestModuleCatalog : ModuleCatalog
	{
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

			if (!Directory.Exists(AssembliesPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, AssembliesPath));
			if (!Directory.Exists(ContentPhysicalPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, ContentPhysicalPath));

			ContentVirtualPath = ContentVirtualPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			if (!ContentPhysicalPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
				ContentPhysicalPath += "\\";

			var rootUri = new Uri(ContentPhysicalPath);

			foreach (var pair in GetModuleManifests(ContentPhysicalPath))
			{
				var manifest = pair.Value;
				var manifestPath = pair.Key;

				var moduleVirtualPath = GetModuleVirtualPath(rootUri, manifestPath);
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

		private string GetModuleVirtualPath(Uri rootUri, string manifestPath)
		{
			var modulePath = Path.GetDirectoryName(manifestPath);
			var moduleUri = new Uri(modulePath);
			var moduleRelativePath = rootUri.MakeRelativeUri(moduleUri).ToString();
			var moduleVirtualPath = string.Join("/", ContentVirtualPath, moduleRelativePath);

			return moduleVirtualPath;
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
