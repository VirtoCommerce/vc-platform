using FluentAssertions;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Extensions;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Caching
{
    public class CacheKeyTests
    {
        [Fact]
        public void Normalize_AlreadyLowercase_ReturnsSameInstance()
        {
            // Arrange
            var key = "getsettingbynamesasync-alpha-123";

            // Act
            var actual = CacheKey.Normalize(key);

            // Assert — same reference proves the guarded scan skips the ToLowerInvariant allocation.
            actual.Should().BeSameAs(key);
        }

        [Theory]
        [InlineData("Abcdefghij", "abcdefghij")]
        [InlineData("ABCDEFGHIJ", "abcdefghij")]
        [InlineData("Prefix-Id", "prefix-id")]
        [InlineData("Type:GetByNames-A;B", "type:getbynames-a;b")]
        public void Normalize_MixedCase_MatchesToLowerInvariant(string key, string expected)
        {
            // Act
            var actual = CacheKey.Normalize(key);

            // Assert
            actual.Should().Be(expected);
            actual.Should().Be(key.ToLowerInvariant());
        }

        [Fact]
        public void Normalize_Object_NonString_PassesThrough()
        {
            // Arrange
            var key = 42;

            // Act
            var actual = CacheKey.Normalize((object)key);

            // Assert
            actual.Should().Be(42);
        }

        [Fact]
        public void Normalize_Object_String_IsNormalized()
        {
            // Act
            var actual = CacheKey.Normalize((object)"AbC");

            // Assert
            actual.Should().Be("abc");
        }

        [Fact]
        public void With_SpanOverload_MatchesArrayOverload()
        {
            // Arrange
            var array = new[] { "a", "b", "c" };

            // Act
            var fromArray = CacheKey.With(array);
            var fromSpan = CacheKey.With("a", "b", "c");

            // Assert
            fromArray.Should().Be("a-b-c");
            fromSpan.Should().Be("a-b-c");
        }

        [Fact]
        public void With_OwnerType_SpanOverload_MatchesArrayOverload()
        {
            // Arrange
            var array = new[] { "a", "b" };
            var expected = $"{typeof(CacheKeyTests).GetCacheKey()}:a-b";

            // Act
            var fromArray = CacheKey.With(typeof(CacheKeyTests), array);
            var fromSpan = CacheKey.With(typeof(CacheKeyTests), "a", "b");

            // Assert
            fromArray.Should().Be(expected);
            fromSpan.Should().Be(expected);
            CacheKey.GetCacheName(fromArray).Should().Be(nameof(CacheKeyTests));
            CacheKey.GetCacheName(CacheKey.Normalize(fromSpan)).Should().Be(nameof(CacheKeyTests));
        }

        [Fact]
        public void ConflictingShortOwnerNamesSwitchOnceToPermanentOwnerFallback()
        {
            var key = CacheKey.With(typeof(First.CollidingOwner), "private-id");
            CacheKey.RegisterCacheName(typeof(First.CollidingOwner), typeof(FirstModel));
            CacheKey.GetCacheName(key).Should().Be(nameof(FirstModel));
            CacheKey.RegisterCacheName(typeof(Second.CollidingOwner), typeof(SecondModel));
            for (var i = 0; i < 100; i++)
            {
                CacheKey.RegisterCacheName(typeof(First.CollidingOwner), typeof(FirstModel));
                CacheKey.RegisterCacheName(typeof(Second.CollidingOwner), typeof(SecondModel));
                CacheKey.GetCacheName(key.ToUpperInvariant()).Should().Be(nameof(First.CollidingOwner));
            }
            CacheKey.With(typeof(Second.CollidingOwner), "private-id").Should().Be(key);
        }

        [Fact]
        [Trait("Contract", "Compatibility")]
        public void With_CaseInsensitiveOwnerCollision_PreservesEachOriginalKeyPrefix()
        {
            // Pins original key identity. This already passed on d0ba70fa; it caught the later,
            // intermediate regression where case-insensitive telemetry metadata supplied key spelling.
            var parts = new[] { "id" };
            var first = CacheKey.With(typeof(CaseDistinctOwner), "id");
            var second = CacheKey.With(typeof(CASEDISTINCTOWNER), parts);

            first.Should().Be($"{typeof(CaseDistinctOwner).GetCacheKey()}:id");
            second.Should().Be($"{typeof(CASEDISTINCTOWNER).GetCacheKey()}:id");
            second.Should().NotBe(first);
            CacheKey.RegisterCacheName(typeof(CaseDistinctOwner), typeof(FirstModel));
            CacheKey.RegisterCacheName(typeof(CASEDISTINCTOWNER), typeof(SecondModel));
            CacheKey.GetCacheName(first).Should().Be(CacheKey.GetCacheName(second));
            CacheKey.With(typeof(CASEDISTINCTOWNER), "id").Should().Be(second);
        }

        private static class First
        {
            public sealed class CollidingOwner;
        }

        private static class Second
        {
            public sealed class CollidingOwner;
        }

        private sealed class FirstModel;
        private sealed class SecondModel;
        private sealed class CaseDistinctOwner;
        private sealed class CASEDISTINCTOWNER;

        [Fact]
        public void UnregisteredOwnerWithSameShortNameSharesExistingModelLabel()
        {
            CacheKey.RegisterCacheName(typeof(Registered.SharedLabelOwner), typeof(FirstModel));
            var key = CacheKey.With(typeof(Unregistered.SharedLabelOwner), "id");
            CacheKey.GetCacheName(key).Should().Be(nameof(FirstModel));
        }

        private static class Registered
        {
            public sealed class SharedLabelOwner;
        }

        private static class Unregistered
        {
            public sealed class SharedLabelOwner;
        }
    }
}
