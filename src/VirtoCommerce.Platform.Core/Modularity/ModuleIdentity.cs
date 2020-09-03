using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ModuleIdentity : ValueObject
    {
        public ModuleIdentity(string id, SemanticVersion version)
        {
            Id = id;
            Version = version;
        }    
     
        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }

        public override string ToString()
        {
            return $"{Id}:{Version.ToString()}";
        }
    }
}
