using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;
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
    public async Task GetApiKeyByKeyAsync_RowStillHoldingPlaintext_IsNotFoundByIt()
    {
        // The AddUserApiKeyHash migration hashes every key and clears its plaintext; the lookup goes by hash only.
        var row = new UserApiKeyEntity { Id = "legacy-row", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110", UserId = "the-user", IsActive = true };
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out _, row);

        var result = await service.GetApiKeyByKeyAsync("0f1e2d3c-4b5a-4968-8776-655443322110");

        result.Should().BeNull();
        row.ApiKey.Should().Be("0f1e2d3c-4b5a-4968-8776-655443322110", "the lookup writes nothing");
        unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
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
    public async Task SaveApiKeysAsync_ValueThatIsAnotherStoredKey_IsRefusedWithoutItsHashAndNothingIsSaved()
    {
        const string value = "vc_someone-elses-key-0123456789abcdef";
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out var added, HashedRow("other-row", value, "another-user"));

        var save = () => service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user", ApiKey = value }]);

        var thrown = await save.Should().ThrowAsync<ApiKeyConflictException>();
        thrown.Which.Message.Should().NotContain(_hasher.Hash(value)).And.NotContain("another-user");
        added.Should().BeEmpty();
        unitOfWork.Verify(x => x.Commit(), Times.Never);
    }

    [Fact]
    public async Task SaveApiKeysAsync_TwoKeysInOneBatchWithTheSameValue_AreRefused()
    {
        const string value = "vc_the-same-value-twice-0123456789abc";
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out var added);

        var save = () => service.SaveApiKeysAsync([
            new UserApiKey { UserId = "first-user", ApiKey = value },
            new UserApiKey { UserId = "second-user", ApiKey = value },
        ]);

        await save.Should().ThrowAsync<ApiKeyConflictException>();
        added.Should().BeEmpty();
        unitOfWork.Verify(x => x.Commit(), Times.Never);
    }

    [Fact]
    public async Task SaveApiKeysAsync_SameBackupRestoredTwice_KeepsItsOwnHash()
    {
        var row = HashedRow("restored", "vc_restored-key-0123456789abcdef");
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out _, row);

        await service.SaveApiKeysAsync([new UserApiKey { Id = "restored", UserId = "the-user", ApiKeyHash = row.ApiKeyHash, ApiKeyHint = row.ApiKeyHint }]);

        unitOfWork.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task SaveApiKeysAsync_ValueTakenByAnotherSaveBeforeTheCommit_IsRefusedAsAConflict()
    {
        const string value = "vc_raced-value-0123456789abcdefghij";
        var stored = new List<UserApiKeyEntity>();
        var repository = CreateRepositoryOver(stored, out var unitOfWork);
        unitOfWork.Setup(x => x.Commit())
            .Callback(() => stored.Add(HashedRow("raced", value, "another-user")))
            .Throws(new DbUpdateException("Cannot insert duplicate key row ... IX_UserApiKey_ApiKeyHash"));
        var service = new UserApiKeyService(() => repository.Object, new RecordingPlatformMemoryCache(), _hasher, new DefaultApiKeyGenerator());

        var save = () => service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user", ApiKey = value }]);

        var thrown = await save.Should().ThrowAsync<ApiKeyConflictException>();
        thrown.Which.Message.Should().NotContain("IX_UserApiKey_ApiKeyHash");
    }

    [Fact]
    public async Task SaveApiKeysAsync_CommitFailingForAnotherReason_IsNotReportedAsAConflict()
    {
        var repository = CreateRepositoryOver([], out var unitOfWork);
        unitOfWork.Setup(x => x.Commit()).Throws(new DbUpdateException("deadlock"));
        var service = new UserApiKeyService(() => repository.Object, new RecordingPlatformMemoryCache(), _hasher, new DefaultApiKeyGenerator());

        var save = () => service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user" }]);

        await save.Should().ThrowAsync<DbUpdateException>().WithMessage("deadlock");
    }

    private static Mock<ISecurityRepository> CreateRepositoryOver(List<UserApiKeyEntity> stored, out Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork = new Mock<IUnitOfWork>();
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserApiKeys).Returns(() => new List<UserApiKeyEntity>(stored).BuildMock());
        repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
        return repository;
    }

    [Fact]
    public async Task SaveApiKeysAsync_ReturnsTheStoredAuditFields_NotTheOnesSent()
    {
        var created = new DateTime(2026, 11, 28, 10, 0, 0, DateTimeKind.Utc);
        var existing = HashedRow("existing", "vc_existing-key-0123456789abcdefghij");
        existing.CreatedDate = created.AddDays(-1);
        existing.CreatedBy = "creator";
        var service = CreateService(new RecordingPlatformMemoryCache(), out var unitOfWork, out var added, existing);
        unitOfWork.Setup(x => x.Commit()).Callback(() =>
        {
            // What the auditing in the data layer does on save.
            foreach (var entity in added)
            {
                entity.CreatedDate = created;
                entity.CreatedBy = "admin";
            }
            existing.ModifiedDate = created;
            existing.ModifiedBy = "admin";
        });

        var result = await service.SaveApiKeysAsync([
            new UserApiKey { UserId = "the-user" },
            new UserApiKey { Id = "existing", IsActive = false },
        ]);

        result[0].CreatedDate.Should().Be(created);
        result[0].CreatedBy.Should().Be("admin");
        result[1].CreatedDate.Should().Be(created.AddDays(-1));
        result[1].CreatedBy.Should().Be("creator");
        result[1].ModifiedDate.Should().Be(created);
        result[1].ModifiedBy.Should().Be("admin");
    }

    [Fact]
    public async Task SaveApiKeysAsync_UuidKeyFromAnOlderAdminUi_IsAccepted()
    {
        var service = CreateService(new RecordingPlatformMemoryCache(), out _, out var added);

        await service.SaveApiKeysAsync([new UserApiKey { UserId = "the-user", ApiKey = "0f1e2d3c-4b5a-4968-8776-655443322110" }]);

        added.Should().ContainSingle();
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

    [Fact]
    public void UserApiKeyJson_LeavesOutAnEmptyHash_AndKeepsAStoredOne()
    {
        // The REST API never fills the hash; a backup does, and must keep it so restored keys work.
        var apiResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new UserApiKey { Id = "key", ApiKey = "vc_A…WXYZ" });
        var backupEntry = Newtonsoft.Json.JsonConvert.SerializeObject(new UserApiKey { Id = "key", ApiKeyHash = "abc123" });

        apiResponse.Should().NotContain("ApiKeyHash");
        backupEntry.Should().Contain("\"ApiKeyHash\":\"abc123\"");
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
        return new UserApiKeyService(() => repository.Object, cache, _hasher, new DefaultApiKeyGenerator());
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
