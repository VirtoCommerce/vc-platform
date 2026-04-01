using System;
using System.IO;

namespace VirtoCommerce.Platform.Modules.External
{
    [Obsolete("Use ModulePackageInstaller and ModuleBootstrapper classes instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IExternalModulesClient
    {
        Stream OpenRead(Uri address);
    }
}
