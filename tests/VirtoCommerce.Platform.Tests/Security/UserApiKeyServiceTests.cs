using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserApiKeyServiceTests
{
    [Fact]
    public async Task GetApiKeyByKeyAsync_CandidateLongerThanTheColumn_KeysOnADigestAndStillQueriesWithTheCandidate()
    {
        var candidate = new string('k', DbContextBase.Length128 + 1);
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache, new UserApiKeyEntity
        {
            Id = "the-row",
            ApiKey = candidate,
            UserId = "the-user",
            IsActive = true,
        });

        var result = await service.GetApiKeyByKeyAsync(candidate);

        // The repository is queried with the candidate exactly as presented, so a stored row that
        // equals it is still found. The digest changes the cache key, not the outcome.
        result.Should().NotBeNull();
        result.Id.Should().Be("the-row");

        cache.Keys.Should().HaveCount(1);
        cache.Keys[0].Should().NotContain(candidate);

        // Through CacheKey.Normalize, not a hand-lowercased literal: the cache layer's case-folding is
        // not this test's subject, so the expectation goes through the same function the key did.
        cache.Keys[0].Should().Contain(CacheKey.Normalize(Digest(candidate)));
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_CandidateAtTheColumnLength_KeysOnTheCandidate()
    {
        var candidate = new string('k', DbContextBase.Length128);
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache);

        await service.GetApiKeyByKeyAsync(candidate);

        // At or below the threshold nothing changes: the key is the candidate, as it is today.
        cache.Keys.Should().HaveCount(1);
        cache.Keys[0].Should().Contain(CacheKey.Normalize(candidate));
    }

    private static string Digest(string value)
    {
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }

    private static UserApiKeyService CreateService(IPlatformMemoryCache cache, params UserApiKeyEntity[] rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserApiKeys).Returns(new List<UserApiKeyEntity>(rows).BuildMock());

        return new UserApiKeyService(() => repository.Object, cache);
    }

    /// <summary>
    /// Records the key and the entry of every cache write. GetOrCreateExclusiveAsync writes through
    /// IMemoryCache.Set, which goes through CreateEntry, so overriding that one member captures both.
    /// </summary>
    private sealed class RecordingPlatformMemoryCache : PlatformMemoryCache
    {
        public RecordingPlatformMemoryCache()
            : this(new CachingOptions { CacheEnabled = true, CacheSlidingExpiration = TimeSpan.FromMinutes(15) })
        {
        }

        public RecordingPlatformMemoryCache(CachingOptions options)
            : base(
                new MemoryCache(new MemoryCacheOptions()),
                Options.Create(options),
                new Mock<ILogger<PlatformMemoryCache>>().Object)
        {
        }

        public List<string> Keys { get; } = [];

        public List<ICacheEntry> Entries { get; } = [];

        public override ICacheEntry CreateEntry(object key)
        {
            Keys.Add(key.ToString());
            var entry = base.CreateEntry(key);
            Entries.Add(entry);

            return entry;
        }
    }
}
