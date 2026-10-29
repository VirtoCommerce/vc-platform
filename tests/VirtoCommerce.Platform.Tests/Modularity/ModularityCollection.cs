using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

// Members set the static ModuleBootstrapper.Instance and PlatformVersion.CurrentVersion; AppManifestServiceTests resets the latter.
[CollectionDefinition(Name, DisableParallelization = true)]
public class ModularityCollection
{
    public const string Name = "Modularity";
}
