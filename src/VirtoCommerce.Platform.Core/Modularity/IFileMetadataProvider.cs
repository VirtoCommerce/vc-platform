using System;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IFileMetadataProvider
    {
        bool Exists(string filePath);
        DateTime? GetDate(string filePath);
        public Version GetVersion(string filePath);
        public Architecture? GetArchitecture(string filePath);
    }
}
