namespace VirtoCommerce.Platform.Core.DeveloperTools;

public class DeveloperToolDescriptor
{
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsExternal { get; set; }
    public int SortOrder { get; set; }
    public string Permission { get; set; }
}
