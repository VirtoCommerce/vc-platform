using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class OuterEntityServiceMock(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
        : OuterEntityService<TestModel, TestEntity, TestChangeEvent, TestChangedEvent>(repositoryFactory, platformMemoryCache, eventPublisher)
    {
        protected override Task<IList<TestEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
        {
            IList<TestEntity> entities = ((DbContextUnitOfWork)repository.UnitOfWork).DbContext.Set<TestEntity>().Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(entities);
        }
    }
}
