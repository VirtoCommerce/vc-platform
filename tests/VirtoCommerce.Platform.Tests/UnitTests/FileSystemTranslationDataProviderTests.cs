using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Data.Localizations;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    /// <summary>
    /// VCST-7069: the anonymous localization endpoint passed "lang" into a file search pattern,
    /// so "../" in it read JSON files outside the localization folders (e.g. appsettings.Production.json).
    /// </summary>
    public sealed class FileSystemTranslationDataProviderTests : IDisposable
    {
        private const string Secret = "Server=prod;Password=secret";

        private readonly string _contentRoot;
        private readonly string _localizationsFolder;

        public FileSystemTranslationDataProviderTests()
        {
            // <contentRoot>/appsettings.Production.json
            // <contentRoot>/wwwroot/Localizations/{en,de,zh-CN}.platform.json
            _contentRoot = Path.Combine(Path.GetTempPath(), "vc-translation-tests-" + Guid.NewGuid().ToString("N"));
            _localizationsFolder = Path.Combine(_contentRoot, "wwwroot", "Localizations");
            Directory.CreateDirectory(_localizationsFolder);

            File.WriteAllText(Path.Combine(_contentRoot, "appsettings.Production.json"), $"{{\"ConnectionStrings\":{{\"VirtoCommerce\":\"{Secret}\"}}}}");
            File.WriteAllText(Path.Combine(_localizationsFolder, "en.platform.json"), "{\"platform\":{\"title\":\"English\"}}");
            File.WriteAllText(Path.Combine(_localizationsFolder, "de.platform.json"), "{\"platform\":{\"title\":\"Deutsch\"}}");
            File.WriteAllText(Path.Combine(_localizationsFolder, "zh-CN.platform.json"), "{\"platform\":{\"title\":\"Chinese\"}}");
        }

        public void Dispose()
        {
            Directory.Delete(_contentRoot, recursive: true);
        }

        [Theory]
        [InlineData("en", "English")]
        [InlineData("de", "Deutsch")]
        [InlineData("zh-CN", "Chinese")]
        public void GetTranslationDataForLanguage_InstalledLanguage_ReturnsItsTranslations(string lang, string expectedTitle)
        {
            var result = CreateProvider().GetTranslationDataForLanguage(lang);

            Assert.Equal(expectedTitle, result.SelectToken("platform.title")?.ToString());
        }

        [Theory]
        [InlineData("../../appsettings")]
        [InlineData("..\\..\\appsettings")]
        [InlineData("../../*")]
        [InlineData("..")]
        [InlineData("*")]
        [InlineData("?e")]
        [InlineData("en/../../../appsettings")]
        public void GetTranslationDataForLanguage_PathOrWildcardInLanguage_ReadsNothing(string lang)
        {
            var result = CreateProvider().GetTranslationDataForLanguage(lang);

            Assert.False(result.HasValues, $"lang '{lang}' returned: {result}");
            Assert.DoesNotContain(Secret, result.ToString());
        }

        [Fact]
        public void GetAllTranslationFiles_PatternEscapingTheFolder_SkipsFilesOutsideIt()
        {
            var provider = new ExposedProvider(_localizationsFolder);

            var files = provider.Find("../../appsettings.*.json").ToList();

            Assert.Empty(files);
        }

        [Fact]
        public void GetListOfInstalledLanguages_ReturnsLanguagesFromFileNames()
        {
            var languages = CreateProvider().GetListOfInstalledLanguages();

            Assert.Equal(["de", "en", "zh-CN"], languages.OrderBy(x => x, StringComparer.Ordinal));
        }

        private PlatformTranslationDataProvider CreateProvider()
        {
            return new PlatformTranslationDataProvider(Options.Create(new TranslationOptions { PlatformTranslationFolderPath = _localizationsFolder }));
        }

        private sealed class ExposedProvider(string folder) : FileSystemTranslationDataProvider
        {
            protected override IEnumerable<string> DiscoveryFolders => [folder];

            public IEnumerable<string> Find(string pattern) => GetAllTranslationFiles(pattern);
        }
    }
}
