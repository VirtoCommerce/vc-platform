using System;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IAssemblyMetadataProvider
    {
        public Version GetVersion(string filePath);
        public Architecture? GetArchitecture(string filePath);
        bool IsManaged(string filePath);
    }
}
