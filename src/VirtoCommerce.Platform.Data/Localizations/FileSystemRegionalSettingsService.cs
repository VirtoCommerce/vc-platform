using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Data.Localizations;

/// <summary>
/// Reads the regional formats and time zones from the JSON files of <see cref="PlatformOptions.RegionalFormatsFilePath"/>
/// and <see cref="PlatformOptions.TimeZonesFilePath"/>. The lists don't depend on the admin UIs' files or on the
/// time zone data of the host, so they are the same on every installation.
/// </summary>
public class FileSystemRegionalSettingsService : IRegionalSettingsService
{
    private readonly PlatformOptions _platformOptions;
    private readonly IPlatformMemoryCache _memoryCache;

    public FileSystemRegionalSettingsService(IOptions<PlatformOptions> platformOptions, IPlatformMemoryCache memoryCache)
    {
        _platformOptions = platformOptions.Value;
        _memoryCache = memoryCache;
    }

    public virtual Task<IList<string>> GetRegionalFormatsAsync()
    {
        return ReadListAsync(_platformOptions.RegionalFormatsFilePath, x => x.ToLowerInvariant(), StringComparer.Ordinal);
    }

    public virtual Task<IList<string>> GetTimeZonesAsync()
    {
        return ReadListAsync(_platformOptions.TimeZonesFilePath, x => x, StringComparer.Ordinal);
    }

    protected virtual Task<IList<string>> ReadListAsync(string path, Func<string, string> normalize, StringComparer comparer)
    {
        var cacheKey = CacheKey.With(GetType(), nameof(ReadListAsync), path);

        return _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async _ =>
        {
            if (string.IsNullOrEmpty(path))
            {
                return (IList<string>)Array.Empty<string>();
            }

            var filePath = Path.GetFullPath(path);
            if (!File.Exists(filePath))
            {
                return Array.Empty<string>();
            }

            var items = JsonConvert.DeserializeObject<string[]>(await File.ReadAllTextAsync(filePath)) ?? [];

            return (IList<string>)items
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => normalize(x.Trim()))
                .Distinct(comparer)
                .Order(comparer)
                .ToList();
        });
    }
}
