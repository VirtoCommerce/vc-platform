using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface IFileCopyPolicy
{
    string GetTargetRelativePath(string sourceRelativeFilePath);
    bool IsCopyRequired(string sourceFilePath, string targetFilePath, Architecture environmentArchitecture, out FileCompareResult result);
}
