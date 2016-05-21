using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ModuleIdentity
    {
        public ModuleIdentity(string id, SemanticVersion version)
        {
            Id = id;
            Version = version;
        }
        public ModuleIdentity(string id, System.Version version)
              : this(id, new SemanticVersion(version))
        {
        }
        public ModuleIdentity(string id, string version)
            : this(id, new System.Version(version))
        {
        }

        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }

        public override string ToString()
        {
            return string.Join(" ", Id, Version);
        }

        public static bool operator ==(ModuleIdentity a, ModuleIdentity b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(ModuleIdentity a, ModuleIdentity b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to ModuleIdentity return false.
            ModuleIdentity other = obj as ModuleIdentity;
            if ((System.Object)other == null)
            {
                return false;
            }
            // Return true if the fields match:
            return (Id == other.Id) && (Version == other.Version);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

   
    }
}
