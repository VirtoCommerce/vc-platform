using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    [Trait("Category", "Unit"), CollectionDefinition("CacheTests", DisableParallelization = true)]
    public class CancellableCacheRegionTests : MemoryCacheTestsBase
    {
        [Fact]
        public void CreateChangeTokenReturnsCompositeToken()
        {
            //Act
            var token = CacheRegion.CreateChangeTokenForKey("key");
            //Assertion
            Assert.IsType<CompositeChangeToken>(token);
            //Tokens for item, region and global
            Assert.Equal(3, ((CompositeChangeToken)token).ChangeTokens.Count);
            Assert.All(((CompositeChangeToken)token).ChangeTokens, x => Assert.IsType<LazyCancellationChangeToken>(x));

            //Act
            token = CacheRegion.CreateChangeToken();
            //Assertion
            Assert.IsType<CompositeChangeToken>(token);
            //Tokens for item, region and global
            Assert.Equal(2, ((CompositeChangeToken)token).ChangeTokens.Count);
            Assert.All(((CompositeChangeToken)token).ChangeTokens, x => Assert.IsType<LazyCancellationChangeToken>(x));

            //Act
            token = GlobalCacheRegion.CreateChangeToken();
            //Assertion
            Assert.IsType<LazyCancellationChangeToken>(token);
        }

        [Fact]
        public void ExpiredItemTokenRemovesItem()
        {
            //Arrange
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var value = new object();

            //Act
            var expirationToken = CacheRegion.CreateChangeTokenForKey(key);

            //Arrange
            var result = cache.Set(key, value, expirationToken);

            //Act
            Assert.Same(value, result);
            CacheRegion.ExpireTokenForKey(key);

            //Assertion
            result = cache.Get(key);
            Assert.Null(result);
        }

        [Fact]
        public void ExpiredRegionTokenRemovesAllRegionItems()
        {
            //Arrange
            var cache = GetPlatformMemoryCache();
            var key1 = "myKey1";
            var key2 = "myKey2";
            var value1 = new object();
            var value2 = new object();

            var token1 = CacheRegion.CreateChangeTokenForKey(key1);
            var token2 = CacheRegion.CreateChangeTokenForKey(key2);

            cache.Set(key1, value1, token1);
            cache.Set(key2, value2, token2);

            //Act
            CacheRegion.ExpireRegion();

            //Assertion
            var result = cache.Get(key1);
            Assert.Null(result);
            result = cache.Get(key2);
            Assert.Null(result);
        }

        [Fact]
        public void ExpiredGlobalRegionTokenRemovesAll()
        {
            //Arrange
            var cache = GetPlatformMemoryCache();
            var key1 = "myKey1";
            var key2 = "myKey2";
            var value1 = new object();
            var value2 = new object();

            var token1 = CacheRegion.CreateChangeTokenForKey(key1);
            var token2 = CacheRegion2.CreateChangeTokenForKey(key2);

            cache.Set(key1, value1, token1);
            cache.Set(key2, value2, token2);

            //Act
            GlobalCacheRegion.ExpireRegion();

            //Assertion
            var result = cache.Get(key1);
            Assert.Null(result);
            result = cache.Get(key2);
            Assert.Null(result);
        }

        [Fact]
        public void PlatformMemoryCache_Set()
        {
            // Arrange
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var expeted = new object();

            // Act
            var tokenKey = CacheRegion.GenerateRegionTokenKey(key);
            var token = CacheRegion.CreateChangeTokenForKey(key);
            cache.Set(tokenKey, expeted, token);

            // Assert
            var actual = cache.Get(tokenKey);
            Assert.Same(expeted, actual);
        }

        [Fact]
        public void PlatformMemoryCache_Cancel()
        {
            // Arrange
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var expeted = new object();

            // Act
            var tokenKey = CacheRegion.GenerateRegionTokenKey(key);
            var token = CacheRegion.CreateChangeTokenForKey(key);
            cache.Set(tokenKey, expeted, token);
            CancellableCacheRegion.CancelForKey(tokenKey);

            // Assert
            var actual = cache.Get(tokenKey);
            Assert.Null(actual);
        }

        [Fact]
        public void PlatformMemoryCache_CancelRegion()
        {
            // Arrange
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var expeted = new object();
            var expeted2 = new object();

            // Act
            var tokenKey = CacheRegion.GenerateRegionTokenKey(key);
            var tokenKey2 = CacheRegion2.GenerateRegionTokenKey(key);
            var token = CacheRegion.CreateChangeTokenForKey(key);
            var token2 = CacheRegion2.CreateChangeTokenForKey(key);
            cache.Set(tokenKey, expeted, token);
            cache.Set(tokenKey2, expeted2, token2);
            CancellableCacheRegion.CancelForKey(CacheRegion.GenerateRegionTokenKey());

            //Assertion
            var actual = cache.Get(tokenKey);
            var actual2 = cache.Get(tokenKey2);

            Assert.Null(actual);
            Assert.Same(expeted2, actual2);
        }

        [Fact]
        public void PlatformMemoryCache_CancelGlobalRegion()
        {
            // Arrange
            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var expeted = new object();
            var expeted2 = new object();

            // Act
            var tokenKey = CacheRegion.GenerateRegionTokenKey(key);
            var tokenKey2 = CacheRegion2.GenerateRegionTokenKey(key);
            var token = CacheRegion.CreateChangeTokenForKey(key);
            var token2 = CacheRegion2.CreateChangeTokenForKey(key);
            cache.Set(tokenKey, expeted, token);
            cache.Set(tokenKey2, expeted2, token2);
            CancellableCacheRegion.CancelForKey(GlobalCacheRegion.GenerateRegionTokenKey());

            //Assertion
            var actual = cache.Get(tokenKey);
            var actual2 = cache.Get(tokenKey2);

            Assert.Null(actual);
            Assert.Null(actual2);
        }

        [Fact]
        public void PlatformMemoryCache_Propagate()
        {
            //Arrange
            var registeryCallbackInvokedCount = 0;
            CancellableCacheRegion.OnTokenCancelled = args => registeryCallbackInvokedCount++;

            var key = "myKey";
            var tokenKey = CacheRegion.GenerateRegionTokenKey(key);

            // Act
            CancellableCacheRegion.CancelForKey(tokenKey, true);
            CancellableCacheRegion.CancelForKey(CacheRegion.GenerateRegionTokenKey(), true);
            CancellableCacheRegion.CancelForKey(GlobalCacheRegion.GenerateRegionTokenKey(), true);

            // Assert
            Assert.Equal(3, registeryCallbackInvokedCount);
        }

        public class CacheRegion : CancellableCacheRegion<CacheRegion>
        {
        }

        public class CacheRegion2 : CancellableCacheRegion<CacheRegion2>
        {
        }
    }
}
