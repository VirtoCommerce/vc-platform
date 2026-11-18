using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Data.Localizations;
using VirtoCommerce.Platform.Tests.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class RegionalSettingsServiceTests : IDisposable
    {
        private readonly string _folder = Path.Combine(Path.GetTempPath(), "vc-regional-" + Guid.NewGuid().ToString("N"));

        public RegionalSettingsServiceTests()
        {
            Directory.CreateDirectory(_folder);
        }

        public void Dispose()
        {
            Directory.Delete(_folder, recursive: true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetRegionalFormats_NormalizesSortsAndRemovesDuplicates()
        {
            var service = CreateService(regionalFormats: """["en-US", "de", " en-us ", "", "af-na", "af"]""");

            var result = await service.GetRegionalFormatsAsync();

            result.Should().Equal("af", "af-na", "de", "en-us");
        }

        [Fact]
        public async Task GetTimeZones_KeepsIdsCaseAndSorts()
        {
            var service = CreateService(timeZones: """["Europe/Berlin", "Etc/UTC", "America/New_York", "Etc/UTC"]""");

            var result = await service.GetTimeZonesAsync();

            result.Should().Equal("America/New_York", "Etc/UTC", "Europe/Berlin");
        }

        [Fact]
        public async Task MissingFiles_ReturnEmptyLists()
        {
            var service = CreateService();

            (await service.GetRegionalFormatsAsync()).Should().BeEmpty();
            (await service.GetTimeZonesAsync()).Should().BeEmpty();
        }

        // The lists the platform ships: the regional formats the AngularJS admin has locale files for, and the time zones
        // both admin UIs can apply
        [Fact]
        public async Task ShippedFiles_HaveTheDefaultsTheUIsFallBackTo()
        {
            var folder = FindShippedFolder();
            var service = new FileSystemRegionalSettingsService(Options.Create(new PlatformOptions
            {
                RegionalFormatsFilePath = Path.Combine(folder, "regionalFormats.json"),
                TimeZonesFilePath = Path.Combine(folder, "timeZones.json"),
            }), GetCache());

            var formats = await service.GetRegionalFormatsAsync();
            var timeZones = await service.GetTimeZonesAsync();

            formats.Should().HaveCount(398).And.Contain(["en", "en-us", "de-de", "ru-ru", "zh-cn"]);
            timeZones.Should().HaveCount(596).And.Contain(["Etc/UTC", "UTC", "Europe/Berlin", "Europe/Amsterdam", "America/New_York", "Asia/Kolkata"]);
        }

        private FileSystemRegionalSettingsService CreateService(string regionalFormats = null, string timeZones = null)
        {
            var options = new PlatformOptions
            {
                RegionalFormatsFilePath = WriteFile("formats.json", regionalFormats),
                TimeZonesFilePath = WriteFile("zones.json", timeZones),
            };

            return new FileSystemRegionalSettingsService(Options.Create(options), GetCache());
        }

        private string WriteFile(string name, string content)
        {
            var path = Path.Combine(_folder, name);
            if (content != null)
            {
                File.WriteAllText(path, content);
            }

            return path;
        }

        private static string FindShippedFolder()
        {
            for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
            {
                var folder = Path.Combine(dir.FullName, "src", "VirtoCommerce.Platform.Web", "localization", "common");
                if (Directory.Exists(folder))
                {
                    return folder;
                }
            }

            throw new DirectoryNotFoundException("src/VirtoCommerce.Platform.Web/localization/common");
        }

        private static Core.Caching.IPlatformMemoryCache GetCache()
        {
            return MemoryCacheMockHelper.GetPlatformMemoryCache(new CachingOptions { CacheSlidingExpiration = TimeSpan.FromMinutes(1) });
        }
    }
}
