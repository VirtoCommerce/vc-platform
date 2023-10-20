using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Localizations;

public class LocalizedItemService : CrudService<LocalizedItem, LocalizedItemEntity, LocalizedItemChangingEvent, LocalizedItemChangedEvent>, ILocalizedItemService
{
    public LocalizedItemService(
        Func<IPlatformRepository> repositoryFactory,
        IPlatformMemoryCache platformMemoryCache,
        IEventPublisher eventPublisher)
        : base(repositoryFactory, platformMemoryCache, eventPublisher)
    {
    }

    protected override async Task<IList<LocalizedItemEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        if (ids.IsNullOrEmpty())
        {
            return Array.Empty<LocalizedItemEntity>();
        }

        var query = ((IPlatformRepository)repository).LocalizedItems;

        query = ids.Count == 1
           ? query.Where(x => x.Id == ids.First())
           : query.Where(x => ids.Contains(x.Id));

        return await query.ToListAsync();
    }
}
