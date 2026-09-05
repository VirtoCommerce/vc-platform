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
using VirtoCommerce.Platform.Core.Security;
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

    [Theory]
    [InlineData(1)]
    [InlineData(16)]
    [InlineData(36)]
    [InlineData(44)]
    [InlineData(127)]
    [InlineData(128)]
    public async Task GetApiKeyByKeyAsync_RawAndDigestedCandidates_CannotProduceTheSameCacheKey(int rawLength)
    {
        var raw = new string('k', rawLength);
        var digested = new string('k', DbContextBase.Length128 + 1);

        var rawCache = new RecordingPlatformMemoryCache();
        await CreateService(rawCache).GetApiKeyByKeyAsync(raw);

        var digestedCache = new RecordingPlatformMemoryCache();
        await CreateService(digestedCache).GetApiKeyByKeyAsync(digested);

        // The discriminator is its own key component, so the two namespaces cannot meet whatever the
        // caller sends - a raw candidate cannot spell its way into the digested namespace.
        rawCache.Keys[0].Should().NotBe(digestedCache.Keys[0]);
        rawCache.Keys[0].Should().Contain(CacheKey.Normalize($"{nameof(IUserApiKeyService.GetApiKeyByKeyAsync)}-0-"));
        digestedCache.Keys[0].Should().Contain(CacheKey.Normalize($"{nameof(IUserApiKeyService.GetApiKeyByKeyAsync)}-1-"));
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_RawCandidateEqualsDigestOfALongerCandidate_CannotProduceTheSameCacheKey()
    {
        var longCandidate = new string('k', DbContextBase.Length128 + 1);
        var rawCandidateThatLooksLikeADigest = Digest(longCandidate);

        var rawCache = new RecordingPlatformMemoryCache();
        await CreateService(rawCache).GetApiKeyByKeyAsync(rawCandidateThatLooksLikeADigest);

        var digestedCache = new RecordingPlatformMemoryCache();
        await CreateService(digestedCache).GetApiKeyByKeyAsync(longCandidate);

        // The adversarial shape: a raw candidate that IS another candidate's digest string. The
        // discriminator still keeps the two namespaces apart even here.
        rawCache.Keys[0].Should().NotBe(digestedCache.Keys[0]);
        rawCache.Keys[0].Should().Contain(CacheKey.Normalize($"{nameof(IUserApiKeyService.GetApiKeyByKeyAsync)}-0-"));
        digestedCache.Keys[0].Should().Contain(CacheKey.Normalize($"{nameof(IUserApiKeyService.GetApiKeyByKeyAsync)}-1-"));
    }

    [Theory]
    [InlineData(null, 30)]
    [InlineData(5, 5)]
    [InlineData(60, 30)]
    public async Task GetApiKeyByKeyAsync_MissingKey_ClampsTheNegativeEntryToTheShorterOfThirtySecondsAndTheConfiguredAbsolute(int? configuredAbsoluteSeconds, int expectedAbsoluteSeconds)
    {
        var options = new CachingOptions { CacheEnabled = true };
        if (configuredAbsoluteSeconds.HasValue)
        {
            options.CacheAbsoluteExpiration = TimeSpan.FromSeconds(configuredAbsoluteSeconds.Value);
        }

        var cache = new RecordingPlatformMemoryCache(options);
        var service = CreateService(cache);

        var result = await service.GetApiKeyByKeyAsync("no-such-key");

        result.Should().BeNull();

        // The miss is still cached - not caching it would turn every repeat of one guessed key into
        // a query against a store shared with every other module. The clamp holds across no configured
        // absolute, a shorter one, and a longer one: the change reduces residency and must never
        // extend it.
        cache.Entries.Should().HaveCount(1);
        cache.Entries[0].AbsoluteExpirationRelativeToNow.Should().Be(TimeSpan.FromSeconds(expectedAbsoluteSeconds));

        // Absolute, not sliding: sliding is what lets a repeated guess of one key live indefinitely.
        cache.Entries[0].SlidingExpiration.Should().BeNull();
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_ExistingKey_CachesThePositiveResultWithTheDefaultExpiration()
    {
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache, new UserApiKeyEntity
        {
            Id = "the-row",
            ApiKey = "a-real-key",
            UserId = "the-user",
            IsActive = true,
        });

        var result = await service.GetApiKeyByKeyAsync("a-real-key");

        result.Should().NotBeNull();

        // The bound is on the negative entry only: a hit keeps whatever the deployment configures.
        cache.Entries.Should().HaveCount(1);
        cache.Entries[0].AbsoluteExpirationRelativeToNow.Should().BeNull();
        cache.Entries[0].SlidingExpiration.Should().Be(TimeSpan.FromMinutes(15));
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
