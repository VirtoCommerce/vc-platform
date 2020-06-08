using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Caching;
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
        private readonly Mock<ICacheEntry> _cacheEntryMock;

        private readonly TranslationService _translationService;
        public TranslationServiceUnitTests()
        {
            _translationDataProviderMock = new Mock<ITranslationDataProvider>();
            _translationOptionsMock = new Mock<IOptions<TranslationOptions>>();
            var translationOptions = new TranslationOptions();
            _translationOptionsMock.Setup(x => x.Value).Returns(translationOptions);
            _cacheEntryMock = new Mock<ICacheEntry>();
            _cacheEntryMock.SetupGet(c => c.ExpirationTokens).Returns(new List<IChangeToken>());
            IList<ITranslationDataProvider> listProvider = new List<ITranslationDataProvider> { _translationDataProviderMock.Object };

            _translationService = new TranslationService(listProvider, GetCache(), _translationOptionsMock.Object);
        }

        [Fact]
        public void GetResources_SelectToken_Success()
        {
            //Arrange
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
            _translationDataProviderMock.Setup(x => x.GetListOfInstalledLanguages()).Returns(new [] {"en", "de"});

            //Act
            var result = _translationService.GetListOfInstalledLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.Contains("en", result);
        }

        private static IPlatformMemoryCache GetCache()
        {
            var defaultOptions = Options.Create(new CachingOptions() { CacheSlidingExpiration = TimeSpan.FromMilliseconds(10) });
            var logger = new Moq.Mock<ILogger<PlatformMemoryCache>>();
            return new PlatformMemoryCache(new MemoryCache(new MemoryCacheOptions()), defaultOptions, logger.Object);
        }

    }
}
