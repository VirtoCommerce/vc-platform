using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class MimeTypeResolverTests
    {
        [Fact]
        public void ResolveContentType_Default()
        {
            // Arrange
            var impossibleExtension = ".dolphin";

            // Act
            var actual = MimeTypeResolver.ResolveContentType(impossibleExtension);

            // Assert
            actual.Should().Be(MimeTypeResolver.DefaultMimeType);
        }

        [Fact]
        public void ResolveContentType_Null_ShouldThrowException()
        {
            // Arrange
            string nullString = null;

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => MimeTypeResolver.ResolveContentType(nullString));
        }

        [Fact]
        public void ResolveContentType_FileName_ShouldExtract()
        {
            // Arrange
            var testFileName = "cat.jpeg";
            var expected = "image/jpeg";

            // Act
            var actual = MimeTypeResolver.ResolveContentType(testFileName);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void ResolveContentType_UrlPath_ShouldExtract()
        {
            // Arrange
            var testFileName = @"https://github.com/VirtoCommerce/vc-platform/blob/master/vc-logo.ico";
            var expected = "image/x-icon";

            // Act
            var actual = MimeTypeResolver.ResolveContentType(testFileName);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(ImageTestData))]
        [ClassData(typeof(HtmlTestData))]
        public void ResolveContentType_MainExtensions_ShouldExtract(string extension, string expected)
        {
            // Act
            var actual = MimeTypeResolver.ResolveContentType(extension);

            // Assert
            actual.Should().Be(expected);
        }

        private class ImageTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { ".bmp", "image/bmp" };
                yield return new object[] { ".bmp", "image/bmp" };
                yield return new object[] { ".gif", "image/gif" };
                yield return new object[] { ".ico", "image/x-icon" };
                yield return new object[] { ".jpe", "image/jpeg" };
                yield return new object[] { ".jpeg", "image/jpeg" };
                yield return new object[] { ".jpg", "image/jpeg" };
                yield return new object[] { ".png", "image/png" };
                yield return new object[] { ".svg", "image/svg+xml" };
                yield return new object[] { ".tif", "image/tiff" };
                yield return new object[] { ".tiff", "image/tiff" };
                yield return new object[] { ".webp", "image/webp" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class HtmlTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { ".css", "text/css" };
                yield return new object[] { ".csv", "text/csv" };
                yield return new object[] { ".json", "application/json" };
                yield return new object[] { ".htm", "text/html" };
                yield return new object[] { ".html", "text/html" };
                yield return new object[] { ".liquid", "text/html" };
                yield return new object[] { ".md", "text/html" };
                yield return new object[] { ".xml", "text/xml" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
