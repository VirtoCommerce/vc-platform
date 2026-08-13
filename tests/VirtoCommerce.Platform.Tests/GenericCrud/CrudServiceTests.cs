using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Tests.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class CrudServiceTests
    {
        private readonly Mock<IEventPublisher> _eventPublisherMock = new();
        private readonly Mock<ITestRepository> _repositoryMock = new();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

        [Fact]
        public async Task GetAsync_getById_returnTestModel()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var testModels = new List<TestModel> { new() { Id = "1" } };
            var service = GetCrudServiceMock();

            // Act
            var getAsync = await service.GetAsync(ids);

            // Assert
            Assert.Equal(testModels, getAsync);
        }

        [Fact]
        public async Task GetAsync__ProcessModelCalled()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var service = GetCrudServiceMock();

            // Act
            var getAsync = await service.GetAsync(ids);

            // Assert
            Assert.Equal([new() { Id = "1", Name = "ProcessModelCalled" }], getAsync);
        }

        [Fact]
        public async Task SaveChangesAsync_saveChanges_returnChangedEntries()
        {
            // Arrange
            var testModels = new List<TestModel> { new() { Id = "1", Name = "Test" } };
            var service = GetCrudServiceMock();

            // Act
            await service.SaveChangesAsync(testModels);

            // Assert
            Assert.Equal(testModels.FirstOrDefault(), TestChangedEvent.testChangedEntries.First().NewEntry);
            Assert.Equal(2, service.BeforeAndAfterSaveChangesCalled);
        }

        [Fact]
        public async Task DeleteAsync_deleteById_returnChangedEntries()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var service = GetCrudServiceMock();

            // Act
            await service.DeleteAsync(ids);

            // Assert
            Assert.Equal(ids.FirstOrDefault(), TestChangedEvent.testChangedEntries.First().NewEntry.Id);
        }

        [Fact]
        public async Task DeleteAsync_SoftDeleteCalled()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var service = GetCrudServiceMock();

            // Act
            await service.DeleteAsync(ids, true);

            // Assert
            Assert.True(service.SoftDeleteCalled);
        }

        [Fact]
        public async Task DeleteAsync_AfterDeleteAsyncCalled()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var service = GetCrudServiceMock();

            // Act
            await service.DeleteAsync(ids);

            // Assert
            Assert.True(service.AfterDeleteAsyncCalled);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEntitiesLoadedFromDataSource()
        {
            // Arrange
            var ids = new List<string> { "1" };
            var removedEntities = new List<TestEntity>();

            _repositoryMock
                .Setup(x => x.Remove(It.IsAny<TestEntity>()))
                .Callback((TestEntity entity) => removedEntities.Add(entity));

            var service = GetLoadTrackingCrudServiceMock();

            // Act
            await service.DeleteAsync(ids);

            // Assert
            // The removed instance must be the one loaded from the data source: a stub built from the model carries
            // no concurrency token, which makes EF delete nothing and throw DbUpdateConcurrencyException instead.
            var removedEntity = Assert.Single(removedEntities);
            Assert.True(service.LoadedEntities.Any(x => ReferenceEquals(x, removedEntity)),
                "The deleted entity was not loaded from the data source.");
        }

        [Fact]
        public async Task GetByOuterIdAsync_ReturnsCorrectEntity()
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new() { Id = "1", OuterId = "a" },
                new() { Id = "2", OuterId = "b" },
                new() { Id = "3", OuterId = "c" },
            };

            var entitiesDbSetMock = entities.BuildMockDbSet();

            _repositoryMock
                .Setup(x => x.Entities)
                .Returns(entitiesDbSetMock.Object);

            var service = new TestOuterEntityService(() => _repositoryMock.Object, MemoryCacheMockHelper.GetPlatformMemoryCache(), _eventPublisherMock.Object);

            // Act
            var model = await service.GetByOuterIdNoCloneAsync("b");

            // Assert
            Assert.Equal("2", model.Id);
        }

        private CrudServiceMock GetCrudServiceMock()
        {
            _repositoryMock.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
            return new CrudServiceMock(() => _repositoryMock.Object, MemoryCacheMockHelper.GetPlatformMemoryCache(), _eventPublisherMock.Object);
        }

        private LoadTrackingCrudServiceMock GetLoadTrackingCrudServiceMock()
        {
            _repositoryMock.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
            return new LoadTrackingCrudServiceMock(() => _repositoryMock.Object, MemoryCacheMockHelper.GetPlatformMemoryCache(), _eventPublisherMock.Object);
        }

        /// <summary>
        /// Keeps every entity instance handed out by <see cref="LoadEntities"/>, so a test can tell an entity
        /// loaded from the data source from a stub built out of a model.
        /// </summary>
        private sealed class LoadTrackingCrudServiceMock : CrudServiceMock
        {
            public LoadTrackingCrudServiceMock(Func<IRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
                : base(repositoryFactory, platformMemoryCache, eventPublisher)
            {
            }

            public List<TestEntity> LoadedEntities { get; } = [];

            protected override async Task<IList<TestEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
            {
                var entities = await base.LoadEntities(repository, ids, responseGroup);
                LoadedEntities.AddRange(entities);

                return entities;
            }
        }
    }
}
