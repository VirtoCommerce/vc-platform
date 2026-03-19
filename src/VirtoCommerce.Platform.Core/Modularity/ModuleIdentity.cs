using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity;

public class ModuleIdentity : ValueObject
{
    public ModuleIdentity(string id, SemanticVersion version) : this(id, version, false)
    {
    }

    public ModuleIdentity(string id, SemanticVersion version, bool optional)
    {
        Id = id;
        Version = version;
        Optional = optional;
    }

    public string Id { get; private set; }
    public SemanticVersion Version { get; private set; }
    public bool Optional { get; set; }

    public override string ToString()
    {
        return $"{Id}:{Version}";
    }
}
