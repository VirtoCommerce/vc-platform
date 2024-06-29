using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface IFileCopyPolicy
{
    public bool IsCopyRequired(Architecture environment, string sourceFilePath, string targetFilePath, out FileCompareResult result);
}
