using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Properties;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestModuleCatalog : ModuleCatalog
	{
		public string ModulesPath { get; set; }

		protected override void InnerLoad()
		{
			if (string.IsNullOrEmpty(ModulesPath))
				throw new InvalidOperationException(Resources.ModulePathCannotBeNullOrEmpty);

			if (!Directory.Exists(ModulesPath))
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, ModulesPath));

			foreach (var manifest in Directory.GetFiles(ModulesPath, "*.manifest"))
			{
				var doc = XDocument.Load(manifest);
				var module = doc.XPathSelectElement("/module");
				var moduleName = GetAttributeValue(module, "moduleName");
				var assemblyFile = GetAttributeValue(module, "assemblyFile");
				var moduleType = GetAttributeValue(module, "moduleType");

				var dependsOn = GetAttributeValues(module, "dependencies/dependency", "moduleName");
				var styles = GetBundleItems(module, "styles");
				var scripts = GetBundleItems(module, "scripts");

				var moduleInfo = new ManifestModuleInfo(moduleName, moduleType, dependsOn, styles, scripts);

				// Modules without assembly file don't need initialization
				if (string.IsNullOrEmpty(assemblyFile))
					moduleInfo.State = ModuleState.Initialized;
				else
					moduleInfo.Ref = GetFileAbsoluteUri(assemblyFile);

				AddModule(moduleInfo);
			}
		}


		private IEnumerable<ManifestBundleItem> GetBundleItems(XNode parent, string bundleName)
		{
			var items = new List<ManifestBundleItem>();

			var bundle = parent.XPathSelectElement(bundleName);

			if (bundle != null)
			{
				foreach (var element in bundle.Elements())
				{
					var virtualPath = GetAttributeValue(element, "virtualPath");

					switch (element.Name.LocalName)
					{
						case "file":
							items.Add(new ManifestBundleFile { VirtualPath = virtualPath });
							break;
						case "directory":
							var searchPattern = GetAttributeValue(element, "searchPattern");
							var searchSubdirectoriesString = GetAttributeValue(element, "searchSubdirectories");
							var searchSubdirectories = string.Equals(searchSubdirectoriesString, "true", StringComparison.OrdinalIgnoreCase);

							items.Add(new ManifestBundleDirectory { VirtualPath = virtualPath, SearchPattern = searchPattern, SearchSubdirectories = searchSubdirectories });
							break;
					}
				}
			}

			return items;
		}

		private string GetFileAbsoluteUri(string filePath)
		{
			var builder = new UriBuilder
			{
				Host = string.Empty,
				Scheme = Uri.UriSchemeFile,
				Path = Path.GetFullPath(Path.Combine(ModulesPath, filePath))
			};

			return builder.Uri.ToString();
		}

		private string GetAttributeValue(XElement element, string name)
		{
			return element.Attributes()
				.Where(a => a.Name == name)
				.Select(a => a.Value)
				.FirstOrDefault();
		}

		private string[] GetAttributeValues(XNode parentNode, string childElementsPath, string attributeName)
		{
			return parentNode.XPathSelectElements(childElementsPath)
				.SelectMany(d => d.Attributes())
				.Where(a => a.Name == attributeName)
				.Select(a => a.Value)
				.ToArray();
		}
	}
}
