using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.Packaging
{
    public interface IModuleInstaller
    {
        void Install(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);
        void Uninstall(IEnumerable<ManifestModuleInfo> modules, IProgress<ProgressMessage> progress);
    }
}
