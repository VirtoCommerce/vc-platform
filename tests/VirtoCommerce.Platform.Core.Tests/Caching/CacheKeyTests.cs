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
        }
    }
}
