using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class DynamicPropertyServiceUnitTests
    {
        private readonly Mock<IPlatformRepository> _platformRepositoryMock;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPlatformMemoryCache> _memoryCasheMock;
        private readonly DynamicPropertyService _dynamicPropertyService;

        public DynamicPropertyServiceUnitTests()
        {
            _platformRepositoryMock = new Mock<IPlatformRepository>();
            _repositoryFactory = () => _platformRepositoryMock.Object;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _platformRepositoryMock.Setup(ss => ss.UnitOfWork).Returns(_mockUnitOfWork.Object);
            _memoryCasheMock = new Mock<IPlatformMemoryCache>();
            _dynamicPropertyService = new DynamicPropertyService(_repositoryFactory, _memoryCasheMock.Object);
        }

        [Fact]
        public async Task SaveDynamicPropertiesAsync_CreateDynamicProperty()
        {
            //Arrange
            var name = "Some property";
            var properties = new List<DynamicProperty>() { new DynamicProperty { Name = name } };


            //Act
            var result = await _dynamicPropertyService.SaveDynamicPropertiesAsync(properties.ToArray());

            //Assert
            Assert.Contains(result, prop => prop.Name == name);
            Assert.Null(result.First().ModifiedDate);
        }

        [Fact]
        public async Task SaveDynamicPropertiesAsync_UpdateDynamicProperty()
        {
            //Arrange
            var name = "Some property";
            var id = Guid.NewGuid().ToString();
            var properties = new List<DynamicProperty>() { new DynamicProperty { Name = name, Id = id } };
            _platformRepositoryMock.Setup(n => n.GetDynamicPropertiesByIdsAsync(new[] { id })).ReturnsAsync(new DynamicPropertyEntity[]
            {
                new DynamicPropertyEntity { CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Id = id }
            });

            //Act
            var result = await _dynamicPropertyService.SaveDynamicPropertiesAsync(properties.ToArray());

            //Assert
            Assert.Contains(result, prop => prop.Name == name);
            Assert.Equal(id, result.First().Id);
            Assert.Null(result.First().ModifiedDate);
        }

        //TODO remove to separate UnitTests
        //[Fact]
        //public async Task SaveDictionaryItemsAsync_SaveDictionaryItem()
        //{
        //    //Arrange
        //    var name = "Some item";
        //    var properties = new List<DynamicPropertyDictionaryItem>() { new DynamicPropertyDictionaryItem() { Name = name, } };

        //    //Act
        //    await _dynamicPropertyService.SaveDictionaryItemsAsync(properties.ToArray());
        //}
    }

    public class SomeClass : Entity, IHasDynamicProperties
    {
        public string ObjectType => GetType().FullName;
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

    }
}
