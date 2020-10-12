using FluentAssertions;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    public class SemanticVersionTests
    {
        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, "10.5.7", false)]
        [InlineData("14.9.12", null, false)]
        [InlineData("14.9.12", "10.5.7", true)]
        [InlineData("4.9.12", "10.5.7", false)]
        [InlineData("5.5.5", "5.5.5", true)]
        public void OperatorGreaterOrZero(string firstVersion, string secondVersion, bool expected)
        {
            // Arrange
            var a = GetSemanticVersion(firstVersion);
            var b = GetSemanticVersion(secondVersion);

            // Act
            var actual = a >= b;

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, "10.5.7", false)]
        [InlineData("14.9.12", null, false)]
        [InlineData("14.9.12", "10.5.7", false)]
        [InlineData("4.9.12", "10.5.7", true)]
        [InlineData("5.5.5", "5.5.5", true)]
        public void OperatorLessOrZero(string firstVersion, string secondVersion, bool expected)
        {
            // Arrange
            var a = GetSemanticVersion(firstVersion);
            var b = GetSemanticVersion(secondVersion);

            // Act
            var actual = a <= b;

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(null, "10.5.7", false)]
        [InlineData("14.9.12", null, false)]
        [InlineData("14.9.12", "10.5.7", true)]
        [InlineData("4.9.12", "10.5.7", false)]
        [InlineData("5.5.5", "5.5.5", false)]
        public void OperatorGreater(string firstVersion, string secondVersion, bool expected)
        {
            // Arrange
            var a = GetSemanticVersion(firstVersion);
            var b = GetSemanticVersion(secondVersion);

            // Act
            var actual = a > b;

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(null, "10.5.7", false)]
        [InlineData("14.9.12", null, false)]
        [InlineData("14.9.12", "10.5.7", false)]
        [InlineData("4.9.12", "10.5.7", true)]
        [InlineData("5.5.5", "5.5.5", false)]
        public void OperatorLess(string firstVersion, string secondVersion, bool expected)
        {
            // Arrange
            var a = GetSemanticVersion(firstVersion);
            var b = GetSemanticVersion(secondVersion);

            // Act
            var actual = a < b;

            // Assert
            actual.Should().Be(expected);
        }

        private SemanticVersion GetSemanticVersion(string version)
        {
            if (version is null)
                return null;

            try
            {
                return SemanticVersion.Parse(version);
            }
            catch
            {
                return null;
            }
        }
    }
}
