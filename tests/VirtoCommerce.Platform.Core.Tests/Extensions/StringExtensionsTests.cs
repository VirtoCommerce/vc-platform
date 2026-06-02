using FluentAssertions;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Extensions
{
    public class StringExtensionsTests
    {
        #region Truncate

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Truncate_NullOrEmpty_ReturnsValueUnchanged(string value)
        {
            // Act
            var actual = value.Truncate(10);

            // Assert
            actual.Should().Be(value);
        }

        [Fact]
        public void Truncate_ShorterThanMaxLength_ReturnsValueUnchanged()
        {
            // Arrange
            var value = "Short";

            // Act
            var actual = value.Truncate(10);

            // Assert
            actual.Should().Be(value);
        }

        [Fact]
        public void Truncate_EqualToMaxLength_ReturnsValueUnchanged()
        {
            // Arrange
            var value = "1234567890";

            // Act
            var actual = value.Truncate(value.Length);

            // Assert
            actual.Should().Be(value);
        }

        [Fact]
        public void Truncate_LongerThanMaxLength_TruncatesAndAppendsSuffix()
        {
            // Arrange
            var value = "The quick brown fox";

            // Act
            var actual = value.Truncate(10);

            // Assert
            actual.Should().Be("The qui...");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public void Truncate_LongerThanMaxLength_ResultDoesNotExceedMaxLength(int maxLength)
        {
            // Arrange
            var value = "The quick brown fox jumps over the lazy dog";

            // Act
            var actual = value.Truncate(maxLength);

            // Assert
            actual.Length.Should().Be(maxLength);
        }

        [Fact]
        public void Truncate_CustomSuffix_TruncatesAndAppendsSuffix()
        {
            // Arrange
            var value = "The quick brown fox";

            // Act
            var actual = value.Truncate(10, "----");

            // Assert
            actual.Should().Be("The qu----");
            actual.Length.Should().Be(10);
        }

        [Fact]
        public void Truncate_EmptySuffix_TruncatesToExactMaxLength()
        {
            // Arrange
            var value = "The quick brown fox";

            // Act
            var actual = value.Truncate(10, "");

            // Assert
            actual.Should().Be("The quick ");
            actual.Length.Should().Be(10);
        }

        [Fact]
        public void Truncate_NullSuffix_TreatedAsEmpty()
        {
            // Arrange
            var value = "The quick brown fox";

            // Act
            var actual = value.Truncate(10, null);

            // Assert
            actual.Should().Be("The quick ");
            actual.Length.Should().Be(10);
        }

        [Fact]
        public void Truncate_MaxLengthSmallerThanSuffix_DoesNotThrow()
        {
            // Arrange
            var value = "Hello";

            // Act
            var actual = value.Truncate(2);

            // Assert
            actual.Should().Be("...");
        }

        #endregion Truncate
    }
}
