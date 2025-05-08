using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;
using Xunit;

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
            var ids = new List<string>() { "1" };
            var service = GetCrudServiceMock();

            // Act
            var getAsync = await service.GetAsync(ids);

            // Assert
            Assert.Equal(new List<TestModel> { new() { Id = "1", Name = "ProcessModelCalled" } }, getAsync);
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

        [Fact]
        public async Task GetByOuterIdAsync_getById_returnTestModel()
        {
            // Arrange
            var id = "1";
            var mockModels = new List<TestEntity> { new() { Id = "1", OuterId = "1" } };
            var testModels = new List<TestModel> { new() { Id = "1", OuterId = "1" } };

            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("MyInMemoryDatabseName");

            var context = new TestDbContext(optionsBuilder.Options);
            context.TestEntities.AddRange(mockModels);
            context.SaveChanges();

            var unitOfWork = AbstractTypeFactory<DbContextUnitOfWork>.TryCreateInstance(new DbContextUnitOfWork(context), context);
            _repositoryMock.Setup(x => x.UnitOfWork).Returns(unitOfWork);

            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
            var service = new CrudServiceMock(() => _repositoryMock.Object, platformMemoryCache, _eventPublisherMock.Object);

            // Act
            var getAsync = await service.GetByOuterIdAsync(id);

            // Assert
            Assert.Equal(testModels.FirstOrDefault(), getAsync);
        }

        private CrudServiceMock GetCrudServiceMock()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()),
                new Mock<ILogger<PlatformMemoryCache>>().Object);

            _repositoryMock.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
            return new CrudServiceMock(() => _repositoryMock.Object, platformMemoryCache, _eventPublisherMock.Object);
        }

        public class TestDbContext : DbContextBase
        {
            public TestDbContext(DbContextOptions<TestDbContext> options)
                : base(options)
            {
            }

            protected TestDbContext(DbContextOptions options)
                : base(options)
            {
            }

            public DbSet<TestEntity> TestEntities { get; set; }
        }
    }
}
