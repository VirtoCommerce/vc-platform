namespace VirtoCommerce.Platform.Core.Modularity;

public class FileCompareResult
{
    public bool NewFile { get; set; }
    public bool NewDate { get; set; }

    public bool SameVersion { get; set; }
    public bool NewVersion { get; set; }
    public bool SameOrNewVersion => SameVersion || NewVersion;

    public bool CompatibleArchitecture { get; set; }
    public bool SameArchitecture { get; set; }
    public bool NewArchitecture { get; set; }
    public bool SameOrNewArchitecture => SameArchitecture || NewArchitecture;
}
