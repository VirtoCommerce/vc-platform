using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class SettingsManagerUnitTests
    {
        private readonly Mock<IPlatformRepository> _platformRepositoryMock;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPlatformMemoryCache> _memoryCasheMock;
        private readonly Mock<ICacheEntry> _cacheEntryMock;

        private readonly SettingsManager _settingsManager;
        public SettingsManagerUnitTests()
        {
            _platformRepositoryMock = new Mock<IPlatformRepository>();
            _repositoryFactory = () => _platformRepositoryMock.Object;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _platformRepositoryMock.Setup(ss => ss.UnitOfWork).Returns(_mockUnitOfWork.Object);
            _memoryCasheMock = new Mock<IPlatformMemoryCache>();
            _cacheEntryMock = new Mock<ICacheEntry>();
            _cacheEntryMock.SetupGet(c => c.ExpirationTokens).Returns(new List<IChangeToken>());
            _settingsManager = new SettingsManager(_repositoryFactory, _memoryCasheMock.Object);
        }
                        
        [Fact]
        public async Task GetAllObjectSettingsByTypesAndIdsAsync_GetSettings()
        {
            //Arrange
            var names = new[] { "name1", "name2" };
            var objectTypes = new[] { "", "store" };
            var objectIds = new[] { null, Guid.NewGuid().ToString() };
            var settingValues = new[]
            {
                new SettingEntity
                {
                    Name = names[0],
                    ObjectType = objectTypes[0],
                    ObjectId = objectIds[0],
                    SettingValues = new ObservableCollection<SettingValueEntity>()
                },
                new SettingEntity
                {
                    Name = names[1],
                    ObjectType = objectTypes[1],
                    ObjectId = objectIds[1],
                    SettingValues = new ObservableCollection<SettingValueEntity>()
                }
            };
            _platformRepositoryMock.Setup(x => x.GetAllObjectSettingsByTypesAndIdsAsync(objectTypes, objectIds))
                .ReturnsAsync(settingValues);
            _memoryCasheMock.Setup(pmc => pmc.CreateEntry(It.IsAny<object>())).Returns(_cacheEntryMock.Object);

            _settingsManager.RegisterSettings(names.Select(n => new SettingDescriptor { Name = n }));


            //Act
            var result = await _settingsManager.GetAllObjectSettingsByTypesAndIdsAsync(names, objectTypes, objectIds);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeepLoadSettingsAsync_GetSettingByObject()
        {
            //Arrange
            var names = new[] { "name1", "name2" };
            var objectTypes = new[] { typeof(TestSettingEntry).Name, "type2" };
            var objectIds = new[] { Guid.NewGuid().ToString(), null };
            var settingValues = new[]
            {
                new SettingEntity
                {
                    Name = names[0],
                    ObjectType = objectTypes[0],
                    ObjectId = objectIds[0],
                    SettingValues = new ObservableCollection<SettingValueEntity>()
                },
                new SettingEntity
                {
                    Name = names[1],
                    ObjectType = objectTypes[1],
                    ObjectId = objectIds[1],
                    SettingValues = new ObservableCollection<SettingValueEntity>()
                }
            };
            _platformRepositoryMock.Setup(x => x.GetAllObjectSettingsByTypesAndIdsAsync(new[] { objectTypes[0] }, new[] { objectIds[0] }))
                .ReturnsAsync(settingValues);
            _memoryCasheMock.Setup(pmc => pmc.CreateEntry(It.IsAny<object>())).Returns(_cacheEntryMock.Object);

            _settingsManager.RegisterSettingsForType(names.Select(n => new SettingDescriptor { Name = n }), typeof(TestSettingEntry).Name);
            _settingsManager.RegisterSettings(names.Select(n => new SettingDescriptor { Name = n }));
            var entries = new [] { new TestSettingEntry { Id = objectIds[0] } };

            //Act
            await SettingsExtension.DeepLoadSettingsAsync(_settingsManager, entries);

            //Assert
            Assert.NotNull(entries.SelectMany(x => x.Settings));
            Assert.Equal("name1", entries.FirstOrDefault().Settings.FirstOrDefault().Name);
        }
    }

    internal class TestSettingEntry : IHasSettings
    {
        public virtual string TypeName => GetType().Name;

        public ICollection<ObjectSettingEntry> Settings { get; set; }
        public string Id { get; set; }
    }
}
