using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface ICopyFileRequirementValidator
{
    public CopyFileRequirementResult RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath);
}
