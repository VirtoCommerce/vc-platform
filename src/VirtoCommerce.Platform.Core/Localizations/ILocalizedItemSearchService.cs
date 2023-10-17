using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Localizations;

public interface ILocalizedItemSearchService : ISearchService<LocalizedItemSearchCriteria, LocalizedItemSearchResult, LocalizedItem>
{
}
