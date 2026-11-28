using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Caching;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserApiKeyServiceTests
{
    private static readonly Sha256ApiKeyHasher _hasher = new();

    #region Lookup

    [Fact]
    public async Task GetApiKeyByKeyAsync_HashedKey_IsFoundByItsHashAndCachedUnderTheHash()
    {
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache, out _, out _, HashedRow("the-row", "vc_the-real-key-value"));

        var result = await service.GetApiKeyByKeyAsync("vc_the-real-key-value");

        result.Should().NotBeNull();
        result.Id.Should().Be("the-row");
        result.ApiKey.Should().BeNull("a hashed key has no value to hand out");

        cache.Keys.Should().ContainSingle();
        cache.Keys[0].Should().NotContain("the-real-key-value");
        cache.Keys[0].Should().Contain(CacheKey.Normalize(_hasher.Hash("vc_the-real-key-value")));
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_KeyNotHashedByTheMigration_IsFoundByPlaintextHashedAndCleared()
    {
        var row = new UserApiKeyEntity { Id = "legacy-row", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", UserId = "the-user", IsActive = true };
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out _, row);

        var result = await service.GetApiKeyByKeyAsync("0f1e2d3c-4b5a-4968-8776-655443322110");

        result.Should().NotBeNull();
        result.Id.Should().Be("legacy-row");
        row.ApiKeyHash.Should().Be(_hasher.Hash("0f1e2d3c-4b5a-4968-8776-655443322110"));
        row.ApiKeyHint.Should().Be("0f1e…2110");
        row.ApiKey.Should().BeNull("no usable key is left once it is hashed");
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_KeyNotHashedByTheMigration_ExpiresTheApiKeyCacheOnceHashed()
    {
        var row = new UserApiKeyEntity { Id = "legacy-row", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", UserId = "the-user", IsActive = true };
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _, row);
        var token = ApiKeyCacheRegion.CreateChangeToken();

        await service.GetApiKeyByKeyAsync("0f1e2d3c-4b5a-4968-8776-655443322110");

        token.HasChanged.Should().BeTrue("cached key lists still hold the row with its plaintext");
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_HashingTheLegacyKeyFails_StillReturnsTheKey()
    {
        var row = new UserApiKeyEntity { Id = "legacy-row", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", UserId = "the-user", IsActive = true };
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out _, row);
        unitOfWork.Setup(x => x.CommitAsync()).ThrowsAsync(new InvalidOperationException("deadlock"));

        var result = await service.GetApiKeyByKeyAsync("0f1e2d3c-4b5a-4968-8776-655443322110");

        result.Should().NotBeNull("the key is valid; only storing its hash failed");
        result.Id.Should().Be("legacy-row");
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_CandidateLongerThanTheColumn_IsNotLookedUpAsPlaintext()
    {
        var candidate = new string('k', DbContextBase.Length128 + 1);
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache, out var unitOfWork, out _, new UserApiKeyEntity { Id = "the-row", ApiKey = candidate });

        var result = await service.GetApiKeyByKeyAsync(candidate);

        result.Should().BeNull();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
        cache.Keys[0].Should().NotContain(candidate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetApiKeyByKeyAsync_NoCandidate_ReturnsNullWithoutCaching(string candidate)
    {
        var cache = new RecordingPlatformMemoryCache();

        var result = await CreateService(cache, out _, out _).GetApiKeyByKeyAsync(candidate);

        result.Should().BeNull();
        cache.Keys.Should().BeEmpty();
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
        var service = CreateService(cache, out _, out _);

        var result = await service.GetApiKeyByKeyAsync("no-such-key");

        result.Should().BeNull();

        cache.Entries.Should().HaveCount(1);
        cache.Entries[0].AbsoluteExpirationRelativeToNow.Should().Be(TimeSpan.FromSeconds(expectedAbsoluteSeconds));

        // Absolute, not sliding: sliding is what lets a repeated guess of one key live indefinitely.
        cache.Entries[0].SlidingExpiration.Should().BeNull();
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_ExistingKey_CachesThePositiveResultWithTheDefaultExpiration()
    {
        var cache = new RecordingPlatformMemoryCache();
        var service = CreateService(cache, out _, out _, HashedRow("the-row", "a-real-key"));

        var result = await service.GetApiKeyByKeyAsync("a-real-key");

        result.Should().NotBeNull();

        cache.Entries.Should().HaveCount(1);
        cache.Entries[0].AbsoluteExpirationRelativeToNow.Should().BeNull();
        cache.Entries[0].SlidingExpiration.Should().Be(TimeSpan.FromMinutes(15));
    }

    [Fact]
    public async Task GetAllUserApiKeysAsync_ReturnsNoValueForHashedKeysAndAHintForKeysNotHashedYet()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _,
            HashedRow("hashed", "vc_ABCDEFGHIJKLMNOPQRSTUVWXYZ", "the-user"),
            new UserApiKeyEntity { Id = "legacy", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", UserId = "the-user" });

        var result = await service.GetAllUserApiKeysAsync("the-user");

        var hashed = result.Single(x => x.Id == "hashed");
        hashed.ApiKey.Should().BeNull();
        hashed.ApiKeyHint.Should().Be("vc_A…WXYZ");

        var legacy = result.Single(x => x.Id == "legacy");
        legacy.ApiKeyHint.Should().Be("0f1e…2110");
    }

    #endregion

    #region Save

    [Fact]
    public async Task SaveApiKeysAsync_NewKeyWithoutValue_GeneratesAKeyReturnedOnceAndStoresOnlyItsHash()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out var added);
        var apiKey = new UserApiKey { UserId = "the-user", IsActive = true };

        var result = await service.SaveApiKeysAsync([apiKey]);

        var value = result[0].ApiKey;
        value.Should().StartWith(DefaultApiKeyGenerator.Prefix);
        result[0].ApiKeyHash.Should().Be(_hasher.Hash(value));

        added.Should().ContainSingle();
        added[0].ApiKey.Should().BeNull("the key itself is never stored");
        added[0].ApiKeyHash.Should().Be(_hasher.Hash(value));
        added[0].ApiKeyHint.Should().Be(_hasher.GetHint(value));
    }

    [Fact]
    public async Task SaveApiKeysAsync_NewKeyWithAValue_StoresTheHashOfThatValue()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out var added);

        var result = await service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user", ApiKey = "client-made-key-value-0123456789abcd" }]);

        result[0].ApiKey.Should().Be("client-made-key-value-0123456789abcd");
        added[0].ApiKey.Should().BeNull();
        added[0].ApiKeyHash.Should().Be(_hasher.Hash("client-made-key-value-0123456789abcd"));
    }

    [Fact]
    public async Task SaveApiKeysAsync_NewKeyWithAHashOnly_StoresThatHash_SoABackupRestoreKeepsTheKeyWorking()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out var added);
        var hash = _hasher.Hash("vc_restored-key");

        await service.SaveApiKeysAsync([new UserApiKey { Id = "restored", UserId = "the-user", ApiKeyHash = hash, ApiKeyHint = "vc_r…-key" }]);

        added[0].ApiKeyHash.Should().Be(hash);
        added[0].ApiKeyHint.Should().Be("vc_r…-key");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("vc_A…WXYZ")]
    public async Task SaveApiKeysAsync_ExistingKeyWithoutValueOrWithItsHint_KeepsTheStoredKey(string sentValue)
    {
        var row = HashedRow("the-row", "vc_ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var storedHash = row.ApiKeyHash;
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _, row);

        var result = await service.SaveApiKeysAsync([new UserApiKey { Id = "the-row", ApiKey = sentValue, IsActive = false }]);

        result[0].ApiKey.Should().BeNull("no value was set by this save");
        row.ApiKeyHash.Should().Be(storedHash);
        row.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task SaveApiKeysAsync_KeyFromBeforeHashingSentBackAsIs_KeepsTheKeyHashedAndDropsThePlaintext()
    {
        var row = new UserApiKey { Id = "legacy", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110" };
        var entity = new UserApiKeyEntity { Id = "legacy", ApiKey = row.ApiKey };
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _, entity);

        await service.SaveApiKeysAsync([row]);

        entity.ApiKeyHash.Should().Be(_hasher.Hash("0f1e2d3c-4b5a-4968-8776-655443322110"));
        entity.ApiKey.Should().BeNull();
    }

    [Theory]
    [InlineData("short-key")]
    [InlineData("vc_Z…0000")]
    public async Task SaveApiKeysAsync_AnyValue_IsStoredAsGiven_SoAnOldBackupRestoresEveryKey(string restoredValue)
    {
        // Backup restore saves through the service with the plaintext of keys from before key hashing, whatever their
        // length or shape. The REST API checks what clients choose before it calls the service.
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out var added);

        await service.SaveApiKeysAsync([new UserApiKey { Id = "restored", UserId = "the-user", ApiKey = restoredValue }]);

        added.Should().ContainSingle();
        added[0].ApiKey.Should().BeNull();
        added[0].ApiKeyHash.Should().Be(_hasher.Hash(restoredValue));
        unitOfWork.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task SaveApiKeysAsync_ExistingRowWithNeitherHashNorKey_GetsAGeneratedKey()
    {
        var row = new UserApiKeyEntity { Id = "empty-row", UserId = "the-user" };
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _, row);

        var result = await service.SaveApiKeysAsync([new UserApiKey { Id = "empty-row", IsActive = true }]);

        result[0].ApiKey.Should().StartWith(DefaultApiKeyGenerator.Prefix);
        row.ApiKeyHash.Should().Be(_hasher.Hash(result[0].ApiKey));
    }

    [Fact]
    public async Task SaveApiKeysAsync_ExistingKeyWithANewValue_ReplacesTheKey()
    {
        var row = HashedRow("the-row", "vc_old-key-value-0000");
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out _, row);

        var result = await service.SaveApiKeysAsync([new UserApiKey { Id = "the-row", ApiKey = "vc_new-key-value-1111-0123456789abcd" }]);

        result[0].ApiKey.Should().Be("vc_new-key-value-1111-0123456789abcd");
        row.ApiKeyHash.Should().Be(_hasher.Hash("vc_new-key-value-1111-0123456789abcd"));
        row.ApiKey.Should().BeNull();
    }

    #endregion

    [Fact]
    public async Task SaveApiKeysAsync_UuidKeyFromAnOlderAdminUi_IsAccepted()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out var added);

        await service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110" }]);

        added.Should().ContainSingle();
    }

    [Fact]
    public async Task GetApiKeyByKeyAsync_RowWithPlaintextAndAnotherHash_IsRehashedWithTheRegisteredHasherAndCleared()
    {
        // A row that still has its plaintext but a hash the registered hasher does not produce, as a key saved
        // by an older instance during a rolling upgrade may be, is matched by its plaintext.
        var row = new UserApiKeyEntity { Id = "migrated", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", ApiKeyHash = "sha256-from-the-migration" };
        var repository = CreateRepository(out var unitOfWork, out _, row);
        var service = new UserApiKeyService(() => repository.Object, new RecordingPlatformMemoryCache(), new ReversingHasher(), new DefaultApiKeyGenerator(), NullLogger<UserApiKeyService>.Instance);

        var result = await service.GetApiKeyByKeyAsync("0f1e2d3c-4b5a-4968-8776-655443322110");

        result.Should().NotBeNull();
        row.ApiKeyHash.Should().Be(new ReversingHasher().Hash("0f1e2d3c-4b5a-4968-8776-655443322110"));
        row.ApiKey.Should().BeNull();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    private sealed class ReversingHasher : IApiKeyHasher
    {
        public string Hash(string apiKey) => new(apiKey.Reverse().ToArray());

        public string GetHint(string apiKey) => "custom";
    }

    [Fact]
    public async Task UserApiKeySearchService_WithoutSortInfos_PagesInIdOrder()
    {
        var repository = CreateRepository(out _, out _,
            HashedRow("c", "vc_key-c"), HashedRow("a", "vc_key-a"), HashedRow("b", "vc_key-b"));
        var searchService = new UserApiKeySearchService(() => repository.Object);

        var firstPage = await searchService.SearchAsync(new Core.Security.Search.UserApiKeySearchCriteria { Skip = 0, Take = 2 });
        var secondPage = await searchService.SearchAsync(new Core.Security.Search.UserApiKeySearchCriteria { Skip = 2, Take = 2 });

        firstPage.TotalCount.Should().Be(3);
        firstPage.Results.Select(x => x.Id).Should().Equal("a", "b");
        secondPage.Results.Select(x => x.Id).Should().Equal("c");
    }

    #region Hasher and generator

    [Fact]
    public void Sha256ApiKeyHasher_HashesToLowercaseHexOfSha256()
    {
        _hasher.Hash("abc").Should().Be("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad");
    }

    [Theory]
    [InlineData("vc_ABCDEFGHIJKLMNOPQRSTUVWXYZ", "vc_A…WXYZ")]
    [InlineData("0123456789abcdef", "0123…cdef")]
    [InlineData("0123456789abcde", "…")]
    [InlineData("", "…")]
    [InlineData(null, "…")]
    public void Sha256ApiKeyHasher_HintShowsFourCharactersEachEnd_OnlyForKeysOfSixteenOrMore(string apiKey, string expectedHint)
    {
        _hasher.GetHint(apiKey).Should().Be(expectedHint);
    }

    [Fact]
    public void DefaultApiKeyGenerator_MakesPrefixedUrlSafeKeysOf256RandomBits()
    {
        var generator = new DefaultApiKeyGenerator();

        var keys = Enumerable.Range(0, 100).Select(_ => generator.Generate()).ToArray();

        keys.Should().OnlyHaveUniqueItems();
        keys.Should().AllSatisfy(key =>
        {
            key.Should().StartWith("vc_");
            key.Should().HaveLength(3 + 43, "32 bytes are 43 Base64Url characters without padding");
            key[3..].Should().MatchRegex("^[A-Za-z0-9_-]+$");
        });
    }

    #endregion

    private static UserApiKeyEntity HashedRow(string id, string apiKey, string userId = "the-user")
    {
        return new UserApiKeyEntity
        {
            Id = id,
            UserId = userId,
            IsActive = true,
            ApiKeyHash = _hasher.Hash(apiKey),
            ApiKeyHint = _hasher.GetHint(apiKey),
        };
    }

    private static UserApiKeyService CreateService(IPlatformMemoryCache cache, out Mock<IUnitOfWork> unitOfWork, out List<UserApiKeyEntity> added, params UserApiKeyEntity[] rows)
    {
        var repository = CreateRepository(out unitOfWork, out added, rows);
        return new UserApiKeyService(() => repository.Object, cache, _hasher, new DefaultApiKeyGenerator(), NullLogger<UserApiKeyService>.Instance);
    }

    private static Mock<ISecurityRepository> CreateRepository(out Mock<IUnitOfWork> unitOfWork, out List<UserApiKeyEntity> added, params UserApiKeyEntity[] rows)
    {
        var addedRows = new List<UserApiKeyEntity>();
        unitOfWork = new Mock<IUnitOfWork>();

        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserApiKeys).Returns(() => new List<UserApiKeyEntity>(rows).BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        repository.Setup(x => x.Add(It.IsAny<UserApiKeyEntity>())).Callback<UserApiKeyEntity>(addedRows.Add);

        added = addedRows;
        return repository;
    }

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

        // GetOrCreateExclusiveAsync writes through IMemoryCache.Set, which goes through CreateEntry, so
        // overriding that one member captures both.
        public override ICacheEntry CreateEntry(object key)
        {
            Keys.Add(key.ToString());
            var entry = base.CreateEntry(key);
            Entries.Add(entry);

            return entry;
        }
    }
}
