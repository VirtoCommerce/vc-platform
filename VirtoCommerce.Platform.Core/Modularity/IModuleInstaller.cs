using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.Packaging
{
    public interface IModuleInstaller
    {
        void Install(ManifestModuleInfo moduleInfo, IProgress<ProgressMessage> progress);
        void Uninstall(ManifestModuleInfo moduleInfo, IProgress<ProgressMessage> progress);
    }
}
