using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using Microsoft.Extensions.Logging;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class CrudServiceTests
    {
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public CrudServiceTests()
        {
            _eventPublisherMock = new Mock<IEventPublisher>();
            _repositoryMock = new Mock<IRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task GetAsync_getById_returnTestModel()
        {
            // Arrange
            var ids = new List<string>() { "1" };
            var testModels = new List<TestModel> { new TestModel() { Id = "1" } };
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
            var ids = new List<string>() { "1" };
            var service = GetCrudServiceMock();

            // Act
            var getAsync = await service.GetAsync(ids);

            // Assert
            Assert.Equal(new List<TestModel> { new TestModel() { Id = "1", Name = "ProcessModelCalled" } }, getAsync);
        }

        [Fact]
        public async Task SaveChangesAsync_saveChanges_returnChangedEntries()
        {
            // Arrange
            var testModels = new List<TestModel> { new TestModel() { Id = "1", Name = "Test" } };
            var service = GetCrudServiceMock();

            // Act
            await service.SaveChangesAsync(testModels);

            // Assert
            Assert.Equal(testModels.FirstOrDefault(), TestChangedEvent.testChangedEntries.FirstOrDefault().NewEntry);
            Assert.Equal(2, service.BeforeAndAfterSaveChangesCalled);
        }

        [Fact]
        public async Task DeleteAsync_deleteById_returnChangedEntries()
        {
            // Arrange
            var ids = new List<string>() { "1" };
            var service = GetCrudServiceMock();

            // Act
            await service.DeleteAsync(ids);

            // Assert
            Assert.Equal(ids.FirstOrDefault(), TestChangedEvent.testChangedEntries.FirstOrDefault().NewEntry.Id);
        }

        [Fact]
        public async Task DeleteAsync_SoftDeleteCalled()
        {
            // Arrange
            var ids = new List<string>() { "1" };
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
            var ids = new List<string>() { "1" };
            var service = GetCrudServiceMock();

            // Act
            await service.DeleteAsync(ids);

            // Assert
            Assert.True(service.AfterDeleteAsyncCalled);
        }

        private CrudServiceMock GetCrudServiceMock()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);

            _repositoryMock.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
            return new CrudServiceMock(() => _repositoryMock.Object, platformMemoryCache, _eventPublisherMock.Object);
        }
    }
}
