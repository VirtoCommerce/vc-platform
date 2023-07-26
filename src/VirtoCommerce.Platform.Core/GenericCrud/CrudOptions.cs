namespace VirtoCommerce.Platform.Core.GenericCrud;

public class CrudOptions
{
    /// <summary>
    /// The maximum value of skip + take for search requests
    /// </summary>
    public int MaxResultWindow { get; set; } = int.MaxValue;
}
