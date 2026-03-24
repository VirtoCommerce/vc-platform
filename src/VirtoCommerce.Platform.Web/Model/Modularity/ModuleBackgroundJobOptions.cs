using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Modularity;

public class ModuleBackgroundJobOptions
{
    public ModuleAction Action { get; set; }
    public ModuleInstallRequest[] Modules { get; set; }
}
