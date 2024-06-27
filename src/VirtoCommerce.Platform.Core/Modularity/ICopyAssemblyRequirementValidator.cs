using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Core.Modularity;

public interface ICopyAssemblyRequirementValidator
{
    public CopyAssemblyRequirementResult RequireCopy(Architecture architecture, string sourceFilePath, string targetFilePath);
}
