using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class WildcardMatcherTests
    {
        [Theory]
        // Exact matches
        [InlineData("catalog:read", "catalog:read", true)]
        [InlineData("catalog:read", "catalog:update", false)]
        // Match-all
        [InlineData("*", "catalog:read", true)]
        // An empty value matches nothing, not even '*' - a missing name should never satisfy a mask
        [InlineData("*", "", false)]
        // Prefix masks
        [InlineData("catalog:*", "catalog:read", true)]
        [InlineData("catalog:*", "catalog:setting:access", true)]
        [InlineData("catalog:*", "platform:read", false)]
        // A prefix mask requires the separator to be present
        [InlineData("catalog:*", "catalog", false)]
        // Suffix masks
        [InlineData("*:read", "catalog:read", true)]
        [InlineData("*:read", "catalog:update", false)]
        // Masks in the middle
        [InlineData("platform:*:access", "platform:setting:access", true)]
        [InlineData("platform:*:access", "platform:setting:update", false)]
        // Anchored - must match the whole value, not a substring
        [InlineData("catalog", "catalog:read", false)]
        [InlineData("read", "catalog:read", false)]
        // Case insensitive
        [InlineData("Catalog:*", "catalog:read", true)]
        [InlineData("catalog:READ", "catalog:read", true)]
        // '?' matches exactly one character
        [InlineData("cat?log:read", "catalog:read", true)]
        [InlineData("cat?log:read", "catlog:read", false)]
        // Everything other than '*' and '?' is literal
        [InlineData("a.b", "axb", false)]
        [InlineData("a.b", "a.b", true)]
        [InlineData("a+b", "a+b", true)]
        public void IsMatch_ReturnsExpectedResult(string pattern, string value, bool expected)
        {
            //Act
            var actual = WildcardMatcher.IsMatch(pattern, value);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "catalog:read")]
        [InlineData("", "catalog:read")]
        [InlineData(" ", "catalog:read")]
        [InlineData("catalog:*", null)]
        public void IsMatch_WithNullOrEmptyArgument_ReturnsFalse(string pattern, string value)
        {
            //Act
            var actual = WildcardMatcher.IsMatch(pattern, value);

            //Assert
            Assert.False(actual);
        }
    }
}
