using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Extensions
{
    public class EnumerableExtensionsTests : PlatformCoreMockHelper
    {
        #region IsNullOrEmpty

        [Fact]
        public void IsNullOrEmpty_Null_True()
        {
            // Arrange
            IEnumerable<string> items = null;

            // Act
            var actual = items.IsNullOrEmpty();

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsNullOrEmpty_Empty_True()
        {
            // Arrange
            var items = Enumerable.Empty<string>();

            // Act
            var actual = items.IsNullOrEmpty();

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsNullOrEmpty_NotEmpty_False()
        {
            // Arrange
            var items = _fixture.CreateMany<string>();

            // Act
            var actual = items.IsNullOrEmpty();

            // Assert
            actual.Should().BeFalse();
        }

        #endregion IsNullOrEmpty

        #region Paginate

        [Theory]
        [InlineData(0, 0)]
        [InlineData(DEFAULT_PAGE_SIZE, 1)]
        [InlineData(DEFAULT_PAGE_SIZE + 1, 2)]
        public void Paginate(int itemsCount, int pagesExpected)
        {
            // Arrange
            var items = _fixture.CreateMany<string>(itemsCount);

            // Act
            var actual = items.Paginate(DEFAULT_PAGE_SIZE).ToList();

            // Assert
            actual.Count.Should().Be(pagesExpected);
        }

        [Fact]
        public void Paginate_Null()
        {
            // Arrange
            IEnumerable<string> items = null;

            // Act
            var actual = items.Paginate(DEFAULT_PAGE_SIZE).ToList();

            // Assert
            actual.Count.Should().Be(0);
        }

        #endregion Paginate

        #region Apply

        [Fact]
        public void ApplyIEnumerable()
        {
            // Arrange
            var items = _fixture.CreateMany<string>();
            var counter = items.Count();
            Action<string> action = (str) => { counter--; };

            // Act
            items.Apply(action);

            // Assert
            counter.Should().Be(0);
        }

        [Fact]
        public void ApplyList()
        {
            // Arrange
            var items = _fixture.CreateMany<string>().ToList();
            var counter = items.Count;
            Action<string> action = (str) => { counter--; };

            // Act
            items.Apply(action);

            // Assert
            counter.Should().Be(0);
        }

        [Fact]
        public void ApplyDictionary()
        {
            // Arrange
            var items = _fixture.CreateMany<string>().ToDictionary(x => _fixture.Create<string>());
            var counter = items.Keys.Count;
            Action<object, object> action = (obj1, obj2) => { counter--; };

            // Act
            items.Apply(action);

            // Assert
            counter.Should().Be(0);
        }

        #endregion Apply

        #region GetOrderIndependentHashCode

        [Fact]
        public void GetOrderIndependentHashCode_HashIsDiffers()
        {
            // Arrange
            var firstCollection = _fixture.CreateMany<TestComparableClass>(1);
            var secondCollection = firstCollection.ToList();
            _fixture.AddManyTo(secondCollection);

            // Act
            var expected = firstCollection.GetOrderIndependentHashCode();
            var actual = secondCollection.GetOrderIndependentHashCode();

            // Assert
            actual.Should().NotBe(expected);
        }

        #endregion GetOrderIndependentHashCode
    }
}
