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
using VirtoCommerce.Platform.Tests.Common;
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

        [Theory]
        [InlineData("../../appsettings")]
        [InlineData("..\\..\\appsettings")]
        [InlineData("*")]
        public void GetTranslationDataForLanguage_InvalidLanguage_ReturnsFallbackWithoutQueryingProviders(string lang)
        {
            //Arrange
            var fallback = JObject.FromObject(new { platform = new { title = "English" } });
            _translationDataProviderMock.Setup(x => x.GetTranslationDataForLanguage("en")).Returns(fallback);

            //Act
            var result = _translationService.GetTranslationDataForLanguage(lang);

            //Assert
            Assert.Equal("English", result.SelectToken("platform.title")?.ToString());
            _translationDataProviderMock.Verify(x => x.GetTranslationDataForLanguage(lang), Times.Never);
        }

        [Fact]
        public void GetTranslationDataForLanguage_UnknownLanguages_CreateNoCacheEntries()
        {
            //Arrange
            var fallback = JObject.FromObject(new { platform = new { title = "English" } });
            _translationDataProviderMock.Setup(x => x.GetTranslationDataForLanguage("en")).Returns(fallback);
            _translationDataProviderMock.Setup(x => x.GetListOfInstalledLanguages()).Returns(["en", "de"]);
            var memoryCache = MemoryCacheMockHelper.CreateCache();
            var service = new TranslationService([_translationDataProviderMock.Object],
                new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions { CacheEnabled = true }), new Mock<ILogger<PlatformMemoryCache>>().Object),
                _translationOptionsMock.Object);
            service.GetTranslationDataForLanguage("en");
            service.GetListOfInstalledLanguages();
            var entriesBefore = ((MemoryCache)memoryCache).Count;

            //Act
            for (var i = 0; i < 100; i++)
            {
                // Well-formed but not installed: "zzaa", "zzab", ...
                var lang = $"zz{(char)('a' + i / 26)}{(char)('a' + i % 26)}";
                var result = service.GetTranslationDataForLanguage(lang);
                Assert.Equal("English", result.SelectToken("platform.title")?.ToString());
            }

            //Assert
            Assert.Equal(entriesBefore, ((MemoryCache)memoryCache).Count);
            _translationDataProviderMock.Verify(x => x.GetTranslationDataForLanguage(It.Is<string>(l => l != "en")), Times.Never);
        }

        [Theory]
        [InlineData("de")]
        [InlineData("DE")]
        public void GetTranslationDataForLanguage_InstalledLanguage_MergedOverFallback(string lang)
        {
            //Arrange
            _translationDataProviderMock.Setup(x => x.GetTranslationDataForLanguage("en")).Returns(JObject.FromObject(new { platform = new { title = "English", save = "Save" } }));
            _translationDataProviderMock.Setup(x => x.GetTranslationDataForLanguage("de")).Returns(JObject.FromObject(new { platform = new { title = "Deutsch" } }));
            _translationDataProviderMock.Setup(x => x.GetListOfInstalledLanguages()).Returns(["en", "de"]);

            //Act
            var result = _translationService.GetTranslationDataForLanguage(lang);

            //Assert
            Assert.Equal("Deutsch", result.SelectToken("platform.title")?.ToString());
            Assert.Equal("Save", result.SelectToken("platform.save")?.ToString());
        }

        private static IPlatformMemoryCache GetCache()
        {
            return MemoryCacheMockHelper.GetPlatformMemoryCache(new CachingOptions { CacheSlidingExpiration = TimeSpan.FromMilliseconds(10) });
        }

    }
}
