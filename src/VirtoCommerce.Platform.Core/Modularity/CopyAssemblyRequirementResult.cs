namespace VirtoCommerce.Platform.Core.Modularity;

public class CopyAssemblyRequirementResult
{
    public bool CopyRequired
    {
        get => NoTarget == AssemblyCopyRequirement.Required
               || IsVersion == AssemblyCopyRequirement.Required
               || IsBitness == AssemblyCopyRequirement.Required
               || IsSourceNewByDate == AssemblyCopyRequirement.Required;
    }

    public AssemblyCopyRequirement NoTarget { get; set; }
    public AssemblyCopyRequirement IsVersion { get; set; }
    public AssemblyCopyRequirement IsBitness { get; set; }
    public AssemblyCopyRequirement IsSourceNewByDate { get; set; }
}

public enum AssemblyCopyRequirement
{
    Unknown, Required, Prohibited
}

