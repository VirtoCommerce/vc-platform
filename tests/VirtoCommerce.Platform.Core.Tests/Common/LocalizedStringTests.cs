using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class LocalizedStringTests
    {
        [Fact]
        public void Set_ShouldAddValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";

            // Act
            localizedString.Set(languageCode, value);

            // Assert
            Assert.Equal(value, localizedString.Get(languageCode));
        }

        [Fact]
        public void Get_ShouldReturnValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.Set(languageCode, value);

            // Act
            var result = localizedString.Get(languageCode);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Get_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";

            // Act
            var result = localizedString.Get(languageCode);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryGet_ShouldReturnTrueIfFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.Set(languageCode, value);

            // Act
            var result = localizedString.TryGet(languageCode, out var resultValue);

            // Assert
            Assert.True(result);
            Assert.Equal(value, resultValue);
        }

        [Fact]
        public void TryGet_ShouldReturnFalseIfNotFound()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";

            // Act
            var result = localizedString.TryGet(languageCode, out var resultValue);

            // Assert
            Assert.False(result);
            Assert.Null(resultValue);
        }

        [Fact]
        public void Remove_ShouldRemoveValue()
        {
            // Arrange
            var localizedString = new LocalizedString();
            var languageCode = "en-US";
            var value = "Hello";
            localizedString.Set(languageCode, value);

            // Act
            localizedString.Remove(languageCode);

            // Assert
            Assert.Null(localizedString.Get(languageCode));
        }

        [Fact]
        public void Validate_ShouldReturnFalseIfInvalidLanguages()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.Set("en-US", "Hello");
            localizedString.Set("fr-FR", "Bonjour");
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
            localizedString.Set("en-US", "Hello");
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
            localizedString.Set("en-US", "Hello");
            localizedString.Set("fr-FR", "Bonjour");
            var allowedLanguages = new List<string> { "en-US" };

            // Act
            localizedString.Clean(allowedLanguages);

            // Assert
            Assert.Null(localizedString.Get("fr-FR"));
            Assert.Equal("Hello", localizedString.Get("en-US"));
        }

        [Fact]
        public void GetCopy_ShouldReturnCopy()
        {
            // Arrange
            var localizedString = new LocalizedString();
            localizedString.Set("en-US", "Hello");

            // Act
            var copy = localizedString.GetCopy() as LocalizedString;

            localizedString.Set("fr-FR", "Bonjour");

            // Assert
            Assert.NotNull(copy);
            Assert.Equal("Hello", copy.Get("en-US"));
            Assert.Null(copy.Get("fr-FR"));
        }
    }
}
