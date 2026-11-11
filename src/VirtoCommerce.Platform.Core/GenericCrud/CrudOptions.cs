namespace VirtoCommerce.Platform.Core.GenericCrud;

public class CrudOptions
{
    /// <summary>
    /// The maximum value of skip + take for search requests
    /// </summary>
    public int MaxResultWindow { get; set; } = int.MaxValue;

    /// <summary>
    /// The number of models loaded per batch when enumerating all search results
    /// via optimized IExtendedSearchService implementations
    /// </summary>
    public int SearchAllBatchSize { get; set; } = 500;
}
