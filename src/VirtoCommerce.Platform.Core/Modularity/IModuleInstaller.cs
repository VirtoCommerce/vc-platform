using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity
{
    [Obsolete("Use ModulePackageInstaller and ModuleDiscovery classes instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IModuleInstaller
    {
        void Install(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);
        void Uninstall(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);
    }
}
