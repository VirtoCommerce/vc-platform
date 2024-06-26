using System;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface ICopyFilePolicy
{
    public RequireCopyFileReason RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath);
}

public interface ILibraryVersionProvider
{
    public Version GetFileVersion(string filePath);
    public Architecture? GetArchitecture(string filePath);
    bool IsManagedLibrary(string filePath);
}
