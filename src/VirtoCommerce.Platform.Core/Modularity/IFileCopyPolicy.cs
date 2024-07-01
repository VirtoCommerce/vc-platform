using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface IFileCopyPolicy
{
    bool IsCopyRequired(Architecture environment, string sourceFilePath, string targetFilePath, out FileCompareResult result);
}
