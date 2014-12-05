using System;
using System.Globalization;
using System.IO;
using VirtoCommerce.Framework.Web.Properties;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestModuleCatalog : ModuleCatalog
	{
		public string AssembliesPath { get; set; }
		public string ContentPath { get; set; }

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

			foreach (var manifestPath in Directory.GetFiles(ContentPath, "module.manifest", SearchOption.AllDirectories))
			{
				var manifest = ManifestReader.Read(manifestPath);

				if (manifest != null)
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
