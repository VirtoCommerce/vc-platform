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
            public int Prop2 { get; set; }
        }

        class Base
        {
            public int Prop { get; set; }
        }
        class Derived1 : Base
        {
            public int DerivedProp => Prop;
        }
        class Derived2 : Base { }

        readonly List<A> _derivedClassList = new List<A>
        {
            new B{Prop = 8, Prop2 = 3},
            new B{Prop = 3, Prop2 = 2},
            new B{Prop = 10, Prop2 = 1},
            new B{Prop = 5, Prop2 = 4},
            new B{Prop = 3, Prop2 = 10},
        };

        readonly List<B> _declaredClassList = new List<B>
        {
            new B{Prop = 8},
            new B{Prop = 3},
            new B{Prop = 10},
            new B{Prop = 5},
        };

        readonly List<Base> _mutliRegistrationList = new List<Base>
        {
            new Derived1{Prop = 2},
            new Derived1{Prop = 1},
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
        public void OrderBySortInfos_DerivedClassList_IsSorted(SortDirection direction, int expected)
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
        public void OrderBySortInfos_DeclaredClassList_IsSorted(SortDirection direction, int expected)
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
        public void OrderBySortInfos_DeclaredClassListWrongProperty_IsUntouched(string propertyName, SortDirection direction, int expected)
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
        public void OrderBySortInfos_TwoSortInfosDerivedClassList_IsSorted()
        {
            // Arrange
            var sortInfo = new[]
            {
                new SortInfo
                {
                    SortColumn = nameof(B.Prop),
                    SortDirection = SortDirection.Ascending
                },
                new SortInfo
                {
                    SortColumn = nameof(B.Prop2),
                    SortDirection = SortDirection.Descending
                }
            };

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderBySortInfos(sortInfo).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(3, firstB.Prop);
            Assert.Equal(10, firstB.Prop2);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Fact]
        public void OrderBySortInfos_DeclaredClassListNullProperty_Throws()
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

        [Fact]
        public void OrderBy_DerivedClassList_IsSorted()
        {
            // Arrange

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderBy(nameof(B.Prop)).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(3, firstB.Prop);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Fact]
        public void OrderByDescending_DerivedClassList_IsSorted()
        {
            // Arrange

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderByDescending(nameof(B.Prop)).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(10, firstB.Prop);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Fact]
        public void ThenBy_DerivedClassList_IsSorted()
        {
            // Arrange

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderBy(nameof(B.Prop)).ThenBy(nameof(B.Prop2)).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(2, firstB.Prop2);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Fact]
        public void ThenByDescending_DerivedClassList_IsSorted()
        {
            // Arrange

            // Act
            var orderedList = _derivedClassList.AsQueryable().OrderBy(nameof(B.Prop)).ThenByDescending(nameof(B.Prop2)).ToList();

            // Assert
            var firstB = (B)orderedList.First();
            Assert.Equal(10, firstB.Prop2);
            Assert.Equal(_derivedClassList.Count, orderedList.Count);
        }

        [Fact]
        public void OrderBy_MultipleRegistrations_SortedByBaseClassProperty()
        {
            // Arrange
            AbstractTypeFactory<Base>.RegisterType<Derived1>();
            AbstractTypeFactory<Base>.RegisterType<Derived2>();

            // Act
            var orderedList = _mutliRegistrationList.AsQueryable().OrderBy(nameof(Base.Prop)).ToList();

            // Assert
            var firstBase = orderedList.First();
            Assert.Equal(1, firstBase.Prop);
            Assert.Equal(_mutliRegistrationList.Count, orderedList.Count);
        }

        [Fact]
        public void OrderBy_MultipleRegistrations_NotSortedByDerivedProperty()
        {
            // Arrange
            AbstractTypeFactory<Base>.RegisterType<Derived1>();
            AbstractTypeFactory<Base>.RegisterType<Derived2>();

            // Act
            var orderedList = _mutliRegistrationList.AsQueryable().OrderBy(nameof(Derived1.DerivedProp)).ToList();

            // Assert
            var firstBase = orderedList.First();
            Assert.Equal(2, firstBase.Prop);
            Assert.Equal(_mutliRegistrationList.Count, orderedList.Count);
        }
    }
}
