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
		public string AssembliesPath { get; set; }
		public string ContentPath { get; set; }

		public static IEnumerable<ModuleManifest> GetModuleManifests(string rootPath)
		{
			IEnumerable<ModuleManifest> result;

			if (Directory.Exists(rootPath))
				result = Directory.EnumerateFiles(rootPath, "module.manifest", SearchOption.AllDirectories).Select(ManifestReader.Read);
			else
				result = Enumerable.Empty<ModuleManifest>();

			return result;
		}

		protected override void InnerLoad()
		{
			if (string.IsNullOrEmpty(AssembliesPath))
				throw new InvalidOperationException(Resources.AssembliesPathCannotBeNullOrEmpty);
			if (string.IsNullOrEmpty(ContentPath))
				throw new InvalidOperationException(Resources.ContentPathCannotBeNullOrEmpty);

			if (!Directory.Exists(AssembliesPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, AssembliesPath));
			if (!Directory.Exists(ContentPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, ContentPath));

			foreach (var manifest in GetModuleManifests(ContentPath))
			{
				var moduleInfo = new ManifestModuleInfo(manifest);

				// Modules without assembly file don't need initialization
				if (string.IsNullOrEmpty(manifest.AssemblyFile))
					moduleInfo.State = ModuleState.Initialized;
				else
					moduleInfo.Ref = GetFileAbsoluteUri(manifest.AssemblyFile);

				AddModule(moduleInfo);
			}
		}


		private string GetFileAbsoluteUri(string filePath)
		{
			var builder = new UriBuilder
			{
				Host = string.Empty,
				Scheme = Uri.UriSchemeFile,
				Path = Path.GetFullPath(Path.Combine(AssembliesPath, filePath))
			};

			return builder.Uri.ToString();
		}
	}
}
