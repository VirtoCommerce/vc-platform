using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    [Trait("Category", "Unit")]
    public class QueryableExtensionsTests
    {
        class A { }

        class B : A
        {
            public int Prop { get; set; }
        }

        readonly List<A> _derivedClassList = new List<A>
        {
            new B{Prop = 8},
            new B{Prop = 3},
            new B{Prop = 10},
            new B{Prop = 5}
        };

        readonly List<B> _declaredClassList = new List<B>
        {
            new B{Prop = 8},
            new B{Prop = 3},
            new B{Prop = 10},
            new B{Prop = 5}
        };

        public QueryableExtensionsTests()
        {
            if (!AbstractTypeFactory<A>.AllTypeInfos.Any())
            {
                AbstractTypeFactory<A>.RegisterType<B>();
            }
        }

        [Theory]
        [InlineData(SortDirection.Ascending, 3)]
        [InlineData(SortDirection.Descending, 10)]
        public void OrderBy_DerivedClassList_IsSorted(SortDirection direction, int expected)
        {
            // Arrange
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = nameof(B.Prop),
                    SortDirection = direction
                }
            };

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderBySortInfos(sortInfo).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(expected, firstB.Prop);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Theory]
        [InlineData(SortDirection.Ascending, 3)]
        [InlineData(SortDirection.Descending, 10)]
        public void OrderBy_DeclaredClassList_IsSorted(SortDirection direction, int expected)
        {
            // Arrange
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = nameof(B.Prop),
                    SortDirection = direction
                }
            };

            // Act
            var orderedList = _declaredClassList.AsQueryable().OrderBySortInfos(sortInfo).ToList();

            // Assert
            Assert.Equal(expected, orderedList.First().Prop);
            Assert.Equal(_declaredClassList.Count, orderedList.Count);

        }

        [Theory]
        [InlineData("", SortDirection.Ascending, 8)]
        [InlineData("12.234.22", SortDirection.Descending, 8)]
        [InlineData("Prop.Value", SortDirection.Descending, 8)]
        [InlineData("!#$@5^%#$&^$%*", SortDirection.Descending, 8)]
        public void OrderBy_DeclaredClassListWrongProperty_IsUntouched(string propertyName, SortDirection direction, int expected)
        {
            // Arrange
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = propertyName,
                    SortDirection = direction
                }
            };

            // Act
            var orderedList = _declaredClassList.AsQueryable().OrderBySortInfos(sortInfo).ToList();

            // Assert
            Assert.Equal(_declaredClassList.Count, orderedList.Count);
            Assert.Equal(expected, (orderedList.First()).Prop);

        }

        [Fact]
        public void OrderBy_DeclaredClassListNullProperty_Throws()
        {
            // Arrange
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = null,
                    SortDirection = SortDirection.Ascending,
                }
            };

            // Act
            Action action = () => _declaredClassList.AsQueryable().OrderBySortInfos(sortInfo).ToList();

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
