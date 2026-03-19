using System;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IFileMetadataProvider
    {
        bool Exists(string filePath);
        DateTime? GetDate(string filePath);
        Version GetVersion(string filePath);
        Architecture? GetArchitecture(string filePath);
    }
}
