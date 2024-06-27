namespace VirtoCommerce.Platform.Core.Modularity;

public class CopyFileRequirementResult
{
    public bool CopyRequired
    {
        get => NoTarget == CopyFileNecessary.Yes
               || IsVersion == CopyFileNecessary.Yes
               || IsBitness == CopyFileNecessary.Yes
               || IsSourceNewByDate == CopyFileNecessary.Yes;
    }

    public CopyFileNecessary NoTarget { get; set; }
    public CopyFileNecessary IsVersion { get; set; }
    public CopyFileNecessary IsBitness { get; set; }
    public CopyFileNecessary IsSourceNewByDate { get; set; }
}

public enum CopyFileNecessary
{
    Unknown, Yes, No
}
