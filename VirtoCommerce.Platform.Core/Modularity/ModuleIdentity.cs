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
        public ModuleIdentity(string id, string version)
        {
            Id = id;
            Version = version;
        }
        public string Id { get; private set; }
        public string Version { get; private set; }

        public SemanticVersion SemanticVersion
        {
            get
            {
                SemanticVersion retVal = null;
                if (Version != null)
                {
                    Version version;
                    if (System.Version.TryParse(Version, out version))
                    {
                        retVal = new SemanticVersion(version);
                    }
                }
                return retVal;
            }
        }

        public static ModuleIdentity Parse(string str)
        {
            if(str == null)
            {
                throw new ArgumentNullException("str");
            }
            ModuleIdentity retVal = null;
            var parts = str.Split(':');
            if(parts.Length > 1)
            {
                retVal = new ModuleIdentity(parts[0], parts[1]);
            }
            return retVal;
        }

        public override string ToString()
        {
            return String.Join(":", Id, Version);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
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
