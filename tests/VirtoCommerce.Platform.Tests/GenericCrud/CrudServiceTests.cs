using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
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

            var service = new TestOuterEntityService(() => _repositoryMock.Object, GetPlatformMemoryCache(), _eventPublisherMock.Object);

            // Act
            var model = await service.GetByOuterIdNoCloneAsync("b");

            // Assert
            Assert.Equal("2", model.Id);
        }

        private CrudServiceMock GetCrudServiceMock()
        {
            _repositoryMock.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
            return new CrudServiceMock(() => _repositoryMock.Object, GetPlatformMemoryCache(), _eventPublisherMock.Object);
        }

        private static PlatformMemoryCache GetPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            return new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
        }
    }
}
