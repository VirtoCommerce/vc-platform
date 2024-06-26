namespace VirtoCommerce.Platform.Core.Modularity;

public class RequireCopyFileReason
{
    public bool CopyRequired
    {
        get => NoTarget || Version == true || Bitwise == true || LaterDate == true;
    }

    public bool NoTarget { get; set; }
    public bool? Version { get; set; }
    public bool? Bitwise { get; set; }
    public bool? LaterDate { get; set; }
}
