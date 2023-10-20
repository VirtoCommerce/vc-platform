using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Localizations;

public class LocalizedItemSearchService : SearchService<LocalizedItemSearchCriteria, LocalizedItemSearchResult, LocalizedItem, LocalizedItemEntity>, ILocalizedItemSearchService
{
    public LocalizedItemSearchService(
        Func<IPlatformRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        ILocalizedItemService crudService,
        IOptions<CrudOptions> crudOptions)
        : base(repositoryFactory, platformMemoryCache, crudService, crudOptions)
    {
    }

    protected override IQueryable<LocalizedItemEntity> BuildQuery(IRepository repository, LocalizedItemSearchCriteria criteria)
    {
        var query = ((IPlatformRepository)repository).LocalizedItems;

        if (criteria.Names != null)
        {
            query = criteria.Names.Count == 1
                ? query.Where(x => x.Name == criteria.Names.First())
                : query.Where(x => criteria.Names.Contains(x.Name));
        }

        if (criteria.Aliases != null)
        {
            query = criteria.Aliases.Count == 1
                ? query.Where(x => x.Alias == criteria.Aliases.First())
                : query.Where(x => criteria.Aliases.Contains(x.Alias));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(LocalizedItemSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos = new[]
            {
                new SortInfo { SortColumn = nameof(LocalizedItemEntity.Name) },
                new SortInfo { SortColumn = nameof(LocalizedItemEntity.Alias) },
                new SortInfo { SortColumn = nameof(LocalizedItemEntity.LanguageCode) },
            };
        }

        return sortInfos;
    }
}
