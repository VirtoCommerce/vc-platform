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

        public int BeforeAndAfterSaveChangesCalled { get; set; }
        public bool SoftDeleteCalled { get; set; }
        public bool AfterDeleteAsyncCalled { get; set; }

        protected override Task<IEnumerable<TestEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return Task.FromResult(ids.Select(x => AbstractTypeFactory<TestEntity>.TryCreateInstance().FromModel(new TestModel { Id = x }, new PrimaryKeyResolvingMap())));
        }

        protected override TestModel ProcessModel(string responseGroup, TestEntity entity, TestModel model)
        {
            model.Name = "ProcessModelCalled";
            return model;
        }

        protected override Task BeforeSaveChanges(IEnumerable<TestModel> models)
        {
            BeforeAndAfterSaveChangesCalled++;
            return Task.CompletedTask;
        }

        protected override Task AfterSaveChangesAsync(IEnumerable<TestModel> models, IEnumerable<GenericChangedEntry<TestModel>> changedEntries)
        {
            BeforeAndAfterSaveChangesCalled++;
            return Task.CompletedTask;
        }

        protected override Task SoftDelete(IRepository repository, IEnumerable<string> ids)
        {
            SoftDeleteCalled = true;
            return Task.CompletedTask;
        }

        protected override Task AfterDeleteAsync(IEnumerable<TestModel> models, IEnumerable<GenericChangedEntry<TestModel>> changedEntries)
        {
            AfterDeleteAsyncCalled = true;
            return Task.CompletedTask;
        }
    }
}
