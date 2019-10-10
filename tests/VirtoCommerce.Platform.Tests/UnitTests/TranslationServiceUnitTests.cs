using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Data.Localizations;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class TranslationServiceUnitTests
    {
        private readonly Mock<ITranslationDataProvider> _translationDataProviderMock;
        private readonly Mock<IOptions<TranslationOptions>> _translationOptionsMock;
        private readonly Mock<IPlatformMemoryCache> _platformMemoryCacheMock;
        private readonly Mock<ICacheEntry> _cacheEntryMock;

        private readonly TranslationService _translationService;
        public TranslationServiceUnitTests()
        {
            _translationDataProviderMock = new Mock<ITranslationDataProvider>();
            _translationOptionsMock = new Mock<IOptions<TranslationOptions>>();
            var translationOptions = new TranslationOptions();
            _translationOptionsMock.Setup(x => x.Value).Returns(translationOptions);
            _platformMemoryCacheMock = new Mock<IPlatformMemoryCache>();
            _cacheEntryMock = new Mock<ICacheEntry>();
            _cacheEntryMock.SetupGet(c => c.ExpirationTokens).Returns(new List<IChangeToken>());
            IList<ITranslationDataProvider> listProvider = new List<ITranslationDataProvider> { _translationDataProviderMock.Object };

            _translationService = new TranslationService(listProvider, _platformMemoryCacheMock.Object, _translationOptionsMock.Object);
        }

        [Fact]
        public void GetResources_SelectToken_Success()
        {
            //Arrange
            var fallbackJsonCacheKey = CacheKey.With(_translationService.GetType(), "FallbackJson");
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(fallbackJsonCacheKey)).Returns(_cacheEntryMock.Object);
            var obj = JObject.FromObject(new {en = new { platform = new { commands = new { test = "Test success" } } } });
            _translationDataProviderMock.Setup(x => x.GetTranslationDataForLanguage("en")).Returns(obj);

            //Act
            var result = _translationService.GetTranslationDataForLanguage();
            var token = result.SelectToken("en.platform.commands.test");

            //Assert
            Assert.NotNull(result);
            Assert.True(result.HasValues);
            Assert.Equal("Test success", token.ToString());
        }

        [Fact]
        public void GetLocales_ReturnEnglish()
        {
            //Arrange
            var cacheKey = CacheKey.With(_translationService.GetType(), "GetListOfInstalledLanguages");
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(cacheKey)).Returns(_cacheEntryMock.Object);
            _translationDataProviderMock.Setup(x => x.GetListOfInstalledLanguages()).Returns(new [] {"en", "de"});

            //Act
            var result = _translationService.GetListOfInstalledLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.Contains("en", result);
        }
    }
}
