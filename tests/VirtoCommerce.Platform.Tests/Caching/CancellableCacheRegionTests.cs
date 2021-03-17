using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Caching
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

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 4)]
        public void CancelTokenForKeyOnTokenCancelledCallCounts(bool propagate, int callbackCount)
        {
            //Arrange
            var registeryCallbackInvokedCount = 0;
            CancellableCacheRegion.OnTokenCancelled = args =>
            {
                Interlocked.Increment(ref registeryCallbackInvokedCount);
            };

            var cache = GetPlatformMemoryCache();
            var key = "myKey";
            var value1 = new object();
            var value2 = new object();

            var key1 = CacheRegion.GenerateRegionTokenKey(key);
            var token1 = CacheRegion.CreateChangeTokenForKey(key);
            var key2 = CacheRegion2.GenerateRegionTokenKey(key);
            var token2 = CacheRegion2.CreateChangeTokenForKey(key);

            //Act
            cache.Set(key1, value1, token1);
            cache.Set(key2, value2, token2);

            //Assertion
            var result = cache.Get(key1);
            Assert.Same(value1, result);
            result = cache.Get(key2);
            Assert.Same(value2, result);

            //Act
            CancellableCacheRegion.CancelForKey(key1, propagate);

            //Assertion
            result = cache.Get(key1);
            Assert.Null(result);
            result = cache.Get(key2);
            Assert.Same(value2, result);

          
            //Act
            CancellableCacheRegion.CancelForKey(key2, propagate);

            //Assertion
            result = cache.Get(key2);
            Assert.Null(result);

            //Arrange
            token1 = CacheRegion.CreateChangeTokenForKey(key);
            token2 = CacheRegion2.CreateChangeTokenForKey(key);
            cache.Set(key1, value1, token1);
            cache.Set(key2, value2, token2);

            //Act
            //cancel region
            CancellableCacheRegion.CancelForKey(CacheRegion.GenerateRegionTokenKey(), propagate);
            //Assertion
            result = cache.Get(key1);
            Assert.Null(result);
            result = cache.Get(key2);
            Assert.Same(value2, result);

            //Arrange
            token1 = CacheRegion.CreateChangeTokenForKey(key);
            token2 = CacheRegion2.CreateChangeTokenForKey(key);
            cache.Set(key1, value1, token1);
            cache.Set(key2, value2, token2);

            //Act
            //cancel global region
            CancellableCacheRegion.CancelForKey(GlobalCacheRegion.GenerateRegionTokenKey(), propagate);
            result = cache.Get(key1);
            Assert.Null(result);
            result = cache.Get(key2);
            Assert.Null(result);

            //Callback mustn't call
            Assert.Equal(callbackCount, registeryCallbackInvokedCount);
        }

        public class CacheRegion : CancellableCacheRegion<CacheRegion>
        {          
        }
        public class CacheRegion2 : CancellableCacheRegion<CacheRegion2>
        {
        }
    }
}
