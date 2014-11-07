using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestModuleInfo : ModuleInfo
	{
		public ICollection<ManifestBundleItem> Styles { get; private set; }
		public ICollection<ManifestBundleItem> Scripts { get; private set; }

		public ManifestModuleInfo()
			: this(null, null, null, null, null)
		{
		}

		public ManifestModuleInfo(string name, string type, string[] dependsOn, IEnumerable<ManifestBundleItem> styles, IEnumerable<ManifestBundleItem> scripts)
			: base(name, type, dependsOn)
		{
			InitializationMode = InitializationMode.OnDemand;

			Styles = new List<ManifestBundleItem>();
			Scripts = new List<ManifestBundleItem>();

			if (styles != null)
				styles.ForEach(s => Styles.Add(s));

			if (scripts != null)
				scripts.ForEach(s => Scripts.Add(s));
		}
	}
}
