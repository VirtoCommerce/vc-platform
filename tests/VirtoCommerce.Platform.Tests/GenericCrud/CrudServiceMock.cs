using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class CrudServiceMock : CrudService<TestModel, TestEntity, TestChangeEvent, TestChangedEvent>
    {
        public CrudServiceMock(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
            : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
        }

        protected override Task<IEnumerable<TestEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return Task.FromResult(ids.Select(x => AbstractTypeFactory<TestEntity>.TryCreateInstance().FromModel(new TestModel { Id = x }, new PrimaryKeyResolvingMap())));
        }
    }
}
