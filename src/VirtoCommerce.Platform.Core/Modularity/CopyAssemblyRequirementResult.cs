namespace VirtoCommerce.Platform.Core.Modularity;

public class CopyAssemblyRequirementResult
{
    public bool CopyRequired
    {
        get => NoTarget == CopyAssemblyNecessity.Yes
               || IsVersion == CopyAssemblyNecessity.Yes
               || IsBitness == CopyAssemblyNecessity.Yes
               || IsSourceNewByDate == CopyAssemblyNecessity.Yes;
    }

    public CopyAssemblyNecessity NoTarget { get; set; }
    public CopyAssemblyNecessity IsVersion { get; set; }
    public CopyAssemblyNecessity IsBitness { get; set; }
    public CopyAssemblyNecessity IsSourceNewByDate { get; set; }
}

public enum CopyAssemblyNecessity
{
    Unknown, Yes, No
}
