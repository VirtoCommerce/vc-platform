using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class LocalizedStringTests
    {
        [Fact]
        public void SetValue_ShouldAddValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";

            // Act
            localizedString.SetValue(languageCode, value);

            // Assert
            Assert.Equal(value, localizedString.GetValue(languageCode));
        }

        [Fact]
        public void GetValue_ShouldReturnValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.SetValue(languageCode, value);

            // Act
            var result = localizedString.GetValue(languageCode);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void GetValue_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";

            // Act
            var result = localizedString.GetValue(languageCode);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGetValue_ShouldReturnTrueIfFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.SetValue(languageCode, value);

            // Act
            var result = localizedString.TryGetValue(languageCode, out var resultValue);

            // Assert
            Assert.True(result);
            Assert.Equal(value, resultValue);
        }

        [Fact]
        public void TryGetValue_ShouldReturnFalseIfNotFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";

            // Act
            var result = localizedString.TryGetValue(languageCode, out var resultValue);

            // Assert
            Assert.False(result);
            Assert.Null(resultValue);
        }

        [Fact]
        public void RemoveValue_ShouldRemoveValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.SetValue(languageCode, value);

            // Act
            localizedString.RemoveValue(languageCode);

            // Assert
            Assert.Null(localizedString.GetValue(languageCode));
        }

        [Fact]
        public void Validate_ShouldReturnFalseIfInvalidLanguages()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.SetValue("en-US", "Hello");
            localizedString.SetValue("fr-FR", "Bonjour");
            var allowedLanguages = new List<string> { "en-US" };

            // Act
            var result = localizedString.Validate(allowedLanguages, out var invalidLanguages);

            // Assert
            Assert.False(result);
            Assert.Contains("fr-FR", invalidLanguages);
        }

        [Fact]
        public void Validate_ShouldReturnTrueIfAllLanguagesValid()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.SetValue("en-US", "Hello");
            var allowedLanguages = new List<string> { "en-US" };

            // Act
            var result = localizedString.Validate(allowedLanguages, out var invalidLanguages);

            // Assert
            Assert.True(result);
            Assert.Empty(invalidLanguages);
        }

        [Fact]
        public void Clean_ShouldRemoveInvalidLanguages()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.SetValue("en-US", "Hello");
            localizedString.SetValue("fr-FR", "Bonjour");
            var allowedLanguages = new List<string> { "en-US" };

            // Act
            localizedString.Clean(allowedLanguages);

            // Assert
            Assert.Null(localizedString.GetValue("fr-FR"));
            Assert.Equal("Hello", localizedString.GetValue("en-US"));
        }

        [Fact]
        public void GetCopy_ShouldReturnCopy()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.SetValue("en-US", "Hello");

            // Act
            var copy = localizedString.GetCopy() as LocalizedString;

            localizedString.SetValue("fr-FR", "Bonjour");

            // Assert
            Assert.NotNull(copy);
            Assert.Equal("Hello", copy.GetValue("en-US"));
            Assert.Null(copy.GetValue("fr-FR"));
        }
    }
}
