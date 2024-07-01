namespace VirtoCommerce.Platform.Core.Modularity;

public interface IFileCopyPolicy
{
    bool IsCopyRequired(bool is64process, string sourceFilePath, string targetFilePath, out FileCompareResult result);
}
